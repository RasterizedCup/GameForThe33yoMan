using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingFan : FilProjectile
{
    float maxLifeduration;
    float currLifeDuration;
    protected virtual void Start()
    {
        soundSource = GetComponent<AudioSource>();

        maxLifeduration = 6;
        currLifeDuration = Time.time + maxLifeduration;

        this.transform.position = GameObject.Find("Bubble Player").transform.position;

        markForDestruction = false;
        primingDestroy = false;

        soundSource.clip = knifeThrow;
        soundSource.Play();
    }

    protected void HandleProjectile()
    {
        if (!markForDestruction)
        {
            travelToPos.Normalize();
            transform.Translate(travelToPos * Time.deltaTime * velocity);
        }
        else
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

        if (!markForDestruction && currLifeDuration < Time.time)
        {
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
