using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayering : MonoBehaviour
{
    GameObject CMcam;
    public float LayerFollowSpeedMultiplier;
    Vector3 prevCMPosition;
    // Start is called before the first frame update
    void Start()
    {
        CMcam = GameObject.Find("CMcam");
        prevCMPosition = CMcam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        setItemLerpProperty();
        prevCMPosition = CMcam.transform.position;
    }

    void setItemLerpProperty()
    {
        Vector3 moveDelta = CMcam.transform.position - prevCMPosition;
        Vector3 itemMoveProperty = moveDelta * LayerFollowSpeedMultiplier;
        transform.position += itemMoveProperty;
    }
}
