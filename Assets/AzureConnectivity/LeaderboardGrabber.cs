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

               }
          );

        await WebRequests.Get("https://filgame-stats.azurewebsites.net/api/GetLeaderboard?code=CvZROOuMywM1FKE-lS-TcEmwLpWj2-Es6Mn819Caiz8eAzFuokolDA==",
                (string error) =>
                {
                    Debug.Log("error:" + error);
                },
                (string resp) =>
                { //refactor leaderboard level list to dictionary
                    Debug.Log("Response:" + resp);
                    Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(resp);
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
                        placements[i].GetComponent<TextMeshProUGUI>().text = $"{i + 1}: {sortedTimes[i].playerName} - {sortedTimes[i].TimeToComplete}";
                    }
                }
            );
        return;
    }
}
