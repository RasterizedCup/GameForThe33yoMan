using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject BasePlayerObj;
    public GameObject BHcore;
    public GameObject BHinitializer;
    public Rigidbody2D rb2d;
    public float windupScalar;
    public float startingSize;
    float finalSize;
    public float initYoffset;
    public float lifeDuration;
    public float moveSpeed;
    public float damageValue; // scaled with deltatime
    bool baseInit;
    bool initComplete;
    float currTime;
    bool locationSet; // one time check to get screenToRayPoint position for blackhole to travel

    Vector3 travelToPos;

    // orchestrator function for black hole
    public bool HandleBlackHole()
    {
        if (!baseInit)
            defaultValueInit();

        if (!initComplete)
        {
            if (!BHinitializer.active)
                BHinitializer.active = true;
            initComplete = HandleInitialization();
            return true;
        }
        else
        {
            if (BHinitializer.active)
            {
                BHinitializer.active = false;
                BHcore.active = true;
            }
            return HandlePropogation();
        }
    }

    void defaultValueInit()
    {
        BHcore.active = false;
        BHinitializer.active = true;
        locationSet = false;
        initComplete = false;
        finalSize = .75f;
        BHinitializer.transform.localScale = new Vector3(startingSize, startingSize, 1);
        baseInit = true;
    }

    // handles windup
    bool HandleInitialization()
    {
        // use ultimate
        FilUltimates.currentUltCharge = 0;

        // set location
        transform.position = new Vector3(
            BasePlayerObj.transform.position.x,
            BasePlayerObj.transform.position.y + initYoffset,
            BasePlayerObj.transform.position.z
            );

        // quadratically scale up size from startingSize to finalSize with windup scalar. changing windupScalar effects charge time
        BHinitializer.transform.localScale = new Vector3(
            Mathf.Min(finalSize, BHinitializer.transform.localScale.x + (BHinitializer.transform.localScale.x * windupScalar * Time.deltaTime)),
            Mathf.Min(finalSize, BHinitializer.transform.localScale.y + (BHinitializer.transform.localScale.y * windupScalar * Time.deltaTime)),
            1);
        return BHinitializer.transform.localScale.x == finalSize;
    }

    // handles activity of actual black hole
    bool HandlePropogation()
    {
        if (!locationSet)
        {
            currTime = Time.time;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
            float distance;
            xy.Raycast(ray, out distance);
            Vector3 targetPosition = ray.GetPoint(distance);
            travelToPos = targetPosition - transform.position;
            travelToPos.Normalize();
            locationSet = true;
        }

        rb2d.MovePosition(rb2d.position + (Vector2)(travelToPos * Time.deltaTime * moveSpeed));

        // unset all values and end attack if duration complete
        if(currTime + lifeDuration < Time.time)
        {
            baseInit = false;
            initComplete = false;
            locationSet = false;
            BHcore.active = false;
            return false;
        }
        return true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(BHcore.active && collision.CompareTag("MotBot"))
        {
            collision.GetComponent<HealthLogicBase>().CurrentHealth -= (damageValue * Time.deltaTime);
        }
    }
}
