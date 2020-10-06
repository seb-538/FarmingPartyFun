using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public GameObject txt;
    public int money;
    // Start is called before the first frame update
    void Start()
    {
        txt.GetComponent<UnityEngine.UI.Text>().text = money.ToString();
    }

    // Update is called once per frame
    public void SetMoney(int value)
    {
        money = value;
        txt.GetComponent<UnityEngine.UI.Text>().text = money.ToString();
    }
}

