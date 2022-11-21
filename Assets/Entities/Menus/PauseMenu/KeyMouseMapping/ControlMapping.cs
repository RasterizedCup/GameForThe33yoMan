using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMapping : MonoBehaviour
{
    static Dictionary<KeyCode, string> ReverseLookupKeyMap;
    public static Dictionary<string, KeyCode> KeyMap;
    static Dictionary<string, KeyCode> DefaultKeyMap;
    static Array values;

    public static bool isSeekingMap;
    // Start is called before the first frame update
    void Start()
    {
        values = Enum.GetValues(typeof(KeyCode));

        DefaultKeyMap = new Dictionary<string, KeyCode>
        {
            // in game moves
            {"Move Right", KeyCode.D},
            {"Move Left", KeyCode.A},
            {"Jump", KeyCode.Space},
            {"Primary Attack", KeyCode.Mouse0},
            {"Special Attack", KeyCode.E},
            {"Primary Ability", KeyCode.LeftShift},
            {"Ultimate Ability", KeyCode.Q},
            {"Grappling Hook", KeyCode.Mouse1},
            {"Interact", KeyCode.F},
            // cutscene controls
            {"Progress Dialog", KeyCode.Space},
        };
        KeyMap = new Dictionary<string, KeyCode>
        {
            {"Move Right", KeyCode.D},
            {"Move Left", KeyCode.A},
            {"Jump", KeyCode.Space},
            {"Primary Attack", KeyCode.Mouse0},
            {"Special Attack", KeyCode.E},
            {"Primary Ability", KeyCode.LeftShift},
            {"Ultimate Ability", KeyCode.Q},
            {"Grappling Hook", KeyCode.Mouse1},
            {"Interact", KeyCode.F},
            // cutscene controls
            {"Progress Dialog", KeyCode.Space},
        };

        isSeekingMap = false;
    }

    // implement: function to handle remapping to selected key 
       // make sure there is logic to unbind to other action if selected key is bounded already
    public static string setKeyBind(string toBindTo)
    {
        foreach (KeyCode key in values)
        {
            if (Input.GetKeyDown(key)) {
                KeyMap[toBindTo] = key;
                Debug.Log($"{toBindTo} remapped to {key.ToString()}");
                isSeekingMap = false;
                ReadMappingFromUI.currentTime = Time.unscaledTime + ReadMappingFromUI.mappingDelayThreshold;
                return key.ToString();
            }
        }
        return "binding..";
    }

    public static bool validateInput()
    {
        return !DialogProcessor.freezeChar;
    }
    // implement: function to handle default mapping reset
}
