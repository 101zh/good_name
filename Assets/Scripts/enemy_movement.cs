using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{ 
    
    [SerializeField] private float enemyWalkSp;
    [SerializeField] private GameObject Player;
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
        Vector3 PlayerPosition = Player.transform.position;
        enemyrb.AddForce(PlayerPosition, ForceMode2D.Force);
    }
}



// Get the x,y of player
// set direction to that x,y of player
// set speed of movement