using UnityEngine;

namespace SIMPS
{
    public class ConvergenceChecker : MonoBehaviour
    {
        #region Inspector
        [SerializeField]
        [Tooltip("Mostra se houve convergência léxica na simulação atual.")]
        private bool converged;
        #endregion

        #region Private Attributes
        private Manager manager;
        private Spawner spawner;
        #endregion

        #region Properties
        public bool Converged { get; private set; }
        public bool Diverged { get; private set; }
        public bool IsTotalConvergence { get; private set; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            var core = GameObject.FindWithTag("Core");
            spawner = core.GetComponent<Spawner>();
            manager = core.GetComponent<Manager>();
            converged = false;
        }

        private void Update()
        {
            if (Converged)
            {
                Converged = false;
            }

            if (Diverged)
            {
                Diverged = false;
            }

            if (manager.CanLearn)
            {
                var result = CheckIfIsTotalConvergence();

                if (result.Item1 != converged)
                {
                    if (result.Item1)
                    {
                        Converged = true;
                        IsTotalConvergence = result.Item2;
                    }
                    else
                    {
                        Diverged = true;
                        IsTotalConvergence = false;
                    }

                    converged = result.Item1;
                }
            }
        }
        #endregion

        #region Component Methods
        public bool CheckIfConverged()
        {
            // Obtém referência para a primeira presa da lista de presas aprendizes.
            var firstPrey = spawner.PreyControllers[0].Learner;

            // Compara os símbolos usados pela primeira presa com as demais.
            for (int preyIndex = 1; preyIndex < spawner.Preys.Count; ++preyIndex)
            {
                // Obtém referência para a presa com base no índice atual.
                var otherPrey = spawner.PreyControllers[preyIndex].Learner;

                // Verifica o símbolo usado para cada predador.
                for (int predatorIndex = 0; predatorIndex < spawner.AllPredators.Count; ++predatorIndex)
                {
                    // Se na base de conhecimento só há um símbolo utilizado para identificar o predador atual...
                    if (firstPrey.Knowledgement[predatorIndex].Count == 1 && otherPrey.Knowledgement[predatorIndex].Count == 1)
                    {
                        // Se o símbolo usado pela primeira presa é diferente do usado pela outra presa...
                        if (firstPrey.Knowledgement[predatorIndex][0] != otherPrey.Knowledgement[predatorIndex][0])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public (bool, bool) CheckIfIsTotalConvergence()
        {
            var converged = CheckIfConverged();
            var isTotalConvergence = true;

            if (converged)
            {
                var firstPrey = spawner.PreyControllers[0].Learner;

                for (var i = 0; i < spawner.AllPredators.Count - 1; ++i)
                {
                    for (var j = i + 1; j < spawner.AllPredators.Count; ++j)
                    {
                        if (firstPrey.Knowledgement[i][0] == firstPrey.Knowledgement[j][0])
                        {
                            isTotalConvergence = false;
                        }
                    }
                }
            }

            return (converged, isTotalConvergence);
        }
        #endregion
    }
}