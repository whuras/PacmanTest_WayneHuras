using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : Movement
{
    [SerializeField]
    private GameObject homeGameObject;
    private Node homeNode;
    protected Node prevNode;

    public EnemyStateManager.EnemyState currentEnemyState = EnemyStateManager.EnemyState.Scatter; // set for testing
    private new void Start()
    {
        base.Start();
        EnemyStateManager.Instance.AddEnemyToEnemyStateManager(this);
        homeNode = GetHomeNode(homeGameObject);
        
        targetNode = NextChaseNode();
    }

    private void Update()
    {
        if (targetNode != null)
        {
            MoveToNode(targetNode);
        }
    }

    protected override void MoveToNode(Node node)
    {
        isMoving = true;

        transform.position += speed * Time.deltaTime * ((Vector3)node.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, node.position) <= reachedDistance)
        {
            transform.position = node.position;

            prevNode = currentNode;
            currentNode = targetNode;

            if (currentEnemyState == EnemyStateManager.EnemyState.Chase)
            {
                targetNode = NextChaseNode();
            }
            else if(currentEnemyState == EnemyStateManager.EnemyState.Scatter)
            {
                targetNode = NextScatterNode();
            }
            else if(currentEnemyState == EnemyStateManager.EnemyState.Run)
            {
                targetNode = NextRunNode();
            }
        }
    }

    protected abstract Node NextChaseNode();
    protected abstract Node NextScatterNode();
    protected abstract Node NextRunNode();

    protected Node GetHomeNode(GameObject homeGameObject)
    {
        Vector3 homePosition = homeGameObject.transform.position;
        Node closestNode = NodeManager.Instance.ClosestNode(homePosition);
        return closestNode;
    }
    protected abstract Node TargetNode();
}
