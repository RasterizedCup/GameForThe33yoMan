using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImgLifeDuration : MonoBehaviour
{
    float currTime;
    public float timeUntilDestroy;
    // Start is called before the first frame update
    void Start()
    {
        currTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(currTime + timeUntilDestroy < Time.time)
        {
            Destroy(this.gameObject);
        }
    }
}
