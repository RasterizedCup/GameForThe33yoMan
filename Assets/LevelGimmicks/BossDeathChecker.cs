using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathChecker : MonoBehaviour
{
    public MultiDoorClose multiDoorClose;
    public GameObject BossPresent;
    public GameObject BossFuture;
    bool alreadyTriggered;
    // Start is called before the first frame update
    void Start()
    {
        alreadyTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFightCompletion();
    }

    void CheckFightCompletion()
    {
        if(!alreadyTriggered && !BossPresent.active && !BossFuture.active)
        {
            multiDoorClose.MoveDoors(true);
            alreadyTriggered = true;
        }
    }
}
