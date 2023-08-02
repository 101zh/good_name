using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject player;
    Vector2 playerDir;
    [SerializeField] float moveSpeed;
    float acceleration;
    float timeStamp;
    bool flyToPlayer;
    coinText coinTextScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        coinTextScript = GameObject.FindWithTag("CoinUI").GetComponentInChildren<coinText>();
    }

    void Update()
    {
        if (flyToPlayer)
        {
            acceleration= Time.deltaTime/timeStamp;
            playerDir = -(transform.position - player.transform.position).normalized;
            rb.velocity = new Vector2(playerDir.x, playerDir.y) * moveSpeed *acceleration;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("Magnet"))
        {
            timeStamp=Time.deltaTime;
            flyToPlayer = true;
        }
        else if (collider.gameObject.tag.Equals("CoinCollector"))
        {
            coinTextScript.incrementCoins(1);
            Destroy(gameObject);
        }
    }


}
