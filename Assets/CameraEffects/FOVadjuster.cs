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
    private float baseFOV = 3.5f;
    private float deltaRefresh, timeTrack = 0;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 2;
        vCam = GameObject.Find("CMcam").GetComponent<CinemachineVirtualCamera>();
        targetFOV = baseFOV;
        deltaRefresh = .01f;
    }

    // Update is called once per frame
    void Update()
    {
       // if (Time.time > timeTrack)
       // {
           // timeTrack += deltaRefresh;
            if (Mathf.Abs(vCam.m_Lens.OrthographicSize - targetFOV) > .02f)
            {
                if (vCam.m_Lens.OrthographicSize < targetFOV) // need to expand ortho
                {
                    vCam.m_Lens.OrthographicSize += (expansionRate * (1f + Time.deltaTime));
                }
                else if (vCam.m_Lens.OrthographicSize > targetFOV) // need to shrink ortho
                {
                    vCam.m_Lens.OrthographicSize -= (expansionRate * (1f + Time.deltaTime));
                }
            }
       //}
    }

    // must be stay to prioritize new FOVregions
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("FOVcontrol"))
        {
            targetFOV = collision.gameObject.GetComponent<FOVaTrigger>().getTarget();
            expansionRate = collision.gameObject.GetComponent<FOVaTrigger>().getExRate();    
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FOVcontrol"))
        {
            targetFOV = baseFOV;
        }
            
    }
}
