using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tribe : MonoBehaviour
{
    public List<CreatureMind> members;
    public List<string> names;
    public Transform shelter;
    public CreatureMind chief;

    [Range(0f, 1f)]
    public float needToFollowPercent = 1f;//how many of the tribe has to follow the chief for them to behappy
    // Start is called before the first frame update
    void Awake()
    {
        members = new List<CreatureMind>();
        foreach(Transform child in transform)
        {
            if (child.gameObject.activeSelf && child.CompareTag("Creature"))
            {
                members.Add(child.GetComponent<CreatureMind>());
            }
        }
        names = new List<string>();
        foreach(CreatureMind member in members)
        {
            member.tribe = this;
            //tribe is going to name the creatures
            member.name = RandomName(names);
            names.Add(member.name);
        }
        FindChief();
    }
    private void Start()
    {
        //give them some friends
        foreach (CreatureMind member in members)
        {
            if (member != chief)
            {
                Debug.Log(chief.creature.sm);
                member.creature.friends.Add(chief.creature.sm);
            }
        }
    }
    string RandomName(List<string> names)//generate a name that isn't in the list
    {
        List<char> letters = new List<char> { 'w', 'a', 's', 'd' };
        string name = "";
        for(int i = 0; i < 5; i++)
        {
            name += letters[Random.Range(0, 4)];
        }
        if (!names.Contains(name))
        {
            return name;
        }
        else
        {
            return RandomName(names);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void FindChief()
    {
        CreatureMind tempChief = members[0];
        float biggestSize = tempChief.Size;
        for (int i = 1; i < members.Count; i++)
        {
            if(members[i].Size > biggestSize)
            {
                biggestSize = members[i].Size;
                tempChief = members[i];
            }
        }
        if (chief && chief != tempChief)//theres a new chief in town, old chief go away
        {
            chief.isChief = false;
        }
        chief = tempChief;
        chief.isChief = true;
    }
    public bool TribeFollowingChief()//is the tribe following the chief?
    {
        int numWeNeedToStop = (int)(members.Count * needToFollowPercent);
        foreach(CreatureMind member in members)
        {
            if(member == chief)
            {
                numWeNeedToStop -= 1;
            }
            if (member.IsFollowingChief)
            {
                numWeNeedToStop -= 1;
            }
        }
        if(numWeNeedToStop > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
