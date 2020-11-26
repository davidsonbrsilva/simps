namespace SIMPS
{
    public struct Settings
    {
        public int batch;
        public int preys;
        public int landPredators;
        public int aerialPredators;
        public int crowlingPredators;
        public Timer time;
        public SimulationMode mode;

        public Settings(int batch, int preys, int landPredators, int aerialPredators, int crowlingPredators, Timer time, SimulationMode mode)
        {
            this.batch = batch;
            this.preys = preys;
            this.landPredators = landPredators;
            this.aerialPredators = aerialPredators;
            this.crowlingPredators = crowlingPredators;
            this.time = time;
            this.mode = mode;
        }
    }
}
