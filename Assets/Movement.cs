using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("Hunter is stinky");
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKey("up"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*2);
        }

        if (Input.GetKey("down"))
        {
            rb.velocity = new vector2(rb.velocity.x, rb.velocity.y*-2);
        }
    }
}
