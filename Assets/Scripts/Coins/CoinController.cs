using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magnet")
        {
            transform.position = Vector2.MoveTowards(transform.position, collision.transform.position,5f);
            Debug.Log("A");
        }
    }
}
