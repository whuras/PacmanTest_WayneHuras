using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    private static EnemyStateManager instance;
    public static EnemyStateManager Instance => instance;
    public enum EnemyState
    {
        Chase,
        Scatter,
        Run
    }

    private List<EnemyMovement> enemies = new List<EnemyMovement>();
    public void AddEnemyToEnemyStateManager(EnemyMovement enemy) => enemies.Add(enemy);

    private void Awake() => MaintainSingleton();

    public void ChangeState(EnemyState newState)
    {
        foreach(EnemyMovement enemy in enemies)
        {
            enemy.currentEnemyState = newState;
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
            DontDestroyOnLoad(gameObject);
        }
    }
}