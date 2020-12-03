using System;

namespace SIMPS
{
    [Serializable]
    public struct Simulation
    {
        public ulong id;
        public string path;
        public string start;
        public string end;
        public bool hasLearning;
        public string convergedAt;
        public float convergedAtRuntime;
        public bool isTotalConvergence;

        public Simulation(ulong id, string path, bool hasLearning, string start, string end = null, string convergedAt = null, float convergedAtRuntime = -1, bool isTotalConvergence = false)
        {
            this.id = id;
            this.path = path;
            this.hasLearning = hasLearning;
            this.start = start;
            this.end = end;
            this.convergedAt = convergedAt;
            this.convergedAtRuntime = convergedAtRuntime;
            this.isTotalConvergence = isTotalConvergence;
        }
    }
}
