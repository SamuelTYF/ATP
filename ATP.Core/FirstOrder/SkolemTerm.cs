using System.Xml.Linq;

namespace ATP.Core.FirstOrder
{
    public class SkolemTerm
    {
        public bool True;
        public SkolemTerm Mirror;
        public HashSet<int> Back;
        public int Index;
        public Dictionary<int, HashSet<int>[]> Backs;
        public SkolemTerm(bool @true, int index)
        {
            True = @true;
            Back = new();
            Index = index;
            Backs = new();
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
        }
        public override string ToString() => True ? $"{Operator.Name}({string.Join<SkolemTerm>(",",Terms)})" : $"\\lnot {Operator.Name}({string.Join<SkolemTerm>(",", Terms)})";
    }
}