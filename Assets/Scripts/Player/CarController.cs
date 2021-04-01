
using UnityEngine;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float forwardAccel = 3f, reverseAccel = 4f, maxSpeed = 15f, turnStrength = 180;
    private float _turnInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        _turnInput = Input.GetAxis("Horizontal");

        transform.position = _rigidbody.transform.position;
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(transform.forward * (forwardAccel * 200f));
        if (_turnInput != 0 && _turnInput > 0)
        {
            _rigidbody.DOMoveX(0.3f, 0.8f).SetEase(Ease.InOutCirc);
        }

        if (_turnInput != 0 && _turnInput < 0)
        {
            _rigidbody.transform.DOMoveX(-0.3f, 0.8f).SetEase(Ease.InOutCirc);
        }
    }
}