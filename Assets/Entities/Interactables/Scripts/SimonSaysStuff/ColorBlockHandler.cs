using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ColorBlockHandler : MonoBehaviour
{
    public ColorSequenceController colorSeqBrain;
    public int numSeqValue;
    public Light2D lightSource;
    public Light2D successLight;
    public SpriteRenderer disabledBlock;
    public SpriteRenderer enabledBlock;
    public SpriteRenderer successBlock;
    public float EnableDuration;
    float proxyDisableDuration; // for startup display
    public float RenableDelay;
    public float SuccessVisibilityDelay;
    bool validBlock;
    float currTime;
    float maxLightIntensity;
    Color offColor, onColor;
    bool blockEnabled, primeSuccess, wasASuccess;
    AudioSource blockSound;
    // Start is called before the first frame update
    void Start()
    {
        proxyDisableDuration = EnableDuration;
        blockSound = GetComponent<AudioSource>();
        maxLightIntensity = 2;
        offColor = new Color(255, 255, 255, 0);
        onColor = new Color(255, 255, 255, 255);
        enabledBlock.color = offColor;
        disabledBlock.color = onColor;
        lightSource.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if((lightSource.intensity == maxLightIntensity || successLight.intensity == maxLightIntensity) && !primeSuccess)
            HandleDeactivateEnableBlock();

        if (primeSuccess)
            FireOffSuccessBlockDisplay();
    }

    void HandleDeactivateEnableBlock()
    {
        // handle deactivation after enable duration is over
        // set renable threshold
        if(currTime + proxyDisableDuration < Time.time)
        {
            if (wasASuccess)
            {
                wasASuccess = false;
                colorSeqBrain.shouldDisplay = true;
            }
            proxyDisableDuration = EnableDuration;
            disabledBlock.color = onColor;
            enabledBlock.color = offColor;
            successBlock.color = offColor;
            lightSource.intensity = 0;
            successLight.intensity = 0;
            currTime = Time.time;
            blockEnabled = false; // move to renableThreshold Logic
        }
    }

    void HandleRenableThreshold()
    {

    }

    public void SetFireOffSuccessTimer() 
    {
        blockEnabled = true;
        primeSuccess = true;
        currTime = Time.time;
    }

    void FireOffSuccessBlockDisplay()
    {
        if (currTime + SuccessVisibilityDelay < Time.time)
        {
            wasASuccess = true;
            primeSuccess = false;
            proxyDisableDuration = 1.5f;
            blockEnabled = true;
            disabledBlock.color = offColor;
            enabledBlock.color = offColor;
            successBlock.color = onColor;
            successLight.intensity = maxLightIntensity;
            lightSource.intensity = 0;
            currTime = Time.time;
        }
    }

    public void FireOffBlockDisplay()
    {
        proxyDisableDuration = .8f;
        blockSound.Play();
        blockEnabled = true;
        disabledBlock.color = offColor;
        successBlock.color = offColor;
        enabledBlock.color = onColor;
        lightSource.intensity = maxLightIntensity;
        currTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !blockEnabled && 
            !colorSeqBrain.totalSuccess && 
            !colorSeqBrain.shouldDisplay &&
            !colorSeqBrain.performStartup)
        {
            blockSound.Play();
            blockEnabled = true;
            disabledBlock.color = offColor;
            enabledBlock.color = onColor;
            lightSource.intensity = maxLightIntensity;
            currTime = Time.time;
            validBlock = colorSeqBrain.ReceiveAndCheckBlockInput(numSeqValue);
        }
    }
}
