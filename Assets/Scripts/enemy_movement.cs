using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{ 
    
    [SerializeField] private float enemyWalkSp;
    [SerializeField] private Transform playerTransform;
    private Rigidbody2D enemyrb;
    // Start is called before the first frame update
    void Start()
    {
        enemyrb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //enemyrb.velocity = new Vector2.MoveTowards(Player.transform.position.x, Player.transform.position.y).normalized*enemyWalkSp;
        //Debug.Log(Player.transform.position.x);
        float movement = enemyWalkSp * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,new Vector2(playerTransform.position.x,playerTransform.position.y),movement);
    }
}



// Get the x,y of player
// set direction to that x,y of player
// set speed of movement