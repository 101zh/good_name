using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthBar : MonoBehaviour
{
    [SerializeField] Slider fill;
    public void setMaxValue(int maxHealth)
    {
        fill.maxValue = maxHealth;
    }

    public void setValue(int health)
    {
        fill.value = health;
    }
}
