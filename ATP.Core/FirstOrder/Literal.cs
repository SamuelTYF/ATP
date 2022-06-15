namespace ATP.Core.FirstOrder
{
    public class Literal : Term
    {
        public string Name;
        public Literal(string name, bool @true) : base(@true)
        {
            Name = name;
            Literals.Add(this);
        }
        public override string ToString()
            => True ? Name : $"\\lnot {Name}";
    }
}