namespace ATP.Core.FirstOrder
{
    public class Functor : Term
    {
        public Operator Operator;
        public Term[] Terms;
        public int Index;
        public Functor(Operator @operator, Term[] terms, bool @true, int index) : base(@true)
        {
            Operator = @operator;
            Terms = terms;
            Index = index;
            for (int i = 0; i < terms.Length; i++)
            {
                Term term = terms[i];
                BoundLiterals.UnionWith(term.BoundLiterals);
                Literals.UnionWith(term.Literals);
                if (!term.Backs.ContainsKey(@operator.Index))
                {
                    HashSet<int>[] bs = new HashSet<int>[@operator.Count];
                    for (int j = 0; j < @operator.Count; j++) bs[j] = new();
                    term.Backs[@operator.Index] = bs;
                }
                term.Backs[@operator.Index][i].Add(Index);
            }
        }
    }
}
