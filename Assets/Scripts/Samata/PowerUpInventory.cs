using System.Collections.Generic;
using UnityEngine;

namespace BlockBrawl.Samata
{
    public enum PowerUpType
    {
        Bomba,
        Dondurma,
        Karistirma,
        CiftPuan
    }

    public class PowerUpInventory : MonoBehaviour
    {
        private static readonly PowerUpType[] AllTypes = (PowerUpType[])System.Enum.GetValues(typeof(PowerUpType));

        private readonly Dictionary<PowerUpType, int> counts = new Dictionary<PowerUpType, int>();

        public event System.Action<PowerUpType, int> InventoryChanged;

        private void Awake()
        {
            foreach (PowerUpType type in AllTypes)
            {
                counts[type] = 0;
            }
        }

        public int GetCount(PowerUpType type)
        {
            return counts[type];
        }

        public void Add(PowerUpType type, int amount = 1)
        {
            counts[type] += amount;
            InventoryChanged?.Invoke(type, counts[type]);
        }

        public bool TrySpend(PowerUpType type)
        {
            if (counts[type] <= 0)
            {
                return false;
            }

            counts[type]--;
            InventoryChanged?.Invoke(type, counts[type]);
            return true;
        }

        public PowerUpType GetRandomType()
        {
            return AllTypes[Random.Range(0, AllTypes.Length)];
        }
    }
}
