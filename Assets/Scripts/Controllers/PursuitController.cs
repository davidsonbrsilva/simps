using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class PursuitController : MonoBehaviour
    {
        private AgentController agent;
        
        public AgentController Prey { get; private set; }
        public bool PreyIsProtected { get; private set; }

        private void Awake()
        {
            agent = GetComponent<AgentController>();
            PreyIsProtected = false;
        }

        private void Update()
        {
            if (agent.Vision.IsSeeingPrey)
            {
                if (Prey == null)
                {
                    // Pega os componentes da presa selecionada aleatoriamente entre as avistadas.
                    int selected = Random.Range(0, agent.Vision.Preys.Count);
                    Prey = agent.Vision.Preys[selected].GetComponent<AgentController>();
                    //var protection = agentController.Vision.Preys[selected].Find("Rotatable").Find("Marker").GetComponent<ProtectionController>();
                }

                // Se a presa está morta ou protegida contra este predador...
                if (Prey.IsDead || ProtectedAgainstMe(Prey))
                {
                    PreyIsProtected = true;
                }
                else
                {
                    PreyIsProtected = false;
                }
            }
            else
            {
                if (Prey != null)
                {
                    Prey = null;
                }
            }
        }

        private bool ProtectedAgainstMe(AgentController otherAgent)
        {
            if (agent.IsAerialPredator)
            {
                if (otherAgent.Protection.ProtectedAgainstAerialPredator)
                {
                    return true;
                }
            }
            else if (agent.IsLandPredator)
            {
                if (otherAgent.Protection.ProtectedAgainstLandPredator)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
