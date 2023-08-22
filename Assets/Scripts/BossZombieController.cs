using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossZombieController : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    public GameObject player;
    private Rigidbody2D rb;
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
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        desiredPos = transform.position;
    }

    void FixedUpdate()
    {
        // float frameSpeed = movementSpeed * Time.deltaTime;
        // desiredPos = transform.position;
        // if (coolDownTimer > 0) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
        // if (coolDownTimer == 0)
        // {
        //     coolDownTimer = coolDown;
        //     currentAttackState = DetermineAttack();
        //     if (currentAttackState == attackState.groundPound)
        //     {

        //     }
        //     else if (currentAttackState == attackState.rangeAttack)
        //     {

        //     }
        //     else
        //     {

        //     }
        // }


        // enemyrb.transform.position = Vector2.MoveTowards(enemyrb.transform.position, desiredPos, frameSpeed);

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(GroundPound());
        }
    }

    private void DetermineRange()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, rb.transform.position);

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

    private IEnumerator GroundPound()
    {
        Debug.Log("GroundPound");
        animator.Play("boss_zombie_jump");
        Vector2 up = transform.position;
        up.y+=1.5f;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
        StartCoroutine(MoveWithinTime(transform.position, up, .495f));
        yield return new WaitForSeconds(.495f);
        up.y-=1.5f;
        StartCoroutine(MoveWithinTime(transform.position, up, .165f));
        yield return new WaitForSeconds(.165f);
        groundPoundHitbox.GetComponent<GroundPoundHitbox>().Shockwave();
        yield return new WaitForSeconds(0.65f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
        animator.Play("boss_zombie_idle");
    }

    IEnumerator MoveWithinTime(Vector2 startPos, Vector2 endPos, float time)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

    private void ThrowDirtBall()
    {

    }

    Health healthScript;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            healthScript = collision.gameObject.GetComponent<Health>();
            healthScript.OnChangeHealth(1);
            Debug.Log("I've been hit!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grondPoundRange);
        Gizmos.DrawWireSphere(transform.position, rangeAttackRange);
    }
}
