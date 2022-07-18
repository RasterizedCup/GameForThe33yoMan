using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLogicBase : MonoBehaviour
{
    public float MaxHealth;
    protected float CurrentHealth;
    protected bool handleDeath;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        handleDeath = false;
    }

    protected void checkForDeath()
    {
        if (handleDeath)
            Destroy(this.gameObject);
    }

    public float getPercentageCurrentHealth()
    {
        return CurrentHealth / MaxHealth;
    }
}
