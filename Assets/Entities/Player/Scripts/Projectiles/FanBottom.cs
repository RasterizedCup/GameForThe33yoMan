using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBottom : ThrowingFan
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        mousePosition = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetPosition.z = 0;
        travelToPos = targetPosition - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleProjectile();
    }
}
