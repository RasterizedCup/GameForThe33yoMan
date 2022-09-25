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
        minX = SpawnXMin.position.x;
        maxX = SpawnXMax.position.x;
        minY = SpawnYMin.position.y;
        maxY = SpawnYMax.position.y;
        energyReset = EnergyResetPoint.position.y;
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
        if(transform.position.y > energyReset)
        {
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            velocity = Random.Range(VelMin, VelMax);
            transform.position = new Vector3(x, y, 0);
        }
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime * velocity);
        }
    }
}
