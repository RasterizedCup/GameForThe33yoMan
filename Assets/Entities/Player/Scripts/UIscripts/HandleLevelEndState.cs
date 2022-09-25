using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleLevelEndState : MonoBehaviour
{
    Button ContinueButton;
    GameObject EndCard;
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("BUTTON PROCCED");
        ContinueButton = GetComponent<Button>();
        EndCard = GameObject.Find("LevelFinishCard");
        ContinueButton.onClick.AddListener(SetCloseOfEndDisplay);
    }

    public void SetCloseOfEndDisplay()
    {
        //Debug.Log("END CLICKED");
        EndCard = GameObject.Find("LevelFinishCard");
        EndCard.active = false;
    }
}
