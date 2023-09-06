using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    public GameObject player;
    private Rigidbody2D enemyrb;
    [SerializeField] private float attackRange;
    [SerializeField] private float dangerRange;
    [SerializeField] Vector3 desiredPos;
    public enum movementState { chase, safe, runAway };
    public movementState currentMovementState;
    [SerializeField] AudioSource DeathSound;

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
        if (pause_menu.gameIsPaused) return;

        currentMovementState = movementLogic();
        float frameSpeed = movementSpeed * Time.deltaTime;
        desiredPos = pickPosition(currentMovementState);

        if (currentMovementState == movementState.runAway)
        {
            frameSpeed = -Mathf.Abs(frameSpeed);
        }
        else
        {
            frameSpeed = Mathf.Abs(frameSpeed);
        }

        enemyrb.transform.position = Vector2.MoveTowards(enemyrb.transform.position, desiredPos, frameSpeed);

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

    private void PlayDeathSound()
    {
        DeathSound.Play();
    }

    private void OnEnable()
    {
        Health.OnDie += PlayDeathSound;
    }

    private void OnDisable()
    {

        Health.OnDie -= PlayDeathSound;
    }
}



// Get the x,y of player
// set direction to that x,y of player
// set speed of movement