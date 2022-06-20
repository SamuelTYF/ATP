namespace ATP.Core.MO
{
    public class Variable:ITerm
    {
        public string Name;
        public Variable(int index, Sort sort, string name) : base(index, sort, Array.Empty<ITerm>())
        {
            Name = name;
        }
    }
}
