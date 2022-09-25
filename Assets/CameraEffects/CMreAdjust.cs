using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CMreAdjust : MonoBehaviour
{
    // ** list property only applies to colliders with the SAME dimensions
    public List<BoxCollider> confinersToAdjust;
    public Vector3 CenterVals;
    public Vector3 SizeVals;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang"))
        {
            foreach(var confinerToAdjust in confinersToAdjust)
            {
                confinerToAdjust.center = CenterVals;
                confinerToAdjust.size = SizeVals;
            }
        }
    }
}
