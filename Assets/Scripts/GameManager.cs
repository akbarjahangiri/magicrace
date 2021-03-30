using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public UnityEvent endGame;
    public List<GameObject> enemyCars;
    public GameObject playerCar;
    private int _gameScore;
    private float _roadLenght;
    private readonly Vector3 _firstRoadPos = new Vector3(0, 0, 0);
    private readonly Vector3 _secondRoadPos = new Vector3(0, 0, 30);
    public bool isGameActive = true;
    public List<GameObject> roads = new List<GameObject>(2);
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;
    public GameObject[] cars;
    public readonly float[] _carXPosition = {-0.3f, 0.3f};
    public readonly float _carYPosition = 0f;
    public readonly float _carZPosition = 12f;

    public readonly float[] _starXPosition = {-0.3f, 0.3f};
    public readonly float _starYPosition = 0.14f;
    public readonly float _starZposition = 8f;

    private PlayerManager _playerManager;
    private HudController _hudController;
    private ObjectSpawner _objectSpawner;

    private Boolean flag = true;

    private static GameManager _instance;
    private int count = 1;

    [SerializeField] private AudioClip clickSound;
    private AsyncOperation _operation;
    private AudioSource _audioSource;

    public GameObject loadingScreen;

    public Slider slider;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    public static event Action restartGame;

    private void Awake()
    {
        // _playerManager = FindObjectOfType<PlayerManager>();
        // _hudController = FindObjectOfType<HudController>();
        // _objectSpawner = FindObjectOfType<ObjectSpawner>();
        // if (_instance != null && _instance != this)
        // {
        //     Destroy(this.gameObject);
        // }
        // else
        // {
        //     _instance = this;
        // }

        //
    }

    private void OnEnable()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        _hudController = FindObjectOfType<HudController>();
        _objectSpawner = FindObjectOfType<ObjectSpawner>();
        PlayerManager.crashCar += StopGame;
        HudController.resumeAfterCrash += ResumeGameCrash;
        HudController.resumeAfterCrash += SpawnCar;
        HudController.resumeAfterCrash += SpawnStars;
        restartGame += OnRestart;
    }

    // Start is called before the first frame update
    void Start()
    {
        _objectSpawner.InitialSpawnRoad();
        _audioSource = GetComponent<AudioSource>();

        // PlayerManager.crashCar += StopGame;
        // SpawnPlayer();
        // SpawnRoads(roads);
        // Invoke("SpawnCar", 2);
        // Invoke("SpawnStars", 4);
        StartCoroutine(x());
        // StartCoroutine(SpawnEnemyCar());
        StartCoroutine(SpawnStar());
    }

    private void OnRestart()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        PlayerManager.crashCar -= StopGame;
        HudController.resumeAfterCrash -= ResumeGameCrash;
        HudController.resumeAfterCrash -= SpawnCar;
        HudController.resumeAfterCrash -= SpawnStars;
        restartGame -= OnRestart;
    }

    public void RestartGame()
    {
        HudController.resumeAfterCrash -= SpawnCar;
        HudController.resumeAfterCrash -= SpawnStars;
        restartGame?.Invoke();
        // restartGame = null;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LoadGame();

        // SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    // IEnumerator LoadScene
    public void StopGame()
    {
        isGameActive = false;
        StopAllCoroutines();
        // CancelInvoke();
    }

    public void ResumeGameCrash()
    {
        isGameActive = true;
        // StartCoroutine(SpawnEnemyCar());
        // StartCoroutine(SpawnStar());
    }

    #region PlayerCar

    private void SpawnPlayer()
    {
        Instantiate(playerCar, new Vector3(-0.3f, 0f, 1.5f), Quaternion.Inverse(playerCar.transform.rotation));
    }

    #endregion

    #region Star

    private void SpawnStars()
    {
        StartCoroutine(SpawnStar());
        // if (isGameActive)
        // {
        //     _objectSpawner.SpawnStar();
        //     var randomTime = Random.Range(1, 5);
        //     Invoke("SpawnStars", randomTime);
        // }
    }

    private IEnumerator SpawnStar()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(Random.Range(10, 15));
            if (isGameActive)
            {
                _objectSpawner.SpawnStar();
            }
        }
    }

    #endregion

    #region EnemyCar

    private void SpawnCar()
    {
        StartCoroutine(SpawnEnemyCar());
        // if (isGameActive)
        // {
        //     _objectSpawner.SpawnCar();
        //     var randomTime = Random.Range(1, 5);
        //     Invoke("SpawnCar", randomTime);
        // }
    }

    private IEnumerator SpawnEnemyCar()
    {
        count++;

        while (isGameActive)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3.5f));
            if (isGameActive)
            {
                _objectSpawner.SpawnCar();
            }
        }
    }

    private IEnumerator x()
    {


        while (isGameActive)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3.5f));
            if (isGameActive)
            {
                _objectSpawner.SpawnCar();
            }
        }
    }

    #endregion

    public void LoadGame()
    {
        _audioSource.PlayOneShot(clickSound, 0.9f);
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        _operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        loadingScreen.SetActive(true);
        while (!_operation.isDone)
        {
            float progress = Mathf.Clamp01(_operation.progress / 0.9f);
            slider.value = progress;
            // loaderText.text = (int)(progress * 100) + "%";
            yield return null;
        }
    }
}