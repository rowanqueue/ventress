﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//how the player makes sounds
public class SoundMaker : MonoBehaviour
{
 // public sta SoundMaker me;
    public float noteLength;//how long does a creature remember a sound?
    List<Creature> creatures;
    //sound notes w,a,s,d
    List<char> notes = new List<char> { 'w', 'a', 's', 'd' };
    List<Creature> DetectNearbyCreatures()
    {
        List<Creature> creatures = new List<Creature>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
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
    void AffectCreatures(char note)
    {
        Sound sound = new Sound(note, Time.time,transform);
        foreach(Creature creature in creatures)
        {
            creature.Hear(sound);
        }
    }
    public void MakeSound(int i)
    {
        char whatNote = notes[i];
        creatures = DetectNearbyCreatures();
        AffectCreatures(whatNote);
    }
}