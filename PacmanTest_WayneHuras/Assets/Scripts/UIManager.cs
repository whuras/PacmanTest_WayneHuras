using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    [SerializeField]
    private TMP_Text highScore;
    private void SetHighScore(int value) => highScore.text = value.ToString();

    [SerializeField]
    private TMP_Text currentScore;
    private void SetCurrentScore(int value) => currentScore.text = value.ToString();

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private GameObject readyText;

    [SerializeField]
    private GameObject livesObject;

    [SerializeField]
    private Image[] lives;

    private void Awake() => MaintainSingleton();

    private void Start()
    {
        int savedHighScore = SaveSystem.Instance.GetSavedHighScore();
        SetHighScore(savedHighScore);
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
        readyText.SetActive(false);
        livesObject.SetActive(false);
    }

    public void Ready()
    {
        gameOverScreen.SetActive(false);
        readyText.SetActive(true);
        livesObject.SetActive(true);
    }

    public void GameStarted()
    {
        readyText.SetActive(false);
    }

    public void UpdateCurrentScore(int currentScore)
    {
        SetCurrentScore(currentScore);
        if(currentScore > SaveSystem.Instance.GetSavedHighScore())
        {
            SaveSystem.Instance.SaveHighScore(currentScore);
            SetHighScore(currentScore);
        }
    }

    public void UpdateLives(int livesLeft)
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < livesLeft)
                lives[i].enabled = true;
            else
                lives[i].enabled = false;
        }
    }

    private void MaintainSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
