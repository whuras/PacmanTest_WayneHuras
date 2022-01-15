using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementPink : EnemyMovement
{
    // Pink's target is two nodes ahead of Pacman
    protected override Node NextChaseNode()
    {
        PathFinding pathFinding = new PathFinding();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        List<Node> excludeFromPathFinding = new List<Node> { prevNode }; // ghosts should not be able to turn around in chase/scatter

        Node pinkTargetNode;
        Node playerCurrentNode = playerMovement.currentNode;
        Node playerTargetNode = playerMovement.targetNode;
        Node playerFutureNode = playerTargetNode != null ? playerTargetNode.GetNeighbourInDirection(playerMovement.desiredDirection, true) : null;
        Node playerFutureFutureNode = playerFutureNode != null ? playerFutureNode.GetNeighbourInDirection(playerMovement.desiredDirection, true) : null;

        if(playerFutureFutureNode != null)
        {
            pinkTargetNode = playerFutureFutureNode;
        }
        else if (playerFutureNode != null)
        {
            pinkTargetNode = playerFutureNode;
        }   
        else if (playerTargetNode != null)
        {
            pinkTargetNode = playerTargetNode;
        }
        else if (playerCurrentNode != null)
        {
            pinkTargetNode = playerCurrentNode;
        }
        else
        {
            Debug.LogError("Pinky cannot determine a target! EmenyMovementPink->NextChaseNode()");
            pinkTargetNode = null;
        }

        List <Node> path = pathFinding.FindPath(currentNode, pinkTargetNode, excludeFromPathFinding);
        if (path != null && path.Count > 1)
        {
            // Debugging
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].position, path[i + 1].position, Color.magenta, 1f);
            }
            return path[1];
        }

        return currentNode;
    }
}
