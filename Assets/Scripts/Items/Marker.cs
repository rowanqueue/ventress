using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//marks a spot for an enemy to come, then goes back to you
public class Marker : Item
{
    bool marked;
    public int mode;
    //0: just go there //purple
    //1: destroy!! //red
    //2: 
    public Creature creature;
    public List<Material> materials;
    ItemHandler ih;
    // Start is called before the first frame update
    public override void Awake()
    {
        ih = ItemHandler.me;
        base.Awake();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (thrown && !creature.hasTarget)
        {
            if (collision.gameObject.CompareTag("Item"))
            {

            }
            else
            {
                creature.SetTarget(collision.gameObject,transform.position,mode);
                thrown = false;
                marked = true;
                ih = ItemHandler.me;
            }
        }
    }
    private void Update()
    {
        if (marked)
        {
            transform.LookAt(ih.transform.position);
            transform.position += transform.forward * Time.deltaTime * 5f;
            if (Vector3.Distance(ih.transform.position, transform.position) < 1f)
            {
                marked = false;
                ih.PickUpItem(this);
            }
        }
        if (held)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                mode += 1;
                mode %= 2;
                mr.material = materials[mode];
            }
        }
    }
}
