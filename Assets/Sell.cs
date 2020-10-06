using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Sell : MonoBehaviour
{
    public Inventory inventory;
    public GameObject _item;
    public Money money;
    public Money price;

    public void SellVegetable()
    {
        foreach (IInventoryItem vegetable in inventory.mItems)
        {
            if (vegetable.Name == _item.GetComponent<IInventoryItem>().Name)
            {
                money.SetMoney(money.money + price.money);
                inventory.RemoveItem(vegetable);
                Destroy((vegetable as MonoBehaviour).gameObject);
                break;
            }
        }
    }
}
