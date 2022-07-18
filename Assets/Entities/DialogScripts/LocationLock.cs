using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationLock : MonoBehaviour
{
    Vector3 init;
    RectTransform loc;
    // Start is called before the first frame update
    void Start()
    {
        init = new Vector3(0, 0, 0);
        loc = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(loc.localPosition.x != 0 || loc.localPosition.y != 0)
        {
            loc.localPosition = init;
        }
            
    }
}
