using System;
using UnityEngine;

namespace CubicChaos.Core
{
    public static class CurrencyManager
    {
        private const string CoinsKey = "CubicChaos.Coins";

        public static event Action<int> CurrencyChanged;

        public static int Coins => PlayerPrefs.GetInt(CoinsKey, 0);

        public static void Add(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            int newValue = Coins + amount;
            PlayerPrefs.SetInt(CoinsKey, newValue);
            CurrencyChanged?.Invoke(newValue);
        }

        public static bool TrySpend(int amount)
        {
            if (amount <= 0 || Coins < amount)
            {
                return false;
            }

            int newValue = Coins - amount;
            PlayerPrefs.SetInt(CoinsKey, newValue);
            CurrencyChanged?.Invoke(newValue);
            return true;
        }
    }
}
