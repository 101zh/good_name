using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinText : MonoBehaviour
{

    private TMP_Text coinCounter;

    private void Start()
    {
        coinCounter = GetComponent<TMP_Text>();
    }

    public void UpdateCoins(int coinNumber)
    {
        coinCounter.text = "Coins: " + coinNumber.ToString();
    }
}
