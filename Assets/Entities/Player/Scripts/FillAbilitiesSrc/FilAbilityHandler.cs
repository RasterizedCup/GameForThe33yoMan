using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Two ability types:
 * -Channelled: require persistent user interaction
 * -OneTime: a single button press handles all the interaction for this ability
 * 
 * Channelled abilities, due to their nature, require some logic in the Handler class
 * OneTime abilities are handled entirely in FilAbilities, and are only called here.
 */

public class FilAbilityHandler : FilAbilities
{
    Dictionary<AbilityType, Func<bool>> AbilityMap;
    GameObject GrappleRootObject;

    public static AbilityType PrimaryAbility, SecondaryAbility;
    public static AbilityType ActiveAbility;
    public static AbilityType toHotSwap;
    bool AbilityIsActive, SecondaryAbilityIsActive;
    bool OnePassAbilityReset;
    AudioSource AbilityToggler;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PrimaryAbility = AbilityType.PhaseShift;
        SecondaryAbility = AbilityType.GrapplingHook;
        ActiveAbility = AbilityType.None;
        GrappleRootObject = GameObject.Find("GrapplePivot");
        GrappleRootObject.SetActive(false);
        AbilityIsActive = false;
        SecondaryAbilityIsActive = false;
        OnePassAbilityReset = false;

        AbilityMap = new Dictionary<AbilityType, Func<bool>>
        {
            { AbilityType.PhaseShift, checkPhaseShift },
            { AbilityType.FillyCopter, checkFillyCopter },
            { AbilityType.Snailian, checkSnailian}
            // dont register grappling hook
            // can dynamically add abilities to FilAbilities later on
        };
        AbilityToggler = GameObject.Find("FilAbilityToggleSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ToggleAbilityTest();
        // always handle stamina regen
        handleStaminaRegen();
        if (!CharMovement.isCutscene)
        {
            // always check jump and doublejump (except cutscene)
            jumping.setCoyote();
            if (!Snailian.isSnailianActive)
            {
                jumping.handleJumping();
                jumping.doubleJumpFlipCheck();
            }
            // check ability on Lshift press
            // (grapple logic is complicated, it is in its own game object/class outside of abilities, and is handled as such)
            if (ActiveAbility != AbilityType.GrapplingHook)
            {
                if (GrappleRootObject.activeSelf)
                {
                    GrappleRootObject.SetActive(false);
                    GameObject.Find("Bubble Player").GetComponent<SpringJoint2D>().enabled = false;
                }
                handlePrimaryAbility();
            }
            else
            {
                handleGrapplingHook();
            }
            // check ability on.. E press? custom mappable?
        }
        // check default state
        setCheckDefaultState();
        
    }

    void ToggleAbilityTest()
    {
        if (Input.GetKeyDown(KeyCode.F) && !AbilityIsActive || (ActiveAbility != PrimaryAbility && ActiveAbility != SecondaryAbility) )
        {
            AbilityToggler.Play();
            ActiveAbility = (ActiveAbility != PrimaryAbility) ? PrimaryAbility : SecondaryAbility;
        }
    }

    void handlePrimaryAbility()
    {
        if (Input.GetKey(KeyCode.LeftShift) || AbilityIsActive && ActiveAbility != AbilityType.GrapplingHook)
        {
            AbilityIsActive = (bool)AbilityMap[ActiveAbility].DynamicInvoke();
            OnePassAbilityReset = true;
        }
    }

    void setCheckDefaultState()
    {
        // reset default state
        if (!AbilityIsActive && OnePassAbilityReset)
        {
            OnePassAbilityReset = false;
            spriteRenderer.transform.localRotation = Quaternion.Euler(spriteRenderer.transform.localRotation.x, 0, spriteRenderer.transform.localRotation.z);
            FillyCopter.isFillyCoptering = false;
            FillyCopter.AbilityTrigger.Stop();
        }
    }

    void handleGrapplingHook()
    {
        if (!GrappleRootObject.activeSelf)
        {
            GrappleRootObject.SetActive(true);
        }
    }

    public static string GetCurrentPrimaryAbilityName()
    {
        return ActiveAbility.ToString();
    }

    public static void AssignPrimaryAbility(AbilityType abilityToAssign)
    {
        PrimaryAbility = abilityToAssign;
    }

    public static void AssignSecondaryAbility(AbilityType abilityToAssign)
    {
        SecondaryAbility = abilityToAssign;
    }
}
