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
    public Text ticker;
    public Color textColor;
    CanvasGroup group;

    //testing

    // Start is called before the first frame update
    void Start()
    {
        ticker.text = null;
        textColor = new Color(254, 207, 255);
        group = gameObject.GetComponent<CanvasGroup>();
        group.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
           // keys.SetActive(true);
            if (Input.GetKeyDown(KeyCode.W))
            {
                letter_w.color = Color.cyan;
                ticker.text += "w";
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                letter_w.color = textColor;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                letter_a.color = Color.cyan;
                ticker.text += "a";
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                letter_a.color = textColor;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                letter_s.color = Color.cyan;
                ticker.text += "s";
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                letter_s.color = textColor;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                letter_d.color = Color.cyan;
                ticker.text += "d";
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                letter_d.color = textColor;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ticker.text += " ";
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Coroutines.DoOverEasedTime(0.05f, Easing.Linear, t =>
            {
                group.alpha = Mathf.Lerp(0, 1, t);
            }));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(Coroutines.DoOverEasedTime(0.2f, Easing.Linear, t =>
            {
                 group.alpha = Mathf.Lerp(1, -0.2f, t);
            }));
            StartCoroutine(KillAlpha());
			//keys.SetActive(false);
			ticker.text = null;
            letter_w.color = textColor;
            letter_a.color = textColor;
            letter_s.color = textColor;
            letter_d.color = textColor;
        }
    }

    IEnumerator KillAlpha()
    {
        yield return new WaitForSeconds(.2f);
        if (!Input.GetMouseButton(0))
        {
            group.alpha = 0;
        }
    }
}
