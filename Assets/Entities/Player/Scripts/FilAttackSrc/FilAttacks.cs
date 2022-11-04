using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilAttacks : MonoBehaviour
{
    // will need a reference to FilAbilities to modify stamina for premiumAttacks
    FilAbilityHandler Abilityhander;
    protected Transform sprite;
    protected Transform spritePivot;
    protected bool attackInProgress;
    public float rotationRate;
    public float flipRate;
    protected bool isInSpin;
    public bool isDoubleJumping;

    // laser burst attack
    public GameObject LaserBurst;

    // single throwing knife attack
    GameObject Knife, Fan;
    public GameObject ThrowingKnife;
    public float knifeDelay;

    // triple throwing fan attack
    public List<GameObject> ThrowingFans;
    public float fanDelay;
    public ThrowingKnifeAttack throwingKnifeAttack;
    public ThrowingFanAttack throwingFanAttack;
    public LaserBurstAttack laserBurstAttack;
    public MeleeAxe meleeAxeAttack;
    float currTime;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Knife = Resources.Load("FilThrowingKnife") as GameObject;
        Fan = Resources.Load("FilThrowingFanMid") as GameObject;
        currTime = 0 - fanDelay; // allow immediate fire of either ability (weird fault of starting state)
        Abilityhander = GameObject.Find("FilAbilities").GetComponent<FilAbilityHandler>();
        sprite = GameObject.FindGameObjectWithTag("FilSprite").GetComponent<Transform>();
        spritePivot = GameObject.Find("FilSpritePivot").GetComponent<Transform>();
        attackInProgress = false;
        isInSpin = false;
        isDoubleJumping = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    protected bool checkThrowingStarSingleAttack()
    {
        return throwingKnifeAttack.throwingStarSingleAttack();
    }

    protected bool checkThrowingStarFanAttack()
    {
        return throwingFanAttack.ThrowingStarFanAttack();
    }

    protected bool checkLaserBurst()
    {
        return laserBurstAttack.FillyLaserBurstAttack();
    }

    protected bool checkAxeSwing()
    {
        return meleeAxeAttack.AxeSwingAttack();
    }

    protected bool LaserBeam()
    {
        return false;
    }

    
}
