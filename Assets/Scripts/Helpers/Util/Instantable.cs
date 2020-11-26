using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    [System.Serializable]
    public class Instantable
    {
        [SerializeField] private List<GameObject> prefabs;
        [SerializeField] private Transform parent;
        [SerializeField] int amount = 1;

        public List<GameObject> Prefabs { get { return prefabs; } set { prefabs = value; } }
        public Transform Parent { get { return parent; } set { parent = value; } }
        public int Amount { get { return amount; } set { amount = value; } }

        public Instantable(GameObject prefab, Transform parent, int amount = 1)
        {
            Prefabs = prefabs;
            Parent = parent;
            Amount = amount;
        }

        public GameObject GetRandomPrefab()
        {
            return Prefabs[Random.Range(0, prefabs.Count)];
        }
    }
}