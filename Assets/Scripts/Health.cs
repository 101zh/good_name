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
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        InitializeHealth(maxHealth);
        StartCoroutine(InitializeDefense(maxDefense));
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
        if (onHitEvent != null)
        {
            onHitEvent();
        }

        currentHealth -= amount;
        sr.material = matWhite;
        Invoke("ResetMaterial", .1f);
        Debug.Log(gameObject.name + "; " + currentHealth.ToString());

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Dead");
            Destroy(gameObject);
            DropCoin();
            if (OnDie != null)
            {
                OnDie();
            }
        }
    }

    IEnumerator InitializeDefense(int defenseValue)
    {
        currentDefense = defenseValue;
        maxDefense = defenseValue;

        while (!isDead)
        {
            yield return new WaitForSeconds(4f);
            if (currentDefense != maxDefense)
            {
                currentDefense += 1;
            }
        }
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }
}
