using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyReadout : MonoBehaviour
{
    TextMeshProUGUI currency;
    // Start is called before the first frame update
    void Start()
    {
        currency = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        currency.text = $"Fruit snacks: {FilState.currency}";
    }
}
