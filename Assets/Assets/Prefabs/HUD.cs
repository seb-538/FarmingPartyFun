using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public Inventory Inventory;
    public GameObject Hand;
    public GameObject MessagePanel;
    public GameObject TextPanel;
    public Transform inventoryPanel;
    public int inv_position = 0;
    public GameObject OnHand;
    public GameObject full_inventory;
    public GameObject Money;
    public GameObject Shop;
    public GameObject Priest;
    public string HandItemName = null;
    public bool IsOnField = false;
    public bool IsPlantable = false;
    public bool Plantable = false;
    public GameObject ObjToMove;
    public bool Occuped = false;
    public GameObject Lettre;

    void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        Inventory.ItemRemoved += InventoryScript_ItemRemoved;
        inventoryPanel = transform.Find("Inventory");
        Button first = inventoryPanel.GetChild(inv_position).GetChild(inv_position).GetComponent<Button>();
        first.Select();
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");
        foreach (Transform slot in inventoryPanel)
        {
            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Image image = imageTransform.GetComponent<Image>();
            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = e.Item.Image;

                itemDragHandler.Item = e.Item;
                if (slot.GetChild(0) == inventoryPanel.GetChild(inv_position).GetChild(0))
                {
                     HandItemName = e.Item.Name;
                    if (IsOnField == true && HandItemName == "Rateau")
                        ObjToMove.SetActive(true);
                    if (e.Item.Name.Length > 6 && e.Item.Name.Substring(0, 6) == "Graine" && IsPlantable == true)
                    {
                        if (Occuped)
                            OpenMessagePanel("-Déja planté-", true);
                        else
                            OpenMessagePanel("-Planter " + HandItemName + "- (Clique Gauche)", false);
                        Plantable = true;
                    }
                    OnHand = (itemDragHandler.Item as MonoBehaviour).gameObject;
                    OnHand.SetActive(true);
                    Destroy(OnHand.transform.GetComponent<Rigidbody>());
                    OnHand.transform.parent = Hand.transform;
                    Vector3 itemrota = (itemDragHandler.Item as Tools).PickRotation;
                    Quaternion rotation = Quaternion.Euler(itemrota.x, itemrota.y, itemrota.z);
                    OnHand.transform.localPosition = (itemDragHandler.Item as Tools).PickPosition;
                    OnHand.transform.localRotation = rotation;
                }
                break;
            }
        }
    }

    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e)
    {
        int i = 0;
        Transform inventoryPanel = transform.Find("Inventory");
        
        foreach (Transform slot in inventoryPanel)
        {
            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Image image = imageTransform.GetComponent<Image>();
            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();
            if (itemDragHandler != null && itemDragHandler.Item.Equals(e.Item))
            {
                if (e.Item.Name == "Rateau")
                    ObjToMove.SetActive(false);
                if (e.Item.Name.Length > 6 && e.Item.Name.Substring(0, 6) == "Graine" && IsPlantable == true)
                {
                    CloseMessagePanel();
                    Plantable = false;
                }
                HandItemName = null;
                image.enabled = false;
                image.sprite = null;
                break;
            }
        }

    }

    public void OpenInventory()
    {
        int counter = 0;
        full_inventory.SetActive(true);
        Transform inventoryPanel = transform.Find("Inventory");
        foreach (Transform slot in inventoryPanel)
        {
            if (counter > 5)
            {
                slot.gameObject.SetActive(true);
            }
            counter += 1;
        }
        Money.SetActive(true);
    }

    public void CloseInventory()
    {
        int counter = 0;
        full_inventory.SetActive(false);
        Transform inventoryPanel = transform.Find("Inventory");
        foreach (Transform slot in inventoryPanel)
        {
            if (counter > 5)
            {
                slot.gameObject.SetActive(false);
            }
            counter += 1;
        }
        Money.SetActive(false);
    }

    public void OpenShop()
    {
        Shop.SetActive(true);
        int counter = 0;
        full_inventory.SetActive(true);
        Transform inventoryPanel = transform.Find("Inventory");
        foreach (Transform slot in inventoryPanel)
        {
            if (counter > 5)
            {
                slot.gameObject.SetActive(true);
            }
            counter += 1;
        }
        Money.SetActive(true);
    }

    public void CloseShop()
    {
        Shop.SetActive(false);
        int counter = 0;
        full_inventory.SetActive(false);
        Transform inventoryPanel = transform.Find("Inventory");
        foreach (Transform slot in inventoryPanel)
        {
            if (counter > 5)
            {
                slot.gameObject.SetActive(false);
            }
            counter += 1;
        }
        Money.SetActive(false);
    }

    public void OpenPriest()
    {
        Priest.SetActive(true);
        Money.SetActive(true);
    }

    public void ClosePriest()
    {
        Priest.SetActive(false);
        Money.SetActive(false);
    }

    public void OpenMessagePanel(string test, bool alert)
    {

        TextPanel.GetComponent<Text>().text = test;
        if (alert)
        {
            TextPanel.GetComponent<Text>().color = Color.red;
        }
        else
        {
            TextPanel.GetComponent<Text>().color = Color.white;
        }
        MessagePanel.SetActive(true);
    }

    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            inv_position += Convert.ToInt32((-Input.GetAxis("Mouse ScrollWheel") * 10));
            if (inv_position > 5)
            {
                inv_position = 0;
            }

            if (inv_position < 0)
            {
                inv_position = 5;
            }
            Button first = inventoryPanel.GetChild(inv_position).GetChild(0).GetComponent<Button>();
            first.Select();

            Transform imageTransform = inventoryPanel.GetChild(inv_position).GetChild(0).GetChild(0);
            IInventoryItem item = imageTransform.GetComponent<ItemDragHandler>().Item;
            if (OnHand != null)
            {

                OnHand.SetActive(false);
                OnHand.transform.parent = null;
                OnHand.AddComponent<Rigidbody>();
            }
            if (item != null && imageTransform.GetComponent<Image>().enabled)
            {
                HandItemName = item.Name;
                if (IsOnField == true)
                {
                    if (HandItemName == "Rateau")
                        ObjToMove.SetActive(true);
                    else
                        ObjToMove.SetActive(false);
                }
                if (IsPlantable == true)
                {
                    if (HandItemName.Substring(0, 6) == "Graine")
                    {
                        if (Occuped)
                            OpenMessagePanel("-Déja planté-", true);
                        else
                        OpenMessagePanel("-Planter " + HandItemName + "- (Clique Gauche)", false);
                        Plantable = true;
                    }
                    else
                    {
                        CloseMessagePanel();
                        Plantable = false;
                    }
                }
                GameObject goItem = (item as MonoBehaviour).gameObject;
                goItem.SetActive(true);
                Destroy(goItem.transform.GetComponent<Rigidbody>());
                goItem.transform.parent = Hand.transform;
                Vector3 itemrota = (item as Tools).PickRotation;
                Quaternion rotation = Quaternion.Euler(itemrota.x, itemrota.y, itemrota.z);
                goItem.transform.localPosition = (item as Tools).PickPosition;
                goItem.transform.localRotation = rotation;
                OnHand = goItem;
            }
            else
            {
                HandItemName = null;
                ObjToMove.SetActive(false);
                if (IsPlantable == true)
                {
                    CloseMessagePanel();
                    Plantable = false;
                }
            }

        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            inventoryPanel.GetChild(inv_position).GetChild(0).GetComponent<Button>().Select();
        }

        if (Input.GetKeyDown(KeyCode.A)) 
        {
   //         int i = 0;
                if (OnHand != null)
                {
                 OnHand.transform.parent = null;
                 OnHand.AddComponent<Rigidbody>();
                 OnHand = null;
                }

                Transform imageTransform = inventoryPanel.GetChild(inv_position).GetChild(0).GetChild(0);
                IInventoryItem item = imageTransform.GetComponent<ItemDragHandler>().Item;
                Image image = imageTransform.GetComponent<Image>();

                Inventory.RemoveItem(item);


        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lettre.GetComponent<Lettre>().Pages[0].SetActive(true);
            Lettre.GetComponent<Lettre>().ActivePage = Lettre.GetComponent<Lettre>().Pages[0];

        }

    }
}
