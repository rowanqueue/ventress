using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDDict : MonoBehaviour
{
    public Dictionary<Verb, string> dict_verbs = new Dictionary<Verb, string>();
    public Dictionary<Noun, string> dict_nouns = new Dictionary<Noun, string>();
    public GameObject entry;
    public Text entryText;
    public Button verbsButton;
    public Button nounsButton;
    public Button adjButton;

    // Start is called before the first frame update

    void Start()
    {
        entryText = entry.GetComponent<Text>();
        dict_verbs.Add(Verb.Move, "wa");
        dict_verbs.Add(Verb.Scatter, "ws");
        dict_nouns.Add(Noun.Me, "a");
        dict_nouns.Add(Noun.You, "s");
        entryText.text = null;
        //ShowVerbs();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowVerbs()
    {
        Debug.Log("ShowVerbs");
        entryText.text = null;
        entryText.text = Language.ShowVerbs();

        verbsButton.interactable = false;
        nounsButton.interactable = true;
    }
    public void ShowNouns()
    {
        Debug.Log("ShowNouns");
        entryText.text = null;
        entryText.text = Language.ShowNouns();
        verbsButton.interactable = true;
        nounsButton.interactable = false;
    }
}
