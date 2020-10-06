using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BuyBuilding : MonoBehaviour
{
    public GameObject build;
    public Money money;
    public Money price;

    public void Buy ()
    {
        if (money.money >= price.money)
        {
            build.SetActive(true);
            money.SetMoney(money.money - price.money);
            Destroy(transform.parent.gameObject);
        }
    }

}