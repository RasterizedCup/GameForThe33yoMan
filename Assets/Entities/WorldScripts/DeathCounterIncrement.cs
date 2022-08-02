using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathCounterIncrement : MonoBehaviour
{
    TextMeshProUGUI deathCountText;
    public static int deathCount;
    // Start is called before the first frame update
    void Start()
    {
        deathCountText = GetComponent<TextMeshProUGUI>();
        deathCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        deathCountText.text = $"Tutorial Death Counter: {deathCount}";
    }
}
