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
        bool moving = Input.GetButton("Horizontal") || Input.GetButton("Vertical");

        if (moving)
        {
            float movementX = Input.GetAxis("Horizontal");
            float movementY = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(movementX*moveSpeed, movementY*moveSpeed);
        }else
        {
            rb.velocity= new Vector2(0,0);
        }

    }
}





// [fail]: OmniSharp.MSBuild.ProjectManager
        // Failure while loading the analyzer reference 'Unity.SourceGenerators': Could not load file or assembly 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51' or one of its dependencies. The system cannot find the file specified.
