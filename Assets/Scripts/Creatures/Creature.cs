using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//what the creature does!!
public class Creature : MonoBehaviour
{
    [HideInInspector]
    public CreatureMind mind;

    //handle pathfinding
    public int pathState = 0;//0: wander, 1: go
    float nextDecision;
    public float brainTime = 2f;
    public int wanderState = 0;//0: stay near, 1: go far
    public float maxSpeed;
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
    public SoundMaker sm;
    ItemHandler ih;
    public Health health;
    [HideInInspector]
    public Item itemToBePickedUp;
    public bool simon;
    public int simonlevel;
    public Transform simonSayer;
    public List<SoundMaker> friends;
    public List<Transform> rivals;
    List<ParticleSystem> effects;
    enum Effect { Frustrated,Confused,Accepted,Friendship,Following};
    ChatLog chatLog;
    // Start is called before the first frame update
    void Awake()
    {
        mind = GetComponent<CreatureMind>();
        goAi = GetComponent<AIDestinationSetter>();
        wanderAI = GetComponent<WanderingDestinationSetter>();
        ai = GetComponent<RichAI>();
        maxSpeed = ai.maxSpeed;
        nextDecision = Random.Range(0, brainTime);
        //testing!
        rb = GetComponent<Rigidbody>();
        jumpTimeOffset = Random.Range(0, 9);
        text = transform.GetChild(0).GetComponent<TextMesh>();
        text.text = "";
        //test soundmaking
        sm = GetComponent<SoundMaker>();
        ih = gameObject.AddComponent<ItemHandler>();
        health = gameObject.AddComponent<Health>();
        effects = new List<ParticleSystem>();
        chatLog = ChatLog.instance;
        foreach(Transform child in transform)
        {
            ParticleSystem s = child.GetComponent<ParticleSystem>();
            if (s)
            {
                effects.Add(s);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //text looks at you
        //transform.LookAt(2 * transform.position - lookingAt.position);

        text.transform.LookAt(2 * transform.position - PlayerController.instance.gameObject.transform.position);
        if (simon)
        {
            transform.LookAt(simonSayer);
        }
        //deal with showing speak
        if (Time.time > whenSpoke + howLongShowSpeak)
        {
            text.text = "";
        }
        if (ih.holdingItem)
        {
            ih.HoldItem(true);
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
                    if (wanderState == 0)//go near
                    {
                        ai.maxSpeed = 0.5f * maxSpeed;
                        rmod = Random.Range(-0.2f, 0.3f);
                        rmod = Mathf.Clamp(rmod, 0f, 0.3f);
                    } else if (wanderState == 1)//go far
                    {
                        ai.maxSpeed = 1f * maxSpeed;
                        rmod = Random.Range(0.75f, 1.25f);
                    }
                    wanderAI.NewPoint(rmod);
                }
                break;
            case 1://going
                ai.maxSpeed = 1f * maxSpeed;
                wanderState = -1;
                wanderAI.enabled = false;
                goAi.enabled = true;
                if(Target == PlayerController.instance.transform)
                {
                    PlayEffect(Effect.Following);
                }
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
                if (goAi.ready && mind.action == "shelter")
                {
                    //write a path state that lets them wander BUT keeps them within a certain range
                    if(mind.shelter > 0.95f)
                    {
                        SetPathState(0);
                        mind.Decide();
                    }
                }
                if (goAi.ready && mind.action == "play")
                {
                    //write a path state for playing
                    mind.play = 1;
                    SetPathState(0);
                    mind.Decide();
                }
                if(mind.action == "attack")
                {
                    if(Vector3.Distance(transform.position,Target.position) < 2.5f)
                    {
                        //attack them
                        Target.GetComponent<Health>().Hurt();
                        mind.safety = 1;
                        SetPathState(0);
                        mind.Decide();
                    }
                    if(mind.safety < 0.6f * mind.bravery)
                    {
                        SetPathState(0);
                        mind.Decide();
                    }
                }
                if (itemToBePickedUp)
                {
                    if (itemToBePickedUp.held)
                    {
                        itemToBePickedUp = null;
                        SetPathState(0);
                        break;
                    }
                    if (Vector3.Distance(transform.position, Target.position) < 2.5f)
                    {
                        ih.PickUpItem(itemToBePickedUp);
                        itemToBePickedUp = null;
                        SetPathState(0);
                    }
                }
                break;
        }

    }
    void PlayEffect(Effect e)
    {
        effects[(int)e].Play();
    }
    public void Hear(Command cmd)//this is where the creature interprets the command!!
    {
        switch (cmd.verb)
        {
            case Verb.Default://wow you're playing simon says!!
                if (simon)
                {
                    string sofar = mind.name.Substring(0, simonlevel);
                    if (cmd.custom == sofar)
                    {
                        simonlevel += 1;
                        if (simonlevel > 5)
                        {
                            simon = false;
                            cmd.speaker.simonSayer = false;
                            simonlevel = 0;
                            Debug.Log("FRIENDS");
                            friends.Add(cmd.speaker);
                            PlayEffect(Effect.Friendship);
                            if (rivals.Contains(cmd.speaker.transform))
                            {
                                rivals.Remove(cmd.speaker.transform);
                            }
                        }
                        else
                        {
                            Speak(mind.name.Substring(0, simonlevel));
                        }
                    }
                    else
                    {
                        Speak(mind.name.Substring(0, simonlevel));
                    }
                }
                else
                {
                    PlayEffect(Effect.Confused);
                }
                break;
            case Verb.Move:
                switch (cmd.noun)
                {
                    case Noun.Me:
                        if (friends.Contains(cmd.speaker))
                        {
                            Target = cmd.speaker.transform;
                            SetPathState(1);
                            PlayEffect(Effect.Accepted);
                        }
                        break;
                    default:
                        if (cmd.custom != "")
                        {
                            Item item = ih.CheckNearby(this, FindTraitOfName(cmd.custom));
                            if (item)
                            {
                                Target = item.transform;
                                SetPathState(1);
                                PlayEffect(Effect.Accepted);
                            }
                        }
                        break;
                }
                break;
            case Verb.Scatter://does it really matter who they say to stop following???
                switch (cmd.noun)
                {
                    case Noun.Me:
                        if (Target == cmd.speaker.transform)
                        {
                            SetPathState(0);
                            PlayEffect(Effect.Accepted);
                        }
                        break;
                    default:
                        SetPathState(0);
                        break;
                }
                break;
            case Verb.Get:
                if (cmd.custom != "")
                {
                    Item item = ih.CheckNearby(this, FindTraitOfName(cmd.custom));
                    if (item)
                    {
                        itemToBePickedUp = item;
                        Target = item.transform;
                        SetPathState(1);
                        PlayEffect(Effect.Accepted);
                    }
                }
                break;
            case Verb.Put:
                if (ih.holdingItem)
                {
                    switch (cmd.noun)
                    {
                        case Noun.Me:
                            ih.GiveItem(PlayerController.instance.ih);
                            PlayEffect(Effect.Accepted);
                            break;
                        default:
                            ih.DropItem();
                                                        PlayEffect(Effect.Accepted);
                            break;
                    }
                }
                break;
            case Verb.What:
                switch (cmd.noun)
                {
                    case Noun.Me:
                        Speak("wasda");
                        break;
                    case Noun.You:
                        Speak("das");
                        break;
                    default:
                        if (cmd.subject)
                        {
                            Speak(FindNameOfItem(cmd.subject));
                        }
                        break;
                }
                break;
            case Verb.Sing:
                switch (cmd.noun)
                {
                    case Noun.You:
                        if (!friends.Contains(cmd.speaker))
                        {
                            simon = true;
                            simonlevel = 1;
                            cmd.speaker.SetSimon(this);
                            simonSayer = cmd.speaker.transform;
                            Speak(mind.name.Substring(0, simonlevel));
                        }
                        else
                        {
                            Speak(mind.name);
                        }
                        break;
                    default:
                        if (cmd.custom != "")
                        {
                            if (cmd.custom == "ass")
                            {
                                Speak(mind.tribe.chief.name);
                            }
                        }
                        break;
                }
                break;
        }
    }
    public string FindNameOfItem(Transform subject)
    {
        string n = "";
        if (subject.CompareTag("Food"))
        {
            Item item = subject.GetComponent<Item>();
            if (item.trait == ItemTrait.Shiny)
            {
                n = "aaa";
            }
            else
            {
                n = "sss";
            }
        }
        return n;
    }
    public ItemTrait FindTraitOfName(string name)
    {
        if (name == "aaa")
        {
            return ItemTrait.Shiny;
        }
        else
        {
            return ItemTrait.Dull;
        }
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
    public void SetPathState(int i)
    {
        pathState = i;
        if (i == 0)
        {
            Target = null;
        }
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
            rb.AddForce(Vector3.up * (jumpForce * Random.Range(0.3f, 0.5f)), ForceMode.Impulse);
        }
    }
    public void SetAction(string action, Transform target)
    {
        //rn going to make it so actions dont take over the pathstate tho
        if (pathState == 0 || pathState == 1)
        {
            if (action == "eat")
            {
                SetPathState(1);
                Target = target;
            }
            if (action == "shelter")
            {
                SetPathState(1);
                Target = target;
            }
            if (action == "play")
            {
                SetPathState(1);
                Target = target;
            }
            if(action == "attack")
            {
                SetPathState(1);
                Target = target;
            }
        }
    }
    public void Speak(string message)
    {
        Language.TakeMessage(message, sm);
        text.text = message;
        whenSpoke = Time.time;
        chatLog.TakeMessage(mind.name +": " + message + "\n");
    }
}
