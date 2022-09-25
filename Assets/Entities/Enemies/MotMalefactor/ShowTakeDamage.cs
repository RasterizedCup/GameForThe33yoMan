using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTakeDamage : MonoBehaviour
{
    public List<SpriteRenderer> damageObjects;
    Color damageColor, defaultColor;
    public float damageShowDuration;
    float currTime;
    bool ColorSwitchActive;
    // Start is called before the first frame update
    void Start()
    {
        damageColor = Color.red;
        defaultColor = Color.white;
        ColorSwitchActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ColorSwitchActive)
            VisualizeDamage();
    }

    public void VisualizeDamage()
    {
        if (!ColorSwitchActive)
        {
            ColorSwitchActive = true;
            currTime = Time.time;
            for(var i=0; i<damageObjects.Count; i++)
            {
                damageObjects[i].color = damageColor;
            }
        }
        else if(currTime + damageShowDuration < Time.time)
        {
            ColorSwitchActive = false;
            for (var i = 0; i < damageObjects.Count; i++)
            {
                damageObjects[i].color = defaultColor;
            }
        }
    }
}
