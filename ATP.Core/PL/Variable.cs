namespace ATP.Core.PL
{
    public class Variable : ITerm
    {
        public string Name;
        public Variable(string name, int index, IOperator[] operators) : base(0, index, operators)
        {
            Name = name;
        }
        public override string ToString() => Name;
    }
}
