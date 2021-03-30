using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Advertisements;

public class HudController : MonoBehaviour, IUnityAdsListener
{
    public static event Action resumeAfterCrash;
    private Animator _startAnimator;
    public Button restartButton;
    public Button adButton;
    public Button pauseButton;

    public Button soundOff;
    private bool _sound = true;
    public Image soundSprite;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public TextMeshProUGUI soundText;
    public string soundOnText;
    public string soundOffText;

    public Button resumeButton;

    //turn Left ,Right buttons
    public Button turnRightButton;
    public Button turnLeftButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI starScoreText;
    public TextMeshProUGUI distance;
    public int scoreCount = 0;
    public int starScoreCount = 0;
    [SerializeField] private Image starImage;


    private GameManager _gameManager;
    private static HudController _instance;
    private Boolean _flag = true;
    private Boolean _resume = true;
    private int count = 1;

    public static HudController Instance
    {
        get { return _instance; }
    }

    public String advertisementType = "rewardedVideo";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnEnable()
    {
        PlayerManager.crashCar += EnableRestartButton;
        PlayerManager.crashCar += EnableAdButton;
        PlayerManager.crashCar += DisableTurnButtons;
        PlayerManager.HitStar += StarScoreUp;
        resumeAfterCrash += DisableRestartButton;
        PlayerManager.crashCar += EnableRestartButton;
        resumeAfterCrash += DisableAdButton;
        resumeAfterCrash += EnableTurnButtonResume;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startAnimator = starImage.GetComponent<Animator>();
        Advertisement.Initialize("4034519", true);
        Advertisement.AddListener(this);
        _gameManager = FindObjectOfType<GameManager>();


        restartButton.onClick.AddListener(RestartGame);
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        soundOff.onClick.AddListener(SoundOff);
        adButton.onClick.AddListener(ShowAdd);
    }

    private void OnDisable()
    {
        PlayerManager.crashCar -= EnableRestartButton;
        PlayerManager.HitStar -= StarScoreUp;
        PlayerManager.crashCar -= EnableAdButton;
        PlayerManager.crashCar -= DisableTurnButtons;
        PlayerManager.crashCar -= EnableRestartButton;
        resumeAfterCrash -= DisableRestartButton;
        PlayerManager.crashCar -= EnableAdButton;
        resumeAfterCrash -= DisableAdButton;
        GameManager.restartGame -= EnableTurnButtonsRestart;
        resumeAfterCrash -= EnableTurnButtonResume;
        restartButton.onClick.RemoveListener(RestartGame);
        pauseButton.onClick.RemoveListener(PauseGame);
        resumeButton.onClick.RemoveListener(ResumeGame);
        soundOff.onClick.RemoveListener(ResumeGame);
        adButton.onClick.RemoveListener(ShowAdd);
        Advertisement.RemoveListener(this);
        
    }

    private void RestartGame()

    {
        _gameManager.RestartGame();
    }

    private void EnableRestartButton()
    {
        restartButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        // _playerManager.crashCar -= EnableRestartButton;
    }

    private void DisableRestartButton()
    {
        restartButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

        // resumeAfterCrash -= DisableRestartButton;
    }

    private void EnableAdButton()
    {
        transform.Find("AdButton").gameObject.SetActive(true);
        // adButton.gameObject.SetActive(true);
        // _playerManager.crashCar -= EnableAdButton;
    }

    private void DisableAdButton()
    {
        transform.Find("AdButton").gameObject.SetActive(false);
        // adButton.gameObject.SetActive(false);
        // resumeAfterCrash -= DisableAdButton;
    }

    private void DisableTurnButtons()
    {
        turnLeftButton.gameObject.SetActive(false);
        turnRightButton.gameObject.SetActive(false);
        // _playerManager.crashCar -= DisableTurnButtons;
    }

    private void EnableTurnButtonResume()
    {
        transform.Find("TurnRightButton").gameObject.SetActive(true);
        transform.Find("TurnLeftButton").gameObject.SetActive(true);
        // turnLeftButton.gameObject.SetActive(true);
        // turnRightButton.gameObject.SetActive(true);
        // resumeAfterCrash -= EnableTurnButtonResume;
    }

    private void EnableTurnButtonsRestart()
    {
        turnLeftButton.gameObject.SetActive(true);
        turnRightButton.gameObject.SetActive(true);
        // _gameManager.restartGame -= EnableTurnButtonsRestart;
    }


    public void StarScoreUp()
    {
        starScoreCount++;
        starScoreText.text = starScoreCount.ToString("0000");
        _startAnimator.SetTrigger("HitStar");
    }

    private void PauseGame()
    {
        resumeButton.gameObject.SetActive(true);
        soundOff.gameObject.SetActive(true);
        Time.timeScale = 0;
        pauseButton.gameObject.SetActive(false);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        soundOff.gameObject.SetActive(false);
    }

    private void SoundOff()
    {
        _sound = !_sound;
        if (_sound)
        {
            AudioListener.volume = 1;
            soundSprite.sprite = soundOnSprite;
            soundText.text = soundOnText;
        }
        else
        {
            AudioListener.volume = 0;
            soundSprite.sprite = soundOffSprite;
            soundText.text = soundOffText;


        }
    }

    private void ShowAdd()
    {
        // resumeAfterCrash?.Invoke();

        if (Advertisement.IsReady(advertisementType) && _resume)
        {
            // resumeAfterCrash?.Invoke();

            Advertisement.Show(advertisementType);
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            resumeAfterCrash?.Invoke();
        }
        else if (showResult == ShowResult.Failed)
        {
            // failed
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }
}