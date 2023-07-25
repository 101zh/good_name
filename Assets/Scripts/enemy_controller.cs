using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{

    [SerializeField] private float enemyWalkSp;
    private GameObject player;
    private Rigidbody2D enemyrb;
    [SerializeField] private float attackRange = 5;
    [SerializeField] private float dangerRange = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyrb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float movement = enemyWalkSp * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, move(), movement);
    }

    private Vector2 move()
    {
        Vector2 point= player.transform.position;

        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceFromPlayer >= attackRange)
        {
            point = player.transform.position;
        }
        else if (distanceFromPlayer >= dangerRange)
        {
            float randX = Random.Range(-2, 2);
            float randY = Random.Range(-2, 2);
            point = new Vector2(transform.position.x + randX, transform.position.y + randY);
        }

        return point;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, dangerRange);
    }
}



// Get the x,y of player
// set direction to that x,y of player
// set speed of movement