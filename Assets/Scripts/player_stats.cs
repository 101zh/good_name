using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_stats : MonoBehaviour
{
    [SerializeField] private HUD_bar health_bar;
    public int health;
    public int maxHealth;

    // Update is called once per frame

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            health -= 1;
            health_bar.setValue(health, maxHealth);
        }
    }
}
