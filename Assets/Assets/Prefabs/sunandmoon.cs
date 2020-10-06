using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunandmoon : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private Transform rotatearound;
    [SerializeField]
    private Transform lookat;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(rotatearound.position, Vector3.right, speed * Time.deltaTime);
        transform.LookAt(lookat.position);
    }
}

