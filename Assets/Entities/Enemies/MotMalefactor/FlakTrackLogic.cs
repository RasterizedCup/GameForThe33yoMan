using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlakTrackLogic : FilProjectile
{
    GameObject player;
    public Rigidbody2D rb2d;
    public CapsuleCollider2D col;
    public UnityEngine.Rendering.Universal.Light2D shellLight;
    public SpriteRenderer shellSprite;
    public float maxLifeDuration;
    public float maxIntensity;
    float currLifeDuration;
    public float proxThreshold;
    public float detonationDuration;
    public float explosionIntensityDecayRate;
    float currTime, startTime;
    bool detonate;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Bubble Player");
        targetPosition = GameObject.Find("Bubble Player").transform.position;
        targetPosition.z = 0;
        currLifeDuration = Time.time + maxLifeDuration;

        Vector3 worldPoint = targetPosition;
        worldPoint.z = 0;

        Vector3 posDelta = worldPoint - transform.position;
        posDelta.Normalize();
        float rotZ = Mathf.Atan2(posDelta.y, posDelta.x) * Mathf.Rad2Deg;
        shellSprite.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        travelToPos = targetPosition - transform.position;

        markForDestruction = false;
        primingDestroy = false;

        shellLight.intensity = maxIntensity;
    }

    void Update()
    {
        HandleProjectile();
    }

    void HandleProjectile()
    {
        if (!detonate)
        {
            travelToPos.Normalize();
            rb2d.MovePosition(rb2d.position + (Vector2)(travelToPos * Time.fixedDeltaTime * velocity));
        }
        else if(currTime + detonationDuration > Time.time) // handle flak detonation
        {
            shellLight.intensity -= (Time.deltaTime * explosionIntensityDecayRate);
            if (shellLight.intensity < 0)
                shellLight.intensity = 0;
        }
        else // delete obj
        {
            Destroy(this.gameObject);
        }
        // despawn after a period of time anyway
        if (currLifeDuration < Time.time)
        {
            Destroy(this.gameObject);
        }
        if (!detonate)
        {
            detonate = checkDetonationProximity();
        }
    }

    bool checkDetonationProximity()
    {
        var setDetonation = 
           (Mathf.Abs(transform.position.x - targetPosition.x) <= proxThreshold &&
            Mathf.Abs(transform.position.y - targetPosition.y) <= proxThreshold) ||
           (Mathf.Abs(transform.position.x - player.transform.position.x) <= proxThreshold &&
            Mathf.Abs(transform.position.y - player.transform.position.y) <= proxThreshold);
        if (setDetonation)
        {
            currTime = Time.time;
            shellLight.intensity = 100;
            shellLight.pointLightOuterRadius = 1.5f;
            shellSprite.enabled = false;
            // play detonation sound
            GetComponent<CircleCollider2D>().enabled = true;
        }
        return setDetonation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"Entered trigger collision: {collision.name} : {isNonConcernedCollision(collision.tag)}");
        if (!isNonConcernedCollision(collision.tag))
        {
            markForDestruction = true;
            primingDestroy = true;
                
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<HealthLogicBase>().CurrentHealth -= DamageDealt;
                return;
            }
            currTime = Time.time;
            shellLight.intensity = 100;
            shellLight.pointLightOuterRadius = 1.5f;
            shellSprite.enabled = false;
            // play detonation sound
            GetComponent<CircleCollider2D>().enabled = true;
            detonate = true;
        }
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log($"Stay trigger collision: {collision.name} : {isNonConcernedCollision(collision.tag)}");
        if (!isNonConcernedCollision(collision.tag))
        {
            markForDestruction = true;
            primingDestroy = true;

            if (collision.CompareTag("Player") && !detonate)
            {
                collision.GetComponent<HealthLogicBase>().CurrentHealth -= DamageDealt;
                detonate = true;
                currTime = Time.time;
            }
        }
    }*/

    private bool isNonConcernedCollision(string collisionTag)
    {
        return collisionTag == "MainCamera" ||
            collisionTag == "CamBound" ||
            collisionTag == "PlayerInvis" ||
            collisionTag == "ThrowingKnife" ||
            collisionTag == "FilSprite" ||
            collisionTag == "PlayerProximity" ||
            collisionTag == "background" ||
            collisionTag == "SnailRegion" ||
            collisionTag == "MotLaser" ||
            collisionTag == "MotBot" ||
            collisionTag == "VisualMap" ||
            collisionTag == "CheckpointRegion" ||
            collisionTag == "FOVcontrol";
    }
}
