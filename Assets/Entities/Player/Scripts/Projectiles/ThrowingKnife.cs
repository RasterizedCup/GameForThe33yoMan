using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : FilProjectile
{
    float maxLifeduration;
    float currLifeDuration;

    void OnEnable()
    {
        soundSource = GetComponent<AudioSource>();

        maxLifeduration = 6;
        currLifeDuration = Time.time + maxLifeduration;

        this.transform.position = GameObject.Find("Bubble Player").transform.position;
        mousePosition = Input.mousePosition;

        targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetPosition.z = 0;
        Debug.Log(targetPosition);
        travelToPos = targetPosition - transform.position;

        Debug.Log(travelToPos);

        markForDestruction = false;
        primingDestroy = false;

        soundSource.clip = knifeThrow;
        soundSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        HandleProjectile();
    }

    void HandleProjectile()
    {
        if (!markForDestruction)
        {
            travelToPos.Normalize();
            transform.Translate(travelToPos * Time.deltaTime * velocity);
        }
        else // despawn after impact
        {
            if (primingDestroy)
            {
                baseTime = Time.time;
                primingDestroy = false;

                soundSource.clip = knifeHit;
                soundSource.Play();
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
        Debug.Log($"Entered trigger collision: {collision.name} : {isNonConcernedCollision(collision.tag)}");
        if (!isNonConcernedCollision(collision.tag))
        {
            markForDestruction = true;
            primingDestroy = true;

            if (collision.CompareTag("Missile"))
            {
                Destroy(this.gameObject);
            }
        }
    }

    private bool isNonConcernedCollision(string collisionTag)
    {
        return collisionTag == "MainCamera" ||
            collisionTag == "CamBound" ||
            collisionTag == "Player" ||
            collisionTag == "PlayerInvis" ||
            collisionTag == "ThrowingKnife" ||
            collisionTag == "FilSprite" ||
            collisionTag == "PlayerProximity" ||
            collisionTag == "background" ||
            collisionTag == "PlayerFlashbang" ||
            collisionTag == "SnailRegion" ||
            collisionTag == "FOVcontrol";
    }
}
