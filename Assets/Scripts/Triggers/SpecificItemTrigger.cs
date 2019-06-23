using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//item has to have the right name
[RequireComponent(typeof(Collider))]
public class SpecificItemTrigger : ItemTrigger
{
    public string itemName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Item") && other.name.Equals(itemName))
        {
            Item item = other.GetComponent<Item>();
            OneTimeChild otc = item.GetComponent<OneTimeChild>();
            if(otc == null)
            {
                otc = item.gameObject.AddComponent<OneTimeChild>();
                otc.newParent = transform;
            }
            myEvent.AddListener(otc.Trigger);
            if (item.held)
            {
                ItemHandler.me.DropItem();
            }
            item.rb.isKinematic = true;
            item.collider.enabled = false;
            myEvent.Invoke();
        }
    }
}
