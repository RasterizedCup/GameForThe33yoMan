using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterFowarding : MonoBehaviour
{
    MissileLogic parent;
    // Start is called before the first frame update
    private void Start()
    {
        parent = transform.parent.GetComponent<MissileLogic>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        parent.OnTriggerEnter2D(collision);
    }
}
