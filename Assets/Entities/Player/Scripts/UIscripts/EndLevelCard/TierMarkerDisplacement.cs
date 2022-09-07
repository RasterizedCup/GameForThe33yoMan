using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TierMarkerDisplacement : MonoBehaviour
{
    public TextMeshProUGUI snackReadout, timeReadout;
    float lengthCap = 500;
    bool doneExpanding;
    float completionBarPercentFromLeft;
    public float finalBarExpansionRate;
    public List<GameObject> tierSlots;
    WorldTimeExpectancies WTE;
    RectTransform backgroundMeter, currentMeter;
    SpriteRenderer flagObj;
    float completionTime, adjustedCompletionTime;
    FilPerLevelState fpls;
    // Start is called before the first frame update
    void Start()
    {
        doneExpanding = false;
        fpls = GameObject.Find("PlayerStateTracker").GetComponent<FilPerLevelState>();
        flagObj = GameObject.Find("FillyFlag").GetComponent<SpriteRenderer>();
        backgroundMeter = GameObject.Find("MeterBackground").GetComponent<RectTransform>();
        currentMeter = GameObject.Find("MeterCurrent").GetComponent<RectTransform>();
        WTE = GameObject.Find("EndStateHandler").GetComponent<WorldTimeExpectancies>();
        WTE.ensurePopulateDict();
        completionTime = fpls.getNonAdjustedCompletionTime();
        var successTimes = WTE.getTimeExpectanciesFromSuccessVals();
        successTimes.Reverse(); // get worst times first

        // zero out timing window
        var bestTime = WTE.baseExpectedTime;
        var worstTime = WTE.baseExpectedTime + (5 * WTE.tierDropOffset);
        worstTime -= bestTime;

        // set adjusted completion time;
        adjustedCompletionTime = (completionTime - bestTime) + WTE.tierDropOffset;
        completionBarPercentFromLeft = 1 - (adjustedCompletionTime / worstTime);
        // skip first and last
        for (int i=0; i<5; i++)
        {
            var adjustedTime = (successTimes[i] - bestTime) + WTE.tierDropOffset;
            var percentDistanceFromLeft = 1 - (adjustedTime / worstTime);
            var unitsFromLeft = backgroundMeter.rect.width * percentDistanceFromLeft;
            Debug.Log($"UnitsFromLeft: {unitsFromLeft}, adjustedTime: {adjustedTime}, percentDistanceFromLeft: {percentDistanceFromLeft}, worstTime: {worstTime}, successTime: {successTimes[i]}");
            Debug.Log($"final offset: {(-1 * (backgroundMeter.rect.width / 2) + unitsFromLeft)}, widthRead: {(-1 * (backgroundMeter.rect.width / 2))}");
            tierSlots[i].GetComponent<RectTransform>().localPosition = new Vector3((-1*(backgroundMeter.rect.width/2) + unitsFromLeft), 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        setFinishBarLength();
    }

    void setFinishBarLength()
    {
        if((currentMeter.sizeDelta.x <= backgroundMeter.rect.width * completionBarPercentFromLeft) && currentMeter.sizeDelta.x < lengthCap)
            currentMeter.sizeDelta = new Vector2(currentMeter.rect.width + finalBarExpansionRate * Time.deltaTime, 12);
        else
        {
            // set actual text stating success case
            snackReadout.text = $"Snacks Collected: {FilState.currency}";

            var minutes = completionTime / 60;
            var seconds = completionTime % 60;
            timeReadout.text = seconds > 10 ? $"Total Time: {(int)minutes}:{(int)seconds}" : $"Total Time: {(int)minutes}:0{(int)seconds}";
        }
    }
}
