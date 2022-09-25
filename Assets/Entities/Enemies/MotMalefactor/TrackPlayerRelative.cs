using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerRelative : MonoBehaviour
{
    float baseRotation;
    public GameObject target;
    public float maxDegreeRotationThreshold;
    public float maxRotOffset;
    // Start is called before the first frame update
    void Start()
    {
        baseRotation = transform.localRotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        handlePlayerTracking();
    }

    void handlePlayerTracking()
    {
        Vector3 worldPoint = target.transform.position;
        worldPoint.z = 0;
        Vector3 posDelta = worldPoint - transform.position;
        posDelta.Normalize();
        float rotZ = Mathf.Atan2(posDelta.y, posDelta.x) * Mathf.Rad2Deg;
        rotZ -= 90;
        if(gameObject.name == "bossTurretPivot_7")
        {
            Debug.Log($"{rotZ} - {baseRotation - maxDegreeRotationThreshold - maxRotOffset} - {baseRotation + maxDegreeRotationThreshold - maxRotOffset}");
        }
        rotZ = Mathf.Clamp(rotZ, baseRotation - maxDegreeRotationThreshold - maxRotOffset, baseRotation + maxDegreeRotationThreshold - maxRotOffset);
        
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

    }
}
