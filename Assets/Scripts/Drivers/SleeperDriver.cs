using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS {
    public class SleeperDriver : SimpsDriver
    {
        #region Inspector
        [SerializeField] private float smoothTime = 1f;
        [SerializeField] private float fatigue = 0f;
        #endregion

        #region Private Attributes
        private PolyNavAgent agent;
        #endregion

        #region Private Properties
        private PolyNavAgent Agent
        {
            get { return agent != null ? agent : agent = GetComponent<PolyNavAgent>(); }
        }
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (Agent.currentSpeed == 0f)
            {
                fatigue -= 0.1f * Time.deltaTime * smoothTime;
                fatigue = Mathf.Clamp(fatigue, 0f, 1f);
            }
            else if (Agent.currentSpeed > Agent.maxSpeed / 2)
            {
                fatigue += (0.05f * Agent.currentSpeed / Agent.maxSpeed) * Time.deltaTime * smoothTime;
                fatigue = Mathf.Clamp(fatigue, 0f, 1f);
            }

            if (fatigue > 0.5f)
            {
                motivation = fatigue;
            }
            else if (Agent.currentSpeed == 0 && fatigue > 0f)
            {
                motivation = 0.5f;
            }
            else
            {
                motivation = 0f;
            }
        }

        private void OnEnable()
        {
            fatigue = 0f;
        }
        #endregion
    }
}