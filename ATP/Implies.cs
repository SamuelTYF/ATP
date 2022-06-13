namespace ATP
{
    public class Implies:ITerm
    {
        public ITerm Left;
        public ITerm Right;
        public int Deep { get; private set; }
        public Implies(ITerm left,ITerm right)
        {
            Left = left;
            Right = right;
            Deep = (Left.Deep > Right.Deep) ? Left.Deep + 1 : Right.Deep + 1;
        }
        public int CompareTo(ITerm other)
        {
            if (other is Implies implies)
            {
                int r = Left.CompareTo(implies.Left);
                if(r==0)r= Right.CompareTo(implies.Right);
                return r;
            }
            else return -1;
        }
        public override string ToString() => $"{Left} \\to {Right}";
    }
}
