using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : Movement
{
    [SerializeField]
    private GameObject homeGameObject;
    protected Node homeNode;
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
    protected Node NextScatterNode()
    {
        PathFinding pathFinding = new PathFinding();
        List<Node> excludeFromPathFinding = new List<Node> { prevNode }; // ghosts should not be able to turn around in chase/scatter

        List<Node> path = pathFinding.FindPath(currentNode, homeNode, excludeFromPathFinding);
        if (path != null && path.Count > 1)
        {
            // Debugging
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].position, path[i + 1].position, Color.blue, 1f);
            }
            return path[1];
        }

        // If the enemy reached the closest node to their home then go to the next neighbour
        // this makes the enemy circle around area rather than stop
        Node nextValidNode = currentNode;
        foreach (Node neighbour in currentNode.neighbours)
        {
            if (neighbour == prevNode)
                continue;

            nextValidNode = neighbour;
        }

        return nextValidNode;
    }

    protected abstract Node NextRunNode();

    protected Node GetHomeNode(GameObject homeGameObject)
    {
        Vector3 homePosition = homeGameObject.transform.position;
        Node closestNode = NodeManager.Instance.ClosestNode(homePosition);
        return closestNode;
    }
    protected abstract Node TargetNode();
}
