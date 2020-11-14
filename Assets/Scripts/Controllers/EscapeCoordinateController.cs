using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SIMPS
{
    public class EscapeCoordinateController : MonoBehaviour
    {
        [SerializeField] private float securityWeight = 1f;
        [SerializeField] private float dangerWeight = 1f;
        [SerializeField] private float coordinateValue;

        private Spawner spawner;
        private bool collided;
        [SerializeField] public List<EscapeCoordinateController> escapeCoordinateControllers;

        public float MeanPredatorsDistance { get; set; }
        public float MeanHiddingPlacesDistance { get; set; }
        public float CoordinateValue { get { return coordinateValue; } set { coordinateValue = value; } }
        /*
        private void Awake()
        {
            spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            MeanPredatorsDistance = 0f;
            collided = false;

            escapeCoordinateControllers = new List<EscapeCoordinateController>();

            foreach (var escapeCoordinate in spawner.EscapeCoordinates)
            {
                escapeCoordinateControllers.Add(escapeCoordinate.GetComponent<EscapeCoordinateController>());
            }
        }

        private void Update()
        {
            if (collided)
            {
                CalculateGrossValue();
            }
        }

        private void LateUpdate()
        {
            if (collided)
            {
                CalculateNormalizedValue();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Action Radius"))
            {
                collided = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Action Radius"))
            {
                collided = false;
            }
        }

        private void CalculateGrossValue()
        {
            float sumPredatorsDistance = 0f;
            float sumHiddingPlacesDistance = 0f;

            foreach (var predator in spawner.AllPredators)
            {
                sumPredatorsDistance += Vector2.Distance(transform.position, predator.transform.position);
            }

            foreach (var bush in spawner.Bushes)
            {
                sumHiddingPlacesDistance += Vector2.Distance(transform.position, bush.transform.position);
            }

            foreach (var tree in spawner.Trees)
            {
                sumHiddingPlacesDistance += Vector2.Distance(transform.position, tree.transform.position);
            }

            MeanPredatorsDistance = sumPredatorsDistance / spawner.AllPredators.Count;
            MeanHiddingPlacesDistance = sumHiddingPlacesDistance / (spawner.Bushes.Count + spawner.Trees.Count);
        }
        private void CalculateNormalizedValue()
        {
            float min = escapeCoordinateControllers[0].MeanPredatorsDistance;

            for (int i = 1; i < escapeCoordinateControllers.Count; ++i)
            {
                if (min > escapeCoordinateControllers[i].MeanPredatorsDistance)
                {
                    min = escapeCoordinateControllers[i].MeanPredatorsDistance;
                }
            }


            float min = escapeCoordinateControllers
                .Min(ec => ec.MeanPredatorsDistance);

            float max = escapeCoordinateControllers
                .Max(ec => ec.MeanPredatorsDistance);

            float normalizedDangerValue = (MeanPredatorsDistance - min) / (max - min);

            min = escapeCoordinateControllers
                .Min(ec => ec.MeanHiddingPlacesDistance);

            max = escapeCoordinateControllers
                .Max(ec => ec.MeanHiddingPlacesDistance);

            float normalizedSecurityValue = 1 - ((MeanHiddingPlacesDistance - min) / (max - min));

            CoordinateValue = dangerWeight * normalizedDangerValue + securityWeight * normalizedSecurityValue;
        }*/
    }
}