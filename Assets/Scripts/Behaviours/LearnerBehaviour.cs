using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    /// <summary>
    /// Classe para agentes que aprendem.
    /// </summary>
    public class LearnerBehaviour : MonoBehaviour
    {
        [SerializeField] private int receptions;
        [SerializeField] private float timeUntilNextAssociation = 2f;
        [SerializeField] private float weakenSmooth = 1;

        private Spawner spawner;
        private AgentController agent;
        //private SignalController lastHeardSignal;

        public float[,] AssociationMemory { get; private set; }
        public float[,] Inibition { get; private set; }
        //public int[,] NumberOfAssociations { get; private set; }
        public List<int>[] Knowledgement { get; private set; }
        //public int Receptions { get { return receptions; } set { receptions = value; } }
        //public bool Learned { get; private set; }
        //public int PredatorSeen { get; private set; }

        private void Awake()
        {
            spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            agent = GetComponent<AgentController>();
            //lastHeardSignal = null;
        }

        private void Start()
        {
            AssociationMemory = new float[spawner.Symbols, spawner.AllPredators.Count];
            Inibition = new float[spawner.Symbols, spawner.AllPredators.Count];
            //NumberOfAssociations = new int[spawner.Symbols, spawner.AllPredators.Count];
            Knowledgement = new List<int>[spawner.AllPredators.Count];
            //Receptions = 0;

            InitLearning();
            InitKnowledgement();
            PrintAssociationValues();
        }

        private void Update()
        {
            for (int symbol = 0; symbol < Inibition.GetLength(0); ++symbol)
            {
                for (int predator = 0; predator < Inibition.GetLength(1); ++predator)
                {
                    Inibition[symbol, predator] += Time.deltaTime;
                }
            }

            for (int signal = 0; signal < agent.Hearing.Signals.Count; ++signal)
            {
                for (int predator = 0; predator < agent.Vision.AllPredators.Count; ++predator)
                {
                    Reinforce
                    (
                        agent.Hearing.Signals[signal].Symbol,
                        spawner.AllPredators.IndexOf(agent.Vision.AllPredators[predator])
                    );
                    
                }
            }

            for (int symbol = 0; symbol < spawner.Symbols; ++symbol)
            {
                for (int predator = 0; predator < spawner.AllPredators.Count; ++predator)
                {
                    Weaken(symbol, predator);
                }
            }
        }

        public void InitLearning()
        {
            for (int line = 0; line < AssociationMemory.GetLength(0); ++line)
            {
                for (int column = 0; column < AssociationMemory.GetLength(1); ++column)
                {
                    AssociationMemory[line, column] = 0.0f;
                }
            }
        }

        private void InitKnowledgement()
        {
            for (int column = 0; column < AssociationMemory.GetLength(1); column++)
            {
                Knowledgement[column] = new List<int>();
            }
        }

        private void Reinforce(int symbol, int predator)
        {
            if (Inibition[symbol, predator] >= timeUntilNextAssociation)
            {
                float biggest = 0f;

                if (Knowledgement[predator].Count > 0)
                {
                    var choice = Random.Range(0, Knowledgement[predator].Count);
                    var randomSymbol = Knowledgement[predator][choice];
                    biggest = AssociationMemory[randomSymbol, predator];
                }

                AssociationMemory[symbol, predator] += AssociationMemory[symbol, predator] + 0.1f * (1f - (biggest - AssociationMemory[symbol, predator])) + 0.01f;
                AssociationMemory[symbol, predator] = Mathf.Clamp(AssociationMemory[symbol, predator], 0f, 1f);
                UpdateKnowledgement(predator);
                Inibition[symbol, predator] = 0f;
                PrintAssociationValues();
            }
        }

        private void Weaken(int symbol, int predator)
        {
            if (Inibition[symbol, predator] >= timeUntilNextAssociation)
            {
                float biggest = 0f;

                if (Knowledgement[predator].Count > 0)
                {
                    var choice = Random.Range(0, Knowledgement[predator].Count);
                    var randomSymbol = Knowledgement[predator][choice];
                    biggest = AssociationMemory[randomSymbol, predator];
                }

                AssociationMemory[symbol, predator] = (AssociationMemory[symbol, predator] - 0.1f * (biggest - AssociationMemory[symbol, predator]) - 0.01f) * Time.deltaTime * weakenSmooth;
                AssociationMemory[symbol, predator] = Mathf.Clamp(AssociationMemory[symbol, predator], 0f, 1f);
                UpdateKnowledgement(predator);
            }
        }

        private void UpdateKnowledgement(int predator)
        {
            // Limpa todo o conhecimento relacionado ao predador.
            Knowledgement[predator].Clear();

            // Armazena o valor da primeira associação à variável "bigger".
            float bigger = AssociationMemory[0, predator];

            // Adiciona o primeiro símbolo na base de conhecimento.
            Knowledgement[predator].Add(0);

            // Percorre todos os outros símbolos para descobrir o maior valor de associação.
            for (int symbol = 1; symbol < AssociationMemory.GetLength(0); ++symbol)
            {
                // Se encontrou valor maior...
                if (AssociationMemory[symbol, predator] > bigger)
                {
                    // Armazena o valor encontrado à variável "bigger".
                    bigger = AssociationMemory[symbol, predator];

                    // Limpa a base de conhecimento.
                    Knowledgement[predator].Clear();

                    // Adiciona o símbolo encontrado à base de conhecimento.
                    Knowledgement[predator].Add(symbol);
                }
                // Se encontrou valor igual...
                else if (AssociationMemory[symbol, predator] == bigger)
                {
                    // Adiciona o símbolo encontrado à base de conhecimento.
                    Knowledgement[predator].Add(symbol);
                }
            }
        }

        public void Restart()
        {
            InitLearning();
            //Receptions = 0;
        }

        private float Sigmoide(int x)
        {
            return 1 / (1 + (float)System.Math.Exp(-(0.1 * x - 5)));
        }

        private void PrintAssociationValues()
        {
            string text = agent.Name + "\n";

            for (int predator = 0; predator < AssociationMemory.GetLength(1); ++predator)
            {
                for (int symbol = 0; symbol < AssociationMemory.GetLength(0); ++symbol)
                {
                    text += System.Math.Round(AssociationMemory[symbol, predator], 2).ToString("0.00 ");
                }

                text += "\n";
            }

            Debug.Log(text);
        }
    }
}