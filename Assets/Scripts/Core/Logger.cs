using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class Logger : MonoBehaviour
    {
        private Manager manager;
        private Spawner spawner;
        private List<AgentController> preys;

        public List<string> Deaths { get; private set; }
        public List<string> Learning { get; private set; }

        public string DeathsHeader { get; private set; }
        public string LearningHeader { get; private set; }
        public string Runtime { get { return string.Format("{0:0.0000}", manager.Runtime); } }

        private void Awake()
        {
            var core = GameObject.FindWithTag("Core");
            manager = core.GetComponent<Manager>();
            spawner = core.GetComponent<Spawner>();
            preys = new List<AgentController>();

            Deaths = new List<string>();
            Learning = new List<string>();

            DeathsHeader = "time;prey\n";
            LearningHeader = "time;prey;symbol;predator;value";

            for (int symbol = 0; symbol < spawner.Symbols; ++symbol)
            {
                for (int predator = 0; predator < spawner.AllPredators.Count; ++predator)
                {
                    LearningHeader += string.Format(";{0}{1}", symbol, spawner.PredatorControllers[predator].ShortName);
                }
            }

            LearningHeader += "\n";
        }

        private void Update()
        {
            foreach (var prey in spawner.PreyControllers)
            {
                if (prey.Learner.Learned)
                {
                    AppendLearning(prey);
                }

                if (prey.Protection.WasCaptured)
                {
                    AppendDeaths(prey);
                }
            }
        }

        private void AppendLearning(AgentController agent)
        {
            foreach (var association in agent.Learner.LastAssociations)
            {
                int symbol = association.Symbol;
                var predatorController = spawner.AllPredators[association.Predator].GetComponent<AgentController>();
                int predator = predatorController.GlobalIndex;
                string value = string.Format("{0:0.0000}", agent.Learner.AssociationMemory[symbol, predator]);
                string associations = "";

                for (int s = 0; s < spawner.Symbols; ++s)
                {
                    for (int p = 0; p < spawner.AllPredators.Count; ++p)
                    {
                        associations += string.Format("{0:0.0000};", agent.Learner.AssociationMemory[s, p]);
                    }
                }

                LearningHeader += "\n";

                string record = string.Format("{0};{1};{2};{3};{4};{5}\n", Runtime, agent.ShortName, symbol, predatorController.ShortName, value, associations);

                Learning.Add(record);
            }
        }

        private void AppendDeaths(AgentController agent)
        {
            string record = string.Format("{0};{1}\n", Runtime, agent.ShortName);
            Deaths.Add(record);
        }
    }
}