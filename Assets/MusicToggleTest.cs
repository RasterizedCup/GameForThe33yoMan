using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicToggleTest : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    Color defaultColor, selectedColor, hoverColor;
    public AudioSource music;
    public Text musicText;
    bool toggleMusic;
    void Start()
    {
        toggleMusic = true;
        defaultColor = GetComponent<Image>().color;
        selectedColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, .5f);
        hoverColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, .8f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        toggleMusic = !toggleMusic;
        if (toggleMusic)
        {
            music.volume = .1f;
            musicText.text = "Stop Music Pls";
        }
        else
        {
            music.volume = 0;
            musicText.text = "Start Music Pls";
        }
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
