using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnvToggle : MonoBehaviour
{
    AudioSource teleSound;
    GameObject teleFlash;
    GameObject player, cmCam;
    public GrappleGun playerGrapple;
    public float toggleCooldown;
    public float yOffset;
    bool presentActive;
    float currTime;
    public BoxCollider currentCollider, futureCollider;
    //TODO: disable grapple on swap

    // Start is called before the first frame update
    void Start()
    {
        teleSound = GetComponent<AudioSource>();
        teleFlash = GameObject.Find("Teleport flash");
        cmCam = GameObject.Find("CMcam");
        player = GameObject.Find("Bubble Player");
        presentActive = true;
        currTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        handleObjSwap();
    }

    //
    // set to teleport player, set the cm cam offset to what it was pre move
    void handleObjSwap()
    {
        if (Input.GetKeyDown(ControlMapping.KeyMap["Special Attack"]) && currTime + toggleCooldown < Time.time)
        {
            playerGrapple.DisableGrappleHook();
            teleSound.Play();
            teleFlash.GetComponent<TeleportFlash>().handleTeleFlashInit();
            currTime = Time.time;
            if (presentActive)
            {
                presentActive = false;
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - yOffset, player.transform.position.z);
                cmCam.transform.position = new Vector3(cmCam.transform.position.x, cmCam.transform.position.y - yOffset, cmCam.transform.position.z);
                cmCam.GetComponent<CinemachineConfiner>().m_BoundingVolume = futureCollider;
            }
            else
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z);
                cmCam.transform.position = new Vector3(cmCam.transform.position.x, cmCam.transform.position.y + yOffset, cmCam.transform.position.z);
                presentActive = true;
                cmCam.GetComponent<CinemachineConfiner>().m_BoundingVolume = currentCollider;
            }
        }
    }

    public void handleBoundarySwap(string toggleSetExpectancy)
    {

        if(toggleSetExpectancy == "present")
        {
            presentActive = true;
            cmCam.GetComponent<CinemachineConfiner>().m_BoundingVolume = currentCollider;
        }
        else
        {
            presentActive = false;
            cmCam.GetComponent<CinemachineConfiner>().m_BoundingVolume = futureCollider;
        }
        cmCam.transform.position = new Vector3(cmCam.transform.position.x, cmCam.transform.position.y - yOffset, cmCam.transform.position.z);
    }

}
