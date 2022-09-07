using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class WorldTimeExpectancies : MonoBehaviour
{
    public float baseExpectedTime;
    public float baseSnackAllocation;
    public float tierDropOffset;
    public float snackAllocOffset;
    public List<string> tierNames;
    Dictionary<float, TimeTierScenario> SuccessVals;
    void Start()
    {
        SuccessVals = new Dictionary<float, TimeTierScenario>();
        // successVals is adjusted linearly, appended from best to worst tier
        for (int i=0; i<5; i++)
        {
            SuccessVals.Add(
                baseExpectedTime + (i * tierDropOffset), 
                new TimeTierScenario(tierNames[i], baseSnackAllocation + (snackAllocOffset * 5-i))
            );
        }
    }

    public TimeTierScenario getTimeTierScenarioFromCompletion(float completionTime)
    {
        //why am I using a dictionary for this lol
        foreach(var SuccessVal in SuccessVals)
        {
            if (completionTime <= SuccessVal.Key)
                return SuccessVal.Value;
        }
        // return worst possible value if no other values match
        return SuccessVals[baseExpectedTime + (4 * tierDropOffset)];
    }

    public void ensurePopulateDict()
    {
        SuccessVals = new Dictionary<float, TimeTierScenario>();
        // successVals is adjusted linearly, appended from best to worst tier
        for (int i = 0; i < 5; i++)
        {
            SuccessVals.Add(
                baseExpectedTime + (i * tierDropOffset),
                new TimeTierScenario(tierNames[i], baseSnackAllocation + (snackAllocOffset * 5 - i))
                );
        }
    }

    public List<float> getTimeExpectanciesFromSuccessVals()
    {
        return SuccessVals.Select(a => a.Key).ToList();
    }
}
