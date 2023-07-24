using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    [SerializeField]
    private bool isDead = false;
    private Material matWhite;
    private Material matDefault;
    private UnityEngine.Object explosionRef;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        explosionRef = Resources.Load("Explosion");
    }
    //void DropCoin()
    //{
//        Vector2 position = transform.position;
        //GameObject coin = Instantiate(Coin, position,Quaternion.identity);
   // }
    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount)
    {
        if (isDead)
        {
            return;
            currentHealth -= amount;
            sr.material = matWhite;
        }
        else
        {
            Invoke("ResetMaterial", .1f);
        }
        if (currentHealth <= 0)
        {
            isDead = true;
            Destroy(gameObject);
            GameObject explosion = (GameObject)Instantiate(explosionRef);
            explosionRef.transform.position = new Vector2(transform.position.x,transform.position.y + .3f,);
            //ropCoin();
        }
    }
    void ResetMaterial()
    {
        sr.material = matDefault;
    }
}
