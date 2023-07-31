using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    Health healthscript;
    Animator animator;
    Rigidbody2D rb;

    void Start(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            healthscript = collision.gameObject.GetComponent<Health>();
            healthscript.GetHit(1);
            Debug.Log("I've been hit!");
        }
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        animator.Play("bullet_impact");
        Destroy(gameObject, 0.4f);
    }

}

