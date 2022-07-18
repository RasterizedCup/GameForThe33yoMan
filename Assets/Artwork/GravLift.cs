using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravLift : MonoBehaviour
{
    public float Thrust;
    AudioSource liftNoise;
    // Start is called before the first frame update
    private void Start()
    {
        liftNoise = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // propel player upwards with Thrust amount of thrust
        if(collision.gameObject.tag == "PlayerBase")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * Thrust);
            liftNoise.Play();
        }
    }
}
