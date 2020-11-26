namespace SIMPS
{
    public class Association
    {
        public int Symbol { get; set; }
        public int Predator { get; set; }

        public Association(int symbol, int predator)
        {
            Symbol = symbol;
            Predator = predator;
        }
    }
}