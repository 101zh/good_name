using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float timePassed = 0f; 
    Health healthScript;
    private Transform Player;
    bool inLaser;
    void Start()
    {
        Player= GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        timePassed += Time.deltaTime;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if(timePassed > 6f)
        {
            
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
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
            inLaser = true;      
            StartCoroutine(TakeDamage());      
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            inLaser = false;
        }
    }
    IEnumerator TakeDamage()
    {
        while (inLaser)
        {
            Debug.Log("I've been hit!");
            yield return new WaitForSeconds(1f);
            healthScript.OnChangeHealth(1);
        }
    }
}
