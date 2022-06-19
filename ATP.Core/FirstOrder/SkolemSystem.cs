using ATP.Core.PL;

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
        public bool Unify(SkolemTerm a, SkolemTerm b,out SkolemTerm r,Dictionary<int,int> map=null)
        {
            if (map == null) map = new();
            r = null;
            a = Substitution(a, map);
            b = Substitution(b, map);
            if (a.Index == b.Index)
            {
                r = a;
                return true;
            }
            if (a.Index == b.Mirror.Index) return false;
            if (!a.True)
            {
                if(Unify(a.Mirror,b.Mirror,out SkolemTerm t,map))
                {
                    r = t.Mirror;
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
                    r = b;
                    return true;
                }
            }
            if (b is SkolemVariable)
            {
                if (a.Variables.Contains(b.Index)) return false;
                else
                {
                    map[b.Index] = a.Index;
                    r = a;
                    return true;
                }
            }
            if (a is SkolemConstant && b is SkolemConstant) return false;
            if(a is SkolemFunctor fa && b is SkolemFunctor fb && b.True)
            {
                if (fa.Operator != fb.Operator) return false;
                int c = fa.Operator.Count;
                SkolemTerm[] ts = new SkolemTerm[c];
                for (int i=0;i< c; i++)
                    if (!Unify(fa.Terms[i], fb.Terms[i], out ts[i], map)) return false;
                r = Substitution(Call(fa.Operator, ts),map);
                return true;
            }
            return false;
        }
        public bool Unify(SkolemTerm[] terms,out SkolemTerm result)
        {
            Dictionary<int, int> map = new();
            result = terms[0];
            for(int i=1;i< terms.Length;i++)
                if (!Unify(result, terms[i], out result, map))
                {
                    result = null;
                    return false;
                }
            return true;
        }
        public class OperatorConflict
        {
            public HashSet<int> Terms;
            public HashSet<int> Variables;
            public bool Conflict;
            public OperatorConflict()
            {
                Terms = new();
                Variables = new();
                Conflict = false;
            }
            public void Register(SkolemTerm term)
            {
                if (Conflict) return;
                if (Terms.Contains(term.Index)|| Variables.Contains(term.Index)) return;
                if(Terms.Contains(term.Mirror.Index) || Variables.Contains(term.Mirror.Index))
                {
                    Conflict = true;
                    return;
                }
                if(term is SkolemVariable)
                    Variables.Add(term.Index);
                else
                {
                    Terms.Add(term.Index);
                    if (Terms.Count > 1) Conflict = true;
                }
            }
        }
        public SkolemTerm[] Test(SkolemTerm[] terms)
        {
            SkolemTerm[] results = new SkolemTerm[terms.Length];
            for (int i = 0; i < results.Length; i++)
                results[i] = terms[i];
            Dictionary<int, int> map = new();
            bool success;
            do
            {
                success = false;
                OperatorConflict[][] ops = new OperatorConflict[Operators.Count][];
                for (int i = 0; i < ops.Length; i++)
                {
                    OperatorConflict[] op = new OperatorConflict[Operators[i].Count];
                    for (int j = 0; j < op.Length; j++)
                        op[j] = new();
                    ops[i] = op;
                }
                for (int i = 0; i < terms.Length; i++)
                    SearchConflict(terms[i], ops);
                for (int i = 0; i < ops.Length; i++)
                {
                    OperatorConflict[] op = ops[i];
                    for (int j = 0; j < op.Length; j++)
                        if (!op[j].Conflict && op[i].Variables.Count>0)
                        {
                            //TODO
                        }
                }
            } while (success);
            return results;
        }
        public void SearchConflict(SkolemTerm term, OperatorConflict[][] ops)
        {
            if(term is SkolemFunctor f)
            {
                for (int i = 0; i < f.Operator.Count; i++)
                {
                    ops[f.Operator.Index][i].Register(f.Terms[i]);
                    SearchConflict(f.Terms[i], ops);
                }
            }
        }
        public bool Combine(SkolemTerm[] terms1, SkolemTerm[] terms2, out SkolemTerm[] result1,out SkolemTerm[] result2,out SkolemTerm r)
        {
            result1 = new SkolemTerm[terms1.Length];
            result2 = new SkolemTerm[terms2.Length];
            Dictionary<int, int> map = new();
            bool success = false;
            r=null;
            for(int i=0;i<terms1.Length&&!success;i++)
                for(int j=0;j<terms2.Length;j++)
                {
                    map.Clear();
                    if (Unify(terms1[i], terms2[j],out r, map))
                    {
                        if (r == null) throw new Exception();
                        success = true;
                        for (int t = 0; t < terms1.Length; t++)
                            result1[t] = Substitution(terms1[t], map);
                        for (int t = 0; t < terms2.Length; t++)
                            result2[t] = Substitution(terms2[t], map);
                        break;
                    }
                }
            if (!success) return false;
            map.Clear();
            for (int i = 0; i < terms1.Length; i++)
            {
                Dictionary<int, int> tmap = new(map);
                SkolemTerm temp;
                if (Unify(r, result1[i], out temp, tmap))
                {
                    map = tmap;
                    r = temp;
                }
            }
            for (int i = 0; i < terms2.Length; i++)
            {
                Dictionary<int, int> tmap = new(map);
                SkolemTerm temp;
                if (Unify(r, result2[i], out temp, tmap))
                {
                    map = tmap;
                    r = temp;
                }
            }
            for (int t = 0; t < terms1.Length; t++)
                result1[t] = Substitution(result1[t], map);
            for (int t = 0; t < terms2.Length; t++)
                result2[t] = Substitution(result2[t], map);
            return true;
        }
        public HashSet<SkolemTerm> Resolution(HashSet<SkolemTerm> a, HashSet<SkolemTerm> b,out SkolemTerm unify)
        {
            unify = null;
            HashSet<int> variables = new();
            foreach (SkolemTerm t in a)
                variables.UnionWith(t.Variables);
            foreach (SkolemTerm t in b)
                variables.ExceptWith(t.Variables);
            Dictionary<int, int> map = new();
            foreach (int v in variables)
                map[v] = GetVariable($"t_{Variables.Count}").Index;
            List<SkolemTerm> ma = new();
            foreach (SkolemTerm t in a)
                ma.Add(Substitution(t, map).Mirror);
            if (Combine(ma.ToArray(), b.ToArray(), out SkolemTerm[] ra, out SkolemTerm[] rb,out unify))
            {
                HashSet<SkolemTerm> rs = new();
                foreach (SkolemTerm t in ra)
                    if (t.Index != unify.Index && t.Index!=unify.Mirror.Index) rs.Add(t.Mirror);
                foreach (SkolemTerm t in rb)
                    if (t.Index != unify.Index && t.Index != unify.Mirror.Index) rs.Add(t);
                return rs;
            }
            else return null;
        }
        public bool Test(SkolemClauses cnf, IDAG<int, HashSet<SkolemTerm>, (HashSet<SkolemTerm>, HashSet<SkolemTerm>)> dag, out string tree)
        {
            tree = null;
            List<int> clauses = new(cnf.Clauses);
            List<List<(int, int, HashSet<SkolemTerm>)>> parents = new();
            Dictionary<int, int> map = new();
            for (int i = 0; i < clauses.Count; i++)
            {
                map[clauses[i]] = i;
                parents.Add(new());
            }
            for (int i = 0; i < clauses.Count; i++)
            {
                HashSet<SkolemTerm> c1 = Clauses[clauses[i]];
                dag.RegisterVertex(clauses[i], c1);
                for (int j = 0; j < i; j++)
                {
                    HashSet<SkolemTerm> c2 = Clauses[clauses[j]];
                    HashSet<SkolemTerm> c = Resolution(c1, c2, out SkolemTerm unify);
                    if (c == null) continue;
                    HashSet<SkolemTerm> u = new();
                    u.Add(unify);
                    u.Add(unify.Mirror);
                    Console.WriteLine($"$$\\cfrac{{\\{{{string.Join(" , ", c1)}\\}}\\qquad\\{{{string.Join(" , ", c2)}\\}}}}{{\\{{{string.Join(" , ", u)}\\}}\\qquad\\{{{string.Join(" , ", c)}\\}}}}$$");
                    int p = Find(c);
                    dag.RegisterEdge(clauses[i], clauses[j], (u, c));
                    if (!clauses.Contains(p))
                    {
                        map[p] = clauses.Count;
                        clauses.Add(p);
                        parents.Add(new());
                    }
                    parents[map[p]].Add((i, j, u));
                    if (c.Count == 0)
                    {
                        tree = Trace(map[p], clauses, parents, out _);
                        return true;
                    }
                }
            }
            return false;
        }
        public string Trace(int index, List<int> clauses, List<List<(int, int, HashSet<SkolemTerm>)>> parents, out HashSet<int> cs)
        {
            List<(int, int, HashSet<SkolemTerm>)> ps = parents[index];
            if (ps.Count > 0)
            {
                (int, int, HashSet<SkolemTerm>) p = ps[0];
                Console.WriteLine($"Combine {PrintClause(Clauses[clauses[p.Item1]])}\t\t{PrintClause(Clauses[clauses[p.Item2]])}");
                string s1 = Trace(p.Item1, clauses, parents, out HashSet<int> cs1);
                string s2 = Trace(p.Item2, clauses, parents, out HashSet<int> cs2);
                cs = new(cs1.Union(cs2));
                if (s1 == null && s2 == null)
                {
                    s1 = string.Join(",", Clauses[clauses[index]]) + "\\vdash";
                    s2 = string.Join(",", p.Item3) + "\\vdash";
                }
                else if (s1 == null)
                {
                    s1 = s2;
                    s2 = string.Join(",", p.Item3) + "\\vdash";
                }
                else if (s2 == null) s2 = string.Join(",", p.Item3) + "\\vdash";
                return $"\\cfrac{{{s1} \\qquad {s2}}}{{{string.Join(",", cs.Select(c => PrintClause(Clauses[c])))},{PrintClause(Clauses[clauses[index]])}\\vdash}}";
            }
            else
            {
                cs = new();
                cs.Add(clauses[index]);
                return null;
            }
        }
        public string PrintClause(HashSet<SkolemTerm> clause)
            => clause.Count == 1 ? $"{string.Join(",", clause)}" : $"\\{{{string.Join(",", clause)}\\}}";
        public string PrintTable(IDAG<int, HashSet<SkolemTerm>, (HashSet<SkolemTerm>, HashSet<SkolemTerm>)> dag)
        {
            List<int> keys = new(dag.Keys);
            List<string> rows = new();
            List<string> headers = new();
            headers.Add("From");
            headers.AddRange(dag.Values.Select(value => $"${PrintClause(value)}$"));
            rows.Add($"| {string.Join(" | ", headers)} |");
            rows.Add($"| {string.Join(" | ", headers.Select(_ => ":-:"))} |");
            foreach (int key in keys)
            {
                List<string> values = new();
                values.Add($"${PrintClause(dag.GetVertex(key))}$");
                foreach (int k in keys)
                {
                    if (dag.GetEdge(key, k, out (HashSet<SkolemTerm>, HashSet<SkolemTerm>) e)) values.Add($"$${PrintClause(e.Item1)}\\qquad {PrintClause(e.Item2)}$$");
                    else values.Add("");
                }
                rows.Add($"| {string.Join(" | ", values)} |");
            }
            return string.Join("\n", rows);
        }
    }
}