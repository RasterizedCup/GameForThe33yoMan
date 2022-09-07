using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleConveyorMotion : MonoBehaviour
{
    public GameObject sourceObj;
    public Transform teleportTo;
    public float MovementRate;
    public bool isVertical;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMotion();
    }

    void HandleMotion()
    {
        if(!isVertical)
        sourceObj.transform.position = new Vector3(
            sourceObj.transform.position.x + (MovementRate * Time.deltaTime),
            sourceObj.transform.position.y,
            sourceObj.transform.position.z);
        else
        {
            sourceObj.transform.position = new Vector3(
           sourceObj.transform.position.x,
           sourceObj.transform.position.y + (MovementRate * Time.deltaTime),
           sourceObj.transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ElevatorTeleporter"))
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Default");
            if (!isVertical)
            {
                sourceObj.transform.position = new Vector3(
                    teleportTo.position.x,
                    sourceObj.transform.position.y,
                    sourceObj.transform.position.z);
            }
            else
            {
                sourceObj.transform.position = new Vector3(
                    sourceObj.transform.position.x,
                    teleportTo.position.y,
                    sourceObj.transform.position.z);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ElevatorTeleporter"))
        {
            if (!isVertical)
            {
                sourceObj.transform.position = new Vector3(
                    teleportTo.position.x,
                    sourceObj.transform.position.y,
                    sourceObj.transform.position.z);
            }
            else
            {
                sourceObj.transform.position = new Vector3(
                    sourceObj.transform.position.x,
                    teleportTo.position.y,
                    sourceObj.transform.position.z);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ElevatorTeleporter"))
        {
            transform.gameObject.layer = LayerMask.NameToLayer("GrappleObject");
        }
    }
}
