using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    private GameObject player;
    private Rigidbody2D enemyrb;
    [SerializeField] private float attackRange = 5;
    [SerializeField] private float dangerRange = 2.5f;
    Vector3 desiredPos;
    private enum movementState { chase, safe, runAway };
    movementState currentMovementState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyrb = GetComponent<Rigidbody2D>();
        desiredPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentMovementState = movementLogic();
        movementSpeed = movementSpeed * Time.deltaTime;
        desiredPos = pickPosition(currentMovementState);

        if (currentMovementState == movementState.runAway)
        {
            movementSpeed = -Mathf.Abs(movementSpeed);
        }
        else
        {
            movementSpeed = Mathf.Abs(movementSpeed);
        }

        enemyrb.transform.position = Vector2.MoveTowards(enemyrb.transform.position, desiredPos, movementSpeed);

    }

    private movementState movementLogic()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, enemyrb.transform.position);

        movementState state = movementState.safe;

        if (distanceFromPlayer >= attackRange)
        {
            state = movementState.chase;
        }
        else if (distanceFromPlayer >= dangerRange)
        {
            state = movementState.safe;
        }
        else if (distanceFromPlayer <= dangerRange)
        {
            state = movementState.runAway;
        }

        return state;
    }

    private Vector2 pickPosition(movementState state)
    {
        Vector2 point = new Vector2(transform.position.x, transform.position.y);

        if (state == movementState.chase)
        {
            point = player.transform.position;
        }
        else if (state == movementState.safe)
        {
            point = transform.position;
        }
        else if (state == movementState.runAway)
        {
            point = player.transform.position;
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