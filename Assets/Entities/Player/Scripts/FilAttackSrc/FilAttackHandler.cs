using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilAttackHandler : FilAttacks
{
    public FilAttacks attacks;
    Dictionary<AttackType, Func<bool>> AttackMap;

    // need set of premium attacks and standard attacks to distinguish

    AttackType PrimaryAttack, SecondaryAttack, PremiumAttack;
    bool PrimaryAttackIsActive, SecondaryAttackIsActive, premiumAttackIsActive;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PrimaryAttack = AttackType.ThrowingStarSingle;
        SecondaryAttack = AttackType.ThrowingStarFan;
        PremiumAttack = AttackType.LaserBeam;

        PrimaryAttackIsActive = false;
        SecondaryAttackIsActive = false;
        premiumAttackIsActive = false;

        AttackMap = new Dictionary<AttackType, Func<bool>>
        {
            { AttackType.ThrowingStarFan, ThrowingStarFanAttack },
            { AttackType.ThrowingStarSingle, throwingStarSingleAttack },
            { AttackType.LaserBeam, LaserBeam },
            // can dynamically add attacks to FilAttacks later on
        };
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (ActiveToggle.isMenuActive)
        {
            sprite.localRotation = Quaternion.Euler(0,0,0);
            isInSpin = false;
            attackInProgress = false;
            PrimaryAttackIsActive = false;
            SecondaryAttackIsActive = false;
        }
        if (!CharMovement.isCutscene && !ActiveToggle.isMenuActive)
        {
            base.Update();
            // check to not perform primary attack if grapple is active, change grapple to attack?
            if(FilAbilityHandler.ActiveAbility != AbilityType.GrapplingHook)
                handlePrimaryAttack();
            handleSecondaryAttack();
        }
    }

    void handlePrimaryAttack()
    {
        if (Input.GetMouseButtonDown(0) || PrimaryAttackIsActive)
        {
            PrimaryAttackIsActive = (bool)AttackMap[PrimaryAttack].DynamicInvoke();
        }
    }

    void handleSecondaryAttack()
    {
        if (Input.GetMouseButtonDown(1) || SecondaryAttackIsActive)
        {
            SecondaryAttackIsActive = (bool)AttackMap[SecondaryAttack].DynamicInvoke();
        }
    }
}
