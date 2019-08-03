using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//a test creature for testing out sounds
public class Creature : MonoBehaviour
{

    public List<Sound> lastHeardSounds;
    public string heardNotes;

    //handle pathfinding
    public int pathState = 0;//0: wander, 1: go
    float nextDecision;
    public float brainTime = 2f;
    public int wanderState = 0;//0: stay near, 1: go far
    AIDestinationSetter goAi;
    WanderingDestinationSetter wanderAI;
    AIPath ai;

    float whenJoined; //when started following
    float followTime = 5f;

    //testing!
    RimColorInfo rci;
    TextMesh text;

    //test jump
    float jumpForce = 5;
    Rigidbody rb;
    int jumpTimeOffset;

    //test soundmaking
    SoundMaker sm;
    // Start is called before the first frame update
    void Start()
    {
        goAi = GetComponent<AIDestinationSetter>();
        wanderAI = GetComponent<WanderingDestinationSetter>();
        ai = GetComponent<AIPath>();
        lastHeardSounds = new List<Sound>();
        nextDecision = Random.Range(0, brainTime);
        //testing!
        rci = GetComponent<RimColorInfo>();
        rb = GetComponent<Rigidbody>();
        jumpTimeOffset = Random.Range(0, 9);
        text = transform.GetChild(0).GetComponent<TextMesh>();
        //test soundmaking
        sm = GetComponent<SoundMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        //test soundmaking
        if(Random.value < 0.01f)
        {
            sm.MakeSound(0);
        }
        text.text = heardNotes;
        //deal with pathfinding
        if (Mathf.Round(Time.time * 60f) % 10 == jumpTimeOffset)//every 5 frames??
        {
            Jump();
        }
        switch (pathState)
        {
            case 0://wandering
                wanderAI.enabled = true;
                goAi.enabled = false;
                if (nextDecision <= Time.time && (wanderAI.ready || nextDecision + brainTime <= Time.time))//ready to move and time for the next decision
                {
                    float r = Random.value;
                    if (r < 0.25f)//go far
                    {
                        wanderState = 1;
                    }
                    else//go near
                    {
                        wanderState = 0;
                    }
                    nextDecision = Time.time + brainTime;
                }
                if ((wanderAI.ready || nextDecision + brainTime <= Time.time) && wanderState != -1)//ready to move!!
                {
                    float rmod = 0;
                    if(wanderState == 0)//go near
                    {
                        ai.maxSpeed = 0.5f;
                        rmod = Random.Range(-0.2f, 0.3f);
                        rmod = Mathf.Clamp(rmod,0f, 0.3f);
                    }else if(wanderState == 1)//go far
                    {
                        ai.maxSpeed = 2f;
                        rmod = Random.Range(0.75f, 1.25f);
                    }
                    wanderAI.NewPoint(rmod);
                }
                rci.SetValue(wanderState);
                break;
            case 1://going
                rci.SetValue(2);
                ai.maxSpeed = 2f;
                wanderState = -1;
                wanderAI.enabled = false;
                goAi.enabled = true;
                if(Time.time > whenJoined + followTime && Vector3.Distance(goAi.target.position,transform.position) > 5f)
                {
                    SetPathState(0);
                }
                break;
        }
        //deal with sounds
        if(lastHeardSounds.Count > 0)
        {
            //forget old notes!
            if(Time.time > lastHeardSounds[0].timePlayed + 2f)
            {
                Debug.Log("Forgot "+lastHeardSounds[0].note);
                lastHeardSounds.Remove(lastHeardSounds[0]);
                heardNotes = heardNotes.Substring(1);
            }
            //interpret notes!!
            if(pathState != 1 && heardNotes.Contains("wass"))
            {
                SetPathState(1);
                whenJoined = Time.time;
            }

        }
        
    }
    public void Hear(Sound sound)
    {
        lastHeardSounds.Add(sound);
        heardNotes += sound.note;
        Debug.Log(sound.note);
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
    public void SetPathState(int i)
    {
        pathState = i;
    }
    public void Jump()
    {
        bool isGrounded = false;
        if (Physics.Raycast(transform.position, Vector3.down, 0.2f)) //&& Mathf.Abs(rb.velocity.y) < 1f)
        {
            isGrounded = true;
        }
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * (jumpForce*Random.Range(0.3f,0.5f)), ForceMode.Impulse);
        }
    }
}
