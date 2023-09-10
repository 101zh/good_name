using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoss : MonoBehaviour
{
    BossGunController StatBossGun;
    GameObject healthBar;
    BossHealthBar healthBarScript;
    Health health;

    [SerializeField] GameObject preLaser;
    [SerializeField] GameObject laser;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject preSword;
    float coolDownTimer;
    [SerializeField] float coolDown;
    [SerializeField] AudioSource deathSound;

    private bool attackLock = false;

    // Update is called once per frame
    void Start()
    {

        healthBar = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(5).gameObject;
        healthBarScript = healthBar.GetComponent<BossHealthBar>();

        health = GetComponent<Health>();

        health.timeToDestroy = deathSound.clip.length;
        healthBar.SetActive(true);
        healthBarScript.setMaxValue(health.maxHealth);
        healthBarScript.setValue(health.currentHealth);

        ///attack = GameObject.FindGameObjectWithTag("Instantiator").GetComponent<InstantiationExample>();
        StatBossGun = transform.GetChild(0).GetComponent<BossGunController>();
        StartCoroutine(ShootHomingFireballCoroutine());
    }

    void Update()
    {
        if (pause_menu.gameIsPaused || attackLock) return;

        if (coolDownTimer > 0) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
        if (coolDownTimer == 0)
        {
            int rand = Random.Range(1, 11);
            if (rand <= 4)
            {
                StartCoroutine(Lasers());
            }
            else if (rand <= 8)
            {
                StartCoroutine(Swords());
            }
            else
            {
                StartCoroutine(Lasers());
                StartCoroutine(Swords());
            }
            coolDownTimer = coolDown;
        }
    }

    IEnumerator ShootHomingFireballCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.25f);
            StatBossGun.shootHomingBulletAtPlayer(true);
        }
    }

    IEnumerator Lasers()
    {
        InstantiatePreLasers();
        yield return new WaitForSeconds(.75f);
        InstantiateLasers();
    }

    IEnumerator Swords()
    {
        InstantiatePreSwords();
        yield return new WaitForSeconds(.5f);
        InstantiateSwords();
    }

    private void InstantiatePreLasers()
    {
        Instantiate(preLaser, new Vector2(0, 0), preLaser.transform.rotation);
        for (int i = 0; i < 5; i++)
        {
            Instantiate(preLaser, new Vector2(6 * i, 0), preLaser.transform.rotation);
            Instantiate(preLaser, new Vector2(-6 * i, 0), preLaser.transform.rotation);
        }
    }

    private void InstantiateLasers()
    {
        Instantiate(laser, new Vector2(0, 0), laser.transform.rotation);
        for (int i = 0; i < 5; i++)
        {
            Instantiate(laser, new Vector2(6 * i, 0), laser.transform.rotation);
            Instantiate(laser, new Vector2(-6 * i, 0), laser.transform.rotation);
        }
    }

    private void InstantiateSwords()
    {
        Instantiate(sword, new Vector2(-25, 0), sword.transform.rotation);
        for (int i = 0; i < 4; i++)
        {
            Instantiate(sword, new Vector2(-25, 5.5f * i), sword.transform.rotation);
            Instantiate(sword, new Vector2(-25, -5.5f * i), sword.transform.rotation);
        }
    }

    private void InstantiatePreSwords()
    {
        Instantiate(preSword, new Vector2(0, 0), preSword.transform.rotation);
        for (int i = 0; i < 4; i++)
        {
            Instantiate(preSword, new Vector2(0, 5.5f * i), preSword.transform.rotation);
            Instantiate(preSword, new Vector2(0, -5.5f * i), preSword.transform.rotation);
        }
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
        attackLock = true;
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
