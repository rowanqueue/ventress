using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;
using TMPro;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class TextLog : MonoBehaviour
{
     
    void Awake()
    {
        StartStory();
    }

    // Creates a new Story object with the compiled story which we can then play!
    void StartStory()
    {
        TextLogObj.SetActive(true);
        story = new Story(inkJSONAsset.text);
        RefreshView();
    }

    // This is the main function called every time the story changes. It does a few things:
    // Destroys all the old content and choices.
    // Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
    void RefreshView()
    {
        // Remove all the UI on screen
        //RemoveChildren();

        // Read all the content until we can't continue any more
        while (story.canContinue)
        {
            // Continue gets the next line of the story
            string text = story.Continue();
            // This removes any white space from the text.
            text = text.Trim();
            // Display the text on screen!
            CreateContentView(text);
        }

        // Display all the choices, if there are any!
        if (story.currentChoices.Count > 0)
        {
            if (story.currentChoices.Count == 1)
            {
                Choice choice = story.currentChoices[0];
                Button button = CreateChoiceView(choice.text.Trim(), 1);
                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate {
                    OnClickChoiceButton(choice);
                });
            }
            else if (story.currentChoices.Count == 2)
            {
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    Choice choice = story.currentChoices[i];
                    Button button = CreateChoiceView(choice.text.Trim(), 2);
                    // Tell the button what to do when we press it
                    button.onClick.AddListener(delegate {
                        OnClickChoiceButton(choice);
                    });
                }
            }
            
        }
        // If we've read all the content and there's no choices, the story is finished!
        else
        {
            ButtonTwo.SetActive(false);
            ButtonOne.gameObject.SetActive(true);
            Button button = CreateChoiceView("Exit", 1);
            // Tell the button what to do when we press it
            button.onClick.AddListener(delegate {
                TextLogObj.SetActive(false);
            });
        }
    }

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }

    // Creates a button showing the choice text
    void CreateContentView(string text)
    {
        LogText.text = text;
        LogText.transform.SetParent(viewport, false);
    }

    // Creates a button showing the choice text
    Button CreateChoiceView(string text, int choices)
    {
        if (choices == 1)
        {
            ButtonTwo.SetActive(false);
            ButtonOne.gameObject.SetActive(true);
            // Gets the text from the button prefab
            Button choice = ButtonOne.GetComponentInChildren<Button>();

            // Make the button expand to fit the text
           // HorizontalLayoutGroup layoutGroup = ButtonOne.GetComponent<HorizontalLayoutGroup>();
            //layoutGroup.childForceExpandHeight = false;

            return ButtonOne;
        }
        else if (choices == 2)
        {
            ButtonOne.gameObject.SetActive(false);
            ButtonTwo.SetActive(true);

            Button[] choice = ButtonTwo.GetComponentsInChildren<Button>();
            Debug.Log(choice[0] + " " + choice[1]);
            // Gets the text from the button prefab
            for (int i = 0; i < 2; i++)
            {
                Transform buttonChild = choice[i].gameObject.transform.GetChild(0);
                TextMeshProUGUI choiceText = buttonChild.GetComponent<TextMeshProUGUI>();

                choiceText.text = text;

                // Make the button expand to fit the text
                //HorizontalLayoutGroup layoutGroup = choice[i].GetComponent<HorizontalLayoutGroup>();
                //layoutGroup.childForceExpandHeight = false;

                return choice[i];
            }
        }

        return null;
    }

    // Destroys all the children of this gameobject (all the UI)
    void RemoveChildren()
    {
        int childCount = canvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }

    [SerializeField]
    private TextAsset inkJSONAsset;
    public Story story;

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Transform viewport;

    // UI Prefabs
    [SerializeField]
    private TextMeshProUGUI LogText;
    [SerializeField]
    private Button ButtonOne;
    [SerializeField]
    private GameObject ButtonTwo;
    [SerializeField]
    private GameObject TextLogObj;
}