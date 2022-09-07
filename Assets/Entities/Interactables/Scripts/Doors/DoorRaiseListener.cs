using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRaiseListener : MonoBehaviour
{
    public float doorRaiseDistance;
    public float doorRaiseRate;
    public ColorSequenceController eventDelegator; // make generic later
    float initDistance;
    // Start is called before the first frame update
    void Start()
    {
        initDistance = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventDelegator.totalSuccess && transform.position.y <= initDistance + doorRaiseDistance)
            LiftDoor();
    }

    void LiftDoor()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + (doorRaiseRate * Time.deltaTime),
            transform.position.z);
    }
}
