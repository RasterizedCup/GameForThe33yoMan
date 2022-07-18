using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilMovement : MonoBehaviour
{
    Dictionary<int, Func<bool>> AbilityMap;
    // Start is called before the first frame update
    void Start()
    {
        AbilityMap = new Dictionary<int, Func<bool>>
        {
            { 1, TestDictCall}
        };

    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TestDictCall()
    {
        return true;
    }
}
