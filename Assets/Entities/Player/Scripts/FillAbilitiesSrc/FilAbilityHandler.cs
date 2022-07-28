using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
            // dont register grappling hook, this has to be handle in it's own way

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
        SetupAbilityUI();
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

    public void SetupAbilityUI()
    {
        if (Snailian.isSnailianAllowed)
        {
            if (PrimaryAbility == AbilityType.Snailian)
            {
                PrimaryRef.GetComponent<Text>().color = Color.green;
                return;
            }
            else if(SecondaryAbility == AbilityType.Snailian)
            {
                SecondaryRef.GetComponent<Text>().color = Color.green;
                return;
            }           
        }

        // de-set whatever snail abilities are left over (sprite? Rotation)

        // also disable all other things here
        if (ActiveAbility == AbilityType.Snailian)
        {
            snailian.filSprite.color = new Color(snailian.filSprite.color.r, snailian.filSprite.color.g, snailian.filSprite.color.b, 1);
            snailian.SnailSprite.color = new Color(snailian.filSprite.color.r, snailian.filSprite.color.g, snailian.filSprite.color.b, 0);
            snailian.Snailimator.enabled = false;
            snailian.rb2d.bodyType = RigidbodyType2D.Dynamic;
            Snailian.isSnailianPrimed = false;
            Snailian.isSnailianActive = false;
            snailian.SnailModeEnabled = false;
        }
        if (PrimaryAbility == AbilityType.Snailian)
        {
            PrimaryRef.GetComponent<Text>().color = Color.red;
            return;
        }
        else if (SecondaryAbility == AbilityType.Snailian)
        {
            SecondaryRef.GetComponent<Text>().color = Color.red;
            return;
        }
    }

    public static string GetCurrentPrimaryAbilityName()
    {
        return MapAbilityEnumToString(PrimaryAbility);
    }

    public static string GetCurrentSecondaryAbilityName()
    {
        return MapAbilityEnumToString(SecondaryAbility);
    }

    public static void AssignPrimaryAbility(AbilityType abilityToAssign)
    {
        PrimaryAbility = abilityToAssign;
    }

    public static void AssignSecondaryAbility(AbilityType abilityToAssign)
    {
        SecondaryAbility = abilityToAssign;
    }

    public static string MapAbilityEnumToString(AbilityType abilityToString)
    {
        switch (abilityToString)
        {
            case AbilityType.FillyCopter:
                return "Filly Copter";
            case AbilityType.PhaseShift:
                return "Nyoom";
            case AbilityType.GrapplingHook:
                return "Grappling Hook";
            case AbilityType.Snailian:
                return "Snailian";
            case AbilityType.None:
                return "Nothing";
            default:
                return "Lol something broke in the Enum to string mapping";
        }
    }
}
