using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField]
    protected float speed = 2f;
    public Node currentNode { get; protected set; } // node the unit is on / has reached
    public Node targetNode { get; protected set; } // node the unit is heading to
    protected Node restartNode;
    protected float reachedDistance = 0.01f;

    protected bool isMoving = false;

    protected abstract void MoveToNode(Node node);

    protected void Start()
    {
        currentNode = NodeManager.Instance.ClosestNode(gameObject.transform.position);
        targetNode = currentNode;
        restartNode = currentNode;
    }
}
