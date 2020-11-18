using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class RecognitionController : MonoBehaviour
    {
        [SerializeField] private bool heardedAerialPredator;
        [SerializeField] private bool heardedLandPredator;
        [SerializeField] private bool heardedCrowlingPredator;
        [SerializeField] private float timeOfDangerMemory = 2f;

        private AgentController agent;
        private Spawner spawner;

        private float HeardedAerialPredatorAt { get; set; }
        private float HeardedLandPredatorAt { get; set; }
        private float HeardedCrowlingPredatorAt { get; set; }

        public SignalController Signal { get; private set; }
        public bool IsHearingSomething { get; private set; }
        public bool HeardAerialPredator { get { return heardedAerialPredator; } private set { heardedAerialPredator = value; } }
        public bool HeardLandPredator { get { return heardedLandPredator; } private set { heardedLandPredator = value; } }
        public bool HeardCrowlingPredator { get { return heardedCrowlingPredator; } private set { heardedCrowlingPredator = value; } }
        public bool HeardSomething { get { return HeardAerialPredator || HeardLandPredator || HeardCrowlingPredator; } }

        private void Awake()
        {
            agent = transform.parent.parent.GetComponent<AgentController>();
            spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

            HeardedAerialPredatorAt = 0f;
            HeardedLandPredatorAt = 0f;
            HeardedCrowlingPredatorAt = 0f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Signal"))
            {
                SignalController signalCollided = collision.GetComponent<SignalController>();

                if (signalCollided.Sender != transform.parent.parent.gameObject)
                {
                    var allRecognizedPredators = Recognize(signalCollided.Symbol);

                    if (allRecognizedPredators.Count > 0)
                    {
                        var recognizedPredator = allRecognizedPredators[Random.Range(0, allRecognizedPredators.Count)];
                        var recognizedPredatorController = spawner.AllPredators[recognizedPredator].GetComponent<AgentController>();

                        if (recognizedPredatorController.IsAerialPredator)
                        {
                            HeardAerialPredator = true;
                        }
                        else if (recognizedPredatorController.IsLandPredator)
                        {
                            HeardLandPredator = true;
                        }
                        else if (recognizedPredatorController.IsCrowlingPredator)
                        {
                            HeardCrowlingPredator = true;
                        }

                        Signal = signalCollided;
                        IsHearingSomething = true;
                        Debug.Log("Ouviu alguma coisa.");
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Signal"))
            {
                if (IsHearingSomething)
                {
                    IsHearingSomething = false;
                }
            }
        }

        private void Update()
        {
            if (HeardAerialPredator)
            {
                if (HeardedAerialPredatorAt > timeOfDangerMemory)
                {
                    HeardAerialPredator = false;
                }
            }
            else
            {
                HeardedAerialPredatorAt = 0f;
            }

            if (HeardLandPredator)
            {
                if (HeardedLandPredatorAt > timeOfDangerMemory)
                {
                    HeardLandPredator = false;
                }
            }
            else
            {
                HeardedLandPredatorAt = 0f;
            }

            if (HeardCrowlingPredator)
            {
                if (HeardedCrowlingPredatorAt > timeOfDangerMemory)
                {
                    HeardCrowlingPredator = false;
                }
            }
            else
            {
                HeardedCrowlingPredatorAt = 0f;
            }

            HeardedAerialPredatorAt += Time.deltaTime;
            HeardedLandPredatorAt += Time.deltaTime;
            HeardedCrowlingPredatorAt += Time.deltaTime;
        }

        /// <summary>
        /// Verifica se reconhece o símbolo recebido a partir da base de conhecimento.
        /// </summary>
        /// <param name="receivedSymbol">Símbolo recebido.</param>
        /// <returns>Lista de predadores reconhecidos.</returns>
        private List<int> Recognize(int receivedSymbol)
        {
            // Cria uma lista vazia de predadores reconhecidos.
            List<int> recognized = new List<int>();

            for (int predator = 0; predator < agent.Learner.Knowledgement.Length; ++predator)
            {
                for (int symbol = 0; symbol < agent.Learner.Knowledgement[predator].Count; ++symbol)
                {
                    if (receivedSymbol == agent.Learner.Knowledgement[predator][symbol])
                    {
                        recognized.Add(predator);

                        // Passa para o próximo predador.
                        break;
                    }
                }
            }

            return recognized;
        }
    }
}