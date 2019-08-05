using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//throw this item and it creates a platform where it lands
public class DeployablePlatform : Item
{
    public bool madePlatform;
    public GameObject whatImStuckTo;
    bool stuckToOtherPlatform;
    DeployablePlatform other;
    GameObject platform;
    // Update is called once per frame
    public override void Awake()
    {
        base.Awake();
        platform = transform.GetChild(0).gameObject;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!madePlatform && thrown)
        {
            bool youCanStick = true;
            if (collision.gameObject.CompareTag("Item"))
            {
                youCanStick = false;
                Item item = collision.gameObject.GetComponent<Item>();
                if (item.GetType() == typeof(DeployablePlatform))
                {
                    DeployablePlatform platform = (DeployablePlatform)item;
                    if (platform.madePlatform)
                    {
                        youCanStick = true;
                        stuckToOtherPlatform = true;
                        other = platform;
                    }
                    else
                    {
                        youCanStick = false;
                    }
                }
            }
            if (youCanStick)
            {
                whatImStuckTo = collision.gameObject;
                Debug.Log("MAKE PLATFORM");
                madePlatform = true;
                //turn on platform
                platform.SetActive(true);
                platform.transform.eulerAngles = Vector3.zero;
                //turn off item
                rb.isKinematic = true;
                mr.enabled = false;
                collider.enabled = false;
            }
        }
        else
        {
            //madePlatform = true;
        }

    }
    public void Update()
    {
        if (madePlatform)
        {
            if (!whatImStuckTo.activeSelf)
            {
                whatImStuckTo = null;
                TurnOn();
                thrown = false;
            }
            else
            {
                if (stuckToOtherPlatform)
                {
                    if (!other.madePlatform)
                    {
                        whatImStuckTo = null;
                        stuckToOtherPlatform = false;
                        other = null;
                        TurnOn();
                        thrown = false;
                    }
                }
            }
        }
    }
    public override void PickUp()
    {
        whatImStuckTo = null;
        madePlatform = false;
        //turn off platform
        platform.SetActive(false);
        //turn on item
        mr.enabled = true;
        base.PickUp();
    }
    public override void TurnOn()
    {
        madePlatform = false;
        platform.SetActive(false);
        mr.enabled = true;
        rb.isKinematic = false;
        collider.enabled = true;
        Debug.Log("A");
    }
}
