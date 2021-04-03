using System;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private bool _isGameActive = true;
    private Animator _animator;
    private Rigidbody _rigidbody;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private ParticleSystem explodeFire;
    [SerializeField] private ParticleSystem _egzozSmoke = null;
    [SerializeField] private Boolean isCrashed = false;

    void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        PlayerManager.crashCar += StopMove;
        HudController.resumeAfterCrash += StartMove;
    }

    private void OnDisable()
    {
        PlayerManager.crashCar -= StopMove;
        HudController.resumeAfterCrash -= StartMove;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isGameActive)
        {
            var velocity = Vector3.forward * speed;
            _rigidbody.AddRelativeForce(velocity - _rigidbody.velocity, ForceMode.VelocityChange);
        }
    }

    public void StopMove()
    {
        _isGameActive = false;
        if (_egzozSmoke != null)
        {
            _egzozSmoke.Stop();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("player"))
        {
            isCrashed = true;
            // _animator.SetBool("crashed", true);
            // explodeFire.Play();
        }
    }


    public void StartMove()
    {
        _isGameActive = true;
        if (isCrashed)
        {
            isCrashed = false;
            // _animator.SetBool("crashed", false);
            Invoke("DelayedSetActive", 0.1f);
        }
    }

    void DelayedSetActive()
    {
        gameObject.SetActive(false);
    }
}