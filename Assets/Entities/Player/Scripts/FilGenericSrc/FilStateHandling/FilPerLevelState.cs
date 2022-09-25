using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class FilPerLevelState : MonoBehaviour
{
    public TextMeshProUGUI TierReadout;

    float numSnacksCollected;
    bool completionPass;
    float StartTime;
    float CompletionTime;
    float AdjustedFinishTime;
    SpriteRenderer flagObj;
    WorldTimeExpectancies WTE;
    public GameObject completionCard;
    LeaderboardGrabber leaderboardGrabber;
    // Start is called before the first frame update
    void Start()
    {
        leaderboardGrabber = GetComponent<LeaderboardGrabber>();
        numSnacksCollected = FilState.currency;
        completionCard.active = false;
        completionPass = false;
        StartTime = Time.time;
        flagObj = GameObject.Find("FillyFlag").GetComponent<SpriteRenderer>();
        flagObj.enabled = false;
        WTE = GameObject.Find("EndStateHandler").GetComponent<WorldTimeExpectancies>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAndHandleLevelCompletion();
    }

    async void CheckAndHandleLevelCompletion()
    {
        // change to reference script method whenever that is made
        if (flagObj.enabled && !completionPass)
        {
            // set text disp obj
            numSnacksCollected = FilState.currency - numSnacksCollected;

            completionPass = true;
            CompletionTime = Time.time;

            // set Total time Completion (convert to minutes and seconds for readability)

            AdjustedFinishTime = CompletionTime - StartTime;
            var finishScenario = WTE.getTimeTierScenarioFromCompletion(CompletionTime);
            var preIncreaseSnacks = FilState.currency;
            FilState.currency += finishScenario.SnackAllocation;
            //Debug.Log($"{preIncreaseSnacks}, {FilState.currency}, {finishScenario.TierName}");
            completionCard.active = true;
            TierReadout.text = $"Ranking: {finishScenario.TierName}";

            LeaderboardSingleMapper leaderboardSingleMapper = new LeaderboardSingleMapper
            {
                levelName = SceneManager.GetActiveScene().name,
                playerName = FilState.playerName,
                TimeToComplete = (int)AdjustedFinishTime
            };

            await leaderboardGrabber.handleUpdateAndFetchLeaderboard(leaderboardSingleMapper);

            // WorldTimeExpectancies instance call
            // get timeTierScenario for player
            // coordinate FilState stuff
            // set UI values
        }
    }

    public float getNonAdjustedCompletionTime()
    {
        return CompletionTime;
    }
}
