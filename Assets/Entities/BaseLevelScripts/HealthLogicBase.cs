using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLogicBase : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    protected bool handleDeath;
    public 
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        handleDeath = false;
    }

    protected void checkForDeath()
    {
        if (handleDeath)
            this.gameObject.active = false;
    }

    public float getPercentageCurrentHealth()
    {
        return CurrentHealth / MaxHealth;
    }
}
