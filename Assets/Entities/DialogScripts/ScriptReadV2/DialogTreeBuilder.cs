using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTreeBuilder
{
    /*
     * DIALOG TREE HANDLING
     * 
     * (if choice)
     * - store all choice strings in decision map with null value for dialogObj
     * - LINEARLY iterate through choice dialog after that
     * - choice branches can be handled in a few ways:
     *  - <RETURN> point the next dialogNode back to the starting point for the choice
     *  - <CONTINUE> point the next dialogNode to the node directly after the choice
     *  - <END> end of dialog tree, close the dialog prompt
     *  
     * IE (choice handling):
     * <speaker1> some normal chatting
     * <CHOICE>This is choice 1|This is choice 2|This is choice 3
     * <speaker1>This is choice 1 <- (start dialog node for choice 1)
     * <speaker2>talking about choice 1
     * <speaker1>talking more about choice 1
     * <RETURN> <- (this will return us to choice node)
     * <speaker1>This is choice 2 <- (start dialog node for choice 2)
     * <speaker2>talking about choice 2
     * <speaker1>talking more about choice 2
     * <CONTINUE> <- (this will put us at the node after the choice node)
     * <speaker1>This is choice 3 <- (start dialog node for choice 3)
     * <speaker2>talking about choice 3
     * <speaker1>talking more about choice 3
     * <END> <- (this will end the dialog processing and close the prompt box)
     * 
     * - there is a CONTRACT for the number of <CONTINUE/RETURN/END> blocks to match 
     * the respective count of choices in the choice block that preceeds it
     */


    // read appropriate script from resource/file
    // populate dialogNode tree appropriately
    // pass to dialogProcessor

    /*
     * PARSING LOGIC
     * 
     * <SPEAKER>test dialog that with an end indicated by the following symbol._
     * 
     * (For choice see above), also end with '_'
     * parse each choice into subseqent nodes in decisionMap
     * 
     */

    public DialogNode TextNodeBuilder(string fileName)
    {
        DialogNode root = null;
        DialogNode curr = null;
        List<DialogNode> choiceTrees = new List<DialogNode>();
        // read textfile content to string
        TextAsset mytxtData = Resources.Load(fileName) as TextAsset;
        string input = mytxtData.text;
        string[] parsedInputArray = input.Split('\n');
        List<string> parsedInput = new List<string>(parsedInputArray);

        List<DialogNode> continueCallbacks = new List<DialogNode>(); // for when continue is used, we will have to wait until the next node is set
                                                                     // then populate the next node of these nodes

        for(var i=0; i<parsedInput.Count; i++)
        {
            Debug.Log(parsedInput[i]);
        }

        for (var i=0; i<parsedInput.Count; i++)
        {
            if (string.IsNullOrEmpty(parsedInput[i]))
                continue;

            if (root == null)
            {
                var speakerOrAction = parsedInput[i].Substring(1, parsedInput[i].IndexOf('>')-1);
                if (speakerOrAction != "CHOICE")
                {
                    root = new DialogNode
                    {
                        speaker = speakerOrAction,
                        dialogType = DialogType.speaking,
                        dialogText = parsedInput[i].Substring(parsedInput[i].IndexOf('>') + 1),
                        nextNode = null
                    };
                    curr = root;
                }
            }
            else
            {
                var speakerOrAction = parsedInput[i].Substring(1, parsedInput[i].IndexOf('>') - 1);
                // handle standard speaker flow
                if (speakerOrAction != "CHOICE")
                {
                    curr.nextNode = new DialogNode
                    {
                        speaker = speakerOrAction,
                        dialogType = DialogType.speaking,
                        dialogText = parsedInput[i].Substring(parsedInput[i].IndexOf('>') + 1),
                        nextNode = null
                    };
                    curr = curr.nextNode;

                    // check continueCallbacks map
                    if (continueCallbacks.Count > 0)
                    {
                        for(var j=0; j<continueCallbacks.Count; j++)
                        {
                            continueCallbacks[j].nextNode = curr;
                        }
                        continueCallbacks.Clear();
                    }
                }
                // handle decision flow (HOLY FUCK THIS IS GONNA SUCK)
                else
                {
                    // instantiate base next node here
                    curr.nextNode = new DialogNode
                    {
                        speaker = speakerOrAction,
                        dialogType = DialogType.decision,
                        nextNode = null
                    };
                    curr = curr.nextNode;

                    // choiceNode will iterate through every given choice option,
                    // options listed linearly in text parse, so read until while-loop ends, map like normal flow link list from there
                    // push choice node into tree map, move to next until complete
                    // after all are parsed, move treeMap to existing currNext node

                    List<DialogNode> treeMap = new List<DialogNode>();

                    DialogNode choiceNode = null;
                    DialogNode subTemp = null;
                    var numDecisions = parsedInput[i].Substring(parsedInput[i].IndexOf('>') + 1).Split('|').Length;
                    // for each choice
                    for (var j=0; j< numDecisions; j++)
                    {
                        i++; // iterate past decision statement
                        while (parsedInput[i].Substring(1, parsedInput[i].IndexOf('>') - 1) != "END" &&
                            parsedInput[i].Substring(1, parsedInput[i].IndexOf('>') - 1) != "CONTINUE" &&
                            parsedInput[i].Substring(1, parsedInput[i].IndexOf('>') - 1) != "RETURN")
                        {
                            var subSpeakerOrAction = parsedInput[i].Substring(1, parsedInput[i].IndexOf('>') - 1);
                            if (choiceNode == null)
                            {
                                choiceNode = new DialogNode
                                {
                                    speaker = subSpeakerOrAction,
                                    dialogType = DialogType.speaking,
                                    dialogText = parsedInput[i].Substring(parsedInput[i].IndexOf('>') + 1),
                                    nextNode = null
                                };
                                subTemp = choiceNode;
                            }
                            else
                            {
                                subTemp.nextNode = new DialogNode
                                {
                                    speaker = subSpeakerOrAction,
                                    dialogType = DialogType.speaking,
                                    dialogText = parsedInput[i].Substring(parsedInput[i].IndexOf('>') + 1),
                                    nextNode = null
                                };
                                subTemp = subTemp.nextNode;
                                Debug.Log("textForSubNode: " + subTemp.dialogText);
                            }

                            // set edge case boundary (should never happen, throw ex)
                            i++;
                            if (i >= parsedInput.Count)
                                break;
                        }
                        switch(parsedInput[i].Substring(1, parsedInput[i].IndexOf('>') - 1))
                        {
                            case "CONTINUE": // add to callback list, populate next itr
                                continueCallbacks.Add(subTemp);
                                break;
                            case "RETURN": // return to current node
                                subTemp.nextNode = curr;
                                break;
                        }

                        treeMap.Add(choiceNode);
                        choiceNode = null;
                    }
                    // set decision map to parent node
                    curr.decisionMap = treeMap;
                    
                }
            }
        }
        return root;
    }
}
