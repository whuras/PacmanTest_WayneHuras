using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 position { get; private set; }
    public List<Node> neighbours { get; private set; }
    public bool isTraversable { get; private set; }
    public bool canPlacePellet { get; private set; }

    public Node(Vector3 position, LayerMask wallLayer, LayerMask emptyLayer)
    {
        this.position = position;
        neighbours = new List<Node>();
        isTraversable = IsTraversable(wallLayer);
        canPlacePellet = CanPlacePellet(emptyLayer);
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

    public void CheckNeighbours(Node[,] nodes, float distanceToNeighbourNode)
    {
        foreach(Node node in nodes)
        {
            if (node == this)
                continue;

            float dist = Vector2.Distance(position, node.position);

            if (dist <= distanceToNeighbourNode * 1.1 && node.isTraversable)
            {
                AddNeighbour(node);
            }
        }
    }

    public Node GetNeighbourInDirection(Vector2 direction)
    {
        foreach(Node neighbour in neighbours)
        {
            Vector2 testDirection = (neighbour.position - position).normalized;
            if (testDirection == direction)
            {
                return neighbour;
            }
        }

        return null;
    }

    private void AddNeighbour(Node neighbour)
    {
        neighbours.Add(neighbour);
    }
}