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
    private EnemyMovement[] enemies;

    public int score { get; private set; }
    public void IncreaseScore(int amount) => score += amount;

    private void Awake()
    {
        MaintainSingleton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Welcome");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Level");
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
            StartCoroutine(NewRound());
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

    private IEnumerator NewRound()
    {
        yield return new WaitForSeconds(3);

        // TODO New Round
        Debug.Log("New Round");

        yield return null;
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
