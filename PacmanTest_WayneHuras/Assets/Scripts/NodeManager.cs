using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private static NodeManager instance;
    public static NodeManager Instance => instance;

    [SerializeField]
    private float stepSize = 0.5f;

    [SerializeField]
    private int graphWidth = 26;
    public int GetGraphWidth() => graphWidth;

    [SerializeField]
    private int graphHeight = 29;
    public int GetGraphHeight() => graphHeight;

    [SerializeField]
    private Node[,] nodes;
    public Node[,] GetAllNodes() => nodes;

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
                Node node = new Node(nodePosition, wallLayer, emptyAreaLayer, i, j);
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

        return closestNode;
    }

    private void MaintainSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
