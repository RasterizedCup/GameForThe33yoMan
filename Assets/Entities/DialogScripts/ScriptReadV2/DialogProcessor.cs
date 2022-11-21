using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class DialogProcessor : MonoBehaviour
{
    TextMeshProUGUI speakerReadout;
    TextMeshProUGUI textReadout;
    GameObject choiceBlockParent;
    float iterationTime;
    float currTime;
    int itr = 0;
    int startPtr = 0;
    List<string> alphaTiers;
    int windowSize;
    bool readoutComplete;
    string baseText;
    DialogNode root;
    public string fileName;
    DialogTreeBuilder treeBuilder;
    bool decisionLock;
    public static bool freezeChar;
    public GameObject dialogParent;
    // Start is called before the first frame update
    void OnEnable()
    {
        freezeChar = true;
        speakerReadout = GameObject.Find("DialogSpeaker").GetComponent<TextMeshProUGUI>();
        choiceBlockParent = GameObject.Find("ChoiceObjects");
        treeBuilder = new DialogTreeBuilder();
        root = treeBuilder.TextNodeBuilder(fileName);
        alphaTiers = new List<string>
        {
            "FF", "EE", "DD", "CC", "BB", "AA", "88", "66", "44", "22"
        };
        currTime = Time.time;
        windowSize = alphaTiers.Count;
        iterationTime = .01f;
        textReadout = GetComponent<TextMeshProUGUI>();
        textReadout.text = root.dialogText;
        speakerReadout.text = root.speaker;
        baseText = textReadout.text;
        appendAlphaTermToText();
        readoutComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!decisionLock)
        {
            if (root.dialogType == DialogType.speaking)
                TextProcessing();
            else if (root.dialogType == DialogType.decision)
                ChoiceProcessing();
            HandlePlayerInput();
        }
    }

    void TextProcessing()
    {
        if (!readoutComplete)
        {
            StringBuilder sb = new StringBuilder(textReadout.text);
            if (currTime + iterationTime < Time.time)
            {
                for (var i = 0; i < windowSize; i++)
                {
                    if (startPtr + (i * 12) + 9 < sb.Length)
                    {
                        sb[startPtr + (i * 12) + 8] = alphaTiers[i][0];
                        sb[startPtr + (i * 12) + 9] = alphaTiers[i][1];
                    }
                    else
                    {
                        appendMaxAlphaText();
                        readoutComplete = true;
                    }
                }
                if (!readoutComplete)
                {
                    textReadout.text = sb.ToString();
                    itr++;
                    startPtr = 12 * itr;
                    currTime = Time.time;
                }
            }
        }
    }

    void ChoiceProcessing()
    {
        Debug.Log($"PROCESSING CHOICE OF SIZE: {root.decisionMap.Count}");
        var itr = 0;
        var localOffset = 0;
        decisionLock = true;
        foreach (var node in root.decisionMap)
        {
            var op = new GameObject("choice" + itr, typeof(TextMeshProUGUI), typeof(Button), typeof(MouseOverChoice));
            op.transform.SetParent(choiceBlockParent.transform);
            // set appearance data
            op.GetComponent<RectTransform>().localPosition = (new Vector3(0, localOffset, 0));
            op.GetComponent<RectTransform>().sizeDelta = (new Vector2(550, 30));
            op.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            op.GetComponent<TextMeshProUGUI>().text = node.dialogText;
            op.GetComponent<TextMeshProUGUI>().fontSize = 24;
            ColorBlock buttonC = op.GetComponent<Button>().colors;
            buttonC.highlightedColor = new Color(0, 1, 1, 1);   // cyan (color for hovering over choice)
            op.GetComponent<Button>().colors = buttonC;
            op.GetComponent<Button>().onClick.AddListener(() => SetNewRootNode(node.nextNode));
            localOffset -= 30;
            itr++;
        }

        // create and instantiate prefab text select box?
        // map decisionMap nodes to each box,
        // on select, map that node to root node
        // follow textRead flow as normal (graph should point to the correct place despite the remapping) 
    }

    void SetNewRootNode(DialogNode node)
    {
        // set new root
        root = node;
        decisionLock = false;
        HandleTextSequenceFromDecision();
        Debug.Log(root.dialogText);

        List<GameObject> choices = new List<GameObject>();
        foreach(Transform child in choiceBlockParent.transform)
        {
            choices.Add(child.gameObject);
        }

        foreach(var choice in choices)
        {
            Destroy(choice);
        }
    }

    void HandlePlayerInput()
    {
        if(Input.GetKeyDown(ControlMapping.KeyMap["Progress Dialog"]))
        {
            if (!readoutComplete)
            {
                appendMaxAlphaText();
            }
            else
            {
                HandleNextTextSequence();
            }
        }
    }

    void HandleTextSequenceFromDecision()
    {
        // reset values for subsequent read
        textReadout.text = root.dialogText;
        speakerReadout.text = root.speaker;
        baseText = textReadout.text;
        appendAlphaTermToText();
        readoutComplete = false;
        startPtr = 0;
        itr = 0;
    }

    void HandleNextTextSequence()
    {
        // reset values for subsequent read
        root = root.nextNode;

        if (root == null)
        {
            freezeChar = false;
            dialogParent.SetActive(false);
        }

        textReadout.text = root.dialogText;
        speakerReadout.text = root.speaker;
        baseText = textReadout.text;
        appendAlphaTermToText();
        readoutComplete = false;
        startPtr = 0;
        itr = 0;
    }

    void appendAlphaTermToText()
    {
        for(var i=0; i< textReadout.text.Length; i+=12)
        {
            if(i==0)
                textReadout.text = textReadout.text.Substring(0, i) + "<alpha=#FF>" + textReadout.text.Substring(i);
            else
                textReadout.text = textReadout.text.Substring(0, i) + "<alpha=#00>" + textReadout.text.Substring(i);
        }
    }

    void appendMaxAlphaText()
    {
        textReadout.text = "<alpha=#FF>" + baseText;
    }
}
