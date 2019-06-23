using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//put this on an object you want to respawn if it falls off the map
[RequireComponent(typeof(Rigidbody))]
public class FallRespawn : MonoBehaviour
{
    Vector3 originalPosition;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -50f)
        {
            transform.position = originalPosition;
            rb.velocity = Vector3.zero;
        }
    }
}
