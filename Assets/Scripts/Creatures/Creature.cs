using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a test creature for testing out marker
public class Creature : MonoBehaviour
{
    public float speed;
    public bool hasTarget;
    public int targetGoal;//0: go,1: destroy
    Vector3 targetPos;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            transform.LookAt(targetPos);
            transform.position += transform.forward * Time.deltaTime * speed;
            if (Vector3.Distance(transform.position, targetPos) < 1f)
            {
                if(targetGoal == 1)//destroy it!
                {
                    if (target.CompareTag("Destructible"))
                    {
                        Destroy(target);
                    }
                }
                targetPos = Vector3.zero;
                hasTarget = false;
                targetGoal = 0;
                target = null;

            }
        }
    }
    public void SetTarget(GameObject target, Vector3 pos,int mode)
    {
        targetPos = pos;
        this.target = target;
        targetGoal = mode;
        hasTarget = true;
    }
}
