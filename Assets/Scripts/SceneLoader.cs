using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    private AsyncOperation _operation;
    private AudioSource _audioSource;

    public GameObject loadingScreen;

    public Slider slider;
     

    // public TextMeshProUGUI loaderText;

    // Start is called before the first frame update
    void Start()
    {
        
        _audioSource = GetComponent<AudioSource>();
    }


    public void LoadGame()
    {
        _audioSource.PlayOneShot(clickSound, 0.9f);
        StartCoroutine(LoadAsynchronously());
    }

    public void QuitGame()
    {
        _audioSource.PlayOneShot(clickSound, 0.9f);

        Application.Quit();
    }

    IEnumerator LoadAsynchronously()
    {
        _operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadingScreen.SetActive(true);
        while (!_operation.isDone)
        {
            float progress = Mathf.Clamp01(_operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}