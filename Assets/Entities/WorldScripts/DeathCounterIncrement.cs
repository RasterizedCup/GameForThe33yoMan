using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounterIncrement : MonoBehaviour
{
    Text deathCountText;
    public static int deathCount;
    // Start is called before the first frame update
    void Start()
    {
        deathCountText = GetComponent<Text>();
        deathCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        deathCountText.text = $"Tutorial Death Counter: {deathCount}";
    }
}
