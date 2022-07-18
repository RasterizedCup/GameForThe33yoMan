using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackHover : MonoBehaviour
{
    Vector3 BasePosition;
    Transform transform;
    public float MaxOffset;
    public float OffsetAmount;
    bool hoverDirectionToggle;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        BasePosition = transform.position;
        hoverDirectionToggle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hoverDirectionToggle)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (OffsetAmount * Time.deltaTime), transform.position.z);
            if (transform.position.y > BasePosition.y + MaxOffset)
                hoverDirectionToggle = !hoverDirectionToggle;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (OffsetAmount * Time.deltaTime), transform.position.z);
            if (transform.position.y < BasePosition.y - MaxOffset)
                hoverDirectionToggle = !hoverDirectionToggle;
        }
    }
}
