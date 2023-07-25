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
    [SerializeField] Vector3 desiredPos;
    private enum movementState { chase, strafe, runAway };
    [SerializeField] movementState currentMovementState;
    float minStrafe;
    float maxStrafe;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyrb = GetComponent<Rigidbody2D>();
        desiredPos = transform.position;
        minStrafe = -4;
        maxStrafe = 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentMovementState = movementLogic();
        float movementSpeed = enemyWalkSp * Time.deltaTime;
        if (currentMovementState == movementState.strafe && desiredPos == enemyrb.transform.position)
        {
            desiredPos = pickPosition(currentMovementState);
            Debug.Log(desiredPos.ToString()+"; "+enemyrb.transform.position.ToString());
        }
        else
        {
            desiredPos = pickPosition(currentMovementState);
        }

        if (currentMovementState == movementState.runAway)
        {
            movementSpeed = -Mathf.Abs(movementSpeed);
        }
        else
        {
            movementSpeed = Mathf.Abs(movementSpeed);
        }

        // Vector2 dir= enemyrb.position;
        // dir.x=Mathf.MoveTowards(enemyrb.position.x, desiredPos.x, movementSpeed);
        // dir.y=Mathf.MoveTowards(enemyrb.position.y, desiredPos.y, movementSpeed);

        // enemyrb.MovePosition(dir);

        Debug.Log(movementSpeed);
        enemyrb.transform.position = Vector2.MoveTowards(enemyrb.transform.position, desiredPos, movementSpeed);

    }

    private movementState movementLogic()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, enemyrb.transform.position);

        movementState state = movementState.strafe;

        if (distanceFromPlayer >= attackRange)
        {
            state = movementState.chase;
        }
        else if (distanceFromPlayer >= dangerRange)
        {
            state = movementState.strafe;
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
        else if (state == movementState.strafe)
        {
            float randX = Random.Range(minStrafe, maxStrafe);
            float randY = Random.Range(minStrafe, maxStrafe);
            point = new Vector2(enemyrb.transform.position.x + randX, enemyrb.transform.position.y + randY);

        }
        else if (state == movementState.runAway)
        {
            point = new Vector2(player.transform.position.x, player.transform.position.y);
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