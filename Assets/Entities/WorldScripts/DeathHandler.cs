using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    BoxCollider2D deathRegion;
    public Vector3 resetPosition;
    // Start is called before the first frame update
    void Start()
    {
        deathRegion = GetComponent<BoxCollider2D>();
    }
}
