using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//start test of what the creature should think, largely for debugging rn
//what the creature thinks!!
public class CreatureMind : MonoBehaviour
{
    Creature creature;
    public string name;
    //stats
    public float Size
    {
        get { return size * currentBabyPercent; }//get the ACTUAL SIZE AT THIS MOMENT
    }
    float size;//1.0-2.0, babies start at 0.5 of their adult size and grow
    public float age;//0 is baby, 1 is adult, 3 is dead

    public float bravery;//these numbers are multiplied by 0.5 to generate whatever threshold
    public float weight;//determines its threshold for being hungry
    public float social;//determines threshold for shelter
    public float fun;//determines threshold for play

    //needs where 1 is satisfied, 0 is DEAD
    public float safety;
    public float hunger;
    public float shelter;
    public float play;

    //emotions we will figure out later
    public int emotion;//0 aggressive; 1 passive; 2 paranoid; 3 helpful

    //tribe
    public Tribe tribe;
    public bool isChief;
    public bool IsFollowingChief
    {
        get { return creature.pathState == 1 && creature.Target != null && creature.Target == tribe.chief.transform; }
    }

    //action
    public string action;

    //temporarilty important
    public float currentBabyPercent;

    //needed for mind
    float needsUpdateTime;
    float nextDecisionTime;
    float decisionPeriod = 5f;

    //datta about world
    float transformScale;
    public List<ItemTrait> likes;
    void Start()
    {
        creature = GetComponent<Creature>();
        //stats
        age = CustomRange(0f, 2f);
        size = CustomRange(1f, 2f);
        //needs
        safety = CustomRange(0f, 1f);
        hunger = CustomRange(0f, 1f);
        shelter = CustomRange(0f, 1f);
        play = CustomRange(0f, 1f);
        //emotions
        emotion = Random.Range(0, 4);

        transformScale = transform.localScale.x;
        float sizeEffect = Mathf.Lerp(0.5f * size, size, age) * transformScale;
        transform.localScale = new Vector3(sizeEffect, sizeEffect, sizeEffect);
    }
    float CustomRange(float min, float max)
    {
        float num = Random.Range(min, max);
        num = CustomRound(num);
        return num;
    }
    float CustomRound(float num)
    {
        num = Mathf.Round(num * 100f) / 100f;
        return num;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > needsUpdateTime)
        {
            needsUpdateTime = Time.time + 1;
            UpdateNeeds();
        }
        if(Time.time > nextDecisionTime)
        {
            Decide();
            nextDecisionTime = Time.time + decisionPeriod;
            //test screaming
            if (isChief)
            {
                if (!tribe.TribeFollowingChief())//tribe aint following the chief
                {
                    creature.Speak("wa a");//chief tells followers to follow them
                }
            }
        }
    }
    public void UpdateNeeds()//another tick in the needs
    {
        //age them!
        age += 0.01f;
        //if they're a baby, set their size
        if(age < 1f)
        {
            float sizeEffect = Mathf.Lerp(0.5f * size, size, age)*transformScale;
            transform.localScale = new Vector3(sizeEffect, sizeEffect, sizeEffect);
        }
        //safety
        //if unsafe, safety goes down
        //if safe, goes up
        //if below certain number, increase bravery
        //if above certain number, decrease bravery

        //hunger
        //if above certain number, increase weight
        if(hunger > weight)
        {
            weight += 0.01f;
        } else if(hunger < weight*0.25f)//if below certain other number, decrease weight
        {
            weight -= 0.01f;
        }
        //always tick down
        hunger -= 0.01f;

        //shelter
        //check if they're near their shelter
        if (isMyShelterNearby(4f))
        {
            shelter += 0.10f;
        }
        else
        {
            shelter -= 0.01f;
        }
        //if above certain number, increase social
        if(shelter > social)
        {
            social += 0.01f;
        }else if(shelter < social * 0.25f)
        {
            social -= 0.01f;
        }
        //if below certain number, decrease social

        //play
        //if they're not currently playing, tick down
        if(action == "play")
        {
            play += 0.10f;
        }
        else
        {
            play -= 0.01f;
        }
        //if they are, tick up
        //if above certain number, increase fun
        if(play > fun)
        {
            fun -= 0.01f;
        }
        else if(play < fun*0.25f)
        {
            fun += 0.01f;
        }
        //if below certain number, decrease fun

        //fix all the numbers
        age = CustomRound(age);
        hunger = CustomRound(hunger);
        shelter = CustomRound(shelter);
        play = CustomRound(play);
    }
    public void Eat()
    {
        hunger = 1.0f;
    }
    public void Decide()//based on current situation, what do
    {
        action = "";
        //needs
        /*if(safety < 0.5f*bravery)//maybe make this threshold based on something??
        {
            //IN DANGER
            if(emotion == 0)//AGGRESSIVE
            {
                action = "Fight back!";
            }
            else
            {
                action = "Run away!";
            }
            return;
        }*/
        if(hunger < 0.5f*weight)//these thresholds definitely need to be allowed to change
        {
            //HUNGRY
            Transform food = DetectNearbyTag("Food");
            bool isFoodNearby = food != transform;
            if (isFoodNearby)
            {
                action = "eat";
                creature.SetAction(action, food);
                return;
            }
        }
        if(shelter < 0.5f*social)
        {
            if (isMyShelterNearby(10f))
            {
                action = "shelter";
                creature.SetAction(action, tribe.shelter);
                return;
            }
            else if (shelter < 0.1f)//they're more desperate for shelter
            {
                //bool isAShelterNearby = Random.value < 0.5f;//maybe go to a different shelter
                //action = "Go to a different shelter";
                //creature.SetAction(action, tribe.shelter);//will make this actually work later
                //return;
            }
        }
        if(play < 0.5f*fun)
        {
            Transform nearbyCreature = DetectNearbyTag("Creature");
            bool isCreatureNearby = nearbyCreature != transform;
            if (isCreatureNearby)
            {
                action = "play";
                creature.SetAction(action, nearbyCreature);
                return;
            }
        }
        //needs satisfied, do tribe stuff or random
        action = "Work for tribe";
    }
    Transform DetectNearbyTag(string tag)//just returns one, not
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider c in hitColliders)
        {
            if (c.transform == transform)
            {
                continue;
            }
            if (c.CompareTag(tag) && c.transform != transform)
            {
                return c.transform;
            }
        }
        return transform;
    }
    bool isMyShelterNearby(float distance)
    {
        if(tribe == null)
        {
            return false;
        }
        bool isIt = false;
        if(Vector3.Distance(transform.position, tribe.shelter.position) < distance)
        {
            isIt = true;
        }
        return isIt;
    }
}
