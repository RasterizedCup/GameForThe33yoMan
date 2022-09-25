using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDoorClose : MonoBehaviour
{
    public List<GameObject> doors;
    public float translateValue;
    public float translateRate;
    float currAmountTranslated;
    bool shouldTranslate;
    bool goingUp;
    public bool areDown;
    // Start is called before the first frame update
    void Start()
    {
        shouldTranslate = false;
        currAmountTranslated = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldTranslate)
            MoveDoors(goingUp);
    }

    // call to initialize/handle door toggle
    public void MoveDoors(bool goingUp)
    {
        if (!shouldTranslate) // init door toggle
        {
            areDown = !goingUp;
            shouldTranslate = true;
            this.goingUp = goingUp;
        }
        else if(currAmountTranslated <= translateValue) // handle door toggle
        {
            float preVal = doors[0].transform.position.y;
            for(var i=0; i<doors.Count; i++)
            {
                float yChange = (goingUp) ? doors[i].transform.position.y + (translateRate * Time.deltaTime) :
                     doors[i].transform.position.y - (translateRate * Time.deltaTime);
                doors[i].transform.position = new Vector3(doors[i].transform.position.x, yChange, doors[i].transform.position.z);
            }

            currAmountTranslated += Mathf.Abs(preVal - doors[0].transform.position.y);
        }
        else
        {
            currAmountTranslated = 0;
            shouldTranslate = false;
        }
    }
}
