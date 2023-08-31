using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLaser : MonoBehaviour
{
    float timePassed = 0f; 
    void Update()
    {
        timePassed += Time.deltaTime;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if(timePassed > 5f)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        if(timePassed > 6f)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (timePassed > 8f)
        {
            timePassed = 0f;
        }
    }
}
