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
    public void DecrementLives() => lives -= 0;
    private void SetLives(int value) => lives = value;

    public int score { get; private set; }
    public void IncreaseScore(int amount) => score += amount;
    private void SetScore(int value) => score = value;

    private void Awake()
    {
        MaintainSingleton();
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Welcome");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NewGame();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("GameOver");
        }
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

        if(lives <= 0)
        {
            GameOver();
        }
        else
        {
            ResetPositions();
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
        Debug.Log("New Round");
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

    private void GameOver()
    {

    }
}
