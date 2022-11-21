using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInteract : MonoBehaviour
{
    public string objOfInteraction;
    public GameObject charStage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || 
            collision.CompareTag("PlayerFlashbang") || 
            collision.CompareTag("PlayerInvis"))
        {
            InteractTextUpdate.UpdateInteractionText($"Press {ControlMapping.KeyMap["Interact"].ToString()} to talk to {objOfInteraction}");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") ||
            collision.CompareTag("PlayerFlashbang") ||
            collision.CompareTag("PlayerInvis")) && Input.GetKeyDown(ControlMapping.KeyMap["Interact"]))
        {
            InteractTextUpdate.UpdateInteractionText(string.Empty);
            charStage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") ||
            collision.CompareTag("PlayerFlashbang") ||
            collision.CompareTag("PlayerInvis"))
        {
            InteractTextUpdate.UpdateInteractionText(string.Empty);
        }
    }
}
