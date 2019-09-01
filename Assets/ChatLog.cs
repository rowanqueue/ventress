using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatLog : MonoBehaviour
{
    static public ChatLog instance;
    public Text chatText;

    // Start is called before the first frame update
    void Awake()
    {
        chatText.text = null;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeMessage(string message)
    {
        chatText.text += message;
    }
}
