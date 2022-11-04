using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoginIndicator : MonoBehaviour
{
    TextMeshProUGUI loginIndicator;
    // Start is called before the first frame update
    void Start()
    {
        loginIndicator = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        loginIndicator.text = (FilState.playerName == string.Empty) ? "Not logged in!" : $"Logged in as {FilState.playerName}";
    }
}
