using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    Health healthscript;
    bool inLaser;
    [SerializeField] float laserDuration;

    void Start()
    {
        StartCoroutine(WaitToDisappear());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            healthscript = collider.gameObject.GetComponent<Health>();
            inLaser = true;
            StartCoroutine(TakeDamage());
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            inLaser=false;
        }
    }

    IEnumerator TakeDamage()
    {
        while (inLaser)
        {
            healthscript.OnChangeHealth(1);
            Debug.Log("I've been hit!");
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator WaitToDisappear(){
        yield return new WaitForSeconds(laserDuration);
        Destroy(gameObject, 0.1f);
        StopAllCoroutines();
    }
}
