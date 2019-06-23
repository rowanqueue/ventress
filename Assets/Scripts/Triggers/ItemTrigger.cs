using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//this item will start a trigger if it notices an item inside
[RequireComponent(typeof(Collider))]
public class ItemTrigger : MonoBehaviour
{
    public UnityEvent myEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Item"))
        {
            myEvent.Invoke();
        }
    }
}
