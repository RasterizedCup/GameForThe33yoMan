using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayTestLoad : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject SpinningFil;
    Color defaultColor, selectedColor, hoverColor;
    void Start()
    {
        defaultColor = GetComponent<Image>().color;
        selectedColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, .5f);
        hoverColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, .8f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("Level 1-0");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().color = selectedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = hoverColor;
        SpinningFil.GetComponent<FilGyrate>().rotationRateX = 10000;
        SpinningFil.GetComponent<FilGyrate>().rotationRateY = 10000;
        SpinningFil.GetComponent<FilGyrate>().rotationRateZ = 10000;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = defaultColor;
        SpinningFil.GetComponent<FilGyrate>().rotationRateX = 50;
        SpinningFil.GetComponent<FilGyrate>().rotationRateY = 50;
        SpinningFil.GetComponent<FilGyrate>().rotationRateZ = 50;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().color = defaultColor;
    }
}
