using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static event Action restartGame;
    public bool isGameActive = true;

    //spawn scene objects properties
    public GameObject playerCar;
    public readonly float[] _carXPosition = {-0.3f, 0.3f};
    public readonly float _carYPosition = 0f;
    public readonly float[] _starXPosition = {-0.3f, 0.3f};
    public readonly float _starYPosition = 0.14f;
    private ObjectSpawner _objectSpawner;

    //loading scene 
    private AsyncOperation _operation;
    [SerializeField] private AudioClip clickSound;
    private AudioSource _audioSource;
    [SerializeField] private GameObject loadingScreen;
    public Slider slider;


    private void OnEnable()
    {
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
        _audioSource = GetComponent<AudioSource>();

        _objectSpawner.InitialSpawnRoad();
        StartCoroutine(SpawnCars());
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
        LoadGame();
    }

    public void StopGame()
    {
        isGameActive = false;
        StopAllCoroutines();
    }

    public void ResumeGameCrash()
    {
        isGameActive = true;
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

    #region Car

    //this function made because cant use IEnumerator in event
    private void SpawnCar()
    {
        StartCoroutine(SpawnCars());
    }

    private IEnumerator SpawnCars()
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
            yield return null;
        }
    }
}