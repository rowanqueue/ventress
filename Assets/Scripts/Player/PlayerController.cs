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
    public float talkSpeed = 0.1f; //multiplies run/walk speed while talking
    public float slowSpeed = 0.5f; //multiplies time.timeScale
    
    //testing data
    public float verticalPos;
    public float horizontalPos;
    public bool isSpeaking;
    private bool safeRelease; //true after comms exit lerping is complete
	private bool lookEnabled;

    //situation data
    public GameObject itemHeld;


    //private data
    Rigidbody rb;
    Vector3 moveDirection;
    CapsuleCollider collider;
    Camera cam;
    Vector3 itemHeldOffset;
    Vector3 groundContactNormal = Vector3.up;//the slope of whatever you're standing on
    LayerMask layerGround;
    MouseLook mouseLook;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        cam = Camera.main;
        layerGround = LayerMask.NameToLayer("Ground");
        //Cursor.lockState = CursorLockMode.None;
        mouseLook = cam.gameObject.GetComponent<MouseLook>();
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

        //disables mouselook when esc is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
		{
			mouseLook.enabled = false;
			lookEnabled = false;
		}
        if (Input.GetMouseButtonDown(0))
        {
            if (lookEnabled == false)
			{
				mouseLook.enabled = true;
			}
			//CheckInteraction();
			isSpeaking = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            StartCoroutine(Coroutines.DoOverEasedTime(0.1f, Easing.Linear, t =>
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 57, t);
            }));
        }
        if (Input.GetMouseButtonUp(0))
        {
            //CheckInteraction();
            isSpeaking = false;
            StartCoroutine(Coroutines.DoOverEasedTime(0.1f, Easing.Linear, t =>
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 47, t);
            }));
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSpeaking)
        {
            safeRelease = false;
            StartCoroutine(Coroutines.DoOverEasedTime(0.1f, Easing.Linear, t =>
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 50, t);
            }));
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isSpeaking)
        {
            StartCoroutine(Coroutines.DoOverEasedTime(0.1f, Easing.Linear, t =>
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 50, t);
            }));
        }
        if (itemHeld != null)
        {
            itemHeld.transform.position = transform.position + (transform.up * verticalPos + transform.right * horizontalPos + transform.forward).normalized;
            itemHeld.transform.forward = transform.forward;
        }
        //end items

        //time slow
        if (isSpeaking)
        {
            Time.timeScale = slowSpeed;
        }
        else
        {
            Time.timeScale = 1f;
        }
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
                //itemHeld = hit.transform.gameObject;
            }
        }
    }
    void Move()
    {

        Vector3 yVel = new Vector3(0, rb.velocity.y, 0);
        if (running)
        {
            if (isSpeaking)
            {
                rb.velocity = moveDirection * runSpeed * talkSpeed * Time.deltaTime;
            }
            else if (!isSpeaking)
            {
                rb.velocity = moveDirection * runSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (isSpeaking)
            {
                rb.velocity = moveDirection * walkSpeed * talkSpeed * Time.deltaTime;
            }
            else
            {
                rb.velocity = moveDirection * walkSpeed * Time.deltaTime;
            }
        }
        rb.velocity += yVel;
    }
    void Jump()
    {
        if (isGrounded())
        {
            rb.velocity += new Vector3(0, jumpSpeed * Time.deltaTime, 0);
            Debug.Log("Jumping");
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
        if (Input.GetMouseButton(0))
        {
            //return false;
        }
        
        return true;
    }
    public bool isGrounded()
    {
        float distanceToPoints = collider.height / 2 - collider.radius;
        Vector3 point1 = transform.position + collider.center + Vector3.up * distanceToPoints;
        Vector3 point2 = transform.position + collider.center - Vector3.up * distanceToPoints;
        float castDistance = 0.1f;
        RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, collider.radius, transform.up*-1f, castDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.layer == layerGround)
            {
                groundContactNormal = hit.normal;
                return true;
            }
        }
        groundContactNormal = Vector3.up;
        return false;
    }
}
