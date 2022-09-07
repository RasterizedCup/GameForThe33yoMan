using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapiocaPickup : MonoBehaviour
{
    CircleCollider2D snackCollider;
    SpriteRenderer snackSprite;
    bool firstSnackPickup;
    bool disableSnack;
    float snackDisableDuration; // can't delete in case player messes up
    float snackDisableStartTime;
    float snackCurrencyVal;
    private void Start()
    {
        firstSnackPickup = true;
        snackCurrencyVal = 5;
        snackDisableDuration = 4;
        snackSprite = GetComponent<SpriteRenderer>();
        snackCollider = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        if (!disableSnack)
        {
            snackCollider.radius = PhaseShift.isPhaseShifting ? .1f : .05f;
        }
        else
        {
            if (snackDisableStartTime + snackDisableDuration > Time.time)
            {
                snackCollider.enabled = false;
                snackSprite.color = new Color(snackSprite.color.r, snackSprite.color.g, snackSprite.color.b, 0);
            }
            else
            {
                snackCollider.enabled = true;
                snackSprite.color = new Color(snackSprite.color.r, snackSprite.color.g, snackSprite.color.b, 1);
                disableSnack = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "PlayerFlashbang" || collision.tag == "PlayerInvis")
        {
            PlayPickupSound.playPickup();
            snackDisableStartTime = Time.time;
            //Destroy(this.gameObject);
            disableSnack = true;
            collision.gameObject.GetComponent<BubblePickUp>().incrBubbleCount();
            if (firstSnackPickup)
            {
                FilState.currency += snackCurrencyVal;
                firstSnackPickup = false;
            }
        }
    }
}
