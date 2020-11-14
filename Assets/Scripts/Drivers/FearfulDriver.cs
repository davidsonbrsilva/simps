using SIMPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class FearfulDriver : SimpsDriver
    {
        [SerializeField] private float smoothTime = 1f;
        [SerializeField] private float fear = 0.01f;

        private AgentController agentController;

        private void Awake()
        {
            agentController = GetComponent<AgentController>();
        }

        private void Update()
        {
            if (agentController.Vision.IsSeeingPredator)
            {
                fear = 1.0f;
            }
            else
            {
                fear -= 0.05f * (1f - fear) * Time.deltaTime * smoothTime;
            }

            fear = Mathf.Clamp(fear, 0.01f, 0.99f);

            motivation = fear;

            activated = true;

            if (motivation == 0.01f)
            {
                activated = false;
            }
        }

        private void OnEnable()
        {
            fear = 0.01f;
        }
    }
}