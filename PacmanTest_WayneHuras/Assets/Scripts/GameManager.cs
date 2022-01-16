using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private List<GameObject> enemies;

    [SerializeField]
    private float newRoundWaitTime = 3f;

    [SerializeField]
    private int startingLives = 3;
    public int lives { get; private set; }
    public void DecrementLives() => lives -= 1;
    private void SetLives(int value) => lives = value;

    public int score { get; private set; }
    private void SetScore(int value) => score = value;
    
    public GameState currentGameState { get; private set; }

    private AudioManager audioManager;

    public enum GameState
    {
        Wait,
        Play,
        GameOver
    }

    private void Awake()
    {
        MaintainSingleton();
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
        EnterWaitState();
    }

    private void Update()
    {
        if(currentGameState == GameState.Wait && Input.anyKeyDown)
            EnterPlayState();
        else if (currentGameState == GameState.GameOver && Input.anyKeyDown)
            EnterWaitState();
    }

    public void EnterWaitState()
    {
        audioManager.PlayIntro();
        currentGameState = GameState.Wait;
        NewGame();
        EnemyStateManager.Instance.PauseEnemies();
        player.SetActive(false);
        UIManager.Instance.Ready();
    }

    public void EnterPlayState()
    {
        currentGameState = GameState.Play;
        EnemyStateManager.Instance.UnpauseEnemies();
        player.SetActive(true);
        UIManager.Instance.GameStarted();
        NewGame();
    }

    public void EnterGameOverState()
    {
        audioManager.PlayDeath();
        currentGameState = GameState.GameOver;
        EnemyStateManager.Instance.PauseEnemies();
        player.SetActive(false);
        UIManager.Instance.ShowGameOver();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateCurrentScore(score);
    }

    public void EatPellet(Pellet pellet)
    {
        IncreaseScore(pellet.pointValue);
        pellet.gameObject.SetActive(false);

        PelletManager pelletManager = PelletManager.Instance;
        pelletManager.DecrementRemainingPellets();

        if(pelletManager.remainingPellets <= 0)
        {
            player.SetActive(false);
            Invoke("NewRound", newRoundWaitTime);
        }
    }

    public void EatEnemy(Ghost ghost)
    {
        audioManager.PlayEatGhost();
        IncreaseScore(ghost.ghostPointValue);
    }

    public void EatPowerPellet(PowerPellet powerPellet)
    {
        EatPellet(powerPellet);
        EnemyStateManager.Instance.ChangeAllEnemyStates(EnemyStateManager.EnemyState.Run);
    }

    public void EatPlayer()
    {
        DecrementLives();
        ResetPositions();

        UIManager.Instance.UpdateLives(lives);
        
        if (lives <= 0)
        {
            EnterGameOverState();
        }
    }

    private void ResetPositions()
    {
        foreach(GameObject go in enemies)
        {
            go.GetComponent<EnemyMovement>().RestartEnemy();
        }

        player.GetComponent<PlayerMovement>().RestartPlayer();
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(startingLives);
        NewRound();
    }

    private void NewRound()
    {
        ResetPositions();
        UIManager.Instance.UpdateLives(lives);
        player.SetActive(true);
        PelletManager pelletManager = PelletManager.Instance;
        pelletManager.ResetPellets();
    }

    private void MaintainSingleton()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
