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
    void DropCoin() 
    {
       Vector2 position = transform.position;
       Instantiate(coinPrefab, position,Quaternion.identity);
   }
    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void onHit(int amount)
    {
        if (onHitEvent != null){onHitEvent();}
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
