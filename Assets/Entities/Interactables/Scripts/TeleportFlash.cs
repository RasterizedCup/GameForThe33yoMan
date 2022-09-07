using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportFlash : MonoBehaviour
{
    bool teleFlashActive;
    public float baseTeleGlare;
    public float decayRate;
    Image teleImage;
    // Start is called before the first frame update
    void Start()
    {
        teleFlashActive = false;
        teleImage = GetComponent<Image>();
        teleImage.color = new Color(teleImage.color.r, teleImage.color.g, teleImage.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(teleFlashActive)
            handleTeleFlashSustain();
    }

    public void handleTeleFlashInit()
    {
        teleFlashActive = true;
        teleImage.color = new Color(teleImage.color.r, teleImage.color.g, teleImage.color.b, baseTeleGlare);
    }

    void handleTeleFlashSustain()
    {
        float opacityVal = teleImage.color.a - (decayRate * Time.deltaTime);
        if (opacityVal < 0) {
            opacityVal = 0;
            teleFlashActive = false;
        }
        teleImage.color = new Color(teleImage.color.r, teleImage.color.g, teleImage.color.b, opacityVal);
    }
}
