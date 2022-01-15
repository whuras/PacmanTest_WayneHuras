using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int pointValue { get; private set; }
    public void SetPointValue(int amount) => pointValue = amount;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.EatPellet(this);
        }
    }
}
