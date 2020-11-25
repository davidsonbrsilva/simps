using SIMPS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SIMPS
{
    public class FearfulBehaviour : SimpsBehaviour
    {
        [SerializeField] private float turnSpeed = 10f;
        [SerializeField] private float treesWeight = 1f;
        [SerializeField] private float bushesWeight = 1f;
        [SerializeField] private float predatorsWeight = 1f;

        private Spawner spawner;
        private ActionRadiusController actionRadiusController;
        private PolyNavAgent polyNavAgent;
        private AgentController agent;
        private Transform rotatable;
        private GameObject safest;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            actionRadiusController = transform.Find("Action Radius").GetComponent<ActionRadiusController>();
            polyNavAgent = GetComponent<PolyNavAgent>();
            agent = GetComponent<AgentController>();
            rotatable = transform.Find("Rotatable");
        }

        private void Update()
        {
            var alertAgaintsAerialPredator = agent.Vision.SawAerialPredator || agent.Hearing.HeardAerialPredator;
            var alertAgainstLandPredator = agent.Vision.SawLandPredator || agent.Hearing.HeardLandPredator;

            foreach (var escapeCoordinate in actionRadiusController.EscapeCoordinates)
            {
                CalculateGrossValue(escapeCoordinate, alertAgaintsAerialPredator, alertAgainstLandPredator);
            }

            foreach (var escapeCoordinate in actionRadiusController.EscapeCoordinates)
            {
                CalculateNormalizedValue(escapeCoordinate, actionRadiusController.EscapeCoordinates);
            }

            safest = actionRadiusController.EscapeCoordinates.Aggregate(
                    (i1, i2) => i1.GetComponent<EscapeCoordinateController>().CoordinateValue > i2.GetComponent<EscapeCoordinateController>().CoordinateValue ? i1 : i2);

            polyNavAgent.SetDestination(safest.transform.position);

            Vector2 movementDirection = polyNavAgent.nextPoint - polyNavAgent.position;
            float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            rotatable.rotation = Quaternion.Slerp(rotatable.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * turnSpeed);
        }

        private void OnEnable()
        {
            animator.SetBool("IsAlert", true);
        }

        private void OnDisable()
        {
            animator.SetBool("IsAlert", false);
        }

        private void CalculateGrossValue(GameObject escapeCoordinate, bool alertAgainstAerialPredator, bool alertAgainstLandPredator)
        {
            float sumPredatorsDistance = 0f;
            float sumBushesDistance = 0f;
            float sumTreesDistance = 0f;

            foreach (var predator in spawner.AllPredators)
            {
                sumPredatorsDistance += Vector2.Distance(escapeCoordinate.transform.position, predator.transform.position);
            }

            if (alertAgainstAerialPredator)
            {
                foreach (var bush in spawner.Bushes)
                {
                    sumBushesDistance += Vector2.Distance(escapeCoordinate.transform.position, bush.transform.position);
                }
            }

            if (alertAgainstLandPredator)
            {
                foreach (var tree in spawner.Trees)
                {
                    sumTreesDistance += Vector2.Distance(escapeCoordinate.transform.position, tree.transform.position);
                }
            }

            var escapeCoordinateController = escapeCoordinate.GetComponent<EscapeCoordinateController>();

            escapeCoordinateController.MeanPredatorsDistance = sumPredatorsDistance / spawner.AllPredators.Count;
            escapeCoordinateController.MeanBushesDistance = sumBushesDistance / spawner.Bushes.Count;
            escapeCoordinateController.MeanTreesDistance = sumTreesDistance / spawner.Trees.Count;
        }
        private void CalculateNormalizedValue(GameObject escapeCoordinate, List<GameObject> escapeCoordinates)
        {
            var escapeCoordinateController = escapeCoordinate.GetComponent<EscapeCoordinateController>();

            float min = escapeCoordinates.Min(ec => ec.GetComponent<EscapeCoordinateController>().MeanPredatorsDistance);
            float max = escapeCoordinates.Max(ec => ec.GetComponent<EscapeCoordinateController>().MeanPredatorsDistance);
            float normPredatorsDistance = (escapeCoordinateController.MeanPredatorsDistance - min) / (max - min);

            min = escapeCoordinates.Min(ec => ec.GetComponent<EscapeCoordinateController>().MeanTreesDistance);
            max = escapeCoordinates.Max(ec => ec.GetComponent<EscapeCoordinateController>().MeanTreesDistance);
            float normTreesDistance = 1 - ((escapeCoordinateController.MeanTreesDistance - min) / (max - min));

            min = escapeCoordinates.Min(ec => ec.GetComponent<EscapeCoordinateController>().MeanBushesDistance);
            max = escapeCoordinates.Max(ec => ec.GetComponent<EscapeCoordinateController>().MeanBushesDistance);
            float normBushesDistance = 1 - ((escapeCoordinateController.MeanBushesDistance - min) / (max - min));

            if (float.IsNaN(normTreesDistance))
            {
                normTreesDistance = 0f;
            }

            if (float.IsNaN(normPredatorsDistance))
            {
                normPredatorsDistance = 0f;
            }

            if (float.IsNaN(normBushesDistance))
            {
                normBushesDistance = 0f;
            }

            escapeCoordinateController.CoordinateValue = (predatorsWeight * normPredatorsDistance + treesWeight * normTreesDistance + bushesWeight * normBushesDistance) / (predatorsWeight + treesWeight + bushesWeight);

            foreach (var bush in spawner.Bushes)
            {
                if ((escapeCoordinate.transform.position == bush.transform.position) && agent.Vision.IsSeeingAerialPredator)
                {
                    escapeCoordinateController.CoordinateValue = 1f;
                }
            }

            foreach (var tree in spawner.Trees)
            {
                if ((escapeCoordinate.transform.position == tree.transform.position) && agent.Vision.IsSeeingLandPredator)
                {
                    escapeCoordinateController.CoordinateValue = 1f;
                }
            }
        }
    }
}