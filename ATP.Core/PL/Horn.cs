namespace ATP.Core.PL
{
    public class Horn
    {
        public Goal Goal;
        public int Literal;
        public int Index;
        public Horn(int index,Goal goal,int literal)
        {
            Index = index;
            Goal = goal;
            Literal = literal;
        }
    }
    public class Goal
    {
        public HashSet<int> Literals;
        public int Index;
        public Goal(int index, IEnumerable<int> literals)
        {
            Index = index;
            Literals = new(literals);
        }
    }
    public class State
    {
        public HashSet<int> Positive;
        public HashSet<int> Negative;
        public Goal Goal;
        public HashSet<int> Horns;
        public Mode Mode;
        public List<State> Nodes;
        public State()
        {
            Positive = new();
            Negative = new();
            Goal = null;
            Horns = new();
            Mode = Mode.None;
        }
        public void RegisterLiteralP(int literal)
        {
            Positive.Add(literal);
            if (Negative.Contains(literal)) Mode = Mode.Success;
            if (Goal != null && Goal.Literals.Intersect(Positive).Count() == Goal.Literals.Count) Mode = Mode.Success;
        }
        public void RegisterLiteralN(int literal)
        {
            Negative.Add(literal);
            if (Positive.Contains(literal)) Mode = Mode.Success;
        }
        public void Register(Horn horn)
        {
            if (horn.Goal.Literals.Count == 0) RegisterLiteralP(horn.Literal);
            else Horns.Add(horn.Index);
        }
        public void Register(Goal goal)
        {
            if (goal.Literals.Count == 0) throw new Exception();
            else if (goal.Literals.Count == 1) RegisterLiteralN(goal.Literals.First());
            else if (Goal == goal) throw new Exception();
            else
            {
                Goal = goal;
                if (goal.Literals.Intersect(Positive).Count() == goal.Literals.Count) Mode = Mode.Success;
            }
        }
        public State Clone()
            => new()
            {
                Positive = new(Positive),
                Negative = new(Negative),
                Goal = Goal,
                Horns = new(Horns),
                Mode = Mode,
            };
    }
}
