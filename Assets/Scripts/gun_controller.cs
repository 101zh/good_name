using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_pos : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera mainCam;
    private SpriteRenderer sprite;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float Bulletspread;
    [SerializeField] private float coolDown; //after each shot
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool held;
    float angle;
    private float coolDownTimer;
    private Renderer gunRenderer;
    private void Awake()
    {
        mainCam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        gunRenderer = GetComponent<Renderer>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (!pause_menu.gamePaused)
        {
            if (coolDownTimer > 0) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
            if (held)
            {
                gunRotate();
                if (Input.GetButtonDown("Fire1") && coolDownTimer == 0) //checks if player has pressed the shoot button
                {
                    shootBullet();
                    coolDownTimer = coolDown;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (!held) { gunRenderer.material.SetFloat("_Thickness", 0.06f); } else { gunRenderer.material.SetFloat("_Thickness", 0.0f); }
            // Allows player to pickup gun by pressing interact button (set to "e")
            if (Input.GetButtonDown("Interact"))
            {
                //Makes Gun follow the player, so it looks like the player is always holding it
                transform.SetParent(collision.gameObject.transform, true);
                held = true;
                transform.position = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - 0.5f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gunRenderer.material.SetFloat("_Thickness", 0.0f);
    }

    private void gunRotate()
    {
        // finds the position of the mouse using camera (has to be relative to it) and position of gun
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
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
