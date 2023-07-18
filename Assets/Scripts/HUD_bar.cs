using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_bar : MonoBehaviour
{
    Slider fill;

    private void Start(){
        fill = GetComponent<Slider>();
    }

    public void setMaxValue(int health){
        fill.maxValue=health;
    }

    public void setValue(int health){
        fill.value=health;
    }
}
