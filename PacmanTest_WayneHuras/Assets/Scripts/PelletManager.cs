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

    private List<Pellet> pellets;

    private int totalPellets;
    public int remainingPellets { get; private set; }
    public void DecrementRemainingPellets() => remainingPellets -= 1;
    public void IncrementRemainingPellets() => remainingPellets += 1;

    private void Awake() => MaintainSingleton();

    private void Start()
    {
        pellets = new List<Pellet>();
        PopulatePellets();
        totalPellets = remainingPellets;
    }

    private void PopulatePellets()
    {
        NodeManager nodeManager = NodeManager.Instance;

        Node[,] nodes = nodeManager.GetAllNodes();

        for (int i = 0; i < nodeManager.GetGraphWidth(); i++)
        {
            for (int j = 0; j < nodeManager.GetGraphHeight(); j++)
            {
                Node node = nodes[i, j];
                bool createPowerPellet = false;

                if (node.canPlacePellet)
                {
                    // Run through list of powerPellet positions to determine if a regular or power pellet should be placed
                    for (int k = 0; k < powerPelletPositions.Length; k++)
                    {
                        if (node.position == (Vector2)powerPelletPositions[k].transform.position)
                        {
                            createPowerPellet = true;
                            break;
                        }
                    }

                    GameObject pelletGO = Instantiate(createPowerPellet ? powerPelletPrefab : pelletPrefab, transform);
                    pelletGO.transform.position = node.position;

                    Pellet pellet = pelletGO.GetComponent<Pellet>();
                    pellet.SetPointValue(createPowerPellet ? powerPelletPointValue : pelletPointValue);

                    pellets.Add(pellet);

                    IncrementRemainingPellets();
                }
            }
        }
    }

    public void ResetPellets()
    {
        foreach(Pellet pellet in pellets)
            pellet.gameObject.SetActive(true);

        remainingPellets = totalPellets;
    }

    public bool HaveThirtyPelletsBeenEaten() => totalPellets - remainingPellets > 30;
    public bool HaveOneThirdPelletBeenEaten() => (remainingPellets / (float)totalPellets) < (float)1/3;

    private void MaintainSingleton()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
