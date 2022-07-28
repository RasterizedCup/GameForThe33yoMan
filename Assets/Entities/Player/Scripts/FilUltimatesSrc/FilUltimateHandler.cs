using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilUltimateHandler : FilUltimates
{
    Dictionary<UltimateType, Func<bool>> UltimateMap;
    public static UltimateType selectedUltimate;
    bool isUltimateActive;
    // Start is called before the first frame update
    void Start()
    {
        isUltimateActive = false;
        selectedUltimate = UltimateType.LaserBeam;
        UltimateMap = new Dictionary<UltimateType, Func<bool>> {
            { UltimateType.Flashbang, handleFlashbang },
            { UltimateType.LaserBeam, handleLaserBeam },
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

    public static void AssignUltimate(UltimateType abilityToAssign)
    {
        if(abilityToAssign != selectedUltimate)
            currentUltCharge = 0;
        selectedUltimate = abilityToAssign;
    }

    public static string GetStringFromActiveUltimateTime()
    {
        return MapAbilityEnumToString(selectedUltimate);
    }

    public static string MapAbilityEnumToString(UltimateType abilityToString)
    {
        switch (abilityToString)
        {
            case UltimateType.Flashbang:
                return "Flashbang";
            case UltimateType.LaserBeam:
                return "Laser Beam";
            default:
                return "Lol something broke in the Enum to string mapping for ultimates";
        }
    }
}
