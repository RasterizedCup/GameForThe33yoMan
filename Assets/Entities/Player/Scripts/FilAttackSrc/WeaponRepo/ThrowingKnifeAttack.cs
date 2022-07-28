using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeAttack : MonoBehaviour
{
    FilAbilityHandler Abilityhander;
    GameObject Knife;
    protected Transform sprite;
    protected bool attackInProgress;
    public float rotationRate;
    public float flipRate;
    protected bool isInSpin;
    public bool isDoubleJumping;
    float currTime;
    public float knifeDelay;
    // Start is called before the first frame update
    void Start()
    {
        Knife = Resources.Load("FilThrowingKnife") as GameObject;
        currTime = 0 - knifeDelay; // allow immediate fire of either ability (weird fault of starting state)
        Abilityhander = GameObject.Find("FilAbilities").GetComponent<FilAbilityHandler>();
        sprite = GameObject.FindGameObjectWithTag("FilSprite").GetComponent<Transform>();
        attackInProgress = false;
        isInSpin = false;
        isDoubleJumping = false;
    }

    public bool throwingStarSingleAttack()
    {
        if (!attackInProgress && Time.time >= currTime + knifeDelay)
        {
            currTime = Time.time;
            attackInProgress = true;
            var freshKnife = Instantiate(Knife);
            freshKnife.name = Guid.NewGuid().ToString() + "Knife";
        }

        // rotate sprite X 360 degrees through delta time
        // toggle a check at greater than 180 so we know to reset it once we are lower than 180 again

        if (attackInProgress)
        {
            return handleSpin();
        }
        return false;
    }

    bool handleSpin()
    {
        sprite.localRotation = Quaternion.Euler(sprite.localRotation.eulerAngles.x,
                sprite.localRotation.eulerAngles.y + (rotationRate * Time.deltaTime),
                sprite.localRotation.eulerAngles.z);

        if (sprite.localRotation.eulerAngles.y > 180)
        {
            isInSpin = true;
        }
        if (isInSpin)
        {
            if (sprite.localRotation.eulerAngles.y < 180)
            {
                isInSpin = false;
                attackInProgress = false;
                sprite.localRotation = Quaternion.Euler(sprite.localRotation.x, 0, sprite.localRotation.z);
            }
        }
        return true;
    }
}
