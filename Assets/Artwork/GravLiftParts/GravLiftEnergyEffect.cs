using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravLiftEnergyEffect : MonoBehaviour
{
    public Transform SpawnYMin;
    public Transform SpawnXMin;
    public Transform SpawnYMax;
    public Transform SpawnXMax;
    public Transform EnergyResetPoint;
    float minX, maxX, minY, maxY, energyReset;
    float x, y;
    float velocity;
    public float VelMin, VelMax;
    // Start is called before the first frame update
    void Start()
    {
        minX = SpawnXMin.localPosition.x;
        maxX = SpawnXMax.localPosition.x;
        minY = SpawnYMin.localPosition.y;
        maxY = SpawnYMax.localPosition.y;
        energyReset = EnergyResetPoint.localPosition.y;
        x = Random.Range(minX, maxX);
        y = Random.Range(minY, maxY);
        velocity = Random.Range(VelMin, VelMax);
        transform.position = new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleEnergyTraversal();
    }

    void HandleEnergyTraversal()
    {
        if(transform.localPosition.y > energyReset)
        {
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            velocity = Random.Range(VelMin, VelMax);
            transform.localPosition = new Vector3(x, y, 0);
        }
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime * velocity);
        }
    }
}
