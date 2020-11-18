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

        private Spawner spawner;
        private AgentController agent;
        private SignalController lastHeardSignal;

        public float[,] AssociationValue { get; private set; }
        public int[,] NumberOfAssociations { get; private set; }
        public List<int>[] Knowledgement { get; private set; }
        public int Receptions { get { return receptions; } set { receptions = value; } }
        public bool Learned { get; private set; }
        public int PredatorSeen { get; private set; }

        private void Awake()
        {
            spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            agent = GetComponent<AgentController>();
            //lastHeardSignal = null;
        }

        private void Start()
        {
            AssociationValue = new float[spawner.Symbols, spawner.AllPredators.Count];
            NumberOfAssociations = new int[spawner.Symbols, spawner.AllPredators.Count];
            Knowledgement = new List<int>[spawner.AllPredators.Count];
            Debug.Log(Knowledgement.Length);
            Receptions = 0;

            InitLearning();
            InitKnowledgement();
            PrintAssociationValues();
        }

        private void Update()
        {
            /*if (Learned)
            {
                Learned = false;
            }*/

            
            // Se ouviu alguma coisa.
            if (agent.Hearing.IsHearingSomething)
            {
                Debug.Log("Teste2");
                /*if (lastHeardSignal != agent.Hearing.Signal)
                {*/
                // Se está vendo alguém.
                if (agent.Vision.IsSeeingPredator)
                    {
                    Debug.Log("Teste3");
                    // Escolhe um predador aleatório dentre os vistos.
                    int selected = Random.Range(0, agent.Vision.AllPredators.Count);

                        // Procura pelo índice global do predador escolhido no Simulador
                        PredatorSeen = spawner.AllPredators.IndexOf(agent.Vision.AllPredators[selected]);

                        // Atualiza tabela de aprendizado.
                        UpdateLearning(agent.Hearing.Signal.Symbol, PredatorSeen);
                        PrintAssociationValues();

                        Receptions++;

                        //Learned = true;
                    }

                    lastHeardSignal = agent.Hearing.Signal;
                //}
            }
        }

        public void InitLearning()
        {
            for (int line = 0; line < AssociationValue.GetLength(0); ++line)
            {
                for (int column = 0; column < AssociationValue.GetLength(1); ++column)
                {
                    AssociationValue[line, column] = 0.0f;
                }
            }
        }

        private void InitKnowledgement()
        {
            for (int column = 0; column < AssociationValue.GetLength(1); column++)
            {
                Knowledgement[column] = new List<int>();
            }
        }

        public void UpdateLearning(int symbol, int predator, bool punishment = false)
        {
            if (!punishment)
            {
                NumberOfAssociations[symbol, predator] += 1;
            }
            else
            {
                NumberOfAssociations[symbol, predator] -= 1;
            }

            if (NumberOfAssociations[symbol, predator] < 0)
            {
                NumberOfAssociations[symbol, predator] = 0;
            }

            AssociationValue[symbol, predator] = Sigmoide(NumberOfAssociations[symbol, predator]);
            UpdateKnowledgement(predator);
        }

        private void UpdateKnowledgement(int predator)
        {
            // Limpa todo o conhecimento relacionado ao predador.
            Knowledgement[predator].Clear();

            // Armazena o valor da primeira associação à variável "bigger".
            float bigger = AssociationValue[0, predator];

            // Adiciona o primeiro símbolo na base de conhecimento.
            Knowledgement[predator].Add(0);

            // Percorre todos os outros símbolos para descobrir o maior valor de associação.
            for (int symbol = 1; symbol < AssociationValue.GetLength(0); symbol++)
            {
                // Se encontrou valor maior...
                if (AssociationValue[symbol, predator] > bigger)
                {
                    // Armazena o valor encontrado à variável "bigger".
                    bigger = AssociationValue[symbol, predator];

                    // Limpa a base de conhecimento.
                    Knowledgement[predator].Clear();

                    // Adiciona o símbolo encontrado à base de conhecimento.
                    Knowledgement[predator].Add(symbol);
                }
                // Se encontrou valor igual...
                else if (AssociationValue[symbol, predator] == bigger)
                {
                    // Adiciona o símbolo encontrado à base de conhecimento.
                    Knowledgement[predator].Add(symbol);
                }
            }
        }

        public void Restart()
        {
            InitLearning();
            Receptions = 0;
        }

        private float Sigmoide(int x)
        {
            return 1 / (1 + (float)System.Math.Exp(-(0.1 * x - 5)));
        }

        private void PrintAssociationValues()
        {
            string text = "";

            for (int predator = 0; predator < AssociationValue.GetLength(1); ++predator)
            {
                for (int symbol = 0; symbol < AssociationValue.GetLength(0); ++symbol)
                {
                    text += AssociationValue[symbol, predator] + " ";
                }

                text += "\n";
            }

            Debug.Log(text);
        }
    }
}