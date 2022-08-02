using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePickUp : MonoBehaviour
{
    int bubbCount;
    public int bubbMax;
    // Start is called before the first frame update
    void Start()
    {
        bubbCount = 0;
    }

    public void incrBubbleCount(){
        bubbCount++;
        if(bubbCount >= bubbMax)
        {
            Debug.Log("Ur doin a win!!!");
        }
        FilAbilities.currentStamina = FilAbilities.maxStamina;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
