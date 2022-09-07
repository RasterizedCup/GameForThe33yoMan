using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    TextMeshProUGUI frames;
    // Start is called before the first frame update
    void Start()
    {
        frames = GetComponent <TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        frames.text = $"{(int)(1/Time.deltaTime)}";
    }
}
