using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoss : MonoBehaviour
{
    float timePassed = 0f;
    InstantiationExample attack;
    // Update is called once per frame
    void Start()
    {
        attack = GameObject.FindGameObjectWithTag("Instantiator").GetComponent<InstantiationExample>();
        
    }
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= 5)
        {
            attack.SwordAttack();
            timePassed = 0;
        }
    }
}
