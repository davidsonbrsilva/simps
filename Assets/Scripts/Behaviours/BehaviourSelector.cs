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

            // Procura por cada um dos tipos de drivers do SIMPS.
            var explorerDriver = GetComponent<ExplorerDriver>();
            var sleeperDriver = GetComponent<SleeperDriver>();
            var hunterDriver = GetComponent<HunterDriver>();
            var fearfulDriver = GetComponent<FearfulDriver>();

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

            if (bestDriver is SleeperDriver && bestDriver.activated)
            {
                sleeperBehaviour.enabled = true;
            }
            else if (bestDriver is HunterDriver && bestDriver.activated)
            {
                hunterBehaviour.enabled = true;
            }
            else if (bestDriver is FearfulDriver && bestDriver.activated)
            {
                fearfulBehaviour.enabled = true;
            }
            else
            {
                explorerBehaviour.enabled = true;
            }
        }
    }
}