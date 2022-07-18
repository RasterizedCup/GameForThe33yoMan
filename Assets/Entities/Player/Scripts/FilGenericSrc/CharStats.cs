using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public float maxVelocity;   // bubbles' maximum speed in any lateral direction
    public float velIncr;       // the rate that bubble gains velocity (factored through deltaTime)
    public float velDecr;       // the rate that bubble loses velocity on the ground (factored through deltaTime)
    public float aerialVelDecr; // the rate that bubble loses velocity in the air (factored through deltaTime)
    public float velRedirect;   // when attempting to change the direction of a velocity, the rate of acceleration in said direction (factored through deltaTime)
    public float jumpMulti;
    public float doubleJumpMulti;
    public float fallMulti;
    public float jumpDecay;
    public float accelRate;
    protected bool canDoubleJump;
    // unlocks
    protected bool doubleJumpUnlock;
    protected bool floatingUnlock;
    protected bool phaseShiftUnlock;
    protected bool canPhaseShift;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected bool doubleJump()
    {
        // something tied to character stats
        if (!doubleJumpUnlock)
            return false;

        return canDoubleJump;
    }

    protected bool phaseShift()
    {
        // something tied to character stats
        if (!phaseShiftUnlock)
            return false;

        return canPhaseShift;

    }
}
