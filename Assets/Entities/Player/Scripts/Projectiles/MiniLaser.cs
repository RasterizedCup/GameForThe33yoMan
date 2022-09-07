using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MiniLaser : FilProjectile
{
    public Rigidbody2D rb2d;
    public CapsuleCollider2D col;
    public Light2D laserLight;
    public SpriteRenderer laserSprite;
    public float maxIntensity, minIntensity, lightOscRate;
    float maxLifeduration;
    float currLifeDuration;
    Camera attackOrientationCam;
    // Start is called before the first frame update
    void Start()
    {
        attackOrientationCam = GameObject.Find("OrthoTrackingCamera").GetComponent<Camera>();
        maxLifeduration = 6;
        currLifeDuration = Time.time + maxLifeduration;

        this.transform.position = GameObject.Find("FilAttacks").transform.position;
        mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        xy.Raycast(ray, out distance);
        targetPosition = ray.GetPoint(distance);

        Vector3 worldPoint = attackOrientationCam.ScreenToWorldPoint(Input.mousePosition);
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
        if(laserLight.intensity >= maxIntensity)
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

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!isNonConcernedCollision(collision.tag) && !markForDestruction)
        {
            markForDestruction = true;
            primingDestroy = true;

            if (collision.CompareTag("Missile"))
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isNonConcernedCollision(collision.tag) && !markForDestruction)
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
            collisionTag == "VisualMap" ||
            collisionTag == "PlayerFlashbang" ||
            collisionTag == "SnailRegion" ||
            collisionTag == "CheckpointRegion" ||
            collisionTag == "MotLaser" ||
            collisionTag == "FOVcontrol";
    }
}
