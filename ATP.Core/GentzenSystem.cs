namespace ATP.Core
{
    public class GentzenSystem:FormalSystem
    {
        public int And=>0;
        public int Or=>1;
        public int Not => 2;
        public int Implies =>3;
        public GentzenSystem() : base(
            new Operator1(2, "And"),
            new Operator1(2, "Or"),
            new Operator1(1, "Not"),
            new Operator1(2, "Implies")
            )
        {

        }
        public bool Test(ITerm term)
        {
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            Test(sequent);
            return sequent.Mode == Mode.Success;
        }
        public void Test(FiniteSequent sequent)
        {
            if (sequent.Mode == Mode.Success) return;
            foreach (int left in sequent.Left)
                if (Terms[left] is Term1 t)
                {
                    if(t.Operator.Index==And)
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
                    else if(t.Operator.Index==Not)
                    {
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        next.Left.Remove(left);
                        next.RegisterRight(t.Terms[0].Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next);
                        Test(next);
                        sequent.Mode = next.Mode;
                        return;
                    }
                }
            foreach(int right in sequent.Right)
                if (Terms[right] is Term1 t)
                {
                    if(t.Operator.Index==Or)
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
                    else if(t.Operator.Index==Implies)
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
                    else if (t.Operator.Index == Not)
                    {
                        FiniteSequent next = new();
                        next.Left.UnionWith(sequent.Left);
                        next.Right.UnionWith(sequent.Right);
                        next.Right.Remove(right);
                        next.RegisterLeft(t.Terms[0].Index);
                        sequent.Nodes = new();
                        sequent.Nodes.Add(next);
                        Test(next);
                        sequent.Mode = next.Mode;
                        return;
                    }
                }
            foreach (int left in sequent.Left)
                if (Terms[left] is Term1 t)
                {
                    if (t.Operator.Index == Or)
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
                    else if (t.Operator.Index == Implies)
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
                    else throw new Exception();
                }
            foreach (int right in sequent.Right)
                if (Terms[right] is Term1 t)
                {
                    if (t.Operator.Index == And)
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
                    else throw new Exception();
                }
            sequent.Mode = Mode.Fail;
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
        public bool FastTest(ITerm term)
        {
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            FiniteSequent next = new();
            foreach (int left in sequent.Left)
                InsertLeft(next, Terms[left]);
            foreach (int right in sequent.Right)
                InsertRight(next, Terms[right]);
            sequent.Nodes = new();
            sequent.Nodes.Add(next);
            FastTest(next);
            sequent.Mode = next.Mode;
            return sequent.Mode == Mode.Success;
        }
        public void FastTest(FiniteSequent sequent)
        {
            if (sequent.Mode == Mode.Success) return;
            foreach (int left in sequent.Left)
            {
                if (Terms[left] is Term1 t)
                {
                    if (t.Operator.Index == Or)
                    {
                        FiniteSequent next1 = new();
                        next1.Left.UnionWith(sequent.Left);
                        next1.Right.UnionWith(sequent.Right);
                        next1.Left.Remove(left);
                        FiniteSequent next2 = new();
                        next2.Left.UnionWith(sequent.Left);
                        next2.Right.UnionWith(sequent.Right);
                        next2.Left.Remove(left);
                        InsertLeft(next1, t.Terms[0]);
                        InsertLeft(next2, t.Terms[1]);
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
                    else if (t.Operator.Index == Implies)
                    {
                        FiniteSequent next1 = new();
                        next1.Left.UnionWith(sequent.Left);
                        next1.Right.UnionWith(sequent.Right);
                        next1.Left.Remove(left);
                        FiniteSequent next2 = new();
                        next2.Left.UnionWith(sequent.Left);
                        next2.Right.UnionWith(sequent.Right);
                        next2.Left.Remove(left);
                        InsertRight(next1, t.Terms[0]);
                        InsertLeft(next2, t.Terms[1]);
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
            }
            foreach (int right in sequent.Right)
            {
                if (Terms[right] is Term1 t)
                {
                    if (t.Operator.Index == And)
                    {
                        FiniteSequent next1 = new();
                        next1.Left.UnionWith(sequent.Left);
                        next1.Right.UnionWith(sequent.Right);
                        next1.Right.Remove(right);
                        FiniteSequent next2 = new();
                        next2.Left.UnionWith(sequent.Left);
                        next2.Right.UnionWith(sequent.Right);
                        next2.Right.Remove(right);
                        InsertRight(next1, t.Terms[0]);
                        InsertRight(next2, t.Terms[1]);
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
            }
            sequent.Mode = Mode.Fail;
        }
        public void InsertLeft(FiniteSequent sequent,ITerm term)
        {
            if (term is Variable) sequent.RegisterLeft(term.Index);
            else if (term is Term1 t)
            {
                if (t.Operator.Index == And)
                {
                    InsertLeft(sequent, t.Terms[0]);
                    InsertLeft(sequent, t.Terms[1]);
                }
                else if (t.Operator.Index == Not)
                    InsertRight(sequent, t.Terms[0]);
                else if(t.Operator.Index==Implies)
                {
                    if (sequent.Left.Contains(t.Terms[0].Index)) InsertLeft(sequent, t.Terms[1]);
                    else if (sequent.Right.Contains(t.Terms[1].Index)) InsertRight(sequent, t.Terms[0]);
                    else sequent.RegisterLeft(term.Index);
                }
                else sequent.RegisterLeft(t.Index);
            }
            else throw new Exception();
        }
        public void InsertRight(FiniteSequent sequent, ITerm term)
        {
            if (term is Variable) sequent.RegisterRight(term.Index);
            else if (term is Term1 t)
            {
                if (t.Operator.Index == Or)
                {
                    InsertRight(sequent, t.Terms[0]);
                    InsertRight(sequent, t.Terms[1]);
                }
                else if (t.Operator.Index == Implies)
                {
                    InsertLeft(sequent, t.Terms[0]);
                    InsertRight(sequent, t.Terms[1]);
                }
                else if (t.Operator.Index == Not)
                    InsertLeft(sequent, t.Terms[0]);
                else sequent.RegisterRight(t.Index);
            }
            else throw new Exception();
        }
        public string Format(ITerm term,int mode=0)
        {
            if (term is Variable variable) return variable.Name;
            else if (term is Term1 t)
            {
                if (t.Operator.Index == And)
                    if(mode is 0 or 1) return $"{Format(t.Terms[0],1)} \\wedge {Format(t.Terms[1],1)}";
                    else if(mode is 2 or 3) return $"({Format(t.Terms[0], 1)} \\wedge {Format(t.Terms[1], 1)})";
                if (t.Operator.Index == Or)
                    if (mode is 0 or 2) return $"{Format(t.Terms[0])} \\vee {Format(t.Terms[1])}";
                    else if (mode is 1 or 3) return $"({Format(t.Terms[0])} \\vee {Format(t.Terms[1])})";
                if (t.Operator.Index == Implies) return $"{Format(t.Terms[0],3)} \\to {Format(t.Terms[1],3)}";
                if (t.Operator.Index == Not) return $"\\lnot {Format(t.Terms[0],3)}";
            }
            throw new Exception();
        }
        public CNF ToCNF(CNFSystem system,ITerm term,bool @true=true)
        {
            if (term is Variable variable)
            {
                CNF cnf = new();
                HashSet<CNF.Literal> c = new();
                c.Add(system.Literal(variable.Name, @true));
                cnf.Values.Add(c);
                return cnf;
            }
            else if (term is Term1 t)
            {
                if (t.Operator.Index == And)
                {
                    if (@true)
                        return ToCNF(system, t.Terms[0], true) & ToCNF(system, t.Terms[1], true);
                    else
                        return ToCNF(system, t.Terms[0], false) | ToCNF(system, t.Terms[1], false);
                }
                else if (t.Operator.Index == Or)
                {
                    if (@true)
                        return ToCNF(system, t.Terms[0], true) | ToCNF(system, t.Terms[1], true);
                    else
                        return ToCNF(system, t.Terms[0], false) & ToCNF(system, t.Terms[1], false);
                }
                else if (t.Operator.Index == Implies)
                {
                    if (@true)
                        return ToCNF(system, t.Terms[0], false) | ToCNF(system, t.Terms[1], true);
                    else
                        return ToCNF(system, t.Terms[0], true) & ToCNF(system, t.Terms[1], false);
                }
                else if (t.Operator.Index == Not)
                    return ToCNF(system, t.Terms[0], !@true);
            }
            throw new Exception();
        }
    }
}
