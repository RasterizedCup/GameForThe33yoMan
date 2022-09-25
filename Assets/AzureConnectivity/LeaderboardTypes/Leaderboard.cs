using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard
{
    public List<LeaderboardLevel> leaderboardLevelList;
}

public class LeaderboardLevel
{
    public string levelName { get; set; }
    public List<LeaderboardSingle> leaderboardSingleList;
}

public class LeaderboardSingle
{
    public string playerName { get; set; }
    public int TimeToComplete { get; set; }
}

public class LeaderboardSingleMapper
{
    public string playerName { get; set; }
    public int TimeToComplete { get; set; }
    public string levelName { get; set; }
}
