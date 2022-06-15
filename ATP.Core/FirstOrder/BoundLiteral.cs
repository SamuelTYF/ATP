namespace ATP.Core.FirstOrder
{
    public class BoundLiteral : Term
    {
        public string Name;
        public BoundLiteral(string name, bool @true) : base(@true)
        {
            Name = name;
            BoundLiterals.Add(this);
        }
        public override string ToString()
            => True ? Name : $"\\lnot {Name}";
    }
}