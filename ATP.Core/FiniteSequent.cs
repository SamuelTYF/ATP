namespace ATP.Core
{
    public class FiniteSequent
    {
        public HashSet<int> Left;
        public HashSet<int> Right;
        public Mode Mode;
        public List<FiniteSequent> Nodes;
        public FiniteSequent()
        {
            Left = new HashSet<int>();
            Right = new HashSet<int>();
            Mode = Mode.None;
        }
        public void RegisterLeft(int left)
        {
            if (Right.Contains(left)) Mode = Mode.Success;
            Left.Add(left);
        }
        public void RegisterRight(int right)
        {
            if (Left.Contains(right)) Mode = Mode.Success;
            Right.Add(right);
        }
    }
}
