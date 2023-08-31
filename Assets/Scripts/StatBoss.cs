using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoss : MonoBehaviour
{
    float timePassed = 0f;
    InstantiationExample attack;
    Health statbosshealth;
    BossGunController StatBossGun;
    // Update is called once per frame
    void Start()
    {
        ///attack = GameObject.FindGameObjectWithTag("Instantiator").GetComponent<InstantiationExample>();
        StatBossGun = transform.GetChild(0).GetComponent<BossGunController>();
        StartCoroutine(ShootHomingFireballCoroutine());

    }
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= 5)
        {
            
            ///attack.SwordAttack();
            timePassed = 0;
        }
    }
    IEnumerator ShootHomingFireballCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);
            StatBossGun.shootHomingBulletAtPlayer();
        }
    }
}
