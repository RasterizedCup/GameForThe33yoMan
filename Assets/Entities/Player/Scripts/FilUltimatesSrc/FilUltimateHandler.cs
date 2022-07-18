using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilUltimateHandler : FilUltimates
{
    Dictionary<UltimateType, Func<bool>> UltimateMap;
    UltimateType selectedUltimate;
    bool isUltimateActive;
    // Start is called before the first frame update
    void Start()
    {
        isUltimateActive = false;
           selectedUltimate = UltimateType.Flashbang;
        UltimateMap = new Dictionary<UltimateType, Func<bool>> {
            { UltimateType.Flashbang, handleFlashbang },
        };
    }

    // Update is called once per frame
    void Update()
    {
        handleUltimateRegen();
        handleUltimate();
    }

    void handleUltimate()
    {
        if ((Input.GetKeyDown(KeyCode.Q) && currentUltCharge >= maxUltCharge) || isUltimateActive)
        {
            isUltimateActive = (bool)UltimateMap[selectedUltimate].DynamicInvoke();
        }
    }
}
