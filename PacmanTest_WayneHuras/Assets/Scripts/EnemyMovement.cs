using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class EnemyMovement : Movement
{
    [SerializeField]
    private float runSpeed = 1;

    [SerializeField]
    private float normalSpeed = 2;

    [SerializeField]
    private GameObject homeGameObject;
    private Node restartNode;
    protected Node homeNode;
    protected Node prevNode;
    protected Node forbiddenNode; // used in Run state to prevent backtracking

    public EnemyStateManager.EnemyState currentEnemyState;
    private EnemyStateManager.EnemyState prevState;

    private Ghost ghost;

    private new void Start()
    {
        base.Start();
        ghost = GetComponent<Ghost>();
        EnemyStateManager.Instance.AddEnemyToEnemyStateManager(this);
        homeNode = GetHomeNode(homeGameObject);
        restartNode = currentNode;
    }

    private void Update()
    {
        if (currentEnemyState != EnemyStateManager.EnemyState.Wait)
        {
            MoveToNode(targetNode);
        }
    }

    public void RestartGhost()
    {
        CancelInvoke();
        ghost.ShowNormalSprite();
        speed = normalSpeed;
        transform.position = restartNode.position;
        currentNode = restartNode;
        targetNode = currentNode;
        currentEnemyState = EnemyStateManager.EnemyState.Scatter;
    }

    public void ActivateRunState(float runDuration)
    {
        if(currentEnemyState != EnemyStateManager.EnemyState.Wait)
        {
            ghost.ShowRunSprite();
            speed = runSpeed;
            prevState = currentEnemyState;
            currentEnemyState = EnemyStateManager.EnemyState.Run;

            CancelInvoke();
            Invoke("LeaveRunState", runDuration);
        }
    }

    private void LeaveRunState()
    {
        ghost.ShowNormalSprite();
        speed = normalSpeed;
        currentEnemyState = prevState;
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
                forbiddenNode = null;
                targetNode = NextChaseNode();
            }
            else if(currentEnemyState == EnemyStateManager.EnemyState.Scatter)
            {
                forbiddenNode = null;
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
            if (neighbour == prevNode || !neighbour.isTraversable)
                continue;

            nextValidNode = neighbour;
        }

        return nextValidNode;
    }

    protected Node NextRunNode()
    {
        Node nextNode = null;

        // forbiddenNode allows ghost to immediatedly backtrack when running,
        // but then prevents backtracking afterwards
        if (forbiddenNode == null)
        {
            nextNode = prevNode;
            forbiddenNode = prevNode;
        }
        else
        {
            // Select random neighbour to run away to
            List<Node> copyOfNeighbours = currentNode.neighbours.Where(x => x != prevNode && x.isTraversable).ToList();
            int rndIndex = Random.Range(0, copyOfNeighbours.Count);

            nextNode = copyOfNeighbours[rndIndex];
        }

        return nextNode;
    }

    protected Node GetHomeNode(GameObject homeGameObject)
    {
        Vector3 homePosition = homeGameObject.transform.position;
        Node closestNode = NodeManager.Instance.ClosestNode(homePosition);
        return closestNode;
    }
}
