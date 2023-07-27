using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinText : MonoBehaviour
{

    private TMP_Text coinCounter;
    private int coinNumber;

    private void Start()
    {
        coinNumber = 0;
        coinCounter = GetComponent<TMP_Text>();
    }

    public void incrementCoins(int amount)
    {
        coinCounter.text = "Coins: " + coinNumber.ToString();
    }
}
