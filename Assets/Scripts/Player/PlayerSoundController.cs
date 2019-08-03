using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SoundMaker))]
public class PlayerSoundController : MonoBehaviour
{
    public bool makingSound;
    SoundMaker sm;
    private void Awake()
    {
        sm = GetComponent<SoundMaker>();
    }
    // Update is called once per frame
    void Update()
    {
        makingSound = Input.GetMouseButton(0);
        if (makingSound)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                sm.MakeSound(0);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                sm.MakeSound(1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                sm.MakeSound(2);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                sm.MakeSound(3);
            }

        }
    }
}
