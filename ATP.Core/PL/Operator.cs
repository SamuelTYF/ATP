namespace ATP.Core.PL
{
    public class Operator : IOperator
    {
        public int Count { get; }
        public string Name { get; }
        public int Index { get; set; }
        public Operator(int count, string name)
        {
            Count = count;
            Name = name;
        }
    }
}
