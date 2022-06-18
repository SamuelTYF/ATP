using System.Text;

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
            Forall = GetOperator("Forall", 2);
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
        public bool Test(Term term, int depth=10)
        {
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            Test(sequent, depth);
            return sequent.Mode == Mode.Success;
        }
        public void Test(FiniteSequent sequent, int depth)
        {
            if (sequent.Mode == Mode.Success) return;
            if (depth == 0) return;
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
                    Test(next, depth-1);
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
                    Test(next, depth-1);
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
                        Test(next, depth-1);
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
                        Test(next, depth-1);
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
                        Test(next, depth-1);
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
                            Test(next, depth-1);
                            if (next.Mode != Mode.Success)
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
                            Test(next, depth-1);
                            if (next.Mode != Mode.Success)
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
                            Test(next, depth-1);
                            if (next.Mode != Mode.Success)
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
                        Test(next, depth-1);
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
                            Test(next, depth-1);
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
                        Test(next, depth-1);
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
                            Test(next, depth-1);
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
        public PrenexForm ToPrenexForm(Term term)
        {
            if (term is Literal literal) return new(new(), new(), literal);
            else if (term is BoundLiteral boundliteral) return new(new(), new(), boundliteral);
            else if (term is Functor t)
            {
                if (t.Operator == And)
                {
                    PrenexForm p1 = ToPrenexForm(t.Terms[0]);
                    PrenexForm p2 = ToPrenexForm(t.Terms[1]);
                    List<bool> qs = new();
                    qs.AddRange(p1.Quantifiers);
                    qs.AddRange(p2.Quantifiers);
                    List<BoundLiteral> bs = new();
                    bs.AddRange(p1.BoundLiterals);
                    bs.AddRange(p2.BoundLiterals);
                    PrenexForm r = new(qs, bs, Call(And, p1.Term, p2.Term));
                    return t.True ? r : r.Mirror();
                }
                else if (t.Operator == Or)
                {
                    PrenexForm p1 = ToPrenexForm(t.Terms[0]);
                    PrenexForm p2 = ToPrenexForm(t.Terms[1]);
                    List<bool> qs = new();
                    qs.AddRange(p1.Quantifiers);
                    qs.AddRange(p2.Quantifiers);
                    List<BoundLiteral> bs = new();
                    bs.AddRange(p1.BoundLiterals);
                    bs.AddRange(p2.BoundLiterals);
                    PrenexForm r = new(qs, bs, Call(Or, p1.Term, p2.Term));
                    return t.True ? r : r.Mirror();
                }
                else if (t.Operator == Implies)
                {
                    PrenexForm p1 = ToPrenexForm(t.Terms[0]);
                    PrenexForm p2 = ToPrenexForm(t.Terms[1]);
                    List<bool> qs = new();
                    qs.AddRange(p1.Quantifiers);
                    qs.AddRange(p2.Quantifiers);
                    List<BoundLiteral> bs = new();
                    bs.AddRange(p1.BoundLiterals);
                    bs.AddRange(p2.BoundLiterals);
                    PrenexForm r = new(qs, bs, Call(Implies, p1.Term, p2.Term));
                    return t.True ? r : r.Mirror();
                }
                else if (t.Operator == Exist)
                {
                    PrenexForm r = ToPrenexForm(t.Terms[1]);
                    r.Quantifiers.Insert(0,false);
                    r.BoundLiterals.Insert(0,t.Terms[0] as BoundLiteral);
                    return t.True ? r : r.Mirror();
                }
                else if (t.Operator == Forall)
                {
                    PrenexForm r = ToPrenexForm(t.Terms[1]);
                    r.Quantifiers.Insert(0,true);
                    r.BoundLiterals.Insert(0,t.Terms[0] as BoundLiteral);
                    return t.True ? r : r.Mirror();
                }
                else return new(new(), new(), t);
            }
            else throw new Exception();
        }
        public Term Substitution(Term term,Dictionary<int,int> map)
        {
            if (!term.True) return Substitution(term.Mirror, map).Mirror;
            if (map.ContainsKey(term.Index)) return Terms[map[term.Index]];
            if (term is Literal literal) return literal;
            if (term is BoundLiteral boundliteral) return boundliteral;
            if (term is Functor f) return Call(f.Operator, f.Terms.Select(t => Substitution(t, map)).ToArray());
            throw new Exception();
        }
        public string Format(PrenexForm pf)
        {
            StringBuilder builder = new();
            for (int i = 0; i < pf.BoundLiterals.Count; i++)
                builder.Append(pf.Quantifiers[i] ? $"\\forall {pf.BoundLiterals[i]}" : $"\\exist {pf.BoundLiterals[i]}");
            return $"{builder}({Format(pf.Term)})";
        }
        public override Term Parse(string value)
        {
            value = value.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            int index = 0;
            Dictionary<string, string> map = new();
            return FirstOrderParse(value.ToCharArray(), ref index, map);
        }
        public Term FirstOrderParse(char[] values, ref int index, Dictionary<string,string> map)
        {
            StringBuilder builder = new();
            for (; index < values.Length && values[index] is not ('[' or ']' or ','); index++)
                builder.Append(values[index]);
            string name = builder.ToString();
            if (index == values.Length || values[index] is ']' or ',')
            {
                if (map.ContainsKey(name)) return GetBoundLiteral(map[name], true);
                else return GetLiteral(name, true);
            }
            else if (values[index] is '[')
            {
                if (name == "Not")
                {
                    index++;
                    Term term = FirstOrderParse(values, ref index,map);
                    index++;
                    return term.Mirror;
                }
                else if(name=="Exist")
                {
                    index++;
                    builder.Clear();
                    for (; index < values.Length && values[index] is not ('[' or ']' or ','); index++)
                        builder.Append(values[index]);
                    string bound = builder.ToString();
                    index++;
                    if (map.ContainsKey(bound)) throw new Exception();
                    string newbound = $"x_{BoundLiterals.Count}";
                    map[bound] = newbound;
                    Term bl = GetBoundLiteral(newbound);
                    Term r = Call(Exist, bl, FirstOrderParse(values, ref index, map));
                    map.Remove(bound);
                    index++;
                    return r;
                }
                else if (name == "Forall")
                {
                    index++;
                    builder.Clear();
                    for (; index < values.Length && values[index] is not ('[' or ']' or ','); index++)
                        builder.Append(values[index]);
                    string bound = builder.ToString();
                    index++;
                    if (map.ContainsKey(bound)) throw new Exception();
                    string newbound = $"x_{BoundLiterals.Count}";
                    map[bound] = newbound;
                    Term bl = GetBoundLiteral(newbound);
                    Term r = Call(Forall, bl, FirstOrderParse(values, ref index, map));
                    map.Remove(bound);
                    index++;
                    return r;
                }
                else
                {
                    Operator @operator = OperatorMap[name];
                    Term[] terms = new Term[@operator.Count];
                    for (int i = 0; i < terms.Length; i++)
                    {
                        index++;
                        terms[i] = FirstOrderParse(values, ref index,map);
                    }
                    index++;
                    return Call(@operator, terms);
                }
            }
            else throw new Exception();
        }
    }
}
