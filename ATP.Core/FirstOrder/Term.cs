namespace ATP.Core.FirstOrder
{
    public class Term
    {
        public int Index;
        public Term Mirror;
        public bool True;
        public HashSet<Literal> Literals;
        public HashSet<BoundLiteral> BoundLiterals;
        public Dictionary<int, HashSet<int>[]> Backs;
        public Term(int index, bool @true)
        {
            Index = index;
            True = @true;
            Literals = new();
            BoundLiterals = new();
            Backs = new();
        }
        public HashSet<int> GetBack(int key, int index)
            => Backs.ContainsKey(key) ? new(Backs[key][index]) : new();
    }
}
