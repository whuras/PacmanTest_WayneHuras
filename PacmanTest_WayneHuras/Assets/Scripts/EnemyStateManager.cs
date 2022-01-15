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
    private float timer = 0;
    private float scatterTimer = 7; // Enemies scatter for 7 seconds
    private float chaseTimer = 20; // Enemies Chase for 20 seconds
    private float runTimer = 7; // Enemies Run for 10 seconds

    public void AddEnemyToEnemyStateManager(EnemyMovement enemy) => enemies.Add(enemy);

    private void Awake() => MaintainSingleton();

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
        foreach(EnemyMovement enemy in enemies)
        {
            if(enemy.currentEnemyState != EnemyState.Wait && enemy.currentEnemyState != EnemyState.Run)
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
