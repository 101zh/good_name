using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_gun_controller : MonoBehaviour
{
    // Start is called before the first frame update

    private SpriteRenderer sprite;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float Bulletspread;
    [SerializeField] private float coolDown; //after each shot
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool held = true;
    [SerializeField] private bool passive;
    float angle;
    private float coolDownTimer;
    private Renderer gunRenderer;
    enemy_controller enemyControllerScript;
    GameObject player;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        gunRenderer = GetComponent<Renderer>();
        enemyControllerScript = GetComponentInParent<enemy_controller>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!pause_menu.gameIsPaused && !passive)
        {
            if (coolDownTimer > 0) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
            if (held)
            {
                gunRotate();
                if (coolDownTimer == 0 && enemy_controller.currentMovementState== enemy_controller.movementState.safe) //checks if player has pressed the shoot button
                {
                    shootBullet();
                    coolDownTimer = coolDown;
                }
            }
        }
    }

    private void gunRotate()
    {   
        player = enemyControllerScript.player;
        // finds the position of the mouse using camera (has to be relative to it) and position of gun
        Vector2 dir = player.transform.position - transform.position;
        // Finding the angle to rotate using math
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Rotates the gun using math
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (!(angle <= 90 && angle >= -90))
        {
            sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
        }
    }

    private void shootBullet()
    {
        for (int i = 0; i<bulletsPerShot; i++){
            float RNG= Random.Range(-Bulletspread, Bulletspread);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //creates/spawns bullet
            bullet.transform.Rotate(bullet.transform.rotation.x,bullet.transform.rotation.y, bullet.transform.rotation.z+RNG, Space.Self);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
        }
        // GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //creates/spawns bullet

        // Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);

        // Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        // Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
    }
}
