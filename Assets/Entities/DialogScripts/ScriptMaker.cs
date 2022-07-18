using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class ScriptMaker : ScriptReader
{
    bool dec = false;
    public string toRead;
    string currScript;
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        currScript = File.ReadAllText(toRead);
        Debug.Log(currScript);
        Debug.Log(currScript.Length);
        int itr = 0;

        while (itr < currScript.Length)
        {
            string speaker = "";
            string text = "";
            while (currScript[itr] != '<')
            {
                itr++;
                if (itr >= currScript.Length)
                    break;
            }
            if (itr >= currScript.Length)
                break;
            if (currScript[itr] == '<')
            {
                itr++;
                while (currScript[itr] != '>')
                {
                    speaker += currScript[itr];
                    itr++;
                }
                itr++;
            }
            Debug.Log(speaker);
            if (speaker == "CHOICE") // create choice
            {
                List<string> choices = new List<string>();                              // list of text representing effects
                List<List<DialogEffect>> effects = new List<List<DialogEffect>>();      // list of all dialogue effects
                while (currScript[itr] != '_')
                {
                    string choice = "";
                    while (currScript[itr] != '|')                                       // create text of choice
                    {
                        choice += currScript[itr];
                        itr++;
                    }
                    Debug.Log(choice);
                    choices.Add(choice);
                    itr++;

                    List<DialogEffect> effect = new List<DialogEffect>();
                    while (currScript[itr] != '|')                                      // create list of effects
                    {
                        string effector = "";
                        string value = "";
                        while (currScript[itr] != ' ')                                   // get who the choice effects
                        {
                            effector += currScript[itr];
                            itr++;
                        }
                        itr++;
                        Debug.Log(effector);
                        while (currScript[itr] != ' ' && currScript[itr] != '|')        // get the effects of said choice
                        {
                            value += currScript[itr];
                            itr++;
                        }
                        Debug.Log(value);
                        effect.Add(new DialogEffect(effector, int.Parse(value)));
                        if (currScript[itr] == '|')
                        {                                     // break out of loop if we have hit the end of these effects
                            Debug.Log("End of choices");
                            break;
                        }
                        itr++;
                    }
                    effects.Add(effect);
                    itr++;
                }
                script.Add(new Dialog("Player Name", new DialogDecision(choices, effects)));
            }
            else
            {
                while (currScript[itr] != '_')
                {
                    text += currScript[itr];
                    itr++;
                }
                Debug.Log(text);
                script.Add(new Dialog(speaker, text));
                itr++;
            }
        }
        spawnFirstLine = true;
        lines = GetComponentsInChildren<Text>();
        textbasePos = lines[0].transform.position;
        lines[0].text = "";
        lines[1].text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (convoPlace >= script.Count && Input.GetKeyDown(KeyCode.Space))
        {
            GetComponentInParent<Transform>().gameObject.SetActive(false);
        }
        if (convoPlace < script.Count)
        {
            if (!dec)
            {
                if (!script[convoPlace].decision())
                {
                    if (convoPlace == 0)
                    {
                        handleScriptDrive();    // true will autoscroll the line, false will require a space
                    }
                    else
                    {
                        handleScriptDrive();
                    }
                }
                else
                {
                    handleDecision();
                    dec = true;
                }
            }
            if (convoPlace < script.Count)
            {
                if (!script[convoPlace].decision())
                {
                    dec = false;
                }
            }
            setConvGlobal(convoPlace); // put this in every scene to track convoplace for saving
        }
    }
}
