using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinWallet : MonoBehaviour
{
    [SerializeField] public int TotalCoins = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Coin>(out Coin coin))
        {
            int value = coin.Collect();
            TotalCoins += value;
            UIHandler.Singleton.CoinCount.text = TotalCoins.ToString();
        }
    }

    public void SpendCoins(int val)
    {
        TotalCoins -= val;
    }
}
