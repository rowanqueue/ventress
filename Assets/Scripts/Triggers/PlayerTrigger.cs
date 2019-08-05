using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent myEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
           //Debug.Log("A");
            myEvent.Invoke();
        }
    }
}
