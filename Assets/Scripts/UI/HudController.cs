using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class HudController : MonoBehaviour, IUnityAdsListener
{
    public static event Action resumeAfterCrash;
    private Animator _startAnimator;

    //Managements buttons
    public Button restartButton;
    public Button adButton;
    public Button pauseButton;

    public Button resumeButton;

    //turn Left ,Right buttons
    public Button turnRightButton;

    public Button turnLeftButton;

    //sound UI
    public Button soundOff;
    private bool _sound = true;
    public Image soundSprite;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public TextMeshProUGUI soundText;
    public string soundOnText;

    public string soundOffText;

    //ScoreBar
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI starScoreText;
    public TextMeshProUGUI distance;
    public int scoreCount = 0;
    public int starScoreCount = 0;
    [SerializeField] private Image starImage;

    //Advertisement
    public String advertisementType = "rewardedVideo";

    private GameManager _gameManager;


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
        _gameManager = FindObjectOfType<GameManager>();
        _startAnimator = starImage.GetComponent<Animator>();
        
        Advertisement.Initialize("4034519", true);
        Advertisement.AddListener(this);
        
        restartButton.onClick.AddListener(RestartGame);
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        soundOff.onClick.AddListener(SoundOff);
        adButton.onClick.AddListener(ShowAdd);
    }

    private void OnDisable()
    {
        PlayerManager.crashCar -= EnableRestartButton;
        PlayerManager.crashCar -= EnableAdButton;
        PlayerManager.crashCar -= DisableTurnButtons;
        PlayerManager.crashCar -= EnableRestartButton;
        PlayerManager.crashCar -= EnableAdButton;
        PlayerManager.HitStar -= StarScoreUp;
        
        resumeAfterCrash -= DisableRestartButton;
        resumeAfterCrash -= DisableAdButton;
        resumeAfterCrash -= EnableTurnButtonResume;
        
        GameManager.restartGame -= EnableTurnButtonsRestart;
        
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
    }

    private void DisableRestartButton()
    {
        restartButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    private void EnableAdButton()
    {
        transform.Find("AdButton").gameObject.SetActive(true);
    }

    private void DisableAdButton()
    {
        transform.Find("AdButton").gameObject.SetActive(false);
    }

    private void DisableTurnButtons()
    {
        turnLeftButton.gameObject.SetActive(false);
        turnRightButton.gameObject.SetActive(false);
    }

    private void EnableTurnButtonResume()
    {
        transform.Find("TurnRightButton").gameObject.SetActive(true);
        transform.Find("TurnLeftButton").gameObject.SetActive(true);
    }

    private void EnableTurnButtonsRestart()
    {
        turnLeftButton.gameObject.SetActive(true);
        turnRightButton.gameObject.SetActive(true);
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
        if (Advertisement.IsReady(advertisementType))
        {
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