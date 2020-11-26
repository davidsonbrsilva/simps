using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class LonelinessDriver : SimpsDriver
    {
        [SerializeField] private float loneliness = 0.5f;
        [SerializeField] private float decSmooth = 1f;
        [SerializeField] private float incSmooth = 1f;

        private AgentController agent;

        private void Awake()
        {
            agent = GetComponent<AgentController>();
        }

        private void Update()
        {
            if (!agent.Vision.IsSeeingPrey)
            {
                loneliness += 0.1f * loneliness * Time.deltaTime * incSmooth;
                loneliness = Mathf.Clamp(loneliness, 0.01f, 0.99f);
                motivation = 0f;
            }
            else
            {
                loneliness -= 0.1f * (1f - loneliness) * Time.deltaTime * decSmooth;
                loneliness = Mathf.Clamp(loneliness, 0.01f, 0.99f);
                motivation = loneliness;
            }
        }

        private void OnEnable()
        {
            Restart();
        }

        public override void Restart()
        {
            loneliness = 0.5f;
        }
    }
}