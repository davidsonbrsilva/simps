using System;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    /// <summary>
    /// Controlador de visão dos agentes.
    /// </summary>
    public class VisionController : MonoBehaviour
    {
        #region Inspector
        [SerializeField] private List<Transform> preys;
        [SerializeField] private List<Transform> aerialPredator;
        [SerializeField] private List<Transform> landPredator;
        [SerializeField] private List<Transform> crowlingPredator;
        [SerializeField] private float timeOfDangerMemory = 2f;
        #endregion

        #region Properties
        public List<Transform> Preys { get { return preys; } private set { preys = value; } }
        public List<Transform> AerialPredators { get { return aerialPredator; } private set { aerialPredator = value; } }
        public List<Transform> LandPredators { get { return landPredator; } private set { landPredator = value; } }
        public List<Transform> CrowlingPredators { get { return crowlingPredator; } private set { crowlingPredator = value; } }
        public List<Transform> AllPredators { get; private set; }
        public bool IsSeeingPrey { get { return Preys.Count > 0; } }
        public bool IsSeeingPredator { get { return AllPredators.Count > 0; } }
        public bool IsSeeingAerialPredator { get { return AerialPredators.Count > 0; } }
        public bool IsSeeingLandPredator { get { return LandPredators.Count > 0; } }
        public bool IsSeeingCrowlingPredator { get { return CrowlingPredators.Count > 0; } }
        public float SawAerialPredatorAt { get; private set; }
        public float SawLandPredatorAt { get; private set; }
        public float SawCrowlingPredatorAt { get; private set; }
        public float SawPreyAt { get; private set; }
        public bool SawAerialPredator { get; private set; }
        public bool SawLandPredator { get; private set; }
        public bool SawCrowlingPredator { get; private set; }
        public bool SawPredator { get; private set; }
        public bool SawPrey { get; private set; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            Preys = new List<Transform>();
            AerialPredators = new List<Transform>();
            LandPredators = new List<Transform>();
            CrowlingPredators = new List<Transform>();
            AllPredators = new List<Transform>();

            SawAerialPredator = false;
            SawLandPredator = false;
            SawCrowlingPredator = false;
            SawPredator = false;
            SawPrey = false;
        }

        private void Update()
        {
            SawAerialPredatorAt += Time.deltaTime;
            SawLandPredatorAt += Time.deltaTime;
            SawCrowlingPredatorAt += Time.deltaTime;
            SawPreyAt += Time.deltaTime;

            if (!IsSeeingAerialPredator)
            {
                if (SawAerialPredatorAt > timeOfDangerMemory)
                {
                    SawAerialPredator = false;
                }
            }
            else
            {
                SawAerialPredatorAt = 0f;
                SawAerialPredator = true;
            }

            if (!IsSeeingLandPredator)
            {
                if (SawLandPredatorAt > timeOfDangerMemory)
                {
                    SawLandPredator = false;
                }
            }
            else
            {
                SawLandPredatorAt = 0f;
                SawLandPredator = true;
            }

            if (!IsSeeingCrowlingPredator)
            {
                if (SawCrowlingPredatorAt > timeOfDangerMemory)
                {
                    SawCrowlingPredator = false;
                }
            }
            else
            {
                SawCrowlingPredatorAt = 0f;
                SawCrowlingPredator = true;
            }

            if (!IsSeeingPrey)
            {
                if (SawPreyAt > timeOfDangerMemory)
                {
                    SawPrey = false;
                }
            }
            else
            {
                SawPreyAt = 0f;
                SawPrey = true;
            }

            SawPredator = SawAerialPredator || SawLandPredator || SawCrowlingPredator;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Prey"))
            {
                See(collision.transform.parent.parent, Preys);
            }
            else if (collision.CompareTag("Aerial Predator"))
            {
                See(collision.transform.parent.parent, AerialPredators, true);
            }
            else if (collision.CompareTag("Land Predator"))
            {
                See(collision.transform.parent.parent, LandPredators, true);
            }
            else if (collision.CompareTag("Crowling Predator"))
            {
                See(collision.transform.parent.parent, CrowlingPredators, true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Prey"))
            {
                Unsee(collision.transform.parent.parent, Preys);
            }
            else if (collision.CompareTag("Aerial Predator"))
            {
                Unsee(collision.transform.parent.parent, AerialPredators, true);
            }
            else if (collision.CompareTag("Land Predator"))
            {
                Unsee(collision.transform.parent.parent, LandPredators, true);
            }
            else if (collision.CompareTag("Crowling Predator"))
            {
                Unsee(collision.transform.parent.parent, CrowlingPredators, true);
            }
        }
        #endregion

        #region Other Methods
        private void See(Transform element, List<Transform> list, bool addToAllPredators = false)
        {
            if (element != transform.parent.parent)
            {
                if (!list.Contains(element))
                {
                    list.Add(element);

                    if (addToAllPredators)
                    {
                        AllPredators.Add(element);
                    }
                }
            }
        }

        private void Unsee(Transform element, List<Transform> list, bool addToAllPredators = false)
        {
            if (element != transform.parent.parent)
            {
                if (list.Contains(element))
                {
                    list.Remove(element);

                    if (addToAllPredators)
                    {
                        AllPredators.Remove(element);
                    }
                }
            }
        }
        #endregion
    }
}
