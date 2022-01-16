using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField]
    private GameObject flashObject;

    [SerializeField]
    private float flashInverval;

    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > flashInverval)
            flashObject.SetActive(false);

        if (timer > flashInverval * 2)
        {
            flashObject.SetActive(true);
            timer = 0;
        }
    }
}
