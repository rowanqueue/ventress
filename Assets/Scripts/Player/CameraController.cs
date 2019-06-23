using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool cursorLocked = true;
    public float minX = -60f;
    public float maxX = 60f;
    public float minY = -360f;
    public float maxY = 360f;
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    Camera cam;
    float rotationX = 0f;
    float rotationY = 0f;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        offset = cam.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorLocked)
        {
            rotationX += Input.GetAxis("Mouse Y") * sensitivityX;
            rotationY += Input.GetAxis("Mouse X") * sensitivityY;
            rotationX = Mathf.Clamp(rotationX, minX, maxX);
        }

        transform.localEulerAngles = new Vector3(0, rotationY, 0);
        cam.transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);

        cam.transform.position = transform.position + offset;

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorLocked = false;
        }
        if(Input.GetMouseButtonDown(0) && !cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLocked = true;
        }
    }
}
