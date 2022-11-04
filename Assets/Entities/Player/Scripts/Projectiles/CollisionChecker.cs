using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionChecker
{
    public static bool hitPlayer(string impact)
    {
        return impact == "Player" ||
            impact == "PlayerInvis" ||
            impact == "PlayerFlashbang";
    }
}
