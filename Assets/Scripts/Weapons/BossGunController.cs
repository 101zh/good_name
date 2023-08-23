using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGunController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float bulletSpread;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool held = true;
    [SerializeField] private bool passive;
    public float angle;
    public bool aimOverride;
    [SerializeField] Transform Boss;
    BossZombieController BossScript;
    private void Awake()
    {
        if (Boss.tag.Equals("BossZombie")) BossScript = Boss.GetComponent<BossZombieController>();
    }

    public void gunRotate()
    {
        Vector2 dir = BossScript.player.transform.position - transform.position;
        // Finding the angle to rotate using math
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Rotates the gun using math
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void shootBulletCalculated()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //creates/spawns bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
        }
    }

}
