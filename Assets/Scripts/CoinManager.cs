using UnityEngine;
using TMPro; // if using TextMeshPro

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // singleton for easy access

    [Header("UI")]
    public TMP_Text coinText; // assign in inspector

    private int coins = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        UpdateUI();
    }

    /// <summary>
    /// Add coins to the player
    /// </summary>
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    /// <summary>
    /// Remove coins (optional)
    /// </summary>
    public void RemoveCoins(int amount)
    {
        coins = Mathf.Max(coins - amount, 0);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = $"Coins: {coins}";
    }

    /// <summary>
    /// Get current coins
    /// </summary>
    public int GetCoins()
    {
        return coins;
    }
}
