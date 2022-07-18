using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MotBotAttackLogic : MonoBehaviour
{
    public float MissileInstanceInterval;
    public float MissileBurstInterval;
    public int MissileBurstCount;
    int CurrBurstCount;
    bool burstIsActive;
    float currTime;
    float burstCheckTime;
    public GameObject MotMissile;
    public float proximityRadius;
    bool playerInProximity;
    AudioSource startupSound;
    public AudioClip startClip;
    public AudioClip lockonClip;
    // Start is called before the first frame update
    void Start()
    {
        CurrBurstCount = 0;
        currTime = Time.time;
        startupSound = GetComponent<AudioSource>();
        burstIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerOverlap();
        SetAttackingState();
    }

    void CheckPlayerOverlap()
    {
        var tagCollisions = Physics2D.OverlapCircleAll(transform.position, proximityRadius).Select(x => x.tag);
        if (tagCollisions.Contains("Player") || tagCollisions.Contains("PlayerInvis"))
        {
            if (!playerInProximity)
            {
                this.gameObject.GetComponentInChildren<Canvas>().enabled = true;
                playerInProximity = true;
                startupSound.clip = startClip;
                startupSound.Play();
            }
        }
        else
        {
            playerInProximity = false;
            this.gameObject.GetComponentInChildren<Canvas>().enabled = false;
        }
    }

    void SetAttackingState()
    {
        if (playerInProximity)
        {
            if (Time.time > currTime + MissileInstanceInterval && burstIsActive)
            {
                currTime = Time.time;
                var currMissile = Instantiate(MotMissile);
                currMissile.transform.position = transform.position;
                CurrBurstCount++;
                if(CurrBurstCount >= MissileBurstCount)
                {
                    CurrBurstCount = 0;
                    burstIsActive = false;
                    burstCheckTime = Time.time;
                }
            }
            else if (!burstIsActive)
            {
                if (Time.time > burstCheckTime + MissileBurstInterval)
                {
                    startupSound.clip = lockonClip;
                    startupSound.Play();
                    burstIsActive = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }
}
