using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCannonFire : MonoBehaviour
{
    public TurretProxCheck tpc;
    public GameObject firePointL, firePointR, cannonL, cannonR;
    GameObject FlackObj;
    public float fireDelay;
    float currTime;
    bool leftCannonFiring;
    // Start is called before the first frame update
    void Start()
    {
        FlackObj = Resources.Load("MotBossFlakShell") as GameObject;
        currTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(tpc.isInProximity)
            handleFiring();
    }

    void handleFiring()
    {
        if(currTime + fireDelay < Time.time)
        {
            if (leftCannonFiring)
            {
                var freshLaser = Instantiate(FlackObj, firePointL.transform.position, firePointL.transform.rotation);
                freshLaser.name = Guid.NewGuid().ToString() + "FlakRound";
                // sim left cannon recoil
                leftCannonFiring = false;
            }
            else
            {
                var freshLaser = Instantiate(FlackObj, firePointR.transform.position, firePointR.transform.rotation);
                freshLaser.name = Guid.NewGuid().ToString() + "FlakRound";
                // sim right cannon recoil
                leftCannonFiring = true;
            }
            currTime = Time.time;
        }
    }
}
