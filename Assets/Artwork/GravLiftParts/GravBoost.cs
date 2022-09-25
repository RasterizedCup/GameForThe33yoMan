using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravBoost : MonoBehaviour
{
    public float forceOfBoost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang"))
        {
            collision.attachedRigidbody.velocity = new Vector2(collision.attachedRigidbody.velocity.x, 0);
            collision.attachedRigidbody.AddForce(transform.up * forceOfBoost);
            // play launch sound
        }
    }
}
