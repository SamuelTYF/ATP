namespace ATP.Core
{
    public class Operator1:IOperator
    {
        public int Count { get; }
        public string Name { get; }
        public int Index { get; set; }
        public Operator1(int count, string name)
        {
            Count = count;
            Name = name;
        }
    }
}
