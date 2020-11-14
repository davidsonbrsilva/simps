using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class PolyNavSettings : MonoBehaviour
    {
        private void Awake()
        {
            var defaultArea = GameObject.FindWithTag("Default Area").GetComponent<PolyNav2D>();
            var aerialArea = GameObject.FindWithTag("Aerial Area").GetComponent<PolyNav2D>();
            var agentController = GetComponent<AgentController>();
            var polyNavAgent = GetComponent<PolyNavAgent>();

            if (agentController.IsAerialPredator)
            {
                polyNavAgent.map = aerialArea;
            }
            else
            {
                polyNavAgent.map = defaultArea;
            }
        }
    }
}