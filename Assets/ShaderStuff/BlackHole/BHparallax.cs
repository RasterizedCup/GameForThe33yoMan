using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHparallax : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject BaseCam;
    Vector3 prevPos, currentPos;
    public float moveMulti;
    void Start()
    {
        BaseCam = GameObject.Find("CMcam");
        prevPos = BaseCam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = BaseCam.transform.position;
        Vector3 BHmovement = (currentPos - prevPos) * -1;
        transform.position = transform.position + BHmovement;
        prevPos = BaseCam.transform.position;
    }
}
