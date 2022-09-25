using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FilState
{
    // player stats
    public static string playerName;

    // generic stats
    public static float currency;

    // ability unlock indicators (nyoom true by default)
    public static bool hasCopter = false;
    public static bool hasSnail = false;
    public static bool hasGrappleSpike = false;

    // weapon unlock indicators (snack star true by default)
    public static bool hasFan = false;
    public static bool hasLaserBurst = false;

    // ultimate unlock indicators (none true by default)
    public static bool hasFlashbang = false;
    public static bool hasLaserBeam = false;
    public static bool hasBlackHole = false;

    public static HashSet<string> completedLevels = new HashSet<string>();
}
