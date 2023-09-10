using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Transform rotationCenter;
    float posX, posY, angle = 0f;
    [SerializeField] float rotationRadius = 2f, angularSpeed = 2f;

    float coolDownTimer;
    [SerializeField] float coolDown;
    [SerializeField] bool coolDownLock;
    Transform projectileLauncher;
    BossGunController projectileLauncherScript;
    SpriteRenderer spriteRenderer;
    GameObject healthBar;
    BossHealthBar healthBarScript;
    Health health;
    [SerializeField] int FireWallMaximum;
    [SerializeField] AudioSource deathSound;

    private void Start()
    {
        projectileLauncher = transform.GetChild(0);
        projectileLauncherScript = projectileLauncher.GetComponent<BossGunController>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        healthBar = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(5).gameObject;
        healthBarScript = healthBar.GetComponent<BossHealthBar>();

        health = GetComponent<Health>();

        health.timeToDestroy = deathSound.clip.length;
        healthBar.SetActive(true);
        healthBarScript.setMaxValue(health.maxHealth);
        healthBarScript.setValue(health.currentHealth);

        rotationCenter = GameObject.FindGameObjectWithTag("Player").transform;
        coolDownTimer = coolDown;
    }
    void Update()
    {
        if (pause_menu.gameIsPaused) return;
        if (coolDownTimer > 0 && !coolDownLock) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
        if (coolDownTimer == 0)
        {
            bool foundAttack = false;
            while (!foundAttack)
            {
                int rand = Random.Range(1, 11);
                if (rand <= 3)
                {
                    ThrowHomingFireBall();
                }
                else if (rand <= 5)
                {
                    EmitFireBalls();
                }
                else if (rand <= 7)
                {
                    StartCoroutine(Invisibility());
                }
                else if (fireWallCount < FireWallMaximum)
                {
                    StartCoroutine(FireWall());
                }
                else
                {
                    foundAttack = false; // b/c it gets flipped later making it true
                }
                foundAttack = !foundAttack;
            }
            coolDownTimer = coolDown;
        }
    }

    void FixedUpdate()
    {
        if (pause_menu.gameIsPaused) return;
        Move();
    }

    private void Move()
    {
        posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * angularSpeed;

        if (angle >= 360f)
        {
            angle = 0f;
        }
    }

    IEnumerator Invisibility()
    {
        Coroutine _coroutine;
        spriteRenderer.enabled = false;
        coolDownLock = true;
        _coroutine = StartCoroutine(ThrowFireBallsMadly());
        yield return new WaitForSeconds(5f);
        StopCoroutine(_coroutine);
        coolDownLock = false;
        spriteRenderer.enabled = true;
    }

    IEnumerator ThrowFireBallsMadly()
    {
        yield return new WaitForSeconds(.5f);
        projectileLauncherScript.bulletSpeed = 15f;
        while (true)
        {
            ThrowFireBall();
            yield return new WaitForSeconds(.16f);
        }
    }

    [SerializeField] GameObject preFireWall;
    [SerializeField] GameObject fireWall;
    [SerializeField] float fireWallAppearanceTime;
    private int fireWallCount;
    private IEnumerator FireWall()
    {
        Vector2 location = rotationCenter.position;
        location.x = 0;
        fireWallCount += 1;
        GameObject preFireWallInstance = Instantiate(preFireWall, location, preFireWall.transform.rotation);
        Destroy(preFireWallInstance, fireWallAppearanceTime + 5.6f);
        yield return new WaitForSeconds(fireWallAppearanceTime);
        Instantiate(fireWall, location, fireWall.transform.rotation);
        yield return new WaitForSeconds(5.6f);
        fireWallCount -= 1;
    }

    private void ThrowFireBall()
    {
        projectileLauncherScript.shootBulletAtPlayer(true);
    }

    private void ThrowHomingFireBall()
    {
        projectileLauncherScript.shootHomingBulletAtPlayer(true);
    }

    private void EmitFireBalls()
    {
        projectileLauncherScript.bulletSpeed = 7f;
        float[] angles = { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };
        projectileLauncherScript.shootBulletToAngles(angles, true);
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
        if (pause_menu.playerIsDead) return;
        DisableCoroutines();
        UpdateHealthBar();
        deathSound.Play();
        GetComponent<BoxCollider2D>().enabled = false;
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
