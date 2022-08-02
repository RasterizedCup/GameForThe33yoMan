using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyReadout : MonoBehaviour
{
    Text currency;
    // Start is called before the first frame update
    void Start()
    {
        currency = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currency.text = $"Fruit snacks: {FilState.currency}";
    }
}
