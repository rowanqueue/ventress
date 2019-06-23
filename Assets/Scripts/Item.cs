using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool held;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Collider collider;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    public void PickUp()
    {
        rb.isKinematic = true;
        collider.isTrigger = true;
        held = true;
    }
    public void PutDown()
    {
        rb.isKinematic = false;
        collider.isTrigger = false;
        held = false;
    }
}
