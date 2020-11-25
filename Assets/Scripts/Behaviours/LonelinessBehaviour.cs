using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class LonelinessBehaviour : SimpsBehaviour
    {
        [SerializeField] private float turnSpeed = 10f;

        private AgentController agent;
        private PolyNavAgent polyNavAgent;
        private Transform min;
        private Transform max;
        private Transform rotatable;

        public bool IsFollowing { get; set; }
        public Transform Prey { get; private set; }
        public AgentController PreyController { get; private set; }

        private void Awake()
        {
            agent = GetComponent<AgentController>();
            polyNavAgent = GetComponent<PolyNavAgent>();
            rotatable = transform.Find("Rotatable");
        }

        private void Update()
        {
            if (agent.Vision.IsSeeingPrey)
            {
                if (Prey == null)
                {
                    Prey = Closest(agent.Vision.Preys);
                    PreyController = Prey.GetComponent<AgentController>();

                    var followPoints = Prey.Find("Follow Points");
                    min = followPoints.Find("Min");
                    max = followPoints.Find("Max");
                }

                if (!PreyController.IsDead && PreyController.Loneliness.Prey != transform)
                {
                    if ((transform.position - Prey.position).magnitude < (min.position - Prey.position).magnitude)
                    {
                        polyNavAgent.SetDestination(min.position);

                        Vector2 movementDirection = (Vector2)Prey.position - polyNavAgent.position;
                        float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                        rotatable.rotation = Quaternion.Slerp(rotatable.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * turnSpeed);
                    }
                    else if ((transform.position - Prey.position).magnitude > (max.position - Prey.position).magnitude)
                    {
                        polyNavAgent.SetDestination(max.position);

                        Vector2 movementDirection = (Vector2)Prey.position - polyNavAgent.position;
                        float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                        rotatable.rotation = Quaternion.Slerp(rotatable.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * turnSpeed);
                    }
                }
            }
            else
            {
                if (Prey != null)
                {
                    Prey = null;
                    max = null;
                    min = null;
                }
            }
        }

        private Transform Closest(List<Transform> preys)
        {
            if (preys.Count <= 0)
            {
                return null;
            }

            var closest = preys[0];

            for (int prey = 1; prey < preys.Count; ++prey)
            {
                if (Vector2.Distance(transform.position, closest.position) > Vector2.Distance(transform.position, preys[prey].position))
                {
                    closest = preys[prey];
                }
            }

            return closest;
        }
    }
}