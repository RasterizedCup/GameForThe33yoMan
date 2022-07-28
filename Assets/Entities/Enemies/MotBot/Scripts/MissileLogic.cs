using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLogic : WeaponLogicBase
{
    public float velocity;
    public float lifeDuration;
    public float launchHeightMin;
    public float launchHeightMax;
    public float launchLateralMin;
    public float launchLateralMax;
    public float thresholdToEnableCollider;
    float missileLaunchHeight;
    float missileLaunchLateral;
    public float trackingRecovTime;
    float timeTrackingLost;
    float baseHeight;
    float baseLateral;
    bool isTrackingOn;
    bool isLaunching;
    Transform player;

    public Transform AnimationTransform;
    Animator MissileController;
    bool markForDestruction;
    float timeMarkedForDestruction;
    public float destructDelay;

    AudioSource MissileSound;
    public AudioClip MissileExplosion;

    Vector3 launchAngle;
    float startTime;

    bool enableCollInit;
    CapsuleCollider2D Collider;
    // Start is called before the first frame update
    void Start()
    {
        // init missile logic flow
        isTrackingOn = false;
        isLaunching = true;
        markForDestruction = false;

        // set components to initial state 
        player = GameObject.Find("Bubble Player").GetComponent<Transform>();
        MissileController = AnimationTransform.gameObject.GetComponent<Animator>();
        Collider = AnimationTransform.gameObject.GetComponent<CapsuleCollider2D>();
        Collider.enabled = false;
        MissileSound = GetComponent<AudioSource>();

        // set base values
        baseHeight = this.transform.position.y;
        baseLateral = this.transform.position.x;

        // set launch params
        enableCollInit = false;
        missileLaunchHeight = Random.Range(launchHeightMin, launchHeightMax);
        missileLaunchLateral = Random.Range(launchLateralMin, launchLateralMax);
        launchAngle = new Vector3(missileLaunchLateral, 0, 0);
        launchAngle.Normalize();

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        handleColliderEnable(); // necessary to prevent spawn killing missiles
        handleLaunch();
        handleTracking();
        handleImpact();
        handleDestruction();

        // handle flashbang case
        if (Flashbang.flashbangActive)
        {
            isTrackingOn = false;
        }
    }

    void handleColliderEnable()
    {
        if(startTime + thresholdToEnableCollider < Time.time && !enableCollInit)
        {
            enableCollInit = true;
            Collider.enabled = true;
        }
    }

    void handleLaunch()
    {
        if (isLaunching)
        {
            if (player.position.x < transform.position.x)
            {
                transform.Translate((Vector3.up + (launchAngle * -1)) * velocity * Time.deltaTime);
                AnimationTransform.Rotate(Vector3.forward);
            }
            else
            {
                transform.Translate((Vector3.up + launchAngle) * velocity * Time.deltaTime);
                AnimationTransform.Rotate(Vector3.forward * -1);       
            }

            if (transform.position.y > baseHeight + missileLaunchHeight)
            {
                isLaunching = false;
                isTrackingOn = true;
            }
        }
    }

    void handleTracking()
    {
        if (isTrackingOn)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, velocity * Time.deltaTime);
            Vector3 difference = player.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            AnimationTransform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
        }
    }

    void handleImpact()
    {
        if (!isLaunching && !isTrackingOn && !markForDestruction)
        {
            transform.Translate((AnimationTransform.up) * velocity * Time.deltaTime);

            // regain tracking after a period of time
            if (timeTrackingLost + trackingRecovTime < Time.time)
                isTrackingOn = true;
        }
    }

    void handleDestruction()
    {
        if (markForDestruction)
        {
            if (Time.time > timeMarkedForDestruction + destructDelay)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (!collision.CompareTag("MotBot"))
        {
            if (collision.CompareTag("PlayerProximity"))
            {
                Debug.Log("Tracking disabled");
                isTrackingOn = false;
                timeTrackingLost = Time.time;
                return;
            }
            else
            {
                if (!isNonConcernedCollision(collision.tag))
                {
                    Debug.Log(collision.name + " causing destruction");
                    markForDestruction = true;
                    isTrackingOn = false;
                    isLaunching = false;
                    timeMarkedForDestruction = Time.time;
                    MissileController.SetBool("isExploding", true);
                    MissileSound.clip = MissileExplosion;
                    MissileSound.Play();
                    Collider.enabled = false;
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isNonConcernedCollision(collision.collider.tag))
        {
            Debug.Log(collision.collider.tag + " causing destruction from collider");
            markForDestruction = true;
            isTrackingOn = false;
            timeMarkedForDestruction = Time.time;
            
        }
    }

    public void SetExplosionFromExternal()
    {
        markForDestruction = true;
        isTrackingOn = false;
        isLaunching = false;
        timeMarkedForDestruction = Time.time;
        MissileController.SetBool("isExploding", true);
        MissileSound.clip = MissileExplosion;
        MissileSound.Play();
        Collider.enabled = false;
    }

    private bool isNonConcernedCollision(string collisionTag)
    {
        return collisionTag == "MainCamera" ||
            collisionTag == "CamBound" ||
            collisionTag == "MotBot" ||
            collisionTag == "FilSprite" ||
            collisionTag == "PlayerInvis" ||
            collisionTag == "FOVcontrol";
    }
}
