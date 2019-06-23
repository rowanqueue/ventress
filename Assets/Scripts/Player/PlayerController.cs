using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public states
    public bool running;

    //public data
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;

    //testing data
    public float verticalPos;
    public float horizontalPos;

    //situation data
    public GameObject itemHeld;


    //private data
    Rigidbody rb;
    Vector3 moveDirection;
    CapsuleCollider collider;
    Camera cam;
    Vector3 itemHeldOffset;
    Vector3 groundContactNormal = Vector3.up;//the slope of whatever you're standing on
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        float horizontal = 0;
        float vertical = 0;
        if (CanMove(transform.right * Input.GetAxisRaw("Horizontal")))
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        if (CanMove(transform.forward * Input.GetAxisRaw("Vertical")))
        {
            vertical = Input.GetAxisRaw("Vertical");
        }
        moveDirection = (horizontal * transform.right + vertical * transform.forward).normalized;
        running = Input.GetKey(KeyCode.LeftShift);
        //end movement
        //items
        if (Input.GetMouseButtonDown(0))
        {
            CheckInteraction();
        }
        if(itemHeld != null)
        {
            itemHeld.transform.position = transform.position + (transform.up*verticalPos + transform.right*horizontalPos + transform.forward).normalized;
            itemHeld.transform.forward = transform.forward;
        }
        //end items
    }
    private void FixedUpdate()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    void CheckInteraction()
    {
        float distance = 4f;
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, distance))
        {
            if(hit.transform.tag == "Item")
            {
                itemHeld = hit.transform.gameObject;
            }
        }
    }
    void Move()
    {
        Vector3 yVel = new Vector3(0, rb.velocity.y, 0);
        if (running)
        {
            rb.velocity = moveDirection * runSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = moveDirection * walkSpeed * Time.deltaTime;
        }
        rb.velocity += yVel;
    }
    void Jump()
    {
        if (isGrounded())
        {
            rb.velocity += new Vector3(0, jumpSpeed * Time.deltaTime, 0);
        }
    }
    bool CanMove(Vector3 direction)
    {
        float distanceToPoints = collider.height / 2 - collider.radius;
        Vector3 point1 = transform.position + collider.center + Vector3.up * distanceToPoints;
        Vector3 point2 = transform.position + collider.center - Vector3.up * distanceToPoints;
        float radius = collider.radius * 0.95f;
        float castDistance = 0.5f;
        RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, radius, direction, castDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Wall")
            {
                return false;
            }
        }
        return true;
    }
    bool isGrounded()
    {
        float distanceToPoints = collider.height / 2 - collider.radius;
        Vector3 point1 = transform.position + collider.center + Vector3.up * distanceToPoints;
        Vector3 point2 = transform.position + collider.center - Vector3.up * distanceToPoints;
        float castDistance = 0.1f;
        RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, collider.radius, transform.up*-1f, castDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Wall")
            {
                groundContactNormal = hit.normal;
                return true;
            }
        }
        groundContactNormal = Vector3.up;
        return false;
    }
}
