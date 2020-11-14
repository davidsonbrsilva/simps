using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class Manager : MonoBehaviour
    {
        [Tooltip("Configurações da presa.")]
        [SerializeField] private Instantable prey;

        [Tooltip("Configurações do predador terrestre.")]
        [SerializeField] private Instantable landPredator;

        [Tooltip("Configurações do predador aéreo.")]
        [SerializeField] private Instantable aerialPredator;

        [Tooltip("Configurações do predador rastejante.")]
        [SerializeField] private Instantable crawlingPredator;

        public List<Transform> Predators { get; private set; }
        public List<Transform> Preys { get; private set; }
    }
}