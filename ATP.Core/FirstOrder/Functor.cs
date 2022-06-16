namespace ATP.Core.FirstOrder
{
    public class Functor : Term
    {
        public Operator Operator;
        public Term[] Terms;
        public Functor(int index,Operator @operator, Term[] terms, bool @true) : base(index, @true)
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
