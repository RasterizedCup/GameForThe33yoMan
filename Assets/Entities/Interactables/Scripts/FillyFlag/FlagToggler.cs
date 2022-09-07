using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagToggler : MonoBehaviour
{
    public GameObject Teleporter;
    SpriteRenderer fillyFlag;
    // Start is called before the first frame update
    void Start()
    {
        fillyFlag = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang")) && !fillyFlag.enabled)
        {
            InteractTextUpdate.UpdateInteractionText($"Press {ControlMapping.KeyMap["Interact"].ToString()} to establish complete dominance.");
            if (Input.GetKey(ControlMapping.KeyMap["Interact"]))
            {
                fillyFlag.enabled = true;
                Teleporter.active = true;
                InteractTextUpdate.UpdateInteractionText($"");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang")) && !fillyFlag.enabled)
        {
            InteractTextUpdate.UpdateInteractionText($"Press {ControlMapping.KeyMap["Interact"].ToString()} to establish complete dominance.");
            if (Input.GetKey(ControlMapping.KeyMap["Interact"]))
            {
                fillyFlag.enabled = true;
                Teleporter.active = true;
                InteractTextUpdate.UpdateInteractionText($"");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang")))
        {
            InteractTextUpdate.UpdateInteractionText($"");
        }
    }
}
