using UnityEngine;

namespace SIMPS
{
    /// <summary>
    /// Componente para agentes que avisam sobre perigos. Esses agentes dependem da visão.
    /// </summary>
    public class EmitterBehaviour : SimpsBehaviour
    {
        #region Inspector
        [SerializeField] private int emissions = 0;
        [SerializeField] private float signalRate = 0.25f;
        [SerializeField] private float lastEmission = 0f;        
        [SerializeField] private Instantable landSignal;
        [SerializeField] private Instantable aerialSignal;
        [SerializeField] private Instantable crowlingSignal;
        #endregion

        #region Private Attributes
        private AgentController agent;
        private Transform signalParent;
        #endregion

        #region Properties
        private bool CanEmit { get { return lastEmission + signalRate < Time.time; } }

        public bool Emitted { get; private set; }
        public int EmittedSignal { get; private set; }
        public int SeenPredator { get; private set; }
        #endregion

        #region Unity Methods
        public void Awake()
        {
            agent = GetComponent<AgentController>();
            signalParent = GameObject.Find("Signals").transform;
        }

        public void Update()
        {
            if (Emitted)
            {
                Emitted = false;
            }

            if (agent.Vision.IsSeeingPredator)
            {
                if (CanEmit)
                {
                    // Escolhe um predador entre os que estão no campo de visão.
                    int randomPredator = Random.Range(0, agent.Vision.AllPredators.Count);
                    Transform seenPredator = agent.Vision.AllPredators[randomPredator];
                    AgentController predator = seenPredator.GetComponent<AgentController>();

                    // Verifica o tipo de predador para avisar sobre ele.
                    if (predator.IsCrowlingPredator)
                    {
                        Emit(crowlingSignal, predator.GlobalIndex);
                    }
                    else if (predator.IsAerialPredator)
                    {
                        Emit(aerialSignal, predator.GlobalIndex);
                    }
                    else if (predator.IsLandPredator)
                    {
                        Emit(landSignal, predator.GlobalIndex);
                    }

                    lastEmission = Time.time;
                }
            }
        }
        #endregion

        #region Other Methods
        private void Emit(Instantable signal, int indexOfSeenPredator)
        {
            Emitted = true;
            SeenPredator = indexOfSeenPredator;

            emissions++;

            GameObject signalInstance = Instantiate(signal.Prefabs[0], transform.position, Quaternion.identity, signalParent);
            //SignalController signalController = signalInstance.GetComponent<SignalController>();

            /*if (agent.CanLearn)
            {
                if (agent.Learner.Knowledgement[indexOfSeenPredator].Count > 0)
                {
                    int choice = Random.Range(0, agent.Learner.Knowledgement[indexOfSeenPredator].Count);
                    signalController.Symbol = agent.Learner.Knowledgement[indexOfSeenPredator][choice];
                }
                else
                {
                    // Obtém um símbolo aleatório para emitir.
                    int randomSymbol = Random.Range(0, agent.Learner.AssociationValue.GetLength(0));
                    agent.Learner.UpdateLearning(randomSymbol, indexOfSeenPredator);
                    signalController.Symbol = randomSymbol;
                }
            }
            else
            {
                int randomSymbol = Random.Range(0, agent.Learner.AssociationValue.GetLength(0));
                signalController.Symbol = randomSymbol;
            }
            
            signalController.Sender = gameObject;

            EmittedSignal = signalController.Symbol;*/
        }

        public void Restart()
        {
            emissions = 0;
            lastEmission = 0;
        }
        #endregion
    }
}
