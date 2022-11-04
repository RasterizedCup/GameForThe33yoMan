using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravBoost : MonoBehaviour
{
    public bool boostEnabled;
    public float forceOfBoost;
    public float maxVelFromBoost; // if left at 0, max vel is infinite

    bool AppropriateToBoost(Vector2 vel)
    {
        if (transform.rotation.z == 0 || transform.rotation.z == 180)
        {
            return Mathf.Abs(vel.y) < maxVelFromBoost || maxVelFromBoost == 0;
        }
        return Mathf.Abs(vel.x) < maxVelFromBoost || maxVelFromBoost == 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(CollisionChecker.hitPlayer(collision.tag))
        {
            // play launch sound
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CollisionChecker.hitPlayer(collision.tag) && boostEnabled)
        {
            if (AppropriateToBoost(collision.attachedRigidbody.velocity)) {
                collision.attachedRigidbody.AddForce(transform.up * forceOfBoost * Time.deltaTime);
                if (transform.rotation.z != 0) // clamp y vel to 0 while in lift
                {
                    collision.attachedRigidbody.velocity = new Vector2(collision.attachedRigidbody.velocity.x, 0);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CollisionChecker.hitPlayer(collision.tag))
        {
            boostEnabled = false;
        }
    }
}
