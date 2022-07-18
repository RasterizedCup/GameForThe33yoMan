using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloseVisibleOnly : MonoBehaviour
{
    SpriteRenderer tileTexture;
    public float visibleRange;
    public LayerMask potentialPassers; // layers effected
    void Start()
    {

        tileTexture = GetComponent<SpriteRenderer>();
        tileTexture.color = new Color(tileTexture.color.r, tileTexture.color.g, tileTexture.color.b, 0);     // start block as invisble  (disappear when FAR)
    }

    void Update()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, visibleRange, potentialPassers);
        if (player.Length > 0)
        {
            foreach (Collider2D element in player)
            {

                Vector2 playerLoc = element.GetComponentInParent<CharMovement>().getPosition();             // get position of player
                float distance = (Mathf.Pow(Mathf.Abs(playerLoc.x - transform.position.x), 2) + Mathf.Pow(Mathf.Abs((playerLoc.y) - (transform.position.y)), 2));   // a^2 + b^2 = straightline^2 
                distance = Mathf.Sqrt(distance);                                                    // straightline distance
                //Debug.Log("Distance to invis: " + (distance / visibleRange));
                tileTexture.color = new Color(tileTexture.color.r, tileTexture.color.g, tileTexture.color.b, 1 - (distance / visibleRange)); // adjust transparency based on player proximity (disappear when FAR)
            }
        }
        else       
            tileTexture.color = new Color(tileTexture.color.r, tileTexture.color.g, tileTexture.color.b, 0);     // block invisible if no one detected close
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, visibleRange);
    }
}
