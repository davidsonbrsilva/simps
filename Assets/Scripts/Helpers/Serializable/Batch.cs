using System;
using System.Collections.Generic;

namespace SIMPS
{
    [Serializable]
    public struct Batch
    {
        public ulong id;
        public int size;
        public string mode;
        public float wallRatio;
        public int duration;
        public string start;
        public string end;
        public List<Agent> preys;
        public List<Agent> predators;
        public List<Simulation> simulations;

        public Batch(ulong id, int size, SimulationMode mode, float wallRatio, int duration, string start, string end = null, List<Simulation> simulations = null, List<Agent> preys = null, List<Agent> predators = null)
        {
            this.id = id;
            this.size = size;
            this.mode = mode.ToString();
            this.wallRatio = wallRatio;
            this.duration = duration;
            this.start = start;
            this.end = end;
            this.preys = preys;
            this.predators = predators;

            if (simulations == null)
            {
                this.simulations = new List<Simulation>();
            }
            else
            {
                this.simulations = simulations;
            }
        }
    }
}