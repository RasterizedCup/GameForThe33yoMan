using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseShift : MonoBehaviour
{

    Rigidbody2D rb2d;
    Transform filSprite;
    protected SpriteRenderer spriteRenderer;
    GameObject ColliderObj;

    public float phaseForceValue;
    public float phaseForceDuration;
    public float phaseForceCoolDownDuration;
    public float phaseAfterImageDuration, phaseAfterImageFreq;
    float phaseAfterImgSplitTime = 0;
    public GameObject AfterImageObj;
    public GameObject SusAfterImageObj;
    float currPhaseTime;
    public float phaseStaminaCost;
    public AudioClip PhaseShiftSound;
    protected bool canPhaseShift;
    protected AudioSource AbilityTrigger;

    public static bool isPhaseShifting;
    // Start is called before the first frame update
    void Start()
    {
        isPhaseShifting = false;
        rb2d = GameObject.Find("Bubble Player").GetComponent<Rigidbody2D>();
        spriteRenderer = GameObject.Find("FilSprite").GetComponent<SpriteRenderer>();
        filSprite = GameObject.FindGameObjectWithTag("FilSprite").GetComponent<Transform>();
        ColliderObj = GameObject.Find("FilHealthObj");
        AbilityTrigger = GetComponent<AudioSource>();
    }

    public bool checkPhaseShift()
    {
        if (phaseStaminaCost < FilAbilities.currentStamina && canPhaseShift)
        {
            isPhaseShifting = true;

            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Max(rb2d.velocity.y, 0));

            Jumping.canDoubleJump = true; // let phase shifting reset double jump!
            FilAbilities.currentStamina -= phaseStaminaCost;
            currPhaseTime = Time.time;
            canPhaseShift = false;
            phaseAfterImgSplitTime = Time.time;
            // play shift sound
            AbilityTrigger.clip = PhaseShiftSound;
            AbilityTrigger.volume = .3f;
            AbilityTrigger.pitch = 1f;
            AbilityTrigger.Play();
            // make the fil transparent
            Color color = filSprite.gameObject.GetComponent<SpriteRenderer>().color;
            filSprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, .35f);
            // make the fil invulnerable
            transform.gameObject.tag = "PlayerInvis";
            ColliderObj.GetComponent<BoxCollider2D>().gameObject.tag = "PlayerInvis";
            // make the fil boost
            if (!spriteRenderer.flipX)
            {
                rb2d.AddForce(new Vector2(phaseForceValue, 0), ForceMode2D.Impulse);
            }
            else
            {
                rb2d.AddForce(new Vector2(phaseForceValue * -1, 0), ForceMode2D.Impulse);
            }
            return true;
        }
        else if (!canPhaseShift)
        {
            // check to instantiate afterimage
            if (currPhaseTime + phaseAfterImageDuration > Time.time)
            {
                // check frequency to spawn after images
                if (phaseAfterImgSplitTime + phaseAfterImageFreq > Time.time)
                {
                    // handle condition of sus sprite
                    var af = !Flashbang.flashbangSpriteActive ? Instantiate(AfterImageObj) : Instantiate(SusAfterImageObj);
                    af.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    phaseAfterImgSplitTime = Time.time;
                }
            }

            // check to end ability
            if (currPhaseTime + phaseForceDuration < Time.time)
            {
                isPhaseShifting = false;

                Color color = filSprite.gameObject.GetComponent<SpriteRenderer>().color;
                filSprite.gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
                transform.gameObject.tag = "Player";
                ColliderObj.GetComponent<BoxCollider2D>().gameObject.tag = "Player";
            }

            // check to restore ability cooldown
            if (currPhaseTime + phaseForceCoolDownDuration < Time.time)
            {
                canPhaseShift = true;
                return false; // force return false at the end of ability to require re-trigger
            }
            return true;
        }
        return false;
    }
}
