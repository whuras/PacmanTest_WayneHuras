using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    private static EnemyStateManager instance;
    public static EnemyStateManager Instance => instance;
    public enum EnemyState
    {
        Wait,
        Chase,
        Scatter,
        Run
    }

    private List<EnemyMovement> enemies = new List<EnemyMovement>();

    // Timers
    private bool pauseTimer;
    private float timer = 0;
    private float scatterTimer = 7; // Enemies scatter for 7 seconds
    private float chaseTimer = 20; // Enemies Chase for 20 seconds
    private float runTimer = 7; // Enemies Run for 7 seconds

    public void AddEnemyToEnemyStateManager(EnemyMovement enemy) => enemies.Add(enemy);

    private void Awake() => MaintainSingleton();

    public void PauseEnemies()
    {
        pauseTimer = true;
        foreach (EnemyMovement enemy in enemies)
        {
            enemy.PauseEnemyMovement();
        }
    }

    public void UnpauseEnemies()
    {
        pauseTimer = false;
        foreach (EnemyMovement enemy in enemies)
        {
            enemy.UnpauseEnemyMovement();
        }
    }

    public void ChangeState(EnemyMovement enemy, EnemyState newState)
    {
        if(newState == EnemyState.Run)
        {
            enemy.ActivateRunState(runTimer);
        }
        else
        {
            enemy.currentEnemyState = newState;
        }
    }

    public void ChangeAllEnemyStates(EnemyState newState)
    {
        foreach(EnemyMovement enemy in enemies)
        {
            ChangeState(enemy, newState);
        }
    }

    private void Update()
    {
        if (!pauseTimer)
        {
            foreach (EnemyMovement enemy in enemies)
            {
                if(enemy.currentEnemyState == EnemyState.Wait)
                {
                    // Activate Red Enemy - Immediate
                    if (enemy.gameObject.GetComponent<EnemyMovementRed>())
                    {
                        enemy.currentEnemyState = EnemyState.Scatter;
                    }

                    // Activate Pink Enemy - Immediate
                    if (enemy.gameObject.GetComponent<EnemyMovementPink>())
                    {
                        enemy.currentEnemyState = EnemyState.Scatter;
                    }

                    // Activate Blue Enemy
                    if (enemy.gameObject.GetComponent<EnemyMovementBlue>() &&
                        PelletManager.Instance.HaveThirtyPelletsBeenEaten())
                    {
                        enemy.currentEnemyState = EnemyState.Scatter;
                    }

                    // Activate Orange Enemy
                    if (enemy.gameObject.GetComponent<EnemyMovementOrange>() &&
                        PelletManager.Instance.HaveOneThirdPelletBeenEaten())
                    {
                        enemy.currentEnemyState = EnemyState.Scatter;
                    }
                }

                if (enemy.currentEnemyState != EnemyState.Wait && enemy.currentEnemyState != EnemyState.Run)
                {
                    if (timer > scatterTimer + chaseTimer)
                        timer = 0;

                    timer += Time.deltaTime;

                    if (timer < scatterTimer && enemy.currentEnemyState != EnemyState.Scatter)
                    {
                        ChangeState(enemy, EnemyState.Scatter);
                    }
                    else if (timer > scatterTimer && timer < chaseTimer && enemy.currentEnemyState != EnemyState.Chase)
                    {
                        ChangeState(enemy, EnemyState.Chase);
                    }
                }
            }
        }
    }

    private void MaintainSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
