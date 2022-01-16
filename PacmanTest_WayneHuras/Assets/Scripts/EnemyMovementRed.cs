using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementRed : EnemyMovement
{
    // Red's target is Pacman's current node
    protected override Node NextChaseNode()
    {
        PathFinding pathFinding = new PathFinding();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        List<Node> excludeFromPathFinding = new List<Node> { prevNode }; // ghosts should not be able to turn around in chase/scatter

        List<Node> path = pathFinding.FindPath(currentNode, playerMovement.currentNode, excludeFromPathFinding);
        if (path != null && path.Count > 1)
            return path[1];

        return NextSafeNode();
    }
}
