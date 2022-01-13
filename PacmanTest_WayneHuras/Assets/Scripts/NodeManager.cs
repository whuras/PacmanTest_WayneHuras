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
    private List<Node> nodes = new List<Node>();

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
        for(int i = 0; i < graphWidth; i++)
        {
            for(int j = 0; j < graphHeight; j++)
            {
                Vector3 nodePosition = new Vector3(i * stepSize, j * stepSize) + transform.position;
                Node node = new Node(nodePosition, wallLayer, emptyAreaLayer);
                nodes.Add(node);
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach(Node node in nodes)
        {
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

    private void MaintainSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
