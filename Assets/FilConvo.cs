using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilConvo : MonoBehaviour
{
    private bool interactableNPC;   // pass through an interactable npc (a trigger)
    protected bool isCutscene;
    GameObject objectToInteract;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interactableNPC)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                isCutscene = true;
                objectToInteract.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        if (objectToInteract != null)
        {
            if (!objectToInteract.transform.GetChild(0).gameObject.activeSelf)
            {
                isCutscene = false;
            }
        }
    }

    public bool checkIsCutscene()
    {
        return isCutscene;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("story-npc"))
        {
            interactableNPC = true;
            objectToInteract = collision.gameObject;
            Debug.Log("Entering " + collision.gameObject.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactableNPC = false;
        Debug.Log("Leaving " + collision.gameObject.tag);
    }
}
