using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossZombieController : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    public GameObject player;
    private Rigidbody2D enemyrb;
    [SerializeField] private float coolDown;
    private float coolDownTimer;
    [SerializeField] Transform groundPoundHitbox;
    [SerializeField] float grondPoundRange;
    [SerializeField] float rangeAttackRange;
    [SerializeField] Vector3 desiredPos;
    public enum movementState { sprintAttackRange, rangeAttackRange, groundpoundRange };
    public static movementState currentRange;
    attackState currentAttackState;
    public enum attackState { sprintAttack, rangeAttack, groundPound };

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyrb = GetComponent<Rigidbody2D>();
        desiredPos = transform.position;
    }

    void FixedUpdate()
    {
        float frameSpeed = movementSpeed * Time.deltaTime;
        desiredPos = transform.position;
        if (coolDownTimer > 0) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
        if (coolDownTimer == 0)
        {
            coolDownTimer = coolDown;
            currentAttackState = DetermineAttack();
            if (currentAttackState == attackState.groundPound)
            {

            }
            else if (currentAttackState == attackState.rangeAttack)
            {

            }
            else
            {

            }
        }


        enemyrb.transform.position = Vector2.MoveTowards(enemyrb.transform.position, desiredPos, frameSpeed);
    }

    private void DetermineRange()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, enemyrb.transform.position);

        movementState state;
        if (distanceFromPlayer <= grondPoundRange)
        {
            state = movementState.groundpoundRange;
        }
        else if (distanceFromPlayer <= rangeAttackRange)
        {
            state = movementState.rangeAttackRange;
        }
        else
        {
            state = movementState.sprintAttackRange;
        }

        currentRange = state;
    }

    private attackState DetermineAttack()
    {
        DetermineRange();
        int groundPoundParam;
        int rangeAttackParam;
        int sprintAttackParam;
        attackState state;
        if (currentRange == movementState.groundpoundRange)
        {
            groundPoundParam = 75;
            rangeAttackParam = 100;
            sprintAttackParam = 101;
        }
        else if (currentRange == movementState.rangeAttackRange)
        {
            groundPoundParam = 20;
            rangeAttackParam = 70;
            sprintAttackParam = 100;
        }
        else
        {
            groundPoundParam = 10;
            rangeAttackParam = 35;
            sprintAttackParam = 100;
        }
        int rand = Random.Range(0, 101);

        if (rand <= groundPoundParam)
        {
            state = attackState.groundPound;
        }
        else if (rand <= rangeAttackParam)
        {
            state = attackState.rangeAttack;
        }
        else
        {
            state = attackState.sprintAttack;
        }

        return state;
    }

    private void GroundPound(){
        groundPoundHitbox.GetComponent<GroundPoundHitbox>().Shockwave();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grondPoundRange);
        Gizmos.DrawWireSphere(transform.position, rangeAttackRange);
    }
}
