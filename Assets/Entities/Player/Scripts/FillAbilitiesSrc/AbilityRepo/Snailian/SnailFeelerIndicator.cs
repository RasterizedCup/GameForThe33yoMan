using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailFeelerIndicator : MonoBehaviour
{
    public bool EnteredWalkableSurface;
    Vector2 BoxSize = new Vector2(.2f, .2f);
    public LayerMask layermask;
    int frameOffset = 1;
    // Start is called before the first frame update
    void Start()
    {        EnteredWalkableSurface = false;
    }

    // this will allow us to avoid race conditions

    private void OnTriggerEnter2D(Collider2D collision)
    {
            Debug.Log("Entered Ground collision");
            Collider2D[] overlap = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0, layermask);
            if (overlap.Length > 0)
                EnteredWalkableSurface = true;
            frameOffset = 1;
        
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            if (frameOffset == 0)
                EnteredWalkableSurface = false;
            else
                frameOffset--;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       // if (collision.CompareTag("ground"))
      //      EnteredWalkableSurface = true;
    }
}
