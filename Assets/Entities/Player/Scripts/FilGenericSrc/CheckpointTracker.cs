using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    HashSet<Vector3> traversedCheckPoints;
    public static Vector3 currentCheckpoint;
    public static string currentCheckpointName;
    public Transform parentObj;
    public GameObject EventInfo;
    public string EventMessage;
    public float EventDuration;
    // Start is called before the first frame update
    void Start()
    {
        traversedCheckPoints = new HashSet<Vector3>();
        currentCheckpoint = parentObj.position; // make first checkpoint initial spawn
        currentCheckpointName = "present";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckpointRegion"))
        {
            if (!traversedCheckPoints.Contains(collision.transform.position))
            {
                traversedCheckPoints.Add(collision.transform.position);
                currentCheckpoint = collision.transform.position;
                currentCheckpointName = (collision.name.Contains("present")) ? "present" : "future";
                GameObject.Find("EventText").GetComponent<EventTextHandler>().UpdateInteractionText(EventMessage, EventDuration);
            }
        }
    }
}
