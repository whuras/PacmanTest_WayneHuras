using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletManager : MonoBehaviour
{
    private static PelletManager instance;
    public static PelletManager Instance => instance;

    [SerializeField]
    private int pelletPointValue;
    
    [SerializeField]
    private int powerPelletPointValue;

    [SerializeField]
    private GameObject pelletPrefab;

    [SerializeField]
    private GameObject powerPelletPrefab;

    [SerializeField]
    private GameObject[] powerPelletPositions = new GameObject[4];

    public int remainingPellets { get; private set; }
    public void DecrementRemainingPellets() => remainingPellets -= 1;
    public void IncrementRemainingPellets() => remainingPellets += 1;

    private void Awake() => MaintainSingleton();

    private void Start()
    {
        Debug.Log("start pellet manager");
        NodeManager nodeManager = NodeManager.Instance;

        Node[,] nodes = nodeManager.GetAllNodes();
        for(int i = 0; i < nodeManager.GetGraphWidth(); i++)
        {
            for(int j = 0; j < nodeManager.GetGraphHeight(); j++)
            {
                Node node = nodes[i, j];
                bool powerPelletCreated = false;

                if (node.canPlacePellet)
                {
                    for (int k = 0; k < powerPelletPositions.Length; k++)
                    {
                        if (node.position == (Vector2)powerPelletPositions[k].transform.position)
                        {
                            GameObject powerPelletGO = Instantiate(powerPelletPrefab, transform);
                            powerPelletGO.transform.position = node.position;

                            Pellet powerPellet = powerPelletGO.GetComponent<Pellet>();
                            powerPellet.SetPointValue(powerPelletPointValue);
                            IncrementRemainingPellets();
                            powerPelletCreated = true;
                            break;
                        }
                    }

                    if (powerPelletCreated)
                        continue;

                    GameObject pelletGO = Instantiate(pelletPrefab, transform);
                    pelletGO.transform.position = node.position;
                    pelletGO.name = remainingPellets.ToString();

                    Pellet pellet = pelletGO.GetComponent<Pellet>();
                    pellet.SetPointValue(pelletPointValue);
                    IncrementRemainingPellets();
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
