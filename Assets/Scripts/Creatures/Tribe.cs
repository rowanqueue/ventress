using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tribe : MonoBehaviour
{
    public List<CreatureMind> members;
    public Transform shelter;
    // Start is called before the first frame update
    void Awake()
    {
        members = new List<CreatureMind>();
        foreach(Transform child in transform)
        {
            if (child.CompareTag("Creature"))
            {
                members.Add(child.GetComponent<CreatureMind>());
            }
        }
        foreach(CreatureMind member in members)
        {
            member.tribe = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
