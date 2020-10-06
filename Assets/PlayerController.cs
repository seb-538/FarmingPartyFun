using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public float turnSpeed;
    public Inventory inventory;
    public HUD Hud;
    private IInventoryItem _item = null;
    private Door _door = null;
    private ShopCollider _shop = null;
    public fader fade;
    private bool Inv_Open = false;
    private bool Shop_Open = false;
    private bool Priest_Open = false;
    private bool IsOnField = false;
    private GameObject toPlant = null;
    private Plant Dirt;
    private PriestCollider _priest = null;
    private Bed _bed;
    public DayNightController timer;

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("vertical", Input.GetAxis("Vertical"));
        anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));
        if (Input.GetAxis("Vertical") > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
        }

        if (_item != null && Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem(_item);
            Hud.CloseMessagePanel();
        }

        if (_door != null && Input.GetKeyDown(KeyCode.E))
        {
            fade.fadeIn();
            Hud.CloseMessagePanel();
            StartCoroutine(warp());
        }

        if (_shop != null && Input.GetKeyDown(KeyCode.E))
        {
            if (Shop_Open)
            { 
                Hud.CloseShop();
                Shop_Open = false;
            }
            else
            {
                Hud.CloseMessagePanel();
                Hud.OpenShop();
                Shop_Open = true;
            }
        }

        if (_priest != null && Input.GetKeyDown(KeyCode.E))
        {
            if (Priest_Open)
            {
                Hud.ClosePriest();
                Priest_Open = false;
            }
            else
            {
                Hud.CloseMessagePanel();
                Hud.OpenPriest();
                Priest_Open = true;
            }
        }

        if (_bed != null && Input.GetKeyDown(KeyCode.E))
        {
            fade.fadeIn();
            StartCoroutine(sleep());
        }


            if (Input.GetKeyDown(KeyCode.I))
        {
            if (Inv_Open)
            {
                Hud.CloseInventory();
                Inv_Open = false;
            }
            else
            {
                Hud.OpenInventory();
                Inv_Open = true;
            }
        }

        if (Input.GetMouseButtonDown(0) && Hud.Plantable && toPlant != null && !Dirt.isOccuped)
        {
            GameObject Crops = Instantiate(Hud.OnHand.GetComponent<Seed>().plant, toPlant.transform, false);
            Crops.transform.localPosition = Hud.OnHand.GetComponent<Seed>().P_Position;

            Crops.transform.localScale = new Vector3(1f / Hud.OnHand.GetComponent<Seed>().GrowthTime,1f / Hud.OnHand.GetComponent<Seed>().GrowthTime, 1f / Hud.OnHand.GetComponent<Seed>().GrowthTime);
            Dirt.isOccuped = true;
            Dirt.DayToGrowth = Hud.OnHand.GetComponent<Seed>().GrowthTime;
            Dirt.PlantObject = Crops;
            Hud.Occuped = true;
            Hud.CloseMessagePanel();
            inventory.toDrop = false;
            Transform inventoryPanel = Hud.transform.Find("Inventory");
            inventory.RemoveItem(Hud.inventoryPanel.GetChild(Hud.inv_position).GetChild(0).GetChild(0).GetComponent<ItemDragHandler>().Item);
            Destroy(Hud.OnHand);
            inventory.toDrop = true;

        }
    }

    IEnumerator sleep()
    {
        yield return new WaitForSeconds(1);
        timer.currentTime = 29.5f;
        fade.fadeOut();
    }

    IEnumerator warp()
    {
        yield return new WaitForSeconds(1);
        this.transform.localPosition = _door.warpDestination;
        this.transform.Rotate(_door.warpRotation);
        
        fade.fadeOut();

    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.GetComponent<Plant>() != null)
        {
            Hud.IsPlantable = true;

            if (Hud.HandItemName != null && Hud.HandItemName.Substring(0, 6) == "Graine" && collision.collider.GetComponent<Plant>().isOccuped == false)
            {
                Hud.OpenMessagePanel("-Planter " + Hud.HandItemName + "- (Clique Gauche)", false);
                Hud.Plantable = true;
                Hud.IsPlantable = true;
                toPlant = collision.gameObject;
                Dirt = collision.collider.GetComponent<Plant>();
            }
            if (Hud.HandItemName != null && Hud.HandItemName.Substring(0, 6) == "Graine" && collision.collider.GetComponent<Plant>().isOccuped == true)
            {
                Hud.OpenMessagePanel("-Déja planté-", true);
                Hud.Occuped = true;
            }
        }

        if (collision.collider.GetComponent<Farm>() != null)
        {
            Hud.IsOnField = true;
            if (Hud.HandItemName == "Rateau")
                Hud.ObjToMove.SetActive(true);
        }

        Door door = collision.collider.GetComponent<Door>();
        if (door != null)
        {
            Hud.OpenMessagePanel(door.message, false);
            _door = door;
        }
        IInventoryItem item = collision.collider.GetComponent<IInventoryItem>();
        if (item != null && inventory.GetmItems().Count < 30)
        {
            Hud.OpenMessagePanel("- Ramasser "+item.Name+ " (E) -", false);
            _item = item;
        }
        else if (item != null)
        {
            Hud.OpenMessagePanel("- Inventaire plein -", true);
        }
        ShopCollider Shop = collision.collider.GetComponent<ShopCollider>();
        if (Shop != null)
        {
            Hud.OpenMessagePanel("- Parler (E) -", false);
            _shop = Shop;
        }

        PriestCollider Priest = collision.collider.GetComponent<PriestCollider>();
        if (Priest != null)
        {
            Hud.OpenMessagePanel("- Parler (E) -", false);
            _priest = Priest;
        }

        Bed Bed = collision.collider.GetComponent<Bed>();
        if (Bed != null)
        {
            Hud.OpenMessagePanel("- Dormir (E) -", false);
            _bed = Bed;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        IInventoryItem item = collision.collider.GetComponent<IInventoryItem>();
        Door door = collision.collider.GetComponent<Door>();
        ShopCollider Shop = collision.collider.GetComponent<ShopCollider>();
        PriestCollider Priest = collision.collider.GetComponent<PriestCollider>();
        Bed Bed = collision.collider.GetComponent<Bed>();

        if (collision.collider.GetComponent<Farm>() != null)
        {
            Hud.IsOnField = false;
            Hud.ObjToMove.SetActive(false);          
        }

        if (Priest != null)
        {
            Hud.ClosePriest();
            Priest_Open = false;
        }

        if (Shop != null)
        {
            Hud.CloseShop();
            Shop_Open = false;
        }


        if (item != null || door != null || Shop != null || Priest != null || Bed != null)
        {
            Hud.CloseMessagePanel();
            _item = null;
            _door = null;
            _shop = null;
            _priest = null;
            _bed = null;
        }

        if (collision.collider.GetComponent<Plant>() != null)
        {
            Hud.CloseMessagePanel();
            Hud.IsPlantable = false;
            Hud.Plantable = false;
            toPlant = null;
            Hud.Occuped = false;
        }
    }
}
