using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//this trigger goes off when the item it is attached to is picked up
[RequireComponent(typeof(Item))]
public class PickUpTrigger : MonoBehaviour
{
    public UnityEvent myEvent;
    Item item;
    private void Awake()
    {
        item = GetComponent<Item>();
    }
    private void Update()
    {
        if (item.held)
        {
            myEvent.Invoke();
            Destroy(this);
        }
    }
}
