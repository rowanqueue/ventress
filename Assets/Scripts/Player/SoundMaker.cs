using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//how the player makes sounds
public class SoundMaker : MonoBehaviour
{
    public static SoundMaker me;
    public bool makingSound;
    public float noteLength;//how long does a creature remember a sound?
    List<Creature> creatures;
    //sound notes w,a,s,d
    // Start is called before the first frame update
    void Awake()
    {
        me = this;
    }

    // Update is called once per frame
    void Update()
    {
        makingSound = Input.GetMouseButton(0);
        if (makingSound)
        {
            char whatNote = ' ';
            if (Input.GetKeyDown(KeyCode.W))
            {
                whatNote = 'w';
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                whatNote = 'a';
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                whatNote = 's';
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                whatNote = 'd';
            }
            if(whatNote != ' ')
            {
                creatures = DetectNearbyCreatures();
                AffectCreatures(whatNote);
            }
        }
    }
    List<Creature> DetectNearbyCreatures()
    {
        List<Creature> creatures = new List<Creature>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach(Collider c in hitColliders)
        {
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

}
