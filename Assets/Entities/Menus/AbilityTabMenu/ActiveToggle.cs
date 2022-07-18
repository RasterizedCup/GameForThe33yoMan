﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveToggle : MonoBehaviour
{
    public static bool isMenuActive; // need to reference to disable movement
    public bool debugMenuState;
    public float yToLerpTo;
    public float yToLerpFrom;
    public float lerpRate;
    RectTransform UIcontainer;
    // Start is called before the first frame update
    void Start()
    {
        UIcontainer = GetComponent<RectTransform>();
        isMenuActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkMenuToggle();
        handleMenuState();
    }

    void checkMenuToggle()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMenuActive = !isMenuActive;
            debugMenuState = isMenuActive;
        }
    }

    void handleMenuState()
    {
        if (isMenuActive && UIcontainer.localPosition.y != yToLerpTo)
        {
            UIcontainer.localPosition = new Vector3(UIcontainer.localPosition.x, Mathf.Min(yToLerpTo, UIcontainer.localPosition.y + (Time.deltaTime * lerpRate)), 0);
        }

        if (!isMenuActive && UIcontainer.localPosition.y != yToLerpFrom)
        {
            UIcontainer.localPosition = new Vector3(UIcontainer.localPosition.x, Mathf.Max(yToLerpFrom, UIcontainer.localPosition.y - (Time.deltaTime * lerpRate)), 0);
        }
    }


}

