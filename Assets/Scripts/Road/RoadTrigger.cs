using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTrigger : MonoBehaviour
{
    private ObjectSpawner _objectSpawner;

    private void OnEnable()
    {
        _objectSpawner = FindObjectOfType<ObjectSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _objectSpawner.SpawnGround();
            gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);


            // gameObject.SetActive(false);
        }
    }
}