namespace ATP.Core.FirstOrder
{
    public class BoundLiteral : Term
    {
        public string Name;
        public BoundLiteral(int index, string name, bool @true) : base(index, @true)
        {
            Name = name;
            BoundLiterals.Add(this);
        }
        public override string ToString()
            => True ? Name : $"\\lnot {Name}";
    }
}