using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    public Vector2 desiredDirection { get; private set; }
    private Vector2 queuedDirection = Vector2.zero;
    private Node queuedNode;

    private List<Vector2> validDirections = new List<Vector2>
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void Update()
    {
        if (targetNode != null)
        {
            MoveToNode(targetNode);
        }
        else
        {
            ResetQueue();
            isMoving = false;
        }
    }

    public void RestartPlayerMovement()
    {
        transform.position = restartNode.position;
        currentNode = restartNode;
        targetNode = currentNode;
    }

    public void ReceiveMovementRequest(Vector2 direction)
    {
        if (validDirections.Contains(direction))
        {
            if (!isMoving)
            {
                desiredDirection = direction;
                targetNode = currentNode.GetNeighbourInDirection(desiredDirection, true);
            }
            else
            {
                queuedDirection = direction;
            }
        }
    }

    private Node NextNode()
    {
        Node nextNode;

        if (queuedDirection != Vector2.zero)
            queuedNode = currentNode.GetNeighbourInDirection(queuedDirection, true);

        if (queuedNode != null)
        {
            nextNode = queuedNode;
            queuedNode = null;

            desiredDirection = queuedDirection;
            queuedDirection = Vector2.zero;
        }
        else
        {
            nextNode = currentNode.GetNeighbourInDirection(desiredDirection, true);
        }

        return nextNode;
    }

    private void ResetQueue()
    {
        queuedNode = null;
        queuedDirection = Vector2.zero;
    }

    protected override void MoveToNode(Node node)
    {
        isMoving = true;

        transform.position += speed * Time.deltaTime * ((Vector3)node.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, node.position) <= reachedDistance)
        {
            transform.position = node.position;
            currentNode = targetNode;

            targetNode = NextNode();
        }
    }
}
