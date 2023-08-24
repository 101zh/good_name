using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword_hitbox : MonoBehaviour
{
    Health healthscript;
    Rigidbody2D rb;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer==10 || collider.tag.Equals("Player"))
        {
            healthscript = collider.gameObject.GetComponent<Health>();
            healthscript.OnChangeHealth(1);
            Debug.Log("I've been hit!");
        }
        if(collider.gameObject.layer==7){
            collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            collider.GetComponent<Animator>().Play("bullet_impact");
            Destroy(collider.gameObject, 0.4f);
            Debug.Log("blocked bullet");
        }
    }
}
