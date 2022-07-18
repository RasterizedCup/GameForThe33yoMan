using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapiocaPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "PlayerFlashbang")
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<BubblePickUp>().incrBubbleCount();
        }
    }
}
