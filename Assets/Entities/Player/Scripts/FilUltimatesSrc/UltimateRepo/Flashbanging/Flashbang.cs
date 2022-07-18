using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashbang : MonoBehaviour
{
    public float FlashEffectDuration;
    public float MaxOpacityValue;
    public float MaxFlashLingerDuration;
    public float MaxSpriteLingerDuration;
    public float OpacityIncreaseRate;
    public float OpacityDecreaseRate;
    public Sprite BaseFilSprite;
    public Sprite SusFilSprite;
    AudioSource UltimateTrigger;
    GameObject FlashSpriteObj;
    GameObject SusSpriteObj;
    GameObject ColliderObj;
    Color FlashStartColor;
    Color FlashDeltaAdjuster;
    Color SusFlashAdjuster;
    public AudioClip FlashBangAudioClip;
    float flashTimerStart;
    float flashTimerEnd;
    protected SpriteRenderer spriteRenderer;

    public float SusFlashOpacityReductionMulti;
    public float SusFlashBaseOpacity;

    public static bool flashbangSpriteActive;
    public static bool flashbangActive;
    // Start is called before the first frame update
    void Start()
    {
        flashbangSpriteActive = false;
        flashbangActive = false;
        FlashStartColor = new Color(255, 255, 255, MaxOpacityValue);
        UltimateTrigger = GetComponent<AudioSource>();
        FlashSpriteObj = GameObject.Find("WhiteSprite");
        SusSpriteObj = GameObject.Find("SuslianFlash");
        ColliderObj = GameObject.Find("FilHealthObj");
        spriteRenderer = GameObject.Find("FilSprite").GetComponent<SpriteRenderer>();
    }

    public bool handleFlashbang()
    {
        //TODO: custom mapping
        if (!flashbangActive)
        {
            AdjustUltimateBar.killDisplay = false; // let AUB scripts handle with this
            FilUltimates.currentUltCharge = 0;
            flashbangActive = true;
            FlashSpriteObj.GetComponent<Image>().color = FlashStartColor;
            SusFlashAdjuster = FlashStartColor;
            SusFlashAdjuster.a = SusFlashBaseOpacity;
            SusSpriteObj.GetComponent<Image>().color = SusFlashAdjuster;
            FlashDeltaAdjuster = FlashStartColor;
            flashTimerEnd = Time.time + MaxFlashLingerDuration;
            flashTimerStart = Time.time;
            UltimateTrigger.Play();
            ColliderObj.tag = "PlayerFlashbang";
            spriteRenderer.sprite = SusFilSprite;
            flashbangSpriteActive = true;
        }
        else
        {
            SetSuslianDecrease();// we want the sus sprite itself to start disappearing instantly
            if (flashTimerStart + MaxSpriteLingerDuration < Time.time)
            {
                spriteRenderer.sprite = BaseFilSprite;
                flashbangSpriteActive = false;
            }

            if(flashTimerStart + FlashEffectDuration < Time.time)
            {
                ColliderObj.tag = "Player";
                flashbangActive = false;
                return false;
            }
            SetDecrease();
        }

        return true;
    }

    void SetIncrease()
    {

    }

    void SetDecrease()
    {
        if (flashTimerEnd < Time.time && FlashSpriteObj.GetComponent<Image>().color.a > 0)
        {
            FlashDeltaAdjuster.a -= OpacityDecreaseRate * Time.deltaTime;
            FlashSpriteObj.GetComponent<Image>().color = FlashDeltaAdjuster;
        }
    }

    void SetSuslianDecrease()
    {
        if (SusSpriteObj.GetComponent<Image>().color.a > 0)
        {
            SusFlashAdjuster.a -= OpacityDecreaseRate * Time.deltaTime * SusFlashOpacityReductionMulti;
            SusSpriteObj.GetComponent<Image>().color = SusFlashAdjuster;
        }
    }
}
