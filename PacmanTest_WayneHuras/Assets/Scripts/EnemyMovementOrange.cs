using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementOrange : EnemyMovement
{
    // Orange's target has two different modes:
    // If orange is WITHIN 8 tiles of Pacman, his target is determined by Scatter behaviour
    // If orange is FURTHER than 8 tiles of Pacman, his target is Pacman's current node
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
                Debug.DrawLine(path[i].position, path[i + 1].position, Color.yellow, 1f);
            }
            //return path[1];
        }

        if (path.Count > 8)
            return path[1];

        return NextScatterNode();
    }
}
