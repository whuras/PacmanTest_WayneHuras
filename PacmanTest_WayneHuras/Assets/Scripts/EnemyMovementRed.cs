using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementRed : EnemyMovement
{
    protected override Node NextChaseNode()
    {


        PathFinding pathFinding = new PathFinding();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        List<Node> excludeFromPathFinding = new List<Node> { prevNode }; // ghosts should not be able to turn around in chase/scatter

        List<Node> path = pathFinding.FindPath(currentNode, playerMovement.currentNode, excludeFromPathFinding);
        if (path != null && path.Count > 1)
        {
            // Debugging
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].position, path[i + 1].position, Color.red, 1f);
            }
            return path[1];
        }

        return currentNode;
    }

    protected override Node TargetNode()
    {

        return null;
    }
}
