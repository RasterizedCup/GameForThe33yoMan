using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorForceExit : MonoBehaviour
{
    public bool isForcingRight;
    public float forcePower;
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
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerFlashbang") || collision.CompareTag("PlayerInvis"))
        {
            if (isForcingRight)
                GameObject.Find("Bubble Player").GetComponent<Rigidbody2D>().AddForce(Vector2.right * forcePower, ForceMode2D.Impulse);
            else
                GameObject.Find("Bubble Player").GetComponent<Rigidbody2D>().AddForce(Vector2.left * forcePower, ForceMode2D.Impulse);
        }
    }
}
