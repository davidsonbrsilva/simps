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

        public List<SignalController> Signals { get; private set; }
        public bool IsHearingSomething { get { return Signals.Count > 0; } }
        public bool HeardAerialPredator { get { return heardedAerialPredator; } private set { heardedAerialPredator = value; } }
        public bool HeardLandPredator { get { return heardedLandPredator; } private set { heardedLandPredator = value; } }
        public bool HeardCrowlingPredator { get { return heardedCrowlingPredator; } private set { heardedCrowlingPredator = value; } }
        public bool HeardSomething { get { return HeardAerialPredator || HeardLandPredator || HeardCrowlingPredator; } }
        public AgentController RecognizedPredator { get; private set; }

        private void Awake()
        {
            Signals = new List<SignalController>();
            agent = transform.parent.parent.GetComponent<AgentController>();
            spawner = GameObject.FindWithTag("Core").GetComponent<Spawner>();

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
                    var signal = collision.GetComponent<SignalController>();
                    bool contains = false;

                    for (int s = 0; s < Signals.Count; ++s)
                    {
                        if (Signals[s].Symbol == signal.Symbol)
                        {
                            contains = true;
                            break;
                        }
                    }

                    if (!contains)
                    {
                        Signals.Add(collision.GetComponent<SignalController>());
                        var allRecognizedPredators = Recognize(signalCollided.Symbol);

                        if (allRecognizedPredators.Count > 0)
                        {
                            var recognizedPredator = allRecognizedPredators[Random.Range(0, allRecognizedPredators.Count)];
                            RecognizedPredator = spawner.AllPredators[recognizedPredator].GetComponent<AgentController>();

                            if (RecognizedPredator.IsAerialPredator)
                            {
                                HeardAerialPredator = true;
                            }
                            else if (RecognizedPredator.IsLandPredator)
                            {
                                HeardLandPredator = true;
                            }
                            else if (RecognizedPredator.IsCrowlingPredator)
                            {
                                HeardCrowlingPredator = true;
                            }

                            //Signal = signalCollided;
                            //IsHearingSomething = true;
                        }
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Signal"))
            {
                var signalController = collision.GetComponent<SignalController>();

                if (Signals.Contains(signalController))
                {
                    Signals.Remove(signalController);
                }

                /*if (IsHearingSomething)
                {
                    IsHearingSomething = false;
                }*/
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

            if (!HeardSomething)
            {
                RecognizedPredator = null;
            }
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