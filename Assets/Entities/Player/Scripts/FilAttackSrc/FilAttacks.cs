using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilAttacks : MonoBehaviour
{
    // will need a reference to FilAbilities to modify stamina for premiumAttacks
    FilAbilityHandler Abilityhander;
    protected Transform sprite;
    protected bool attackInProgress;
    public float rotationRate;
    public float flipRate;
    protected bool isInSpin;
    public bool isDoubleJumping;

    // single throwing knife attack
    GameObject Knife, Fan;
    public GameObject ThrowingKnife;
    public float knifeDelay;

    // triple throwing fan attack
    public List<GameObject> ThrowingFans;
    public float fanDelay;
    public ThrowingKnifeAttack throwingKnifeAttack;
    public ThrowingFanAttack throwingFanAttack;
    float currTime;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Knife = Resources.Load("FilThrowingKnife") as GameObject;
        Fan = Resources.Load("FilThrowingFanMid") as GameObject;
        currTime = 0 - fanDelay; // allow immediate fire of either ability (weird fault of starting state)
        Abilityhander = GameObject.Find("FilAbilities").GetComponent<FilAbilityHandler>();
        sprite = GameObject.FindGameObjectWithTag("FilSprite").GetComponent<Transform>();
        attackInProgress = false;
        isInSpin = false;
        isDoubleJumping = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }
    /*
    protected bool throwingStarSingleAttack()
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
    }*/

    protected bool checkThrowingStarSingleAttack()
    {
        return throwingKnifeAttack.throwingStarSingleAttack();
    }

    protected bool checkThrowingStarFanAttack()
    {
        return throwingFanAttack.ThrowingStarFanAttack();
    }

    protected bool LaserBeam()
    {
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
