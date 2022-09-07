using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBurstAttack : MonoBehaviour
{
    FilAbilityHandler Abilityhander;
    protected bool attackInProgress;

    AudioSource FireSound;
    GameObject LaserObj;
    public float burstDelay;
    public float firingDelay;
    float burstCount, currCount;
    float currTime;
    float burstDeltaTime; 
    // Start is called before the first frame update
    void Start()
    {
        FireSound = GetComponent<AudioSource>();
        currTime = 0 - firingDelay;
        LaserObj = Resources.Load("LaserBurst") as GameObject;
        attackInProgress = false;
        burstCount = 3;
        currCount = 0;
    }

    public bool FillyLaserBurstAttack()
    {
        if (!attackInProgress & Time.time >= currTime + firingDelay)
        {
            FireSound.Play();
            attackInProgress = true;
            currTime = Time.time;
            burstDeltaTime = Time.time;
        }

        if (attackInProgress)
        {
            if(currCount < burstCount && burstDeltaTime <= Time.time)
            {
                var freshLaser = Instantiate(LaserObj);
                freshLaser.name = Guid.NewGuid().ToString() + "LaserBrst";
                currCount++;
                burstDeltaTime = Time.time + burstDelay;
                if(currCount >= burstCount)
                {
                    currCount = 0;
                    attackInProgress = false;
                    return false;
                }
            }
            return true;
        }

        return false;
    }
}
