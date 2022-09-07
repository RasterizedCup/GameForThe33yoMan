using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReadMappingFromUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public string ActionToMap;
    public static string currentlyMapping;
    public Text ActionText;
    public static float mappingDelayThreshold = .5f;
    public static float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlMapping.isSeekingMap && ActionToMap == currentlyMapping)
        {
            string mapText = ControlMapping.setKeyBind(ActionToMap);
            ActionText.text = $"{ActionToMap} : {mapText}";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // time delay prevents clicks from overriding themselves
        if (!ControlMapping.isSeekingMap && currentTime < Time.unscaledTime)
        {
            ControlMapping.isSeekingMap = true;
            currentlyMapping = ActionToMap;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Down {ActionToMap}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Entered {ActionToMap}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Exited {ActionToMap}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Up {ActionToMap}");
    }
}
