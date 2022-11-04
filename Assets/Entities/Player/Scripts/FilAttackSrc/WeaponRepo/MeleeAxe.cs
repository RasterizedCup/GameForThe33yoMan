using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAxe : MonoBehaviour
{
    // inherit in attack main
    protected bool attackInProgress;
    float internalRotTracker;
 
    public float FilRotation, AxeRotation;
    public float RotationRateMulti; // set to decimal
    public float recoilDelay, staggerTime; 
    float FilAdjustedRot, AxeAdjustedRot;
    bool isWindingBack;

    float currTime;
    public float attackCooldown;
    GameObject axeController;
    public GameObject AxePivot, FilSprite;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        axeController = GameObject.Find("Axe");
        isWindingBack = false;
        FilAdjustedRot = FilRotation * RotationRateMulti;
        AxeAdjustedRot = AxeRotation * RotationRateMulti;
        internalRotTracker = AxeRotation;
        currTime = 0 - attackCooldown;
    }

    public bool AxeSwingAttack()
    {
        if (!attackInProgress)
        {
            if (currTime + attackCooldown < Time.time)
            {
                attackInProgress = true;
                axeController.active = true;
                audioSource.Play();
            }
            else
                return false;
        }
        if (attackInProgress)
        {
            if (!isWindingBack)
            {
                // change to internal tracker (eulers work weird in editor)
                if (internalRotTracker >= 0)
                {
                    AxePivot.transform.eulerAngles = new Vector3(AxePivot.transform.eulerAngles.x,
                        AxePivot.transform.eulerAngles.y,
                        AxePivot.transform.eulerAngles.z - AxeAdjustedRot * Time.deltaTime);
                    FilSprite.transform.eulerAngles = new Vector3(FilSprite.transform.eulerAngles.x,
                        FilSprite.transform.eulerAngles.y,
                        FilSprite.transform.eulerAngles.z - FilAdjustedRot * Time.deltaTime);

                    internalRotTracker -= AxeAdjustedRot * Time.deltaTime;
                }
                else
                {
                    isWindingBack = true;
                    currTime = Time.time;
                }
            }
            else if(isWindingBack && currTime + staggerTime > Time.time)
            {
                return true;
            }
            else
            {
                if (internalRotTracker <= AxeRotation)
                {
                    AxePivot.transform.eulerAngles = new Vector3(AxePivot.transform.eulerAngles.x,
                        AxePivot.transform.eulerAngles.y,
                        AxePivot.transform.eulerAngles.z + AxeAdjustedRot * Time.deltaTime * recoilDelay);
                    FilSprite.transform.eulerAngles = new Vector3(FilSprite.transform.eulerAngles.x,
                        FilSprite.transform.eulerAngles.y,
                        FilSprite.transform.eulerAngles.z + FilAdjustedRot * Time.deltaTime * recoilDelay);

                    internalRotTracker += AxeAdjustedRot * Time.deltaTime * recoilDelay;
                }
                else
                {
                    isWindingBack = false;
                    attackInProgress = false;
                    axeController.active = false;
                    currTime = Time.time;
                    return false;
                }
            }
        }
        // enable axe obj
        // engage and launch attack
        return true;
        // wind back the attack
        //return false;
    }
}
