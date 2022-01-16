using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private List<Node> openList;
    private List<Node> closedList;

    public List<Node> FindPath(Node startNode, Node endNode, List<Node> excludeNodes)
    {
        NodeManager nodeManager = NodeManager.Instance;
        Node[,] nodes = nodeManager.GetAllNodes();

        openList = new List<Node>();
        closedList = new List<Node>();

        foreach(Node node in excludeNodes)
            closedList.Add(node);

        openList.Add(startNode);

        for(int i = 0; i < nodeManager.GetGraphWidth(); i++)
        {
            for (int j = 0; j < nodeManager.GetGraphHeight(); j++)
            {
                Node resetNode = nodes[i, j];
                resetNode.SetGCost(int.MaxValue);
                resetNode.SetFCost(CalculateFCost(resetNode));
                resetNode.SetPrevNode(null);
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateHCost(startNode, endNode));
        startNode.SetFCost(CalculateFCost(startNode));

        while(openList.Count > 0)
        {
            Node currentNode = GetSmallestFCostNode(openList);

            if(currentNode == endNode)
                return CalculatePath(endNode);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(Node neighbour in currentNode.neighbours)
            {
                if (!neighbour.isTraversable && !closedList.Contains(neighbour))
                    closedList.Add(neighbour);
                    
                if (closedList.Contains(neighbour))
                    continue;

                int testGCost = currentNode.gCost + CalculateHCost(currentNode, neighbour);
                if(testGCost < neighbour.gCost)
                {
                    neighbour.SetPrevNode(currentNode);
                    neighbour.SetGCost(testGCost);
                    neighbour.SetHCost(CalculateHCost(neighbour, endNode));
                    neighbour.SetFCost(CalculateFCost(neighbour));

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
        }

        return null;
    }

    private int CalculateFCost(Node node) => node.gCost + node.hCost;

    private int CalculateHCost(Node from, Node to)
    {
        int xDistance = Mathf.Abs(from.iPos - to.iPos);
        int yDistance = Mathf.Abs(from.jPos - to.jPos);

        return xDistance + yDistance;
    }

    private Node GetSmallestFCostNode(List<Node> nodes)
    {
        Node lowestFCostNode = nodes[0];
        foreach(Node node in nodes)
        {
            if(lowestFCostNode.fCost > node.fCost)
                lowestFCostNode = node;
        }

        return lowestFCostNode;
    }

    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);

        Node currentNode = endNode;

        while(currentNode.prevNode != null)
        {
            path.Add(currentNode.prevNode);
            currentNode = currentNode.prevNode;
        }
        
        path.Reverse();
        
        return path;
    }
}
