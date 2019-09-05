using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//how the player makes sounds
public class SoundMaker : MonoBehaviour
{
 // public sta SoundMaker me;
    public float noteLength;//how long does a creature remember a sound?
    public float noteDistance = 5f;
    public bool simonSayer;
    public float timeWhenSimonDies;
    public float simonTimeCheck = 2f;
    [HideInInspector]
    public Creature simonSayee;
    List<Creature> creatures;
    AudioSource audio;
    //sound notes w,a,s,d
    List<char> notes = new List<char> { 'w', 'a', 's', 'd' };
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    List<Creature> DetectNearbyCreatures()
    {
        List<Creature> creatures = new List<Creature>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, noteDistance);
        foreach(Collider c in hitColliders)
        {
            if(c.transform == transform)
            {
                continue;
            }
            if (c.CompareTag("Creature"))
            {
                creatures.Add(c.GetComponent<Creature>());
            }
        }
        return creatures;
    }
    void AffectCreatures(Command cmd)
    {
        foreach(Creature creature in creatures)
        {
            creature.Hear(cmd);
            if(cmd.verb == Verb.Sing && cmd.noun == Noun.You)//REMEMBER THIS RN ONLY ONE CAN HEAR YOU SING AT A TIME
            {
                break;
            }
        }
    }
    public void MakeSound(Command cmd)
    {
        creatures = DetectNearbyCreatures();
        AffectCreatures(cmd);
    }
    public void SetSimon(Creature creature)
    {
        simonSayer = true;
        timeWhenSimonDies = Time.time + simonTimeCheck;
        simonSayee = creature;
    }
}
