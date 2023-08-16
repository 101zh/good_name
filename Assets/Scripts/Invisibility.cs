using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Invisibility : MonoBehaviour
{
    float timePassed = 0f; 
    void Update()
    {
        timePassed += Time.deltaTime;

        if(timePassed > 5f)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        if(timePassed > 10f)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            timePassed = 0f; 
        }
    }
}
