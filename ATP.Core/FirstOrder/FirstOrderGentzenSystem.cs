namespace ATP.Core.FirstOrder
{
    public class FirstOrderGentzenSystem:FirstOrderSystem
    {
        public Operator And;
        public Operator Or;
        public Operator Implies;
        public Operator Forall;
        public Operator Exist;
        public int TempVariableCount;
        public Dictionary<int, HashSet<int>> TempTerms;
        public FirstOrderGentzenSystem():base()
        {
            And = GetOperator("And", 2);
            Or = GetOperator("Or", 2);
            Implies = GetOperator("Implies", 2);
            Forall = GetOperator("ForAll", 2);
            Exist = GetOperator("Exist", 2);
            TempVariableCount = 0;
            TempTerms = new();
            TempTerms[GetLiteral($"t_{TempVariableCount++}").Index]=new();
        }
        public string Format(Term term)
        {
            if (term is Literal literal) return literal.ToString();
            else if (term is BoundLiteral boundliteral) return boundliteral.ToString();
            else if (term is Functor functor)
            {
                string text;
                if (functor.Operator == And) text = $"{Format(functor.Terms[0])} \\wedge {Format(functor.Terms[1])}";
                else if (functor.Operator == Or) text = $"{Format(functor.Terms[0])} \\vee {Format(functor.Terms[1])}";
                else if (functor.Operator == Implies) text = $"({Format(functor.Terms[0])})\\to({Format(functor.Terms[1])})";
                else if (functor.Operator == Forall) text = $"\\forall {Format(functor.Terms[0])}({Format(functor.Terms[1])})";
                else if (functor.Operator == Exist) text = $"\\exist {Format(functor.Terms[0])}({Format(functor.Terms[1])})";
                else text = $"{functor.Operator.Name}({string.Join(",", functor.Terms.Select(term => Format(term)))})";
                return functor.True ? text : $"\\lnot ({text})";
            }
            else throw new Exception();
        }
        public bool Test(Term term)
        {
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            Test(sequent);
            return sequent.Mode == Mode.Success;
        }
        public void Test(FiniteSequent sequent)
        {
            if (sequent.Mode == Mode.Success) return;
            foreach(int left in sequent.Left)
                if (!Terms[left].True)
                {
                    FiniteSequent next = new();
                    next.Left.UnionWith(sequent.Left);
                    next.Right.UnionWith(sequent.Right);
                    next.Left.Remove(left);
                    next.RegisterRight(Terms[left].Mirror.Index);
                    sequent.Nodes = new();
                    sequent.Nodes.Add(next);
                    Test(next);
                    sequent.Mode = next.Mode;
                    return;
                }
            foreach (int right in sequent.Right)
                if (!Terms[right].True)
                {
                    FiniteSequent next = new();
                    next.Left.UnionWith(sequent.Left);
                    next.Right.UnionWith(sequent.Right);
                    next.Right.Remove(right);
                    next.RegisterLeft(Terms[right].Mirror.Index);
                    sequent.Nodes = new();
                    sequent.Nodes.Add(next);
                    Test(next);
                    sequent.Mode = next.Mode;
                    return;
                }
            foreach (int left in sequent.Left)
                if (Terms[left] is Functor t)
                {
                    if (t.Operator == And)
                    {
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        next.Left.Remove(left);
                        next.RegisterLeft(t.Terms[0].Index);
                        next.RegisterLeft(t.Terms[1].Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next);
                        Test(next);
                        sequent.Mode = next.Mode;
                        return;
                    }
                }
            foreach (int right in sequent.Right)
                if (Terms[right] is Functor t)
                {
                    if (t.Operator == Or)
                    {
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        next.Right.Remove(right);
                        next.RegisterRight(t.Terms[0].Index);
                        next.RegisterRight(t.Terms[1].Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next);
                        Test(next);
                        sequent.Mode = next.Mode;
                        return;
                    }
                    else if (t.Operator == Implies)
                    {
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        next.Right.Remove(right);
                        next.RegisterLeft(t.Terms[0].Index);
                        next.RegisterRight(t.Terms[1].Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next);
                        Test(next);
                        sequent.Mode = next.Mode;
                        return;
                    }
                }
            foreach (int left in sequent.Left)
                if (Terms[left] is Functor t)
                {
                    if (t.Operator == Or)
                    {
                        FiniteSequent next1 = new();
                        next1.Left.UnionWith(sequent.Left);
                        next1.Right.UnionWith(sequent.Right);
                        next1.Left.Remove(left);
                        FiniteSequent next2 = new();
                        next2.Left.UnionWith(sequent.Left);
                        next2.Right.UnionWith(sequent.Right);
                        next2.Left.Remove(left);
                        next1.RegisterLeft(t.Terms[0].Index);
                        next2.RegisterLeft(t.Terms[1].Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next1);
                        sequent.Nodes.Add(next2);
                        foreach (FiniteSequent next in sequent.Nodes)
                        {
                            Test(next);
                            if (next.Mode == Mode.Fail)
                            {
                                sequent.Mode = Mode.Fail;
                                return;
                            }
                        }
                        sequent.Mode = Mode.Success;
                        return;
                    }
                    else if (t.Operator == Implies)
                    {
                        FiniteSequent next1 = new();
                        next1.Left.UnionWith(sequent.Left);
                        next1.Right.UnionWith(sequent.Right);
                        next1.Left.Remove(left);
                        FiniteSequent next2 = new();
                        next2.Left.UnionWith(sequent.Left);
                        next2.Right.UnionWith(sequent.Right);
                        next2.Left.Remove(left);
                        next1.RegisterRight(t.Terms[0].Index);
                        next2.RegisterLeft(t.Terms[1].Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next1);
                        sequent.Nodes.Add(next2);
                        foreach (FiniteSequent next in sequent.Nodes)
                        {
                            Test(next);
                            if (next.Mode == Mode.Fail)
                            {
                                sequent.Mode = Mode.Fail;
                                return;
                            }
                        }
                        sequent.Mode = Mode.Success;
                        return;
                    }
                }
            foreach (int right in sequent.Right)
                if (Terms[right] is Functor t)
                {
                    if (t.Operator == And)
                    {
                        FiniteSequent next1 = new();
                        next1.Left.UnionWith(sequent.Left);
                        next1.Right.UnionWith(sequent.Right);
                        next1.Right.Remove(right);
                        FiniteSequent next2 = new();
                        next2.Left.UnionWith(sequent.Left);
                        next2.Right.UnionWith(sequent.Right);
                        next2.Right.Remove(right);
                        next1.RegisterRight(t.Terms[0].Index);
                        next2.RegisterRight(t.Terms[1].Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next1);
                        sequent.Nodes.Add(next2);
                        foreach (FiniteSequent next in sequent.Nodes)
                        {
                            Test(next);
                            if (next.Mode == Mode.Fail)
                            {
                                sequent.Mode = Mode.Fail;
                                return;
                            }
                        }
                        sequent.Mode = Mode.Success;
                        return;
                    }
                }
            foreach(int left in sequent.Left)
                if (Terms[left] is Functor t)
                {
                    if(t.Operator==Exist)
                    {
                        Term temp = GetLiteral($"t_{TempVariableCount++}");
                        TempTerms[temp.Index] = new();
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        next.Left.Remove(left);
                        next.RegisterLeft(Apply(t.Terms[1], t.Terms[0] as BoundLiteral,temp as Literal).Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next);
                        Test(next);
                        sequent.Mode = next.Mode;
                        return;
                    }
                    else if(t.Operator==Forall)
                    {
                        bool success = false;
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        foreach (KeyValuePair<int,HashSet<int>> tt in TempTerms)
                            if(!tt.Value.Contains(t.Index))
                            {
                                next.RegisterLeft(Apply(t.Terms[1], t.Terms[0] as BoundLiteral, Terms[tt.Key] as Literal).Index);
                                success = true;
                                tt.Value.Add(t.Index);
                            }
                        if(success)
                        {
                            sequent.Nodes = new();
                            sequent.Nodes.Add(next);
                            Test(next);
                            sequent.Mode = next.Mode;
                            return;
                        }
                    }
                }
            foreach(int right in sequent.Right)
                if (Terms[right] is Functor t)
                {
                    if (t.Operator == Forall)
                    {
                        Term temp = GetLiteral($"t_{TempVariableCount++}");
                        TempTerms[temp.Index] = new();
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        next.Right.Remove(right);
                        next.RegisterRight(Apply(t.Terms[1], t.Terms[0] as BoundLiteral, temp as Literal).Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next);
                        Test(next);
                        sequent.Mode = next.Mode;
                        return;
                    }
                    else if (t.Operator == Exist)
                    {
                        bool success = false;
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        foreach (KeyValuePair<int, HashSet<int>> tt in TempTerms)
                            if (!tt.Value.Contains(t.Index))
                            {
                                next.RegisterRight(Apply(t.Terms[1], t.Terms[0] as BoundLiteral, Terms[tt.Key] as Literal).Index);
                                success = true;
                                tt.Value.Add(t.Index);
                            }
                        if (success)
                        {
                            sequent.Nodes = new();
                            sequent.Nodes.Add(next);
                            Test(next);
                            sequent.Mode = next.Mode;
                            return;
                        }
                    }
                }
            sequent.Mode = Mode.Fail;
        }
        public Term Apply(Term term, BoundLiteral source, Literal destination)
        {
            if (term is Literal) return term;
            else if (term is BoundLiteral boundliteral)
                if (boundliteral.True)
                {
                    if (boundliteral.Index == source.Index) return destination;
                    else return boundliteral;
                }
                else
                {
                    if (boundliteral.Mirror.Index == source.Index) return destination.Mirror;
                    else return boundliteral;
                }
            else if (term is Functor functor)
            {
                if (!functor.BoundLiterals.Contains(source)) return functor;
                else if (functor.True) return Call(functor.Operator, functor.Terms.Select(term => Apply(term, source, destination)).ToArray());
                else return Call(functor.Operator, functor.Terms.Select(term => Apply(term, source, destination)).ToArray()).Mirror;
            }
            else throw new Exception();
        }
        public string Trace(FiniteSequent sequent)
        {
            string color = sequent.Mode switch
            {
                Mode.None => "black",
                Mode.Success => "green",
                Mode.Fail => "red",
                _ => throw new Exception()
            };
            string info = $"\\color{{{color}}}{string.Join(",", sequent.Left.Select(left => Format(Terms[left])))} \\vdash {string.Join(",", sequent.Right.Select(right => Format(Terms[right])))}";
            if (sequent.Nodes == null) return info;
            else return $"\\cfrac{{{info}}}{{{string.Join(" \\qquad ", sequent.Nodes.Select(node => Trace(node)))}}}";
        }
    }
}
