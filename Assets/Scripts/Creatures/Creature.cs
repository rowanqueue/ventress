using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//what the creature does!!
public class Creature : MonoBehaviour
{
    CreatureMind mind;

    //handle pathfinding
    public int pathState = 0;//0: wander, 1: go
    float nextDecision;
    public float brainTime = 2f;
    public int wanderState = 0;//0: stay near, 1: go far
    AIDestinationSetter goAi;
    public Transform Target
    {
        get { return target; }
        set
        {
            target = value;
            goAi.target = target;
        }
    }
    public Transform target;
    WanderingDestinationSetter wanderAI;
    RichAI ai;

    float whenJoined; //when started following
    float followTime = 5f;

    //testing!
    TextMesh text;
    float whenSpoke;
    float howLongShowSpeak = 2f;

    //test jump
    float jumpForce = 5;
    Rigidbody rb;
    int jumpTimeOffset;

    //test soundmaking
    SoundMaker sm;
    // Start is called before the first frame update
    void Start()
    {
        mind = GetComponent<CreatureMind>();
        goAi = GetComponent<AIDestinationSetter>();
        wanderAI = GetComponent<WanderingDestinationSetter>();
        ai = GetComponent<RichAI>();
        nextDecision = Random.Range(0, brainTime);
        //testing!
        rb = GetComponent<Rigidbody>();
        jumpTimeOffset = Random.Range(0, 9);
        text = transform.GetChild(0).GetComponent<TextMesh>();
        text.text = "";
        //test soundmaking
        sm = GetComponent<SoundMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        //deal with showing speak
        if(Time.time > whenSpoke + howLongShowSpeak)
        {
            text.text = "";
        }
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
                break;
            case 1://going
                ai.maxSpeed = 2f;
                wanderState = -1;
                wanderAI.enabled = false;
                goAi.enabled = true;
                /*if(Time.time > whenJoined + followTime && Vector3.Distance(goAi.target.position,transform.position) > 5f)
                {
                    SetPathState(0);
                }*/
                if (goAi.ready && mind.action == "eat")
                {
                    mind.Eat();
                    SetPathState(0);
                    mind.Decide();
                }
                if(goAi.ready && mind.action == "shelter")
                {
                    //write a path state that lets them wander BUT keeps them within a certain range
                }
                if(goAi.ready && mind.action == "play")
                {
                    //write a path state for playing
                }
                break;
        }
        
    }
    public void Hear(Command cmd)//this is where the creature interprets the command!!
    {
        switch (cmd.verb)
        {
            case Verb.Move:
                switch (cmd.noun)
                {
                    case Noun.Me:
                        Target = cmd.speaker;
                        SetPathState(1);
                        break;
                    case Noun.You:
                        Target = transform;
                        SetPathState(1);
                        break;
                }
                break;
            case Verb.Scatter://does it really matter who they say to stop following???
                SetPathState(0);
                Target = null;
                break;
        }
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
    public void SetAction(string action, Transform target)
    {
        //rn going to make it so actions dont take over the pathstate tho
        if(pathState == 0)
        {
            if (action == "eat")
            {
                SetPathState(1);
                goAi.target = target;
            }
            if (action == "shelter")
            {
                SetPathState(1);
                goAi.target = target;
            }
            if (action == "play")
            {
                SetPathState(1);
                goAi.target = target;
            }
        }
    }
    public void Speak(string message)
    {
        Language.TakeMessage(message, sm);
        text.text = message;
        whenSpoke = Time.time;
    }
}
