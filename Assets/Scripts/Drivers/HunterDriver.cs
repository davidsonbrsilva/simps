using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS {
    public class HunterDriver : SimpsDriver
    {
        #region Inspector
        [SerializeField] private float smoothTime = 1f;
        [SerializeField] private float hunger = 1f;
        #endregion

        #region Private Attributes
        private PolyNavAgent polyNavAgent;
        private AgentController agentController;
        private PursuitController pursuitController;
        #endregion

        #region Private Properties
        private PolyNavAgent Agent
        {
            get { return polyNavAgent != null ? polyNavAgent : polyNavAgent = GetComponent<PolyNavAgent>(); }
        }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            agentController = GetComponent<AgentController>();
            pursuitController = GetComponent<PursuitController>();
        }

        private void Update()
        {
            if (agentController.Capture.HasCaptured)
            {
                hunger = 0.01f;
            }
            else
            {
                hunger += 0.01f * hunger * Time.deltaTime * smoothTime;
                hunger = Mathf.Clamp(hunger, 0f, 1f);
            }

            activated = true;

            if (hunger > 0.5f && agentController.Vision.IsSeeingPrey && !pursuitController.PreyIsProtected)
            {
                motivation = hunger;
            }
            else
            {
                motivation = 0f;
                activated = false;
            }
        }

        private void OnEnable()
        {
            hunger = 1f;
        }
        #endregion
    }
}