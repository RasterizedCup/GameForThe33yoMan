using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanMid : ThrowingFan
{
    Camera attackOrientationCam;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        attackOrientationCam = GameObject.Find("OrthoTrackingCamera").GetComponent<Camera>();
        mousePosition = Input.mousePosition;
        //targetPosition = attackOrientationCam.ScreenToWorldPoint(mousePosition);
        //targetPosition.z = 0;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        xy.Raycast(ray, out distance);
        targetPosition = ray.GetPoint(distance);
        travelToPos = targetPosition - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleProjectile();
    }
}
