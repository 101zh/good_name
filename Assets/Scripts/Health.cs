using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;
    public GameObject CoinPrefab;
    public Transform EnemyTransform;

    [SerializeField]
    private bool isDead = false;
    private Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        InitializeHealth(maxHealth);
    }
    void DropCoin() 
    {
       Vector2 position = EnemyTransform.position;
       Instantiate(CoinPrefab, position,Quaternion.identity);
   }
    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount)
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
        
    }
    void ResetMaterial()
    {
        sr.material = matDefault;
    }
}
