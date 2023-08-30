using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    Health healthScript;
    Animator animator;
    Rigidbody2D rb;

    void Start(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer==10 || collision.collider.tag.Equals("Player"))
        {
            healthScript = collision.gameObject.GetComponent<Health>();
            healthScript.OnChangeHealth(1);
            Debug.Log("I've been hit!");
        }
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        animator.Play("bullet_impact");
        Destroy(gameObject, 0.4f);
    }

}

