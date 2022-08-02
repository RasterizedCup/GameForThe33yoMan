using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPickupSound : MonoBehaviour
{
    int baseCount;
    static AudioSource pickupNoise;
    // Start is called before the first frame update
    void Start()
    {
        baseCount = gameObject.GetComponentsInChildren<Transform>().Length;
        pickupNoise = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        if(ts.Length < baseCount)
        {
            baseCount--;
            pickupNoise.Play();
        }
    }

    public static void playPickup()
    {
        pickupNoise.Play();
    }
}
