using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class SleeperBehaviour : SimpsBehaviour
    {
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
        private void OnEnable()
        {
            Agent.Stop();
        }
        #endregion

        #region Other Methods
        public override void Restart()
        {
            //
        }
        #endregion
    }
}
