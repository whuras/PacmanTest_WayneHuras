using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 position { get; private set; }
    public bool isTraversable { get; private set; }
    public bool canPlacePellet { get; private set; }
    public List<Node> neighbours { get; private set; }
    private void AddNeighbour(Node neighbour) => neighbours.Add(neighbour);

    // For Pathfinding
    public Node prevNode { get; private set; }
    public int iPos { get; private set; }
    public int jPos { get; private set; }
    public int gCost { get; private set; }
    public int hCost { get; private set; }
    public int fCost { get; private set; }
    public void SetPrevNode(Node prevNode) => this.prevNode = prevNode;
    public void SetGCost(int value) => gCost = value;
    public void SetHCost(int value) => hCost = value;
    public void SetFCost(int value) => fCost = value;

    public Node(Vector3 position, LayerMask wallLayer, LayerMask emptyLayer, int i, int j)
    {
        this.position = position;
        neighbours = new List<Node>();
        isTraversable = IsTraversable(wallLayer);
        canPlacePellet = CanPlacePellet(emptyLayer);

        iPos = i;
        jPos = j;
    }

    private bool CanPlacePellet(LayerMask emptyLayer)
    {
        if (!isTraversable)
            return false;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, 0.05f, emptyLayer);
        return hitColliders.Length > 0 ? false : true;
    }

    private bool IsTraversable(LayerMask wallLayer)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, 0.05f, wallLayer);
        return hitColliders.Length > 0 ? false : true;
    }

    public void UpdateNeighbours(Node[,] nodes, float distanceToNeighbourNode)
    {
        foreach(Node node in nodes)
        {
            if (node == this)
                continue;

            float dist = Vector2.Distance(position, node.position);

            if (dist <= distanceToNeighbourNode * 1.1)
            {
                AddNeighbour(node);
            }
        }
    }

    public Node GetNeighbourInDirection(Vector2 direction, bool isTraversable)
    {
        foreach(Node neighbour in neighbours)
        {
            Vector2 testDirection = (neighbour.position - position).normalized;
            if (testDirection == direction && isTraversable == neighbour.isTraversable)
            {
                return neighbour;
            }
        }

        return null;
    }
}