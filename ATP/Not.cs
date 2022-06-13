namespace ATP
{
    public class Not : ITerm
    {
        public ITerm Term;
        public int Deep { get; private set; }
        public Not(ITerm term)
        {
            Term = term;
            Deep = term.Deep + 1;
        }
        public int CompareTo(ITerm other)
        {
            if (other is Not not)
                return Term.CompareTo(not.Term);
            else return -1;
        }
        public override string ToString() => $"\\lnot {Term}";
    }
}
