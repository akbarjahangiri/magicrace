using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnManager : MonoBehaviour
{
    public GameObject[] cars;
    private float[] xPosition = { -0.3f, 0.3f };
    private float yPosition = 0f;
    private float zPosition = 16f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomCars", 1f, Random.Range(1.5f,3));

    }

    // Update is called once per frame
    void Update()
    {
    }
    void SpawnRandomCars()
    {
        int xPositionRandom = Random.Range(0, xPosition.Length);
        int carIndexRandom = Random.Range(0, cars.Length);
        Vector3 carPosition = new Vector3(xPosition[xPositionRandom], yPosition, zPosition);
        Instantiate(cars[carIndexRandom],carPosition,Quaternion.Inverse(cars[carIndexRandom].transform.rotation));
    }
}