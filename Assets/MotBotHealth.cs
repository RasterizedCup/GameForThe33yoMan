using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotBotHealth : HealthLogicBase
{
    public ShowTakeDamage showTakeDamage;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        handleDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        // reset bot health on death
        if (FilHealth.isDead)
            CurrentHealth = MaxHealth;

        checkForDeath();
        if (CurrentHealth <= 0)
            handleDeath = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ThrowingKnife"))
        {
            if (showTakeDamage != null)
                showTakeDamage.VisualizeDamage();
            CurrentHealth -= collision.gameObject.GetComponent<FilProjectile>().DamageDealt;
            if (CurrentHealth <= 0)
                handleDeath = true;
        }
    }
}
