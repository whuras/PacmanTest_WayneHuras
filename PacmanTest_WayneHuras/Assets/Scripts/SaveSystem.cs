using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem instance;
    public static SaveSystem Instance => instance;

    private void Awake() => MaintainSingleton();

    public int GetSavedHighScore()
    {
        int highScore = PlayerPrefs.GetInt("highScore", 0);
        return highScore;
    }

    public void SaveHighScore(int highScore)
    {
        PlayerPrefs.SetInt("highScore", highScore);
    }

    private void MaintainSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
