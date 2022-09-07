using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventTextHandler : MonoBehaviour
{
    float displayDuration, currTime;
    TextMeshProUGUI eventText;
    // Start is called before the first frame update

    static TextMeshProUGUI interactText;
    // Start is called before the first frame update
    void Start()
    {
        eventText = GetComponent<TextMeshProUGUI>();
        eventText.text = "";
    }

    void Update()
    {
        HandleEventText();
    }

    void HandleEventText()
    {
        if (currTime + displayDuration < Time.time)
        {
            eventText.text = "";
        }
    }

    public void UpdateInteractionText(string eventOccurence, float displayDuration)
    {
        eventText.text = eventOccurence;
        this.displayDuration = displayDuration;
        currTime = Time.time;
    }
}
