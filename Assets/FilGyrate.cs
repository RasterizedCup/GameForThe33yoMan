using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilGyrate : MonoBehaviour
{
    public float rotationRateX, rotationRateY, rotationRateZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationRateX * Time.deltaTime, rotationRateY * Time.deltaTime, rotationRateZ * Time.deltaTime);
    }
}
