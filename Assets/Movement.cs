using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Hunter is stinky");
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(moveSpeed*dirX, moveSpeed*dirY);
    }
}
