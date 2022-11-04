using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class LeaderboardToggle : ButtonBase, IPointerClickHandler
{
    public LeaderboardGrabber leaderboardGrabber;
    public GameObject Leaderboard;
    public void OnPointerClick(PointerEventData eventData)
    {
        Leaderboard.active = true;
        leaderboardGrabber.GetEntireLeaderboard();
    }
}
