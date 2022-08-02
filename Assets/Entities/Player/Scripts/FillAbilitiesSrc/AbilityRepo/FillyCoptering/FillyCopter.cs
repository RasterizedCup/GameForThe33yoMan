using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillyCopter : MonoBehaviour
{
    Rigidbody2D rb2d;
    Transform filSprite;
    protected SpriteRenderer spriteRenderer;
    GameObject ColliderObj;

    public float FillyCopterLiftRate;
    public float MaxVelocityIncrease;
    public float NegativeVelocityLiftRateMulti;
    public float FillyCopterStaminaCost;
    public float FillyCopterMinStartStaminaCost;
    public float FillyCopterRotationRate;
    bool canStartCopter;
    public AudioClip FillyCopterSound;
    public static AudioSource AbilityTrigger;
    public static bool isFillyCoptering;
    public float initialBurstCost;
    // Start is called before the first frame update
    void Start()
    {
        isFillyCoptering = false;
        rb2d = GameObject.Find("Bubble Player").GetComponent<Rigidbody2D>();
        spriteRenderer = GameObject.Find("FilSprite").GetComponent<SpriteRenderer>();
        filSprite = GameObject.FindGameObjectWithTag("FilSprite").GetComponent<Transform>();
        ColliderObj = GameObject.Find("FilHealthObj");
        AbilityTrigger = GetComponent<AudioSource>();
    }

    public bool checkFillyCopter()
    {
        // unfortunate jury rig solution (need to check if Lshift is held do to hold instead of toggle)
        if (!Input.GetKey(KeyCode.LeftShift))
            return false;
        if (FilAbilities.currentStamina > FillyCopterMinStartStaminaCost)
        {
            canStartCopter = true;
         //   if (!isFillyCoptering) // burst remove stamina on start to prevent feathering
         //       FilAbilities.currentStamina -= initialBurstCost;

        }
        if (canStartCopter)
        {
            HandleCopterSpin(); // rotate on a delta'd basin
            if (!isFillyCoptering)
            {
                FilAbilities.currentStamina -= initialBurstCost;
                // handle audio init here
                AbilityTrigger.clip = FillyCopterSound;
                AbilityTrigger.Play();
            }
            isFillyCoptering = true;
            // calc velocity inc value
            float yLiftValue = (rb2d.velocity.y >= MaxVelocityIncrease) ? MaxVelocityIncrease : rb2d.velocity.y + (FillyCopterLiftRate * Time.deltaTime);
            if (rb2d.velocity.y < 0)
            {
                yLiftValue = rb2d.velocity.y + (FillyCopterLiftRate * Time.deltaTime * NegativeVelocityLiftRateMulti);
            }
            rb2d.velocity = new Vector2(rb2d.velocity.x, yLiftValue);
            FilAbilities.currentStamina -= FillyCopterStaminaCost * Time.deltaTime;
            if (FilAbilities.currentStamina <= 0)
                canStartCopter = false;
            return true; // ability relies on key hold to be active
        }
        return false;
    }

    private void HandleCopterSpin()
    {
        spriteRenderer.transform.localRotation = Quaternion.Euler(spriteRenderer.transform.localRotation.eulerAngles.x,
        spriteRenderer.transform.localRotation.eulerAngles.y + (FillyCopterRotationRate * Time.deltaTime),
        spriteRenderer.transform.localRotation.eulerAngles.z);
    }
}
