using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int SLOTS = 30;
    public List<IInventoryItem> mItems = new List<IInventoryItem>();
    public GameObject player;
    public bool toDrop = true;


    public event EventHandler<InventoryEventArgs> ItemAdded;

    public event EventHandler<InventoryEventArgs> ItemRemoved;

    public void AddItem(IInventoryItem item)
    {
        if(mItems.Count < SLOTS)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            collider.transform.parent = null;
            if (collider.enabled)
            {
                collider.enabled = false;

                mItems.Add(item);

                item.OnPickup();
                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }

            }
        }
    }

    public void RemoveItem(IInventoryItem item)
    {
        if (mItems.Contains(item))
        {
            mItems.Remove(item);

            if (toDrop)
            { 
                Vector3 playerPos = player.transform.position;
                Vector3 playerDirection = player.transform.forward;
                Quaternion playerRotation = player.transform.rotation;
                Vector3 spawnPos = playerPos + playerDirection + Vector3.up;
                item.OnDrop(spawnPos);
                Collider collider = (item as MonoBehaviour).GetComponent<Collider>();

                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
            if (ItemRemoved != null)
            {
                ItemRemoved(this, new InventoryEventArgs(item));
            }
        }
    }

    public List<IInventoryItem> GetmItems()
    {
        return mItems;
    }
}
