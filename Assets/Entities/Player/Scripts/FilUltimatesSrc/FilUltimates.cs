using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilUltimates : MonoBehaviour
{
    public Flashbang flashbang;
    public LaserBeam laserbeam;
    public BlackHole blackhole;
    public float maxUltCharge;
    public float ultChargeRegenRate;
    public static float currentUltCharge;
    // Start is called before the first frame update
    void Start()
    {
        currentUltCharge = 0;
    }

    public float getCurrentUltCharge()
    {
        return currentUltCharge / maxUltCharge;
    }

    protected void handleUltimateRegen()
    {
        if (currentUltCharge < maxUltCharge)
        {
            currentUltCharge += (ultChargeRegenRate * Time.deltaTime);
        }
        if (currentUltCharge > maxUltCharge)
        {
            currentUltCharge = maxUltCharge;
        }
    }

    // Update is called once per frame
    protected bool handleFlashbang()
    {
        return flashbang.handleFlashbang();
    }

    protected bool handleLaserBeam()
    {
        return laserbeam.HandleLaserBeam();
    }

    protected bool handleBlackHole()
    {
        return blackhole.HandleBlackHole();
    }
}
