using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractTextUpdate : MonoBehaviour
{
    static TextMeshProUGUI interactText;
    // Start is called before the first frame update
    void Start()
    {
        interactText = GetComponent<TextMeshProUGUI>();
        interactText.text = "";
    }

    public static void UpdateInteractionText(string objToInteract)
    {
        interactText.text = objToInteract;
    }
}
