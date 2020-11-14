using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS {
    public class ExplorerDriver : SimpsDriver
    {
        #region Inspector
        [SerializeField] private float smoothTime = 1f;
        [SerializeField] private float boredom = 0.01f;
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
            if (Agent.currentSpeed == 0)
            {
                boredom += 0.1f * boredom * Time.deltaTime * smoothTime;
                boredom = Mathf.Clamp(boredom, 0.01f, 0.99f);
            }
            else
            {
                boredom -= 0.1f;
                boredom = Mathf.Clamp(boredom, 0.01f, 0.99f);
            }

            if (Agent.currentSpeed == 0f && boredom > 0.2f)
            {
                motivation = boredom;
            }
            else
            {
                motivation = 0f;
            }
        }

        private void OnEnable()
        {
            boredom = 0.01f;
        }
        #endregion
    }
}