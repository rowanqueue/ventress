using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    public char note;
    public float timePlayed;
    public Transform origin;
    public Sound(char note, float time,Transform origin)
    {
        this.note = note;
        this.timePlayed = time;
        this.origin = origin;
    }
}
