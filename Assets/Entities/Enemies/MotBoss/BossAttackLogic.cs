using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackLogic : MonoBehaviour
{
    public TurretProxCheck proxCheck;
    public float attackDelay;
    GameObject LaserObj;
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        LaserObj = Resources.Load("MotBossLaserProjectile") as GameObject;
        currentTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (proxCheck.isInProximity)
            FireTurretLaser();
    }

    void FireTurretLaser()
    {
        if (currentTime + attackDelay < Time.time)
        {
            var freshLaser = Instantiate(LaserObj, transform.position, transform.rotation);
            freshLaser.name = Guid.NewGuid().ToString() + "BossLaser";
            currentTime = Time.time;
        }
    }
}
