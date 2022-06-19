namespace ATP.Core.FirstOrder
{
    public class SkolemTerm
    {
        public bool True;
        public SkolemTerm Mirror;
        public HashSet<int> Back;
        public int Index;
        public Dictionary<int, HashSet<int>[]> Backs;
        public HashSet<int> Constants;
        public HashSet<int> Variables;
        public SkolemTerm(bool @true, int index)
        {
            True = @true;
            Back = new();
            Index = index;
            Backs = new();
            Constants = new();
            Variables = new();
        }
        public HashSet<int> GetBack(int key, int index)
            => Backs.ContainsKey(key) ? new(Backs[key][index]) : new();
    }
    public class SkolemConstant : SkolemTerm
    {
        public string Name;
        public SkolemConstant(int index, string name, bool @true) : base(@true, index)
        {
            Name = name;
        }
        public override string ToString() => True ? Name : $"\\lnot {Name}";
    }
    public class SkolemVariable : SkolemTerm
    {
        public string Name;
        public SkolemVariable(int index, string name, bool @true) : base(@true, index)
        {
            Name = name;
        }
        public override string ToString() => True ? Name : $"\\lnot {Name}";
    }
    public class SkolemFunctor : SkolemTerm
    {
        public Operator Operator;
        public SkolemTerm[] Terms;
        public SkolemFunctor(int index, Operator @operator, SkolemTerm[] terms, bool @true) : base(@true, index)
        {
            Operator = @operator;
            Terms = terms;
            for(int i=0;i<terms.Length;i++)
            {
                SkolemTerm term = terms[i];
                Variables.UnionWith(term.Variables);
                Constants.UnionWith(term.Constants);
                if (!term.Backs.ContainsKey(@operator.Index))
                {
                    HashSet<int>[] bs = new HashSet<int>[@operator.Count];
                    for (int j = 0; j < @operator.Count; j++) bs[j] = new();
                    term.Backs[@operator.Index] = bs;
                }
                term.Backs[@operator.Index][i].Add(Index);
            }
        }
        public override string ToString() => True ? $"{Operator.Name}({string.Join<SkolemTerm>(",",Terms)})" : $"\\lnot {Operator.Name}({string.Join<SkolemTerm>(",", Terms)})";
    }
}