using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


// implement DialogBranch structure, for when we need to change
// the script at run time vs compile time ***

public struct DialogEffect  // contains the results of a given dialog decision
{
    string toAffect;    // name of affected entity
    int affectVal;      // the amount in which they are affected by
    public DialogEffect(string name, int change) { toAffect = name; affectVal = change; }

    public string getName()
    {
        return toAffect;
    }

    public int getAmount()
    {
        return affectVal;
    }
}

public struct DialogDecision    // dialog subtype for decision trees
{
    Dictionary<string, List<DialogEffect>> option;  // key: dialog item, value: list result tied to it
    public DialogDecision(List<string> items, List<List<DialogEffect>> results)
    {
        option = new Dictionary<string, List<DialogEffect>>();

        for (int i = 0; i < items.Count; i++)
        {
            option[items[i]] = results[i];
        }
    }
    public Dictionary<string, List<DialogEffect>> getOption()
    {
        return option;
    }
}

// add conditional dialog support***
public struct Dialog
{
    // add object for char sprite
    // add object for text color
    string speaker;             // string containing speaker name
    string text;                // text that a given character says
    bool isShaking;             // discern whether text should shake or not
    DialogDecision choice;      // if isDecision, contains the choices and their respective consequences
    bool isDecision;            // toggle true if the current convoPlace holds a choice
    bool isDecisionHandled;     // toggle true if the current convoPlace holds a choice
    public Dialog(string s, string t) { speaker = s; text = t; isShaking = false; isDecision = false; choice = new DialogDecision(); isDecisionHandled = false; } // default dialog
    public Dialog(string s, string t, bool shake) { speaker = s; text = t; isShaking = shake; isDecision = false; choice = new DialogDecision(); isDecisionHandled = false; } //for shaking
    public Dialog(string s, DialogDecision c) { speaker = s; text = ""; isShaking = false; isDecision = true; choice = c; isDecisionHandled = false; }
    public string getSpeaker() { return speaker; }
    public string getText() { return text; }
    public char getNextLetter(int lett) { return text[lett]; } // for sequential presenting of text
    public int getTextLength() { return text.Length; } // return length of text, to know when to stop parsing
    public bool getShakeFactor() { return isShaking; } // tells us if text will shake or not
    public DialogDecision getChoice() { return choice; }
    public bool decision() { return isDecision; }
    public bool decisionHandled() { Debug.Log(isDecisionHandled); return isDecisionHandled; }

    public void setHandleDecision() { isDecisionHandled = true; Debug.Log(isDecisionHandled); }
}

public struct CharDisplay
{
    // coordinate to set what is needed here
}

public class ScriptReader : MonoBehaviour
{
    private static int currConv = 0;
    protected List<Dialog> script = new List<Dialog>();
    protected int convoPlace = 0;           // serialize
    protected Text[] lines;
    protected float letterDelay = .008f;    // this gets modified to change rate of text reading (lower number is faster rate)
    protected float currTimeDelta = 0;
    protected float skipDelay = .15f;
    protected float currSkipDelta = 0;
    protected int letterParse = 0;          // increment by one every iteration of talking() function, returns next letter in text
    protected bool instantText = false;
    protected bool lineFinished = true;
    protected bool spawnFirstLine = true;   // always autoscroll first line

    protected bool triggerNextLine = false;

    // everything here is for shaky text
    protected bool isShaking = false;
    protected float shakeTimeDelta = 0;
    protected float shakeDelay = .01f;      // modify this to change the shake rate (lower number is higher rate)
    protected float shakeIntensity = 20f;   // modify this to change the intensity of the shake (lower number is higher intensity)
    protected Vector3 textbasePos;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    // for decision branches

    protected void handleChoice(List<DialogEffect> results)
    {
        // debug results to static charStat class for iteration
        for (int i = 0; i < results.Count; i++)
        {
            Debug.Log(results[i].getName());
            Debug.Log(results[i].getAmount());
        }

        // set updated char stats
        //charStats.setItems(results);

        // disable choice boxes, return normal text
        GameObject textBox = GameObject.Find("CharTextBox");
        for (int i = 0; i < textBox.transform.childCount; i++)
        {
            // delete text boxes instead, re-instantiate later
            textBox.transform.GetChild(i).gameObject.SetActive(false);
        }
        convoPlace++;
        textBox.transform.GetChild(0).gameObject.SetActive(true);
        spawnFirstLine = true;
    }

    protected void handleDecision()
    {
        lines[0].gameObject.SetActive(false);
        // toggle off chat, create choices
        float localOffset = 50;
        Transform[] choiceLocation; // is ind 1
        choiceLocation = GetComponentsInChildren<Transform>();
        Transform choiceLoc = choiceLocation[1];
        // get list of keys from dictionary
        List<string> saucers = new List<string>(script[convoPlace].getChoice().getOption().Keys);
        for (int i = 0; i < saucers.Count; i++)
        {
            GameObject op = new GameObject("choice" + i, typeof(Text), typeof(Button), typeof(MouseOverChoice));
            op.transform.parent = choiceLoc;
            // set appearance data
            op.GetComponent<RectTransform>().localPosition = (new Vector3(0, localOffset, 0));
            op.GetComponent<RectTransform>().sizeDelta = (new Vector2(741.4f, 30));
            op.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            op.GetComponent<Text>().text = saucers[i];
            op.GetComponent<Text>().fontSize = 24;
            op.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            ColorBlock buttonC = op.GetComponent<Button>().colors;
            buttonC.highlightedColor = new Color(0, 1, 1, 1);   // cyan (color for hovering over choice)
            op.GetComponent<Button>().colors = buttonC;
            op.GetComponent<Button>().onClick.AddListener(() => handleChoice(script[convoPlace].getChoice().getOption()[op.GetComponent<Text>().text]));
            localOffset -= 30;
        }
    }

    // for actual dialog
    protected void handleScriptDrive()
    {
        if (Input.GetKeyDown("space") && triggerNextLine)
        {
            triggerNextLine = false;
            convoPlace++;
            return;
        }

        if ((Input.GetKeyDown("space") && !instantText && lineFinished) || spawnFirstLine)
        {
            spawnFirstLine = false;
            currSkipDelta = Time.time + skipDelay;
            instantText = false;        // set next spacebar to trigger instaText
            lines[0].text = "";
            lines[1].text = "";
            lineFinished = false;       // reset line           
            Debug.Log("Trigger");
            letterParse = 0;            // reset letter parse location
            setDefaultTextLocation();   // reset text location (in the event of shaking text)
        }

        // create exception for blank space (make instant always)
        if (letterParse < script[convoPlace].getTextLength())
        {
            if (script[convoPlace].getNextLetter(letterParse) == ' ')
            {
                lines[0].text += script[convoPlace].getNextLetter(letterParse);
                letterParse++;
                if (letterParse >= script[convoPlace].getTextLength())
                {
                    lineFinished = true;
                    convoPlace++;
                    instantText = false;
                }
            }
        }

        // shaky text functionality
        if (script[convoPlace].getShakeFactor() && Time.time > shakeTimeDelta && !lineFinished)
        {
            setDefaultTextLocation();
            shakyText();
            shakeTimeDelta = Time.time + shakeDelay;
            isShaking = true;
        }
        else
        {
            isShaking = false;
        }

        // handle dialog creation (discern if we are reading out the time, or instantly displaying it)
        if (!instantText && Time.time > currTimeDelta + letterDelay && !lineFinished)
        {
            currTimeDelta = Time.time + letterDelay;
            talking();
        }
        if (Input.GetKeyDown("space") && !lineFinished && Time.time > currSkipDelta)
        {
            instaText();
        }
    }

    // "typewriter" style text display
    protected void talking()
    {
        lines[1].text = script[convoPlace].getSpeaker();
        lines[0].text += script[convoPlace].getNextLetter(letterParse);
        letterParse++;
        // end condition for talking
        if (letterParse >= script[convoPlace].getTextLength())
        {
            lineFinished = true;
            // update increment method based on wheter there is a decision or not
            if (convoPlace + 1 < script.Count)
            {
                if (script[convoPlace + 1].decision())
                {
                    triggerNextLine = true;
                    return;
                }
            }
            convoPlace++;
            instantText = false;
        }
    }

    // all text appears at once
    protected void instaText()
    {
        lines[1].text = script[convoPlace].getSpeaker();
        lines[0].text = script[convoPlace].getText();
        if (convoPlace + 1 < script.Count)
        {
            if (script[convoPlace + 1].decision())
            {
                triggerNextLine = true;
                return;
            }
        }
        convoPlace++;
        instantText = false; // reset instant text counter
        lineFinished = true;
    }

    protected void shakyText()
    {
        float randNumX = Random.Range(1f, 100f) / shakeIntensity;
        float randNumY = Random.Range(1f, 100f) / shakeIntensity;
        lines[0].transform.position = new Vector3(lines[0].transform.position.x + randNumX, lines[0].transform.position.y + randNumY, lines[0].transform.position.z);
    }

    public int getConvoPlace()
    {
        return convoPlace;
    }

    protected void setDefaultTextLocation()
    {
        lines[0].transform.position = textbasePos;
    }

    public static int getConvGlobal()
    {
        return currConv;
    }

    public static void setConvGlobal(int place)
    {
        currConv = place;
    }

    private bool progressConvo()
    {
        return true;
    }

    public bool shakeState()
    {
        return isShaking;
    }
}
