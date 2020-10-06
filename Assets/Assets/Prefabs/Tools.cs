using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using UnityEngine;

public class Tools : MonoBehaviour, IInventoryItem
{
    public string _Name;

    public string Name
    {
        get
        {
            return _Name;
        }
    }


    public Sprite _Image;

    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }

    public void OnDrop(Vector3 Front)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = Front;
    }

    public Vector3 PickPosition;

    public Vector3 PickRotation;
}