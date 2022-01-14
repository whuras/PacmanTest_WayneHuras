using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // Singleton
    private static NodeManager instance;
    public static NodeManager Instance => instance;

    // Vars
    [SerializeField]
    private float stepSize = 0.5f;

    [SerializeField]
    private int graphWidth = 20;

    [SerializeField]
    private int graphHeight = 25;

    [SerializeField]
    private Node[,] nodes;

    [SerializeField]
    private float distanceToNeighbourNode = 0.2f;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private LayerMask emptyAreaLayer;

    // Methods
    private void Awake()
    {
        MaintainSingleton();
        CreateGraph();
    }

    private void CreateGraph()
    {
        nodes = new Node[graphWidth, graphHeight];

        for(int i = 0; i < graphWidth; i++)
        {
            for(int j = 0; j < graphHeight; j++)
            {
                Vector3 nodePosition = new Vector3(i * stepSize, j * stepSize) + transform.position;
                Node node = new Node(nodePosition, wallLayer, emptyAreaLayer);
                nodes[i, j] = node;
            }
        }

        for (int i = 0; i < graphWidth; i++)
        {
            for (int j = 0; j < graphHeight; j++)
            {
                Node node = nodes[i, j];
                node.UpdateNeighbours(nodes, distanceToNeighbourNode);
            }
        }
    }

    public Node ClosestNode(Vector3 position)
    {
        float closestDistance = float.MaxValue;
        Node closestNode = null;

        for(int i = 0; i < graphWidth; i++)
        {
            for(int j = 0; j < graphHeight; j++)
            {
                Node testNode = nodes[i, j];
                float testDistance = Vector3.Distance(position, testNode.position);
                if(testDistance < closestDistance)
                {
                    closestDistance = testDistance;
                    closestNode = testNode;
                }
            }
        }

        if(closestNode == null)
        {
            Debug.LogError("Closest node is null -> NodeManager.ClosestNode()");
        }

        return closestNode;
    }

    private void OnDrawGizmos()
    {
        if (nodes == null)
            return;

        for (int i = 0; i < graphWidth; i++)
        {
            for (int j = 0; j < graphHeight; j++)
            {
                Node node = nodes[i, j];

                if (!node.canPlacePellet || !node.isTraversable)
                {
                    Gizmos.color = Color.clear;
                    Gizmos.DrawSphere(node.position, 0.1f);
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(node.position, 0.1f);
                }
            }
        }
    }

    private void MaintainSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
