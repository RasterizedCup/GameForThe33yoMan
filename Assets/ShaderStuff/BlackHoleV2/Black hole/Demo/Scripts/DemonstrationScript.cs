using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonstrationScript : MonoBehaviour
{
    // Start is called before the first frame update
    float tm = 0f;
    public float speed = 1f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tm %= 38.6f;
        if (tm >= 1f && tm < 10f) transform.position += new Vector3(0, 0, -Time.deltaTime / 7 * speed);
        if (tm >= 11f && tm < 17f) transform.position += new Vector3(0, 0, Time.deltaTime / 7 * speed);
        if(tm >= 18f && tm < 20.3f) transform.position += new Vector3(0, 0, -Time.deltaTime / 7 * speed);
        if (tm >= 21.3f && tm < 31.3f) Camera.main.transform.Rotate(0, 36 * Time.deltaTime * speed, 0);
        if (tm >= 32.3f && tm < 37.6f) transform.position += new Vector3(0, 0, Time.deltaTime / 7 * speed);
        tm += Time.deltaTime*speed;
    }
}
