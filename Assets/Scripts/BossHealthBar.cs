using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthBar : MonoBehaviour
{
    Slider fill;

    private void Start()
    {
        fill = GetComponent<Slider>();
    }

    public void setMaxValue(int maxHealth)
    {
        fill.maxValue = maxHealth;
    }

    public void setValue(int health)
    {
        fill.value = health;
    }
}
