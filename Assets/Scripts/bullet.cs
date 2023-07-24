using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    Health healthscript;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            healthscript = collision.gameObject.GetComponent<Health>();
            healthscript.GetHit(1);
            Debug.Log("I've been hit!");
        }
        Destroy(gameObject);
    }
}

