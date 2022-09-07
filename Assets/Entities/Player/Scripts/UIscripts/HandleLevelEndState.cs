using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleLevelEndState : MonoBehaviour
{
    Button ContinueButton;
    GameObject EndCard;
    // Start is called before the first frame update
    void Start()
    {
        ContinueButton = GetComponent<Button>();
        EndCard = GameObject.Find("LevelFinishCard");
        ContinueButton.onClick.AddListener(SetCloseOfEndDisplay);
    }

    public void SetCloseOfEndDisplay()
    {
        EndCard = GameObject.Find("LevelFinishCard");
        EndCard.active = false;
    }
}
