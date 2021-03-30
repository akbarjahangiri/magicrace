using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    public Rigidbody rigidbody;

    public float forwardAccel = 3f, reverseAccel = 4f, maxSpeed = 15f, turnStrength = 180;
    private float _turnInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.transform.parent = null;
        // Physics.IgnoreCollision(rigidbody.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        _turnInput = Input.GetAxis("Horizontal");
        // if (_turnInput != 0)
        // {
        //     transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
        //                                           new Vector3(0f, _turnInput * turnStrength * Time.deltaTime, 0f));
        // }

        transform.position = rigidbody.transform.position;
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(transform.forward * forwardAccel * 200f);
        // transform.DOMoveX(-0.3f, 0.3f);
        if (_turnInput != 0 && _turnInput > 0)
        {
            rigidbody.DOMoveX(0.3f, 0.8f).SetEase(Ease.InOutCirc);
        }

        if (_turnInput != 0 && _turnInput < 0)
        {
            rigidbody.transform.DOMoveX(-0.3f, 0.8f).SetEase(Ease.InOutCirc);
        }
    }
}