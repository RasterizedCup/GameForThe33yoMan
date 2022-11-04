using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


// Login button
public class NameChangeButton : ButtonBase, IPointerClickHandler
{
    AccountGrabber accountGrabber = new AccountGrabber();
    public static string nameState = string.Empty;
    public TextMeshProUGUI nameValue;
    public TextMeshProUGUI passValue;
    public TextMeshProUGUI stateMessage;
    private void Update()
    {
        if (nameState != string.Empty)
            NameStateHandler();
    }

    public async void OnPointerClick(PointerEventData eventData)
    {
        Account account = new Account
        {
            PlayerName = nameValue.text,
            EncrpytedPassword = passValue.text,
        };
        Debug.Log(account.PlayerName);
        if (gameObject.name == "RegisterButton")
        {
            await accountGrabber.RegisterAccount(account);
        }
        else
        {
            await accountGrabber.ValidateLogin(account);
        }  
    }

    void NameStateHandler()
    {
        stateMessage.text = nameState;
        // switch statement with varying cases on how to handle
        switch (nameState)
        {
            case "User Validated!":
                stateMessage.text = $"User Validated!";
                FilState.playerName = nameValue.text;
                break;
            default:
                break;
        }
        nameState = string.Empty;
    }
}
