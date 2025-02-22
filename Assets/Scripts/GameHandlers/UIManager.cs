using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI coinText;
    
    void OnEnable()
    {
        if (playerData != null)
        {
            playerData.OnCoinChanged += UpdateCoinText;
            UpdateCoinText(playerData.Coin);
        }
    }

    void OnDisable()
    {
        if (playerData != null)
        {
            playerData.OnCoinChanged -= UpdateCoinText;
        }
    }

    private void UpdateCoinText(int newCoinValue)
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + newCoinValue.ToString();
        }
    }
}
