using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Hunter is stinky");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool moving = Input.GetButton("Horizontal") || Input.GetButton("Vertical");

        if (moving)
        {
            float movementX = Input.GetAxisRaw("Horizontal");
            float movementY = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(movementX, movementY).normalized*moveSpeed;
        }else
        {
            rb.velocity= new Vector2(0,0);
        }
    }

}





// [fail]: OmniSharp.MSBuild.ProjectManager
        // Failure while loading the analyzer reference 'Unity.SourceGenerators': Could not load file or assembly 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51' or one of its dependencies. The system cannot find the file specified.
