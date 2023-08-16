using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    public int currentHealth, maxHealth;
    [SerializeField] private GameObject coinPrefab;

    [SerializeField]
    private bool isDead = false;
    private Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;
    public delegate void GetHit();
    public static event GetHit onHitEvent;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        InitializeHealth(maxHealth);
    }
    private void DropCoin()
    {
        int RNG = Random.Range(1, 7);
        for (int i = 0; i < RNG; i++)
        {
            float randOffsetX = Random.Range(-2f, 2f);
            float randOffsetY = Random.Range(-2f, 2f);
            Vector2 position = new Vector2(transform.position.x+randOffsetX, transform.position.y+randOffsetY);
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
        if (currentHealth <= 0) 
        {
            isDead = true;
            Destroy(gameObject);
            Debug.Log(isDead);
            DropCoin();
        }
        else
        if (!isDead)
        {
            currentHealth -= amount;
            sr.material = matWhite;
            Invoke("ResetMaterial", .1f);
            Debug.Log(currentHealth);
            return;
        
        }
        if (onHitEvent != null) 
        {
            onHitEvent();
        }
    }
    void ResetMaterial()
    {
        sr.material = matDefault;
    }
}
