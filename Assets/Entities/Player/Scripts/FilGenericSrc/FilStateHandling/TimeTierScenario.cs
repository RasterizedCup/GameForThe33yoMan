using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTierScenario : MonoBehaviour
{
    public string TierName;
    public float SnackAllocation;
    public TimeTierScenario(string TierName, float SnackAllocation)
    {
        this.TierName = TierName;
        this.SnackAllocation = SnackAllocation;
    }
}
