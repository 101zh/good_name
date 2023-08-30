using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public float currentTime =  0f;
    float startingTime = 20f;

    [SerializeField] TMP_Text countdownText;
    void Start()
    {
        currentTime = startingTime;
    }
    void Update()
    {
        if (pause_menu.gameIsPaused) return;
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");
        if (currentTime <= 0) 
        {
            currentTime = 0;
        }
    }
}
