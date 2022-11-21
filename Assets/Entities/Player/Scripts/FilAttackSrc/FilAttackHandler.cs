using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilAttackHandler : FilAttacks
{
    Dictionary<AttackType, Func<bool>> AttackMap;

    // need set of premium attacks and standard attacks to distinguish

    public static AttackType PrimaryAttack, SecondaryAttack;
    bool PrimaryAttackIsActive, SecondaryAttackIsActive;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PrimaryAttack = AttackType.AxeSwing;
        SecondaryAttack = AttackType.ThrowingStarFan;

        PrimaryAttackIsActive = false;
        SecondaryAttackIsActive = false;

        AttackMap = new Dictionary<AttackType, Func<bool>>
        {
            { AttackType.ThrowingStarFan, checkThrowingStarFanAttack },
            { AttackType.ThrowingStarSingle, checkThrowingStarSingleAttack },
            { AttackType.LaserBurst, checkLaserBurst },
            { AttackType.AxeSwing, checkAxeSwing }
            // can dynamically add attacks to FilAttacks later on
        };
    }

    // Update is called once per frame
    protected override void Update()
    {
        // prevent menu raise if attack is in progress?
        if (ActiveToggle.isMenuActive)
        {
            sprite.localRotation = Quaternion.Euler(0,0,0);
            spritePivot.localRotation = Quaternion.Euler(0, 0, 0);
            isInSpin = false;
            attackInProgress = false;
            PrimaryAttackIsActive = false;
            SecondaryAttackIsActive = false;
        }
        if (!CharMovement.isCutscene && !ActiveToggle.isMenuActive)
        {
            base.Update();
            handlePrimaryAttack();
        }
    }

    void handlePrimaryAttack()
    {
        if ((Input.GetKeyDown(ControlMapping.KeyMap["Primary Attack"]) && ControlMapping.validateInput()) || PrimaryAttackIsActive)
        {
            PrimaryAttackIsActive = (bool)AttackMap[PrimaryAttack].DynamicInvoke();
        }
    }

    public bool attackActive()
    {
        return PrimaryAttackIsActive;
    }

    public static void AssignPrimaryAttack(AttackType attackToAssign)
    {
        PrimaryAttack = attackToAssign;
    }

    public static void AssignSecondaryAttack(AttackType attackToAssign)
    {
        SecondaryAttack = attackToAssign;
    }
}
