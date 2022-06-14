namespace ATP.Core
{
    public interface IOperator
    {
        public int Count { get; }
        public string Name { get; }
        public int Index { get; set; }
    }
}
