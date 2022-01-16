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
            blueIntermediateTargetNode = playerFutureFutureNode;
        else if (playerFutureNode != null)
            blueIntermediateTargetNode = playerFutureNode;
        else if (playerTargetNode != null)
            blueIntermediateTargetNode = playerTargetNode;
        else if (playerCurrentNode != null)
            blueIntermediateTargetNode = playerCurrentNode;
        else
            blueIntermediateTargetNode = null;

        Vector2 blueTargetPosition = blueIntermediateTargetNode.position + (blueIntermediateTargetNode.position - enemyMovementRedPosition);
        blueTargetNode = NodeManager.Instance.ClosestNode(blueTargetPosition);

        List<Node> path = pathFinding.FindPath(currentNode, blueTargetNode, excludeFromPathFinding);
        if (path != null && path.Count > 1)
            return path[1];

        return NextSafeNode();
    }
}
