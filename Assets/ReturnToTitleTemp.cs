using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToTitleTemp : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    Color defaultColor, selectedColor, hoverColor;
    void Start()
    {
        defaultColor = GetComponent<Image>().color;
        selectedColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, .5f);
        hoverColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, .8f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().color = selectedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = defaultColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().color = defaultColor;
    }
}
