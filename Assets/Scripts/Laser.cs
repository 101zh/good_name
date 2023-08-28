using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float timePassed = 0f; 
    Health healthScript;
    void Update()
    {
        timePassed += Time.deltaTime;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if(timePassed > 6f)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            Debug.Log("Laser BEAM");
            
        }
        if(timePassed > 8f)
        {   
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            timePassed = 0f;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            healthScript = collider.gameObject.GetComponent<Health>();
            healthScript.OnChangeHealth(1);
            Debug.Log("I've Been Hit!");

        }
    }
}
