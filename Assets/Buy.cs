using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buy : MonoBehaviour
{
    public Inventory inventory;
    public GameObject _item;
    public Money money;
    public Money price;

    public void BuySeed()
    {
        if (money.money >= price.money && inventory.mItems.Count < 30)
        {
            GameObject Seed = Instantiate(_item, null);
            inventory.AddItem(Seed.GetComponent<IInventoryItem>());
            money.SetMoney(money.money - price.money);
        }
    }

}
