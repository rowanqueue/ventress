using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemTrait
{
    Dull,Shiny
}

public class Item : MonoBehaviour
{
    public bool held;
    public bool thrown;//some items are activated when thrown
    public ItemTrait trait;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Collider collider;
    [HideInInspector]
    public MeshRenderer mr;
    // Start is called before the first frame update
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();
    }
    public virtual void PickUp()
    {
        TurnOff();
        held = true;
        thrown = false;
    }
    public void PutDown()
    {
        TurnOn();
        held = false;
    }
    public void Throw()
    {
        thrown = true;
        PutDown();
    }
    public virtual void TurnOn()
    {
        rb.isKinematic = false;
        collider.enabled = true;
    }
    public virtual void TurnOff()
    {
        rb.isKinematic = true;
        collider.enabled = false;
    }
}
