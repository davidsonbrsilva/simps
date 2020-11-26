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
            LearningHeader = "time;prey;symbol;predator;value\n";
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
                string predatorName = predatorController.ShortName;
                string value = string.Format("{0:0.0000}", agent.Learner.AssociationMemory[symbol, predator]);

                string record = string.Format("{0};{1};{2};{3};{4}\n", Runtime, agent.ShortName, symbol, predatorName, value);

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