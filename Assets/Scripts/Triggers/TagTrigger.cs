using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//this item will start a trigger if it notices an item inside
[RequireComponent(typeof(Collider))]
public class TagTrigger : MonoBehaviour
{
    public UnityEvent myEvent;
    public string tag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(tag))
        {
            myEvent.Invoke();
        }
    }
}
