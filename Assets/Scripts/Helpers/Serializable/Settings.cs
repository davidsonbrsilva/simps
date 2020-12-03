using System;

namespace SIMPS
{
    [Serializable]
    public struct Settings
    {
        public int batch;
        public int symbols;
        public int preys;
        public int landPredators;
        public int aerialPredators;
        public int crowlingPredators;
        public float wallRatioSize;
        public Timer time;
        public SimulationMode mode;

        public Settings(int batch, int symbols, int preys, int landPredators, int aerialPredators, int crowlingPredators, float wallRatioSize, Timer time, SimulationMode mode)
        {
            this.batch = batch;
            this.symbols = symbols;
            this.preys = preys;
            this.landPredators = landPredators;
            this.aerialPredators = aerialPredators;
            this.crowlingPredators = crowlingPredators;
            this.wallRatioSize = wallRatioSize;
            this.time = time;
            this.mode = mode;
        }
    }
}
