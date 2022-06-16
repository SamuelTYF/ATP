namespace ATP.Core.FirstOrder
{
    public class Literal : Term
    {
        public string Name;
        public Literal(int index, string name, bool @true) : base(index, @true)
        {
            Name = name;
            Literals.Add(this);
        }
        public override string ToString()
            => True ? Name : $"\\lnot {Name}";
    }
}