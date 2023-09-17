using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] public int currentHealth, maxHealth;
    [SerializeField] private int coinDropAmount, coinDropVariance;
    [SerializeField] public int currentDefense, maxDefense;
    [SerializeField] private GameObject coinPrefab;
    private bool isDead = false;
    [HideInInspector] public float timeToDestroy = 0.01f;
    private Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;
    public delegate void GetHit();
    public static event GetHit onHitEvent;
    public delegate void Die();
    public static event Die OnBossDeath;
    public static event Die OnPlayerDie;
    bool recoveringDefense = false;
    Coroutine recoverDefenseCoroutine;
    bool canTakeDamage = true;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        InitializeHealth(maxHealth);
        InitializeDefense(maxDefense);
    }
    private void DropCoin()
    {
        int RNG = Random.Range(coinDropAmount - coinDropVariance, coinDropAmount + coinDropVariance);
        for (int i = 0; i < RNG; i++)
        {
            float randOffsetX = Random.Range(-2f, 2f);
            float randOffsetY = Random.Range(-2f, 2f);
            Vector2 position = new Vector2(transform.position.x + randOffsetX, transform.position.y + randOffsetY);
            Instantiate(coinPrefab, position, Quaternion.identity);
        }
    }
    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void OnChangeHealth(int amount) /// Negative values increase health, positive values take away health. 
    {
        if (!canTakeDamage)
        {
            return;
        }
        else
        {
            RestartDefenseRecover();
            EnableInvincibilityFrames();
            sr.material = matWhite;
            Invoke("ResetMaterial", .1f);
            if (currentDefense <= 0)
            {
                currentHealth -= amount;
                Debug.Log(gameObject.name + "; " + currentHealth.ToString());
            }
            else
            {
                currentDefense -= amount;
            }
        }

        if (currentHealth <= 0)
        {
            if (gameObject.tag.Equals("Player"))
            {
                isDead = true;
                OnPlayerDie?.Invoke();
                FindObjectOfType<AudioManager>().Play("PlayerDeath");
                pause_menu.playerIsDead = true;
            }
            else
            {
                isDead = true;
                Destroy(gameObject, timeToDestroy);
                DropCoin();
            }

            Debug.Log("Dead");

            if (OnBossDeath != null && gameObject.tag=="Boss")
            {
                OnBossDeath();
            }
        }

        if (onHitEvent != null)
        {
            onHitEvent();
        }
    }

    public void InitializeDefense(int defenseValue)
    {
        currentDefense = defenseValue;
        maxDefense = defenseValue;
        if (recoverDefenseCoroutine != null)
        {
            StopCoroutine(recoverDefenseCoroutine);
        }
    }

    IEnumerator RecoverDefense()
    {
        recoveringDefense = true;
        bool recoveredAllDefense = currentDefense >= maxDefense;
        while (!isDead && !recoveredAllDefense)
        {
            yield return new WaitForSeconds(4f);
            currentDefense += 1;
            onHitEvent?.Invoke();
            recoveredAllDefense = currentDefense >= maxDefense;
        }
        recoveringDefense = false;
    }

    private void EnableInvincibilityFrames()
    {
        if (canTakeDamage && gameObject.tag.Equals("Player"))
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    IEnumerator InvincibilityFrames()
    {
        canTakeDamage = false;
        for (int i = 0; i <= 5; i++)
        {

            if (i % 2 == 0)
            {
                sr.material = matWhite;
            }
            else
            {
                sr.material = matDefault;
            }
            yield return new WaitForSeconds(.1f);
        }
        canTakeDamage = true;
    }

    private void RestartDefenseRecover()
    {
        if (!recoveringDefense)
        {
            Debug.Log("Recovering");
            recoverDefenseCoroutine = StartCoroutine(RecoverDefense());
        }
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }

}
