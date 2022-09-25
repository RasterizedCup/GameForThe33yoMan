using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightOscillator : MonoBehaviour
{
    Light2D laserLight;
    public float maxIntensity, minIntensity, flickerRate;
    bool flickerUp;
    // Start is called before the first frame update
    void Start()
    {
        laserLight = GetComponent<Light2D>();
        laserLight.intensity = maxIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        handleOscillation();
    }

    void handleOscillation()
    {
        laserLight.intensity += (flickerUp) ? flickerRate * Time.deltaTime : flickerRate * Time.deltaTime * -1;

        if (laserLight.intensity >= maxIntensity)
        {
            flickerUp = false;
        }
        else if (laserLight.intensity <= minIntensity)
        {
            flickerUp = true;
        }
    }
}
