namespace ATP.Core.FirstOrder
{
    public class SkolemSystem
    {
        public List<SkolemTerm> Terms;
        public Dictionary<string, int> Variables;
        public Dictionary<string, int> Constants;
        public List<Operator> Operators;
        public Dictionary<string, Operator> OperatorMap;
        public List<HashSet<SkolemTerm>> Clauses;
        public SkolemSystem()
        {
            Terms = new();
            Variables = new();
            Constants = new();
            Operators = new();
            OperatorMap = new();
            Clauses = new();
        }
        public Operator GetOperator(string name, int count)
        {
            if (!OperatorMap.ContainsKey(name))
            {
                Operator @operator = new(name, count, Operators.Count);
                OperatorMap[name] = @operator;
                Operators.Add(@operator);
            }
            return OperatorMap[name];
        }
        public SkolemTerm GetConstant(string name, bool @true = true)
        {
            if (!Constants.ContainsKey(name))
            {
                SkolemConstant t = new(Terms.Count, name, true);
                Terms.Add(t);
                Constants[name] = t.Index;
                SkolemConstant f = new(Terms.Count, name, false);
                Terms.Add(t);
                t.Mirror = f;
                f.Mirror = t;
                t.Constants.Add(t.Index);
                f.Constants.Add(f.Index);
            }
            return @true ? Terms[Constants[name]] : Terms[Constants[name]].Mirror;
        }
        public SkolemTerm GetVariable(string name, bool @true = true)
        {
            if (!Variables.ContainsKey(name))
            {
                SkolemVariable t = new(Terms.Count, name, true);
                Terms.Add(t);
                Variables[name] = t.Index;
                SkolemVariable f = new(Terms.Count, name, false);
                Terms.Add(t);
                t.Mirror = f;
                f.Mirror = t;
                t.Variables.Add(t.Index);
                f.Variables.Add(f.Index);
            }
            return @true ? Terms[Variables[name]] : Terms[Variables[name]].Mirror;
        }
        public SkolemTerm Call(string op, params SkolemTerm[] terms)
            => Call(GetOperator(op, terms.Length), terms);
        public SkolemTerm Call(Operator @operator, params SkolemTerm[] terms)
        {
            HashSet<int> funcs = terms[0].GetBack(@operator.Index, 0);
            for (int i = 1; i < terms.Length; i++)
                funcs.IntersectWith(terms[1].GetBack(@operator.Index, i));
            List<SkolemFunctor> functors = new(funcs.Select(i => Terms[i] as SkolemFunctor).Where(func => func.True));
            if (functors.Count > 1) throw new Exception();
            if (functors.Count == 0)
            {
                SkolemFunctor t = new(Terms.Count, @operator, terms, true);
                Terms.Add(t);
                SkolemFunctor f = new(Terms.Count, @operator, terms, false);
                Terms.Add(f);
                functors.Add(t);
                t.Mirror = f;
                f.Mirror = t;
            }
            return functors[0];
        }
        public SkolemForm FromPrenexForm(PrenexForm pf, FirstOrderGentzenSystem system)
        {
            List<SkolemVariable> variables = new();
            List<SkolemConstant> constants = new();
            Dictionary<int, int> map = new();
            int vc = 0;
            int cc = 0;
            for (int i = 0; i < pf.BoundLiterals.Count; i++)
            {
                if (pf.Quantifiers[i])
                {
                    SkolemVariable variable = GetVariable($"x_{vc++}") as SkolemVariable;
                    variables.Add(variable);
                    map[pf.BoundLiterals[i].Index] = variable.Index;
                }
                else if (vc == 0)
                {
                    SkolemConstant constant = GetConstant($"c_{cc++}") as SkolemConstant;
                    constants.Add(constant);
                    map[pf.BoundLiterals[i].Index] = constant.Index;
                }
                else
                {
                    Operator @operator = GetOperator($"S_{vc}", vc);
                    SkolemTerm s = Call(@operator, variables.ToArray());
                    map[pf.BoundLiterals[i].Index] = s.Index;
                }
            }
            foreach(Literal l in pf.Term.Literals)
            {
                SkolemConstant constant = GetConstant(l.Name) as SkolemConstant;
                constants.Add(constant);
                map[l.Index] = constant.Index;
            }
            return new(variables, constants, FromPrenex(pf.Term, system, map));
        }
        public SkolemClauses And(SkolemClauses a, SkolemClauses b)
        {
            SkolemClauses r = new();
            r.Clauses.UnionWith(a.Clauses);
            r.Clauses.UnionWith(b.Clauses);
            return r;
        }
        public SkolemClauses Or(SkolemClauses a, SkolemClauses b)
        {
            SkolemClauses result = new();
            foreach (int t1 in a.Clauses)
            {
                HashSet<SkolemTerm> t = new();
                bool success = false;
                foreach (SkolemTerm l1 in Clauses[t1])
                {
                    t.Add(l1);
                    if (t.Contains(l1.Mirror))
                    {
                        success = true;
                        break;
                    }
                }
                if (success) continue;
                foreach (int t2 in b.Clauses)
                {
                    HashSet<SkolemTerm> r = new(t);
                    success = false;
                    foreach (SkolemTerm l2 in Clauses[t2])
                    {
                        r.Add(l2);
                        if (r.Contains(l2.Mirror))
                        {
                            success = true;
                            break;
                        }
                    }
                    if (success) continue;
                    result.Clauses.Add(Find(r));
                }
            }
            return result;
        }
        public SkolemTerm Substitution(Term term, Dictionary<int, int> map)
        {
            if (!term.True) return Substitution(term.Mirror, map).Mirror;
            if (map.ContainsKey(term.Index)) return Terms[map[term.Index]];
            if(term is Functor f)
            {
                Operator @operator = GetOperator(f.Operator.Name, f.Operator.Count);
                return Call(@operator, f.Terms.Select(t => Substitution(t, map)).ToArray());
            }
            throw new Exception();
        }
        public SkolemClauses FromPrenex(Term term, FirstOrderGentzenSystem system, Dictionary<int,int> map)
        {
            if(term.True)
            {
                if (map.ContainsKey(term.Index))
                {
                    SkolemClauses r = new();
                    r.Clauses.Add(Find(Terms[map[term.Index]]));
                    return r;
                }
                else if (term is Functor f)
                {
                    if (f.Operator == system.And)
                        return And(FromPrenex(f.Terms[0], system, map), FromPrenex(f.Terms[1], system, map));
                    else if (f.Operator == system.Or)
                        return Or(FromPrenex(f.Terms[0], system, map), FromPrenex(f.Terms[1], system, map));
                    else
                    {
                        Operator @operator = GetOperator(f.Operator.Name, f.Operator.Count);
                        SkolemClauses r = new();
                        r.Clauses.Add(Find(Call(@operator, f.Terms.Select(t => Substitution(t, map)).ToArray())));
                        return r;
                    }
                }
                else throw new Exception();
            }
            else
            {
                if (map.ContainsKey(term.Index))
                {
                    SkolemClauses r = new();
                    r.Clauses.Add(Find(Terms[map[term.Index]].Mirror));
                    return r;
                }
                else if (term is Functor f)
                {
                    if (f.Operator == system.And)
                        return Or(FromPrenex(f.Terms[0].Mirror, system, map), FromPrenex(f.Terms[1].Mirror, system, map));
                    else if (f.Operator == system.Or)
                        return And(FromPrenex(f.Terms[0].Mirror, system, map), FromPrenex(f.Terms[1].Mirror, system, map));
                    else
                    {
                        Operator @operator = GetOperator(f.Operator.Name, f.Operator.Count);
                        SkolemClauses r = new();
                        r.Clauses.Add(Find(Call(@operator, f.Terms.Select(t => Substitution(t, map)).ToArray()).Mirror));
                        return r;
                    }
                }
                else throw new Exception();
            }
        }
        public int Find(params SkolemTerm[] terms) => Find(new HashSet<SkolemTerm>(terms));
        public int Find(HashSet<SkolemTerm> raw)
        {
            HashSet<int> cs = new();
            for (int i = 0; i < Clauses.Count; i++)
                if (Clauses[i].Count == raw.Count)
                    cs.Add(i);
            foreach (SkolemTerm l in raw)
                cs.IntersectWith(l.Back);
            if (cs.Count > 1) throw new Exception();
            else if (cs.Count == 1) return cs.First();
            else
            {
                int index = Clauses.Count;
                foreach (SkolemTerm l in raw)
                    l.Back.Add(index);
                Clauses.Add(raw);
                return index;
            }
        }
        public string Format(SkolemForm sf)
            =>string.Join(" , ", sf.Clauses.Clauses.Select(clause => $"\\{{{string.Join(" , ", Clauses[clause])}\\}}"));
        public SkolemTerm Substitution(SkolemTerm term, Dictionary<int, int> map)
        {
            if (!term.True) return Substitution(term.Mirror, map).Mirror;
            if (map.ContainsKey(term.Index)) return Terms[map[term.Index]];
            if (term is SkolemFunctor f)
            {
                Operator @operator = GetOperator(f.Operator.Name, f.Operator.Count);
                return Call(@operator, f.Terms.Select(t => Substitution(t, map)).ToArray());
            }
            else return term;
        }
        public bool Unify(SkolemTerm a, SkolemTerm b,out SkolemTerm ra,out SkolemTerm rb,Dictionary<int,int> map=null)
        {
            if (map == null) map = new();
            ra = null;
            rb = null;
            a = Substitution(a, map);
            b = Substitution(b, map);
            if (a.Index == b.Index)
            {
                ra = a;
                rb = b;
                return true;
            }
            if (a.Index == b.Mirror.Index) return false;
            if (!a.True)
            {
                if(Unify(a.Mirror,b.Mirror,out SkolemTerm ta,out SkolemTerm tb,map))
                {
                    ra = ta.Mirror;
                    rb = tb.Mirror;
                    return true;
                }
                else return false;
            }
            if(a is SkolemVariable)
            {
                if (b.Variables.Contains(a.Index)) return false;
                else 
                {
                    map[a.Index] = b.Index;
                    ra = b;
                    rb = b;
                    return true;
                }
            }
            if (b is SkolemVariable)
            {
                if (a.Variables.Contains(b.Index)) return false;
                else
                {
                    map[b.Index] = a.Index;
                    ra = a;
                    rb = a;
                    return true;
                }
            }
            if (a is SkolemConstant && b is SkolemConstant) return false;
            if(a is SkolemFunctor fa && b is SkolemFunctor fb)
            {
                if (fa.Operator != fb.Operator) return false;
                int c = fa.Operator.Count;
                SkolemTerm[] tas = new SkolemTerm[c];
                SkolemTerm[] tbs = new SkolemTerm[c];
                for (int i=0;i< c; i++)
                    if (!Unify(fa.Terms[i], fb.Terms[i], out tas[i], out tbs[i], map)) return false;
                ra = Substitution(Call(fa.Operator, tas),map);
                rb = Substitution(fb.True ? Call(fb.Operator, tbs) : Call(fb.Operator, tbs).Mirror,map);
                return true;
            }
            return false;
        }
    }
}
