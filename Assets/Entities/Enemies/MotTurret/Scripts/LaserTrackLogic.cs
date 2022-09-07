using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LaserTrackLogic : FilProjectile
{
    public Rigidbody2D rb2d;
    public CapsuleCollider2D col;
    public Light2D laserLight;
    public SpriteRenderer laserSprite;
    public float maxIntensity, minIntensity, lightOscRate;
    public float maxLifeDuration;
    float currLifeDuration;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GameObject.Find("Bubble Player").transform.position;
        targetPosition.z = 0;
        currLifeDuration = Time.time + maxLifeDuration;

        Vector3 worldPoint = targetPosition;
        worldPoint.z = 0;

        Vector3 posDelta = worldPoint - transform.position;
        posDelta.Normalize();
        float rotZ = Mathf.Atan2(posDelta.y, posDelta.x) * Mathf.Rad2Deg;
        laserSprite.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        travelToPos = targetPosition - transform.position;

        markForDestruction = false;
        primingDestroy = false;

        laserLight.intensity = maxIntensity;
    }

    void Update()
    {
        HandleLightPulse();
        HandleProjectile();
    }

    void HandleLightPulse()
    {
        if (laserLight.intensity >= maxIntensity)
        {
            laserLight.intensity -= lightOscRate * Time.deltaTime;
        }
        else
        {
            laserLight.intensity += lightOscRate * Time.deltaTime;
        }
    }

    void HandleProjectile()
    {
        if (!markForDestruction)
        {
            travelToPos.Normalize();
            rb2d.MovePosition(rb2d.position + (Vector2)(travelToPos * Time.fixedDeltaTime * velocity));
        }
        else // despawn after impact
        {
            if (primingDestroy)
            {
                baseTime = Time.time;
                primingDestroy = false;
            }
            if (Time.time > baseTime + timeTillDestroy)
            {
                Destroy(this.gameObject);
            }
        }
        // despawn after a period of time anyway
        if (!markForDestruction && currLifeDuration < Time.time)
        {
            Debug.Log("Destroying");
            Destroy(this.gameObject);
        }
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
                Destroy(this.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log($"Stay trigger collision: {collision.name} : {isNonConcernedCollision(collision.tag)}");
        if (!isNonConcernedCollision(collision.tag) && !markForDestruction)
        {
            markForDestruction = true;
            primingDestroy = true;

            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<HealthLogicBase>().CurrentHealth -= DamageDealt;
                Destroy(this.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

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
