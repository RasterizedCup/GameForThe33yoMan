using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProxCheck : MonoBehaviour
{
    public bool isInProximity;
    // Start is called before the first frame update
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isInProximity = true;
        }
        if (collision.collider.CompareTag("PlayerInvis") ||
            collision.collider.CompareTag("PlayerFlashbang"))
        {
            isInProximity = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") ||
            collision.collider.CompareTag("PlayerInvis") ||
            collision.collider.CompareTag("PlayerFlashbang"))
        {
            isInProximity = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInProximity = true;
        }

        if (collision.CompareTag("PlayerInvis") ||
            collision.CompareTag("PlayerFlashbang"))
        {
            isInProximity = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") ||
            collision.CompareTag("PlayerInvis") ||
            collision.CompareTag("PlayerFlashbang"))
        {
            isInProximity = false;
        }
    }
}
