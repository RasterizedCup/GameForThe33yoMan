using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Navigation : MonoBehaviour
{
    public float speedCap;
    public float speedDecrease;
    public Sprite forward, backward, left, right;
    public float moveSpeed;
    bool isMovingX, isMovingY;
    SpriteRenderer fillyRenderer;
    Rigidbody2D rb2d;
    List<(KeyCode, Sprite, Vector2)> moveDirections;
    // Start is called before the first frame update
    void Start()
    {
        isMovingX = isMovingY = false;
        fillyRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        moveDirections = new List<(KeyCode, Sprite, Vector2)>
        {
            (KeyCode.W, forward, Vector2.up),
            (KeyCode.A, left, Vector2.left),
            (KeyCode.S, backward, Vector2.down),
            (KeyCode.D, right, Vector2.right),
        };
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
    }

    void handleMovement()
    {
        isMovingY = false;
        isMovingX = false;
        Vector2 velocity = rb2d.velocity;
        foreach (var key in moveDirections)
        {
            if (Input.GetKey(key.Item1))
            {
                fillyRenderer.sprite = key.Item2;
                velocity += (key.Item3 * moveSpeed * Time.deltaTime);
                if(key.Item1 == KeyCode.W || key.Item1 == KeyCode.S)
                {
                    isMovingY = true;
                }
                else
                {
                    isMovingX = true;
                }
            }
        }
        velocity.x = Mathf.Clamp(velocity.x, speedCap * -1, speedCap);
        velocity.y = Mathf.Clamp(velocity.y, speedCap * -1, speedCap);
        if (!isMovingX)
        {
            velocity.x = 0;
        }
        if (!isMovingY)
        {

            velocity.y = 0;
        }
        rb2d.velocity = velocity;
    }
}
