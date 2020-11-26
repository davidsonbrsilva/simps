using UnityEngine;

namespace SIMPS
{
    public class Restarter : MonoBehaviour
    {
        private Manager manager;
        private Spawner spawner;

        private void Awake()
        {
            var core = GameObject.FindWithTag("Core");
            manager = core.GetComponent<Manager>();
            spawner = core.GetComponent<Spawner>();
        }

        void Update()
        {
            if (manager.Restart)
            {
                RestartPreys();
                RestartPredators();
            }
        }

        private void RestartPreys()
        {
            foreach (var prey in spawner.PreyControllers)
            {
                if (prey.Explorer != null)
                {
                    prey.Explorer.Restart();
                }

                if (prey.Learner != null && !prey.Learner.enabled)
                {
                    prey.Learner.enabled = true;
                    prey.Learner.Restart();
                }

                if (prey.Emitter != null)
                {
                    prey.Emitter.Restart();
                }

                TryRestartDrivers(prey);

                prey.transform.position = spawner.GetRandomOriginPosition();
            }
        }

        private void RestartPredators()
        {
            foreach (var predator in spawner.PredatorControllers)
            {
                predator.transform.position = spawner.GetRandomOriginPosition();

                TryRestartDrivers(predator);
            }
        }

        private void TryRestartDrivers(AgentController agent)
        {
            TryRestartDriver(agent.GetComponent<ExplorerDriver>());
            TryRestartDriver(agent.GetComponent<FearfulDriver>());
            TryRestartDriver(agent.GetComponent<HunterDriver>());
            TryRestartDriver(agent.GetComponent<LonelinessDriver>());
            TryRestartDriver(agent.GetComponent<SleeperDriver>());
        }

        private void TryRestartDriver(SimpsDriver driver)
        {
            if (driver)
            {
                driver.Restart();
            }
        }
    }
}