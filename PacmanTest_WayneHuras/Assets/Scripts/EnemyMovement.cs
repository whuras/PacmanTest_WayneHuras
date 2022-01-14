using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : Movement
{
    [SerializeField]
    private GameObject homeGameObject;
    private Node homeNode;

    public EnemyStateManager.EnemyState currentEnemyState;
    private void Start()
    {
        EnemyStateManager.Instance.AddEnemyToEnemyStateManager(this);
        homeNode = GetHomeNode(homeGameObject);
    }

    private void Update()
    {
        if (currentEnemyState == EnemyStateManager.EnemyState.Chase)
        {
            Chase();
        }
        else if (currentEnemyState == EnemyStateManager.EnemyState.Scatter)
        {
            Scatter();
        }
        else if (currentEnemyState == EnemyStateManager.EnemyState.Run)
        {
            Run();
        }
    }

    protected abstract void Chase();

    private void Run()
    {
        // common with homeNode
    }

    private void Scatter()
    {
        // common
    }

    protected Node GetHomeNode(GameObject homeGameObject)
    {
        Vector3 homePosition = homeGameObject.transform.position;
        Node closestNode = NodeManager.Instance.ClosestNode(homePosition);
        return closestNode;
    }
    protected abstract Node TargetNode();
}
