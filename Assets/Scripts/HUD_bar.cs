using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUD_bar : MonoBehaviour
{
    Slider fill;
    TMP_Text text;

    private void Start(){
        fill = GetComponent<Slider>();
        text = GetComponentInChildren<TMP_Text>();
    }

    public void setMaxValue(int maxHealth){
        fill.maxValue=maxHealth;
    }

    public void setValue(int health, int maxHealth){
        fill.value=health;
        text.text=health.ToString()+"/"+maxHealth.ToString();
        Debug.Log(text.text);
    }
}
