using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFromProximity : MonoBehaviour
{
    public bool hasBossDoors;
    public MultiDoorClose multiDoorClose;
    public Canvas Overlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // set for only one run
        if (collision.CompareTag("Player")){
            if (hasBossDoors && !multiDoorClose.areDown)
                multiDoorClose.MoveDoors(false);
            Overlay.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang"))
        {
            Overlay.enabled = false;
        }
    }
}
