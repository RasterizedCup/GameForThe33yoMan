using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFilMovement : MonoBehaviour
{
    public float maxVelocity;
    Rigidbody2D player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        player.velocity = new Vector2(Random.Range(1, maxVelocity), Random.Range(1, maxVelocity));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("horizWall"))
        {
            player.velocity = new Vector2(player.velocity.x, player.velocity.y * -1);
        }
        else if (collision.CompareTag("vertWall"))
        {
            player.velocity = new Vector2(player.velocity.x * -1, player.velocity.y);
        }
    }

    public void setVelocityMulti(float multiplier)
    {
        player.velocity = new Vector2(player.velocity.x * multiplier, player.velocity.y * multiplier);
    }
}
