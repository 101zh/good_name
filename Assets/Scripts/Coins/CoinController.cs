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
    player_controller player_script;
    [SerializeField] AudioSource PickUpSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        player_script= player.GetComponent<player_controller>();
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
            player_script.incrementCoins(1);
            PickUpSound.Play();
            GetComponent<Collider2D>().enabled=false;
            GetComponent<SpriteRenderer>().enabled=false;
            Destroy(gameObject,0.2f);
        }
    }


}
