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
    public CreatureMind simonSayee;
    List<Creature> creatures;
    AudioSource audio;
    //sound notes w,a,s,d
    List<char> notes = new List<char> { 'w', 'a', 's', 'd' };
    public List<AudioClip> sounds = new List<AudioClip>();
    bool playingNotes;
    Command cmd;
    float nextNoteTime;
    int nextNoteToPlay;
    int index;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (playingNotes)
        {
            if(Time.time > nextNoteTime)
            {
                audio.PlayOneShot(sounds[nextNoteToPlay]);
                index++;
                if (index >= cmd.plain.Length)
                {
                    playingNotes = false;
                    index = 0;
                }
                else
                {
                    char note = cmd.plain[index];
                    if(note == ' ') { index++; note = cmd.plain[index]; }
                    int i = 0;
                    for (; i < notes.Count; i++)
                    {
                        if(notes[i] == note)
                        {
                            nextNoteToPlay = i;
                            break;
                        }
                    }
                    nextNoteTime = Time.time + 0.5f;
                }
            }
        }
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
    public void MakeSound(Command command)
    {
        creatures = DetectNearbyCreatures();
        AffectCreatures(command);
        //play sounds here
        if (command.plain.Length > 1)
        {
            this.cmd = command;
            index = 0;
            playingNotes = true;
            char note = cmd.plain[index];
            if (note == ' ') { index++; note = cmd.plain[index]; }
            int i = 0;
            for (; i < notes.Count; i++)
            {
                if (notes[i] == note)
                {
                    nextNoteToPlay = i;
                    break;
                }
            }
            nextNoteTime = Time.time + 0.5f;
        }
    }
    public void SetSimon(CreatureMind creature)
    {
        simonSayer = true;
        timeWhenSimonDies = Time.time + simonTimeCheck;
        simonSayee = creature;
    }
}
