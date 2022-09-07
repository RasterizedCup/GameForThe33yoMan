using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject baseObject;
    public GameObject target;
    public float maxDegreeRotationThreshold;
    float maxRotOffset;
    // Start is called before the first frame update
    void Start()
    {
        maxRotOffset = baseObject.transform.rotation.eulerAngles.z;
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
        // handle 90 degree offset
        if (baseObject.transform.rotation.eulerAngles.z == 270 || baseObject.transform.rotation.eulerAngles.z == 90)
        {
            if(baseObject.transform.rotation.eulerAngles.z == 270)
                rotZ = Mathf.Clamp(rotZ, maxDegreeRotationThreshold * -1 - maxRotOffset + 180, maxDegreeRotationThreshold - maxRotOffset + 180);
            else
            {
                rotZ = Mathf.Clamp(rotZ, maxDegreeRotationThreshold * -1 - maxRotOffset - 180, maxDegreeRotationThreshold - maxRotOffset - 180);
            }
        }
        else
        {
            rotZ = Mathf.Clamp(rotZ, maxDegreeRotationThreshold * -1 - maxRotOffset, maxDegreeRotationThreshold - maxRotOffset);
        }
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

    }
}
