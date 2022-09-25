using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FOVadjuster : MonoBehaviour
{
    CinemachineVirtualCamera vCam;
    public LayerMask fovLayers;
    private float expansionRate = .2f;
    private float targetFOV;
    private float baseFOV = 36; //3.5f;
    float camDistance;
    // Start is called before the first frame update
    void Start()
    {
        camDistance = 11;
        //QualitySettings.vSyncCount = 2;
        vCam = GameObject.Find("CMcam").GetComponent<CinemachineVirtualCamera>(); // change to handle active cam (stack logic)
        targetFOV = baseFOV;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(vCam.m_Lens.FieldOfView - targetFOV) > .1f)
        {
            if (vCam.m_Lens.FieldOfView < targetFOV) // need to expand perspective
            {
                vCam.m_Lens.FieldOfView += (expansionRate * (1f + Time.deltaTime));
            }
            else if (vCam.m_Lens.FieldOfView > targetFOV) // need to shrink perspective
            {
                vCam.m_Lens.FieldOfView -= (expansionRate * (1f + Time.deltaTime));
            }
            vCam.m_Lens.OrthographicSize = camDistance * Mathf.Tan(vCam.m_Lens.FieldOfView * 0.5f * Mathf.Deg2Rad);
        }
    }

    // must be stay to prioritize new FOVregions
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("FOVcontrol"))
        {
            CMrefocus.currentTrigger = collision.name;
            targetFOV = collision.gameObject.GetComponent<FOVaTrigger>().getTarget();
            expansionRate = collision.gameObject.GetComponent<FOVaTrigger>().getExRate();    
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FOVcontrol"))
        {
            CMrefocus.currentTrigger = "";
            targetFOV = baseFOV;
        }
            
    }
}
