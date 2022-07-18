﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FilHealth : HealthLogicBase
{
    // Start is called before the first frame update
    public BoxCollider2D FilHitBox;
    float xSize, ySize, xOffset, yOffset;
    Vector2 centerPoint;
    Vector2 boxSize;
    void Start()
    {
        xSize = FilHitBox.size.x + .1f;
        ySize = FilHitBox.size.y + .1f;
        xOffset = FilHitBox.offset.x;
        yOffset = FilHitBox.offset.y;
        CurrentHealth = MaxHealth;
        handleDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checkHitBoxOverlap();
        checkForDeath();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: get tag dictionary for enemy weapons
        if (collision.CompareTag("Missile") && !this.gameObject.CompareTag("PlayerInvis"))
        {
            Debug.Log(collision.gameObject.name);
            // all colliders are child objects tied to sprite angle for weapons
            // we must get the parent component to get Weapon logic -> damage
            CurrentHealth -= collision.gameObject.GetComponentInParent<WeaponLogicBase>().damage;
            if (CurrentHealth <= 0)
                handleDeath = true;
        }

        if (collision.CompareTag("Tapioca"))
        {
            if (CurrentHealth < MaxHealth) {
                CurrentHealth = CurrentHealth + 10 > MaxHealth ? MaxHealth : CurrentHealth + 10;
            }
        }
    }
}