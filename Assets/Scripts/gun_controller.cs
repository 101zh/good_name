using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_pos : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera mainCam;
    private GameObject player;
    private SpriteRenderer sprite;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;
    private void Awake()
    {
        mainCam=Camera.main;
        player = GameObject.FindWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!pause_menu.gamePaused)
        {
            //Makes Gun follow the player, so it looks like the player is always holding it
            transform.position = new Vector2(player.transform.position.x+0.25f, player.transform.position.y-0.75f);
            gunRotate();
            if (Input.GetButtonDown("Fire1")) //checks if player has pressed the shoot button
            {
                shootBullet();
            }   
        }
    }

    private void gunRotate()
    {
        // finds the position of the mouse using camera (has to be relative to it) and position of gun
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        // Finding the angle to rotate using math
        float angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        // Rotates the gun using math
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        if (!(angle<=90 && angle>=-90)) {
            sprite.flipY=true;
        } else{
            sprite.flipY=false;
        }
    }

    private void shootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //creates/spawns bullet

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>(); 
        rb.AddForce(firePoint.up*bulletSpeed, ForceMode2D.Impulse);

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
    }
}
