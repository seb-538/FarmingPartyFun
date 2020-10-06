using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{

    public Inventory Inventory;
    public GameObject Hud;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel,
            Input.mousePosition))
        {

            IInventoryItem item = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;
            if (item != null)
            {
                if (Hud.GetComponent<HUD>().OnHand != null && Hud.GetComponent<HUD>().inventoryPanel.GetChild(Hud.GetComponent<HUD>().inv_position).GetChild(0).GetChild(0).GetComponent<ItemDragHandler>().Item == item)
                {
                    Hud.GetComponent<HUD>().OnHand.transform.parent = null;
                    Hud.GetComponent<HUD>().OnHand.AddComponent<Rigidbody>();
                    Hud.GetComponent<HUD>().OnHand = null;
                }
                Inventory.RemoveItem(item);
            }

        }
    }
}
