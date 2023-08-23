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
    private float angle;
    [SerializeField] private bool bulletSpreadON;
    [SerializeField] Transform Boss;
    BossZombieController BossScript;
    private void Awake()
    {
        if (Boss.tag.Equals("BossZombie")) BossScript = Boss.GetComponent<BossZombieController>();
    }

    private void gunRotateToPlayer()
    {
        Vector2 dir = BossScript.player.transform.position - transform.position;
        // Finding the angle to rotate using math
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Rotates the gun using math
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void gunRotateToAngle(float angle)
    {
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void shootBulletToAngles(float[] angles)
    {
        for (int i = 0; i < angles.Length - 1; i++)
        {
            float RNG = 0;
            if (bulletSpreadON)
            {
                RNG = Random.Range(-bulletSpread, bulletSpread);
            }
            gunRotateToAngle(angles[i]);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //creates/spawns bullet
            bullet.transform.Rotate(bullet.transform.rotation.x, bullet.transform.rotation.y, bullet.transform.rotation.z + RNG, Space.Self);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    public void shootBulletAtPlayer()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float RNG = 0;
            if (bulletSpreadON)
            {
                RNG = Random.Range(-bulletSpread, bulletSpread);
            }
            gunRotateToPlayer();

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //creates/spawns bullet
            bullet.transform.Rotate(bullet.transform.rotation.x, bullet.transform.rotation.y, bullet.transform.rotation.z + RNG, Space.Self);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
        }
    }

}
