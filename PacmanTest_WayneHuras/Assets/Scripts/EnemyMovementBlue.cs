using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyMovementBlue : EnemyMovement
{
    // Blue's target is the node closest to the position which is
    // twice the distance from Red's position to two nodes ahead of Pacman
    protected override Node NextChaseNode()
    {
        PathFinding pathFinding = new PathFinding();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        List<Node> excludeFromPathFinding = new List<Node> { prevNode }; // ghosts should not be able to turn around in chase/scatter

        EnemyMovementRed enemyMovementRed = FindObjectOfType<EnemyMovementRed>();
        Vector2 enemyMovementRedPosition = enemyMovementRed.gameObject.transform.position;

        Node blueTargetNode;
        Node blueIntermediateTargetNode;
        Node playerCurrentNode = playerMovement.currentNode;
        Node playerTargetNode = playerMovement.targetNode;
        Node playerFutureNode = playerTargetNode != null ? playerTargetNode.GetNeighbourInDirection(playerMovement.desiredDirection, true) : null;
        Node playerFutureFutureNode = playerFutureNode != null ? playerFutureNode.GetNeighbourInDirection(playerMovement.desiredDirection, true) : null;

        if (playerFutureFutureNode != null)
        {
            blueIntermediateTargetNode = playerFutureFutureNode;
        }
        else if (playerFutureNode != null)
        {
            blueIntermediateTargetNode = playerFutureNode;
        }
        else if (playerTargetNode != null)
        {
            blueIntermediateTargetNode = playerTargetNode;
        }
        else if (playerCurrentNode != null)
        {
            blueIntermediateTargetNode = playerCurrentNode;
        }
        else
        {
            Debug.LogError("Inky cannot determine a target! EmenyMovementBlue->NextChaseNode()");
            blueIntermediateTargetNode = null;
        }

        Vector2 blueTargetPosition = blueIntermediateTargetNode.position + (blueIntermediateTargetNode.position - enemyMovementRedPosition);
        blueTargetNode = NodeManager.Instance.ClosestNode(blueTargetPosition);

        List<Node> path = pathFinding.FindPath(currentNode, blueTargetNode, excludeFromPathFinding);
        if (path != null && path.Count > 1)
        {
            // Debugging
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].position, path[i + 1].position, Color.cyan, 1f);
            }
            return path[1];
        }

        // Blue's goal node can often become it's prev or current node which returns a null path from Pathfinding.
        // This allows for Blue to continue on a valid path without being stuck or going backwards by finding a new
        // neighbour node which is valid.
        Node safeNode;
        List<Node> copyOfNeighbours = currentNode.neighbours.Where(x => x != prevNode && x.isTraversable).ToList();
        int rndIndex = Random.Range(0, copyOfNeighbours.Count);

        safeNode = copyOfNeighbours[rndIndex];

        return safeNode;
    }
}
