using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int score;
    // public int carName;

    public PlayerData(HudController hudController)
    {
        score = hudController.starScoreCount;
    }
}