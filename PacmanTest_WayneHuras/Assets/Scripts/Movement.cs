using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField]
    protected float speed = 2f;
    protected Node currentNode;
    protected Node targetNode;
    private float reachedDistance = 0.01f;

    protected bool isMoving = false;

    protected abstract Node NextNode();

    private void Start()
    {
        currentNode = NodeManager.Instance.ClosestNode(gameObject.transform.position);
    }

    protected void MoveToNode(Node node)
    {
        isMoving = true;

        transform.position += speed * Time.deltaTime * ((Vector3)node.position - transform.position).normalized;

        if(Vector3.Distance(transform.position, node.position) <= reachedDistance)
        {
            transform.position = node.position;
            currentNode = targetNode;

            targetNode = NextNode();
        }
    }

    
}
