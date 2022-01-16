using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Ghost : MonoBehaviour
{
    public int ghostPointValue = 200;

    private EnemyMovement enemyMovement;

    [SerializeField]
    private GameObject normalSprite;

    [SerializeField]
    private GameObject runSprite;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void ShowNormalSprite()
    {
        runSprite.SetActive(false);
        normalSprite.SetActive(true);
    }

    public void ShowRunSprite()
    {
        normalSprite.SetActive(false);
        runSprite.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(enemyMovement.currentEnemyState == EnemyStateManager.EnemyState.Run)
            {
                GameManager.Instance.EatEnemy(this);
                enemyMovement.ResetEnemyMovement();
            }
            else
            {
                // Eat Player
                GameManager.Instance.EatPlayer();
            }
        }
    }
}
