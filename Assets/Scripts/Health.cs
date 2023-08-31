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
    [SerializeField] private bool isDead = false;
    private Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;
    public delegate void GetHit();
    public static event GetHit onHitEvent;
    public delegate void Die();
    public static event Die OnDie;
    public delegate void PlayerDie();
    public static event Die OnPlayerDie;
    bool recoveringDefense = false;
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
        if (currentDefense <= 0)
        {
            RestartDefenseRecover();
            currentHealth -= amount;
            sr.material = matWhite;
            Invoke("ResetMaterial", .1f);
            Debug.Log(gameObject.name + "; " + currentHealth.ToString());
        }
        else
        {
            RestartDefenseRecover();
            currentDefense -= amount;
            sr.material = matWhite;
            Invoke("ResetMaterial", .1f);
        }

        if (currentHealth <= 0)
        {
            if (gameObject.tag.Equals("Player"))
            {
                isDead = true;
                OnPlayerDie?.Invoke();
                pause_menu.playerIsDead = true;
            }
            else
            {
                isDead = true;
                Destroy(gameObject, 0.01f);
                DropCoin();
            }

            Debug.Log("Dead");

            if (OnDie != null)
            {
                OnDie();
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

    private void RestartDefenseRecover()
    {
        if (!recoveringDefense)
        {
            Debug.Log("Recovering");
            StartCoroutine(RecoverDefense());
        }
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }

}
