using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // public float speed = 10f;
    private bool _isGameActive = true;
    private GameObject roadDestinition;
    [SerializeField] private GameObject _carBody;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float objectDistance = -40f;
    [SerializeField] private float despawnDistance = -100f;
    [SerializeField] private ParticleSystem explodeFire;
    private Animator _animator;

    private bool canspawnGround = true;
    private Rigidbody _rigidbody;
    [SerializeField] private ParticleSystem _egzozSmoke = null;
    private PlayerManager _playerManager;
    private HudController _hudController;
    private Boolean isCrashed = false;
    
    void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _hudController = FindObjectOfType<HudController>();
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
        roadDestinition = GameObject.Find("RoadDestinition");
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

    void ShakeCar()
    {
        if (_isGameActive)
        {
            _carBody.transform.DOShakePosition(0.1f, 1f, 2, 50, false, false);
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
            _animator.SetBool("crashed", true);

            explodeFire.Play();
        }
    }


    public void StartMove()
    {
        _isGameActive = true;
        if (isCrashed)
        {
            isCrashed = false;
            _animator.SetBool("crashed", false);
            Invoke("DelayedSetActive",0.1f);
        }
    }

    void DelayedSetActive()
    {
        gameObject.SetActive(false);

    }
    Vector3 GetLocalDirection(Transform transform, Vector3 destination)
    {
        return transform.InverseTransformDirection((destination - transform.position).normalized);
    }
}