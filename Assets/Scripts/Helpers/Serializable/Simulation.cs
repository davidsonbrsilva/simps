using System;
using System.Collections.Generic;

namespace SIMPS
{
    public struct Simulation
    {
        public ulong id;
        public string path;
        public string start;
        public string end;
        public bool hasLearning;
        public string convergedAt;

        public Simulation(ulong id, string path, bool hasLearning, string start, string end = null, string convergedAt = null)
        {
            this.id = id;
            this.path = path;
            this.hasLearning = hasLearning;
            this.start = start;
            this.end = end;
            this.convergedAt = convergedAt;
        }
    }
}
