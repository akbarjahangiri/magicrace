using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    private Boolean _gameActive = true;

    public static event Action crashCar;
    public static event Action HitStar;

    private Rigidbody _rigidbody;
    public float movementSpeed = 0.1f;
    private float _distance;
    private float _distanceIn = 0;
    private Vector3 _playerReScale = new Vector3(0.36f, 0.3f, 0.24f);


    public ParticleSystem starParticle;
    public ParticleSystem _leftBackSmoke = null;
    public ParticleSystem _rightBackSmoke = null;

    public AudioClip crashSound;
    private HudController _hudController;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        _hudController = FindObjectOfType<HudController>();
        _audioSource = GetComponent<AudioSource>();
        HudController.resumeAfterCrash += PlaySmokeParticles;
        HudController.resumeAfterCrash += StartMove;
    }

    private void OnDisable()
    {
        HudController.resumeAfterCrash -= PlaySmokeParticles;
        HudController.resumeAfterCrash -= StartMove;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(PlayerKillometerDistance());
    }


    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetAxis("Horizontal") == 1)
        {
            GoRightTweens();
        }
        else if (CrossPlatformInputManager.GetAxis("Horizontal") == -1)
        {
            GoLeftTweens();
        }
    }

    private void FixedUpdate()
    {
        if (_gameActive)
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        var velocity = Vector3.forward * movementSpeed;
        _rigidbody.AddRelativeForce(velocity - _rigidbody.velocity, ForceMode.VelocityChange);
    }

    void StopMove()
    {
        _gameActive = false;
    }

    void StartMove()
    {
        _gameActive = true;
    }

    void GoLeftTweens()
    {
        var myTween = transform.DOMoveX(-0.3f, 0.8f).SetEase(Ease.OutQuart);
        transform.DOScale(_playerReScale, 0.4f);
        transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.4f);
        myTween.WaitForCompletion();
    }

    void GoRightTweens()
    {
        var myTween = transform.DOMoveX(0.3f, 0.8f).SetEase(Ease.OutQuart);
        transform.DOScale(_playerReScale, 0.4f);
        transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.4f);
        myTween.WaitForCompletion();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("star"))
        {
            HitStar?.Invoke();
            other.gameObject.SetActive(false);
            starParticle.Play();
            SaveManager.instance.activeSave.starRecord++;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("car"))
        {
            StopMove();
            _audioSource.PlayOneShot(crashSound, 1.0f);
            StopSmokeParticles();
            if (SaveManager.instance.hasLoaded)
            {
                if (_distance > SaveManager.instance.activeSave.distanceRecord)
                {
                    SaveManager.instance.activeSave.distanceRecord = _distance;
                }

                SaveManager.instance.Save();
            }

            crashCar?.Invoke();
        }
    }

    private void StopSmokeParticles()
    {
        if (_leftBackSmoke != null && _rightBackSmoke != null)
        {
            _leftBackSmoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _rightBackSmoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void PlaySmokeParticles()
    {
        if (!(_leftBackSmoke is null)) _leftBackSmoke.Play();
        if (!(_rightBackSmoke is null)) _rightBackSmoke.Play();
    }

    IEnumerator PlayerKillometerDistance()
    {
        while (true)
        {
            _distance = (float) (Math.Round((transform.position.z) / 100));
            if (_distance % 6 == 0 && _distance < 60 && _distanceIn != _distance)
            {
                movementSpeed *= 1.01f;
                _distanceIn = _distance;
            }

            _hudController.distance.text = "KM: " + _distance.ToString("000");
            yield return new WaitForSeconds(5);
        }
    }
}