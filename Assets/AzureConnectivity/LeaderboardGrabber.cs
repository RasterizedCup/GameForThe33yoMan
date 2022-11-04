using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderboardGrabber : MonoBehaviour
{
    public List<GameObject> placements;

    public async Task handleUpdateAndFetchLeaderboard(LeaderboardSingleMapper leaderboardSingleMapper)
    {
        Leaderboard respBoard = null;
        await WebRequests.PostJson("https://filgame-stats.azurewebsites.net/api/AddLevelTime?code=qd1rxoxbDsmXPfbgApA7twszok7j-lqHlH5f1yUBOXXuAzFudqTckQ==",
               JsonConvert.SerializeObject(leaderboardSingleMapper),
               (string error) =>
               {
                   Debug.Log("error:" + error);
               },
               (string resp) =>
               {
                   Debug.Log("Response:" + resp);
                   GetUpdatedBoard();
               }
          );
        return;
    }

    public async void GetUpdatedBoard()
    {
        await WebRequests.Get("https://filgame-stats.azurewebsites.net/api/GetLeaderboard?code=CvZROOuMywM1FKE-lS-TcEmwLpWj2-Es6Mn819Caiz8eAzFuokolDA==",
                (string error) =>
                {
                    Debug.Log("error:" + error);
                },
                (string resp) =>
                { //refactor leaderboard level list to dictionary
                    Debug.Log("Response:" + resp);
                    Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(resp);

                    var sortedTimes = (from board in leaderboard.leaderboardLevelList
                                       where board.levelName
                                       == SceneManager.GetActiveScene().name
                                       select board.leaderboardSingleList).FirstOrDefault().OrderBy(x => x.TimeToComplete).ToList();
                    for (var i = 0; i < placements.Count; i++)
                    {
                        // convert TimeToComplete to mm:ss display
                        int minutes = sortedTimes[i].TimeToComplete / 60;
                        int seconds = sortedTimes[i].TimeToComplete % 60;
                        string minuteStr = (minutes < 10) ? $"0{minutes}" : $"{minutes}";
                        string secondsStr = (seconds < 10) ? $"0{seconds}" : $"{seconds}";
                        placements[i].GetComponent<TextMeshProUGUI>().text = $"{i + 1}: {sortedTimes[i].playerName} - {minuteStr}:{secondsStr}";
                    }
                }
            );
        return;
    }

    public async void GetEntireLeaderboard()
    {
        await WebRequests.Get("https://filgame-stats.azurewebsites.net/api/GetLeaderboard?code=CvZROOuMywM1FKE-lS-TcEmwLpWj2-Es6Mn819Caiz8eAzFuokolDA==",
                (string error) =>
                {
                    Debug.Log("error:" + error);
                },
                (string resp) =>
                {
                    Debug.Log("Response:" + resp);
                    Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(resp);

                    // get all scenes.
                    var ExtractedLevels = (from board in leaderboard.leaderboardLevelList
                                           select board.levelName).ToList();
                    int itr = 0;
                    foreach (var ExtractedLevel in ExtractedLevels)
                    {
                        var LevelTimes = (from board in leaderboard.leaderboardLevelList
                                          where board.levelName == ExtractedLevel
                                          select board.leaderboardSingleList)
                                          .FirstOrDefault().OrderBy(x => x.TimeToComplete).ToList();
                        // will require trial and error
                        var xInit = 56; var xOffsetDelta = 160;
                        var yInit = 20; var yOffsetDelta = 20;
                        var maxPerColumn = 15; var currColumnNum = 0;

                        // refresh existing
                        var levelArray = new List<string> {
                            "Level1TimeParent",
                            "Level2TimeParent",
                        };

                        // set parent gameObject
                        for (var i = 0; i < Mathf.Min(LevelTimes.Count, 50); i++)
                        {
                            // convert TimeToComplete to mm:ss display
                            int minutes = LevelTimes[i].TimeToComplete / 60;
                            int seconds = LevelTimes[i].TimeToComplete % 60;
                            string minuteStr = (minutes < 10) ? $"0{minutes}" : $"{minutes}";
                            string secondsStr = (seconds < 10) ? $"0{seconds}" : $"{seconds}";
                            GameObject playerTime = Instantiate(new GameObject(), GameObject.Find(levelArray[itr]).transform);
                            playerTime.AddComponent<TextMeshProUGUI>();
                            playerTime.GetComponent<TextMeshProUGUI>().text = $"{i + 1}: {LevelTimes[i].playerName} - {minuteStr}:{secondsStr}";
                            playerTime.GetComponent<TextMeshProUGUI>().fontSize = 12;
                            playerTime.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
                            playerTime.GetComponent<RectTransform>().localPosition = new Vector3(xInit, yInit, 0);
                            yInit -= yOffsetDelta;
                            currColumnNum++;
                            if(currColumnNum >= maxPerColumn)
                            {
                                currColumnNum = 0;
                                xInit += xOffsetDelta;
                                yInit = 20;
                            }
                        }
                        itr++;
                    }
                }
            );
        return;
    }
}
