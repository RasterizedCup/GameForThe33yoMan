using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilAbilities : MonoBehaviour
{
    // General Ability Effectors
    protected SpriteRenderer spriteRenderer;
    protected AudioSource AbilityTrigger;
    public float staminaRegenRate;
    public float baseStamina;
    public static float currentStamina; // referenced in ability composites

    // Ability Objects
    public PhaseShift phaseShift;
    public Jumping jumping;
    public FillyCopter fillyCopter;
    public Snailian snailian;

    public GameObject PrimaryRef, SecondaryRef;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentStamina = baseStamina;
        spriteRenderer = GameObject.Find("FilSprite").GetComponent<SpriteRenderer>();
    }

    public float getPercentageCurrentStamina()
    {
        return currentStamina / baseStamina;
    }

    protected void handleStaminaRegen()
    {
        if (currentStamina < baseStamina)
        {
            currentStamina += (staminaRegenRate * Time.deltaTime);
        }
        if (currentStamina > baseStamina)
        {
            currentStamina = baseStamina;
        }
    }


    protected void handleJumping()
    {
        jumping.handleJumping();
    }

    protected bool checkFillyCopter()
    {
        return fillyCopter.checkFillyCopter();
    }

    protected bool checkPhaseShift()
    {
        return phaseShift.checkPhaseShift();
    }

    protected bool checkSnailian()
    {
        if (Snailian.isSnailianAllowed)
        {
            return snailian.handleSnailianMovement(); // be sure to handle disabling all features if false
        }
        // set ability disp color red
        return false;
    }
}
