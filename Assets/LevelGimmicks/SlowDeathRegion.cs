using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDeathRegion : MonoBehaviour
{
    FilHealth filHealth;
    public float damageTick;
    // Start is called before the first frame update
    void Start()
    {
        filHealth = GameObject.Find("FilHealthObj").GetComponent<FilHealth>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("Player"))
        {
            filHealth.CurrentHealth -= damageTick * Time.deltaTime;
        }
    }
}
