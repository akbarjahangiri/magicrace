using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    private float outBound = -10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyOutBound();
    }

    void DestroyOutBound()
    {
        if (transform.position.z < outBound)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}