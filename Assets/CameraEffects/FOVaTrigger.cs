using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVaTrigger : MonoBehaviour
{
    Transform fovTrig;
    public float expansionRate;
    public float targetFOV;
    // Start is called before the first frame update
    void Start()
    {
        fovTrig = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getExRate()
    {
        return expansionRate;
    }
    public float getTarget()
    {
        return targetFOV;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(fovTrig.position, fovTrig.localScale);
    }
}
