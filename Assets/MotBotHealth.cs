using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotBotHealth : HealthLogicBase
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        handleDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkForDeath();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ThrowingKnife"))
        {
            CurrentHealth -= collision.gameObject.GetComponent<FilProjectile>().DamageDealt;
            if (CurrentHealth <= 0)
                handleDeath = true;
        }
    }
}
