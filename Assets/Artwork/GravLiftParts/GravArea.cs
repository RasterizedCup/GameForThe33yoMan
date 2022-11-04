using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravArea : MonoBehaviour
{
    public GravBoost gravBoostRef;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang"))
        {
            gravBoostRef.boostEnabled = true;
            collision.attachedRigidbody.velocity = new Vector2(collision.attachedRigidbody.velocity.x, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang"))
        {
            gravBoostRef.boostEnabled = true;
        }
    }
}
