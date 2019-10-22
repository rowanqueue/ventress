using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tribe : MonoBehaviour
{
    public List<Creature> members;
    public List<string> names;
    public Transform shelter;
    public CreatureMind chief;

    [Range(0f, 1f)]
    public float needToFollowPercent = 1f;//how many of the tribe has to follow the chief for them to behappy
    public List<Transform> rivals;
    // Start is called before the first frame update
    void Awake()
    {
        members = new List<Creature>();
        foreach(Transform child in transform)
        {
            if (child.gameObject.activeSelf && child.CompareTag("Creature"))
            {
                members.Add(child.GetComponent<Creature>());
            }
        }
        names = new List<string>();
        foreach(Creature member in members)
        {
            //member.tribe = this;
            //tribe is going to name the creatures
            member.name = RandomName(names);
            names.Add(member.name);
        }
    }
    private void Start()
    {
        FindChief();
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
        Creature tempChief = members[0];
        for (int i = 1; i < members.Count; i++)
        {
            /*if(members[i].Size > tempChief.Size)
            {
                tempChief = members[i];
            }*/
        }
        tempChief.gameObject.AddComponent<CreatureMind>();
        chief = tempChief.gameObject.GetComponent<CreatureMind>();
        chief.tribe = this;

    }
    public bool TribeFollowingChief()//is the tribe following the chief?
    {
        int numWeNeedToStop = (int)(members.Count * needToFollowPercent);
        foreach(Creature member in members)
        {
            if(member == chief.creature)
            {
                numWeNeedToStop -= 1;
            }
            /*if (member.IsFollowingChief)
            {
                numWeNeedToStop -= 1;
            }*/
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
