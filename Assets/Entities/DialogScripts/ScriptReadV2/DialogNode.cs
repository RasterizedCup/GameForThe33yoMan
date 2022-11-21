using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNode
{
    public DialogType dialogType { get; set;}
    public string speaker { get; set; }
    public string dialogText { get; set; }
    public DialogNode nextNode { get; set; }
    public List<DialogNode> decisionMap;
}
