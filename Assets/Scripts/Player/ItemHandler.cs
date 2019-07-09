using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    //global data
    static public ItemHandler me;
    //situational data
    public bool holdingItem;
    public float grabRange;
    Item itemHeld;
    float whenDropped;//so you don't pick up the same thing instantly after

    //testing data
    public float upHelp;

    //private data
    Camera cam;
    Vector3 itemHeldOffset;

    //private values
    float vPos = 0.5f;
    float hPos = 1;
    public float throwForce = 30f;
    // Start is called before the first frame update
    void Awake()
    {
        me = this;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(holdingItem)
        {
            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Q))
            {
                Vector3 forceDirection = (cam.transform.forward + cam.transform.up * upHelp).normalized;
                Debug.DrawLine(itemHeld.transform.position, itemHeld.transform.position + forceDirection, Color.yellow);
                itemHeld.transform.position = transform.position + (cam.transform.up * vPos + cam.transform.right * 0f + cam.transform.forward).normalized;
                itemHeld.transform.forward = cam.transform.forward;
                if (Input.GetMouseButtonDown(0))//throw
                {
                    itemHeld.Throw();
                    holdingItem = false;
                    //Vector3 forceDirection = (cam.transform.forward + cam.transform.up * 0.5f).normalized;

                    itemHeld.rb.AddForce(forceDirection* throwForce, ForceMode.Impulse);
                    whenDropped = Time.time;
                }
            }
            else
            {
                itemHeld.transform.position = transform.position + (transform.up * vPos + transform.right * hPos + transform.forward).normalized;
                itemHeld.transform.forward = transform.forward;
                if (Input.GetMouseButtonDown(0))//drop
                {
                    DropItem();
                }
            }
        }
        else
        {
            if(itemHeld != null && Time.time > whenDropped+1f)
            {
                itemHeld = null;
            }
            if (Input.GetMouseButton(0) && itemHeld == null)
            {
                CheckInteraction();
            }
        }
    }
    public void DropItem()
    {
        itemHeld.PutDown();
        whenDropped = Time.time;
        holdingItem = false;
    }
    void CheckInteraction()
    {
        float distance = grabRange;
        RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, distance);
        foreach(RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Item")
            {
                PickUpItem(hit.transform.GetComponent<Item>());
                break;
            }
        }
    }
    public void PickUpItem(Item item)
    {
        itemHeld = item;
        item.PickUp();
        holdingItem = true;
    }
}
