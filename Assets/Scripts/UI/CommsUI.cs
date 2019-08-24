using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommsUI : MonoBehaviour
{

    public Image letter_w;
    public Image letter_a;
    public Image letter_s;
    public Image letter_d;
    public Image bg;
    public GameObject keys;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            keys.SetActive(true);
            if (Input.GetKey(KeyCode.W))
            {
                letter_w.color = Color.cyan;
            }
            else
            {
                letter_w.color = Color.white;
            }
            if (Input.GetKey(KeyCode.A))
            {
                letter_a.color = Color.cyan;
            }
            else
            {
                letter_a.color = Color.white;
            }
            if (Input.GetKey(KeyCode.S))
            {
                letter_s.color = Color.cyan;
            }
            else
            {
                letter_s.color = Color.white;
            }
            if (Input.GetKey(KeyCode.D))
            {
                letter_d.color = Color.cyan;
            }
            else
            {
                letter_d.color = Color.white;
            }
        }
        else
        {
            keys.SetActive(false);
            letter_w.color = Color.white;
            letter_a.color = Color.white;
            letter_s.color = Color.white;
            letter_d.color = Color.white;
        }
    }
}
