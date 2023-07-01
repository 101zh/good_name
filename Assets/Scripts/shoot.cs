using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float bulletSpeed= 8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //checks if player has pressed the shoot button
        {
            shootBullet();
        }   
    }

    void shootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //creates/spawns bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
    }
}
