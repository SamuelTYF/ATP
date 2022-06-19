namespace ATP.Core.PL
{
    public class SLD_System
    {
        public List<string> Variables;
        public Dictionary<string, int> VariableMap;
        public List<HashSet<int>> BackGoals;
        public List<HashSet<int>> BackHorns;
        public List<Horn> Horns;
        public List<Goal> Goals;
        public SLD_System()
        {
            Variables = new();
            VariableMap = new();
            BackGoals = new();
            BackHorns = new();
            Horns = new();
            Goals = new();
            Goals.Add(new(0,Array.Empty<int>()));
        }
        public int GetVariable(string name)
        {
            if(!VariableMap.ContainsKey(name))
            {
                VariableMap[name] = Variables.Count;
                Variables.Add(name);
                BackHorns.Add(new());
                BackGoals.Add(new());
            }
            return VariableMap[name];
        }
        public Goal GetGoal(params string[] names)
            => GetGoal(new HashSet<int>(names.Select(name=>GetVariable(name))).ToArray());
        public Goal GetGoal(params int[] variables)
        {
            if (variables.Length == 0) return Goals[0];
            HashSet<int> back = BackGoals[variables[0]];
            for (int i = 1; i < variables.Length; i++)
                back.IntersectWith(BackGoals[variables[1]]);
            List<Goal> goals = new(back.Select(goal => Goals[goal]).Where(goal => goal.Literals.Count == variables.Length));
            if (goals.Count > 1) throw new Exception();
            if(goals.Count==0)
            {
                Goal goal = new(Goals.Count,variables);
                Goals.Add(goal);
                for (int i = 0; i < variables.Length; i++)
                    BackGoals[variables[i]].Add(goal.Index);
                goals.Add(goal);
            }
            return goals[0];
        }
        public Horn GetHorn(int literal, params int[] goals)
            => GetHorn(literal, GetGoal(goals));
        public Horn GetHorn(int literal,Goal goal)
        {
            List<Horn> horns = new(BackHorns[literal].Select(index => Horns[index]).Where(horn => horn.Goal == goal));
            if (horns.Count > 1) throw new Exception();
            else if(horns.Count==0)
            {
                Horn horn = new(Horns.Count, goal, literal);
                Horns.Add(horn);
                BackHorns[literal].Add(horn.Index);
                horns.Add(horn);
            }
            return horns[0];
        }
        public void Test(State state)
        {
            if (state.Mode == Mode.Success) return;
            if (state.Goal == null)
            {
                if (state.Horns.Count == 0)
                {
                    state.Mode = Mode.Fail;
                    return;
                }
                else
                {
                    State t = state.Clone();
                    Horn horn = Horns[t.Horns.First()];
                    t.Horns.Remove(horn.Index);
                    State f = t.Clone();
                    state.Nodes = new();
                    state.Nodes.Add(t);
                    state.Nodes.Add(f);
                    t.RegisterLiteralP(horn.Literal);
                    f.Register(horn.Goal);
                }
            }
            else
            {
                List<int> ls = new(state.Goal.Literals);
                State t = state.Clone();
                t.Goal = null;
                State f = t.Clone();
                t.RegisterLiteralN(ls[0]);
                f.Register(GetGoal(ls.ToArray()[1..]));
                state.Nodes = new();
                state.Nodes.Add(t);
                state.Nodes.Add(f);
            }
            for (int i = 0; i < state.Nodes.Count; i++)
            {
                Test(state.Nodes[i]);
                if (state.Nodes[i].Mode != Mode.Success)
                {
                    state.Mode = Mode.Fail;
                    return;
                }
            }
            state.Mode = Mode.Success;
            return;
        }
        public string Format(State state)
        {
            List<string> ss = new();
            foreach (int p in state.Positive)
                ss.Add(Variables[p]);
            foreach (int n in state.Negative)
                ss.Add($"\\lnot {Variables[n]}");
            foreach(int h in state.Horns)
            {
                Horn horn = Horns[h];
                ss.Add($"\\{{{string.Join(",", horn.Goal.Literals.Select(l => $"\\lnot {Variables[l]}"))},{Variables[horn.Literal]}\\}}");
            }
            if (state.Goal != null)
                ss.Add($"\\{{{string.Join(",", state.Goal.Literals.Select(l => $"\\lnot {Variables[l]}"))}\\}}");
            return string.Join(",", ss)+"\\to";
        }
        public string Trace(State state)
        {
            string color = state.Mode switch
            {
                Mode.None => "black",
                Mode.Success => "green",
                Mode.Fail => "red",
                _ => throw new Exception()
            };
            string info = $"\\color{{{color}}}{Format(state)}";
            if (state.Nodes == null) return info;
            else return $"\\cfrac{{{info}}}{{{string.Join(" \\qquad ", state.Nodes.Select(node => Trace(node)))}}}";
        }
    }
}
