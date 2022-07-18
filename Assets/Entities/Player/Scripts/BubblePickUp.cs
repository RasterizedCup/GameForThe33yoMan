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
        Debug.Log(bubbCount + " bubbles collected");
        if(bubbCount >= bubbMax)
        {
            //this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue; // test
            Debug.Log("Ur doin a win!!!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
