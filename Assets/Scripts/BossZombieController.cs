using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossZombieController : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private float dashSpeedIncrease;
    public GameObject player;
    private Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    [SerializeField] private float coolDown;
    private float coolDownTimer;
    [SerializeField] Transform groundPoundHitbox;
    [SerializeField] float groundPoundRange;
    [SerializeField] float rangeAttackRange;
    [SerializeField] Vector2 desiredPos;
    public enum movementState { sprintAttackRange, rangeAttackRange, groundpoundRange };
    [SerializeField] private movementState currentRange;
    [SerializeField] attackState currentAttackState;
    public enum attackState { sprintAttack, rangeAttack, groundPound };
    Animator animator;
    [SerializeField] bool targetLock = false;
    [SerializeField] bool movementLock = false;
    private bool coolDownLock = false;
    Transform projectileLauncher;
    BossGunController projectileLauncherScript;
    string currentAnimationState;
    GameObject healthBar;
    BossHealthBar healthBarScript;
    Health health;
    [SerializeField] AudioSource deathSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        projectileLauncher = transform.GetChild(1);
        projectileLauncherScript = projectileLauncher.GetComponent<BossGunController>();

        healthBar = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(5).gameObject;
        healthBarScript = healthBar.GetComponent<BossHealthBar>();

        health = GetComponent<Health>();

        health.timeToDestroy = deathSound.clip.length;
        healthBar.SetActive(true);
        healthBarScript.setMaxValue(health.maxHealth);
        healthBarScript.setValue(health.currentHealth);
        desiredPos = transform.position;
    }
    void Update()
    {
        if (pause_menu.gameIsPaused) return;
        if (coolDownTimer > 0 && !coolDownLock) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
        if (coolDownTimer == 0)
        {
            coolDownTimer = coolDown;
            currentAttackState = DetermineAttack();
            if (currentAttackState == attackState.groundPound)
            {
                StartCoroutine(GroundPound());
            }
            else if (currentAttackState == attackState.rangeAttack)
            {
                StartCoroutine(ThrowDirtBall());
            }
            else
            {
                Dash();
            }
        }
        updateAnimation();
    }

    void FixedUpdate()
    {
        if (pause_menu.gameIsPaused) return;
        DeterminePos();
        if (!movementLock)
        {
            transform.position = Vector2.MoveTowards(transform.position, desiredPos, movementSpeed * Time.deltaTime);
        }
    }

    private void DetermineRange()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);

        movementState state;
        if (distanceFromPlayer <= groundPoundRange)
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
        attackState state;
        if (currentRange == movementState.groundpoundRange)
        {
            groundPoundParam = 80;
            rangeAttackParam = 85;

        }
        else if (currentRange == movementState.rangeAttackRange)
        {
            groundPoundParam = 10;
            rangeAttackParam = 70;

        }
        else
        {
            groundPoundParam = 5;
            rangeAttackParam = 50;
        }
        int rand = Random.Range(1, 101);


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

    private void DeterminePos()
    {
        if (targetLock)
        {

        }
        else
        {
            desiredPos = player.transform.position;
        }
    }
    private IEnumerator GroundPound()
    {
        movementLock = true;
        coolDownLock = true;
        changeAnimationState("boss_zombie_jump");
        Vector2 up = transform.position;
        up.y += 1.5f;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
        StartCoroutine(MoveWithinTime(transform.position, up, .395f));
        yield return new WaitForSeconds(.395f);
        up.y -= 1.5f;
        StartCoroutine(MoveWithinTime(transform.position, up, .132f));
        yield return new WaitForSeconds(.132f);
        groundPoundHitbox.GetComponent<GroundPoundHitbox>().Shockwave();
        float[] angles = { 0f, 24f, 48f, 72f, 96f, 120f, 144f, 168f, 192f, 216f, 240f, 264f, 288f, 312f, 336f, 360f };
        projectileLauncherScript.shootBulletToAngles(angles);
        yield return new WaitForSeconds(0.65f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
        changeAnimationState("boss_zombie_idle");
        movementLock = false;
        coolDownLock = false;
    }

    private void Dash()
    {
        coolDownLock = true;
        targetLock = true;
        movementSpeed += dashSpeedIncrease;
        StartCoroutine(DashEnd());
    }

    bool check;
    IEnumerator DashEnd()
    {
        check = Vector2.Distance((Vector2)transform.position, desiredPos) > 1.2;
        StartCoroutine(ThrowDirtBallsPerpendicular());
        while (check)
        {
            check = Vector2.Distance((Vector2)transform.position, desiredPos) > 1.2;
            yield return null;
        }
        targetLock = false;
        movementSpeed -= dashSpeedIncrease;
        StartCoroutine(GroundPound());
    }

    IEnumerator ThrowDirtBallsPerpendicular()
    {
        while (check)
        {
            Vector2 dir = desiredPos - (Vector2)transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float[] angles = { angle - 90f, angle + 90f };
            projectileLauncherScript.shootBulletToAngles(angles);
            yield return new WaitForSeconds(.75f);
        }
    }

    IEnumerator MoveWithinTime(Vector2 startPos, Vector2 endPos, float time)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

    private IEnumerator ThrowDirtBall()
    {
        movementLock = true;
        coolDownLock = true;
        changeAnimationState("BossZombieThrow");
        yield return new WaitForSeconds(0.2f);
        projectileLauncherScript.shootBulletAtPlayer();
        changeAnimationState("boss_zombie_idle");
        movementLock = false;
        coolDownLock = false;
    }

    Health healthScript;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player") && !movementLock)
        {
            healthScript = collision.gameObject.GetComponent<Health>();
            healthScript.OnChangeHealth(1);

        }
    }

    private void updateAnimation()
    {
        string state;
        if (currentAnimationState == "BossZombieThrow" || currentAnimationState == "boss_zombie_jump")
        {
            state = currentAnimationState;
        }
        else if (targetLock)
        {
            state = "BossZombieSprint";
        }
        else if (!movementLock)
        {
            state = "BossZombieWalk";
        }
        else
        {
            state = "boss_zombie_idle";
        }

        changeAnimationState(state);

        if (transform.position.x < desiredPos.x)
        {
            spriteRenderer.flipX = false;
            projectileLauncher.position = new Vector2(transform.position.x + 1.35f, transform.position.y);
        }
        else
        {
            spriteRenderer.flipX = true;
            projectileLauncher.position = new Vector2(transform.position.x - 1.35f, transform.position.y);
        }
    }

    private void changeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, groundPoundRange);
        Gizmos.DrawWireSphere(transform.position, rangeAttackRange);
    }

    private void UpdateHealthBar()
    {
        healthBarScript.setValue(health.currentHealth);
        if (health.currentHealth <= 0)
        {
            healthBar.SetActive(false);
        }
    }
    private void DisableCoroutines()
    {
        StopAllCoroutines();
        Debug.Log("stopped all coroutines");
    }

    private void WhenDying()
    {
        UpdateHealthBar();
        deathSound.Play();
        movementLock = true;
    }

    private void OnEnable()
    {
        Health.onHitEvent += UpdateHealthBar;
        Health.OnDie += WhenDying;
        Health.OnDie += DisableCoroutines;
    }

    private void OnDisable()
    {
        Health.onHitEvent -= UpdateHealthBar;
        Health.OnDie -= WhenDying;
        Health.OnDie -= DisableCoroutines;
    }
}
