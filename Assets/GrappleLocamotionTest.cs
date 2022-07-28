using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLocamotionTest : MonoBehaviour
{
    public float MoveRate;
    public float MoveDelta;
    bool isGoingRight;
    float xRightOffset, xLeftOffset;
    // Start is called before the first frame update
    void Start()
    {
        xRightOffset = transform.position.x + MoveDelta;
        xLeftOffset = transform.position.x - MoveDelta;
        isGoingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoingRight)
        {
            transform.position = new Vector3(transform.position.x + (MoveRate * Time.deltaTime), transform.position.y, 0);
            if(transform.position.x >= xRightOffset)
            {
                isGoingRight = !isGoingRight;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x - (MoveRate * Time.deltaTime), transform.position.y, 0);
            if (transform.position.x <= xLeftOffset)
            {
                isGoingRight = !isGoingRight;
            }
        }
    }
}
