using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanelMotion : MonoBehaviour
{
    public Transform TeleportReceiver;
    public float MovementRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
    }

    void handleMovement()
    {
        transform.Translate(Vector2.up * Time.deltaTime * MovementRate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ElevatorTeleporter"))
        {
            Debug.Log("elevator moved");
            transform.position = TeleportReceiver.position;
        }
    }
}
