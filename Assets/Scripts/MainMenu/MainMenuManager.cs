using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI starScore;

    public TextMeshProUGUI distanceScore;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.instance.hasLoaded)
        {
            starScore.text = "star:" + (SaveManager.instance.activeSave.starRecord).ToString("0000");
            distanceScore.text = "Km:" + (SaveManager.instance.activeSave.distanceRecord).ToString("0000");
        }
    }

    public void SoundOff()
    {
        AudioListener.pause = !AudioListener.pause;
    }
}