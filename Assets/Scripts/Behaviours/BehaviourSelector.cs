using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIMPS
{
    public class BehaviourSelector : SimpsBehaviour
    {
        private ExplorerBehaviour explorerBehaviour;
        private SleeperBehaviour sleeperBehaviour;
        private HunterBehaviour hunterBehaviour;
        private FearfulBehaviour fearfulBehaviour;
        private LonelinessBehaviour lonelinessBehaviour;

        private SimpsDriver bestDriver;

        private List<SimpsBehaviour> behaviours;
        private List<SimpsDriver> drivers;

        private void Awake() {
            // Inicializa listas de comportamentos e drivers.
            behaviours = new List<SimpsBehaviour>();
            drivers = new List<SimpsDriver>();

            // Procura por cada um dos tipos de comportamento do SIMPS.
            explorerBehaviour = GetComponent<ExplorerBehaviour>();
            sleeperBehaviour = GetComponent<SleeperBehaviour>();
            hunterBehaviour = GetComponent<HunterBehaviour>();
            fearfulBehaviour = GetComponent<FearfulBehaviour>();
            lonelinessBehaviour = GetComponent<LonelinessBehaviour>();

            // Procura por cada um dos tipos de drivers do SIMPS.
            var explorerDriver = GetComponent<ExplorerDriver>();
            var sleeperDriver = GetComponent<SleeperDriver>();
            var hunterDriver = GetComponent<HunterDriver>();
            var fearfulDriver = GetComponent<FearfulDriver>();
            var lonelinessDriver = GetComponent<LonelinessDriver>();

            // Adiciona os comportamentos encontrados na lista designada.
            if (explorerBehaviour != null)
            {
                behaviours.Add(explorerBehaviour);
            }
            if (sleeperBehaviour != null)
            {
                behaviours.Add(sleeperBehaviour);
            }
            if (hunterBehaviour != null)
            {
                behaviours.Add(hunterBehaviour);
            }
            if (fearfulBehaviour != null)
            {
                behaviours.Add(fearfulBehaviour);
            }
            if (lonelinessBehaviour != null)
            {
                behaviours.Add(lonelinessBehaviour);
            }

            // Adiciona os drivers encontrados na lista designada.
            if (explorerDriver != null)
            {
                drivers.Add(explorerDriver);
            }
            if (sleeperDriver != null)
            {
                drivers.Add(sleeperDriver);
            }
            if (hunterDriver != null)
            {
                drivers.Add(hunterDriver);
            }
            if (fearfulDriver != null)
            {
                drivers.Add(fearfulDriver);
            }
            if (lonelinessDriver != null)
            {
                drivers.Add(lonelinessDriver);
            }

            if (drivers.Count > 0)
            {
                bestDriver = drivers[0];
            }
        }
        private void LateUpdate()
        {
            // Desabilita todos os comportamentos.
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = false;
            }

            // Escolhe o driver com maior motivação.
            foreach (var driver in drivers)
            {
                if (driver.motivation > bestDriver.motivation)
                {
                    bestDriver = driver;
                }
            }

            if (bestDriver is SleeperDriver && bestDriver.Motivation > 0f)
            {
                sleeperBehaviour.enabled = true;
            }
            else if (bestDriver is HunterDriver && bestDriver.Motivation > 0f)
            {
                hunterBehaviour.enabled = true;
            }
            else if (bestDriver is FearfulDriver && bestDriver.Motivation > 0.01f)
            {
                fearfulBehaviour.enabled = true;
            }
            else if (bestDriver is LonelinessDriver && bestDriver.Motivation > 0.01f)
            {
                lonelinessBehaviour.enabled = true;
            }
            else
            {
                explorerBehaviour.enabled = true;
            }
        }
    }
}