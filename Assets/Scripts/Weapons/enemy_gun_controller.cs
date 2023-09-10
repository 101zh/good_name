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
    [SerializeField] private float bulletdelay;
    [SerializeField] private float coolDown; //after each shot
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool held = true;
    [SerializeField] private bool passive;

    float angle;
    private float coolDownTimer;
    enemy_controller enemyControllerScript;
    GameObject player;
    [SerializeField] AudioSource gunShotSound;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
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
                if (coolDownTimer == 0 && enemyControllerScript.currentMovementState == enemy_controller.movementState.safe) //checks if player has pressed the shoot button
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
        PlayBulletSound();
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Invoke("FireBullet", bulletdelay * i);
        }
    }

    private void PlayBulletSound(){
        if(!gunShotSound.isPlaying) gunShotSound.Play();
    }

    private void FireBullet()
    {
        float RNG = Random.Range(-Bulletspread, Bulletspread);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //creates/spawns bullet
        bullet.transform.Rotate(bullet.transform.rotation.x, bullet.transform.rotation.y, bullet.transform.rotation.z + RNG, Space.Self);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }

}