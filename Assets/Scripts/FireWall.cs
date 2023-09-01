using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : MonoBehaviour
{

    Health healthscript;
    bool inFire;
    Animator fireAnimator;
    [SerializeField] 
    float fireWallDuration;

    void Start()
    {
        fireAnimator = transform.GetChild(0).GetComponent<Animator>();
        StartCoroutine(WaitToDisappear());
        fireWallDuration += 0.4f; 
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            healthscript = collider.gameObject.GetComponent<Health>();
            inFire = true;
            StartCoroutine(TakeDamage());
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            inFire=false;
        }
    }

    IEnumerator TakeDamage()
    {
        while (inFire)
        {
            healthscript.OnChangeHealth(1);
            Debug.Log("I've been hit!");
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator WaitToDisappear(){
        yield return new WaitForSeconds(fireWallDuration);
        fireAnimator.SetTrigger("putOutFire");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject, 0.1f);
        StopAllCoroutines();
    }
}
