using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    Health healthScript;
    private Vector2 moveDirection;
    [SerializeField] float Speed;

    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            healthScript = collision.gameObject.GetComponent<Health>();
            healthScript.OnChangeHealth(1);
            Debug.Log("I've Been Hit!");

        }
    }

    void FixedUpdate()
    {
        moveDirection = transform.position;
        moveDirection.x += 20;
        transform.position = Vector2.MoveTowards(transform.position, moveDirection, Speed * Time.deltaTime);
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject, 0.1f);
        StopAllCoroutines();
    }
}
