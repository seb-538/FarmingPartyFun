using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CubePlacement : MonoBehaviour
{
    public GameObject ObjToMove;
    public GameObject ObjToPlace;
    public LayerMask mask;
    public int LastPosX, LastPosZ;
    public float LastPosY;
    public Vector3 mousepos;
    public GameObject Field;
    public Renderer rend;
    public Material matGrid,matDefault;
    bool occuped = false;
    public GameObject player;

    void Start()
    {
        rend = GameObject.Find("Ground").GetComponent<Renderer>();
    }

    
    void Update()
    {
        //mousepos = Input.mousePosition;
        //Ray ray = Camera.main.ScreenPointToRay(mousepos);
        //RaycastHit hit;

        // if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        //{
        Vector3 playerPos = player.transform.position;
        Vector3 playerDirection = player.transform.forward;
        Quaternion playerRotation = player.transform.rotation;
        float spawnDistance = 0.5f;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        int PosX = (int)Mathf.Round(spawnPos.x); // hit.point.x);
        int PosZ = (int)Mathf.Round(spawnPos.z);//hit.point.z); ; ;

            if(PosX != LastPosX || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosZ = PosZ;
                ObjToMove.transform.position = new Vector3(PosX, LastPosY, PosZ);
                rend.material = matGrid;
            }

            if(Input.GetMouseButtonDown(0))
            {
                foreach (Transform child in Field.transform)
                {
                    if (child.position == ObjToMove.transform.position)
                    {
                        occuped = true;
                        break;
                    }
                }
                if (!occuped)
                    Instantiate(ObjToPlace, ObjToMove.transform.position, Quaternion.identity, Field.transform);
                else
                    occuped = false;
              //  Destroy(gameObject);
               // rend.material = matDefault;
            }
       // }
    }
}
