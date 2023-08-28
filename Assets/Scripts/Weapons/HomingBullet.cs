using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    Health healthScript;
    Animator animator;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float rotateSpeed;
    [SerializeField] string targetTag;
    [SerializeField] Transform target;

    void Start(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag(targetTag).transform;
    }
    
    void FixedUpdate(){
        Vector2 dir = (Vector2)target.position - rb.position;
        dir.Normalize();

        float rotateAmount = Vector3.Cross(dir, transform.up).z;

        rb.angularVelocity =-rotateAmount * rotateSpeed;
        rb.velocity = transform.up * speed;
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
