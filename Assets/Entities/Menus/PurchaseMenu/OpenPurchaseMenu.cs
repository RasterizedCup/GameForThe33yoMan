using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPurchaseMenu : MonoBehaviour
{
    public GameObject PurchaseMenu;
    bool canEnable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canEnable && Input.GetKeyDown(ControlMapping.KeyMap["Interact"]))
        {
            PurchaseMenu.active = true;
            InteractTextUpdate.UpdateInteractionText($"");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canEnable = true;
            InteractTextUpdate.UpdateInteractionText($"Press {ControlMapping.KeyMap["Interact"]} to speak to vendor.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canEnable = false;
            InteractTextUpdate.UpdateInteractionText($"");
        }
    }
}
