namespace ATP.Core.PL
{
    public abstract class ITerm
    {
        public int Index { get; }
        public int Deep { get; }
        public OperatorBack[] OperatorBacks { get; }
        public HashSet<Variable> Variables;
        public ITerm(int deep, int index, IOperator[] operators)
        {
            Deep = deep;
            Index = index;
            OperatorBacks = new OperatorBack[operators.Length];
            for (int i = 0; i < operators.Length; i++)
                OperatorBacks[i] = new(operators[i].Count);
        }
    }
}
