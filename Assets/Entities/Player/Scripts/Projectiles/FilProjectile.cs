using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilProjectile : MonoBehaviour
{
    public float velocity;
    protected Vector3 mousePosition;
    protected Vector3 targetPosition;
    protected Vector3 travelToPos;
    protected bool markForDestruction;
    protected bool primingDestroy;
    public float timeTillDestroy;
    protected float baseTime;
    protected AudioSource soundSource;
    public AudioClip knifeThrow;
    public AudioClip knifeHit;

    public int DamageDealt;

}
