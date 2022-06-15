using static ATP.Core.PL.CNF;

namespace ATP.Core.PL
{
    public class CNFSystem
    {
        public List<Literal> Literals;
        public Dictionary<string, Literal> LiteralMap;
        public List<HashSet<Literal>> Clauses;
        public CNFSystem()
        {
            Literals = new();
            LiteralMap = new();
            Clauses = new();
        }
        public Literal Literal(string name, bool @true)
        {
            if (!LiteralMap.ContainsKey(name))
            {
                Literal t = new(name, Literals.Count, true);
                Literals.Add(t);
                Literal f = new(name, Literals.Count, false);
                Literals.Add(f);
                LiteralMap[name] = t;
                t.Mirror = f;
                f.Mirror = t;
            }
            return @true ? LiteralMap[name] : LiteralMap[name].Mirror;
        }
        public int Find(params Literal[] literals) => Find(new HashSet<Literal>(literals));
        public int Find(HashSet<Literal> raw)
        {
            HashSet<int> cs = new();
            for (int i = 0; i < Clauses.Count; i++)
                if (Clauses[i].Count == raw.Count)
                    cs.Add(i);
            foreach (Literal l in raw)
                cs.IntersectWith(l.Back);
            if (cs.Count > 1) throw new Exception();
            else if (cs.Count == 1) return cs.First();
            else
            {
                int index = Clauses.Count;
                foreach (Literal l in raw)
                    l.Back.Add(index);
                Clauses.Add(raw);
                return index;
            }
        }
        public string Format(CNF cnf)
            => string.Join(" , ", cnf.Values.Select(clause => $"\\{{{string.Join(" , ", Clauses[clause])}\\}}"));
        public CNF And(CNF a, CNF b)
        {
            CNF cnf = new();
            cnf.Values.UnionWith(a.Values);
            cnf.Values.UnionWith(b.Values);
            return cnf;
        }
        public CNF Or(CNF a, CNF b)
        {
            CNF cnf = new();
            foreach (int t1 in a.Values)
            {
                HashSet<Literal> t = new();
                bool success = false;
                foreach (Literal l1 in Clauses[t1])
                {
                    t.Add(l1);
                    if (t.Contains(l1.Mirror))
                    {
                        success = true;
                        break;
                    }
                }
                if (success) continue;
                foreach (int t2 in b.Values)
                {
                    HashSet<Literal> r = new(t);
                    success = false;
                    foreach (Literal l2 in Clauses[t2])
                    {
                        r.Add(l2);
                        if (r.Contains(l2.Mirror))
                        {
                            success = true;
                            break;
                        }
                    }
                    if (success) continue;
                    cnf.Values.Add(Find(r));
                }
            }
            return cnf;
        }
        public bool Test(CNF cnf, IDAG<int, HashSet<Literal>, (HashSet<Literal>, HashSet<Literal>)> dag, out string tree)
        {
            tree = null;
            List<int> clauses = new(cnf.Values);
            List<List<(int, int, HashSet<Literal>)>> parents = new();
            Dictionary<int, int> map = new();
            for (int i = 0; i < clauses.Count; i++)
            {
                map[clauses[i]] = i;
                parents.Add(new());
            }
            for (int i = 0; i < clauses.Count; i++)
            {
                HashSet<Literal> c1 = Clauses[clauses[i]];
                dag.RegisterVertex(clauses[i], c1);
                for (int j = 0; j < i; j++)
                {
                    HashSet<Literal> c2 = Clauses[clauses[j]];
                    HashSet<Literal> u = new();
                    foreach (Literal l in c1)
                        if (c2.Contains(l.Mirror))
                        {
                            u.Add(l);
                            u.Add(l.Mirror);
                            break;
                        }
                    if (u.Count == 0) continue;
                    HashSet<Literal> c = new(c1.Union(c2).Except(u));
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
        public string Trace(int index, List<int> clauses, List<List<(int, int, HashSet<Literal>)>> parents, out HashSet<int> cs)
        {
            List<(int, int, HashSet<Literal>)> ps = parents[index];
            if (ps.Count > 0)
            {
                (int, int, HashSet<Literal>) p = ps[0];
                string s1 = Trace(clauses[p.Item1], clauses, parents, out HashSet<int> cs1);
                string s2 = Trace(clauses[p.Item2], clauses, parents, out HashSet<int> cs2);
                cs = new(cs1.Union(cs2));
                if (s1 == null && s2 == null)
                {
                    s1 = string.Join(",", Clauses[index]) + "\\vdash";
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
        public string PrintClause(HashSet<Literal> clause)
            => clause.Count == 1 ? $"{string.Join(",", clause)}" : $"\\{{{string.Join(",", clause)}\\}}";
        public string PrintTable(IDAG<int, HashSet<Literal>, (HashSet<Literal>, HashSet<Literal>)> dag)
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
                    if (dag.GetEdge(key, k, out (HashSet<Literal>, HashSet<Literal>) e)) values.Add($"$${PrintClause(e.Item1)}\\qquad{PrintClause(e.Item2)}$$");
                    else values.Add("");
                }
                rows.Add($"| {string.Join(" | ", values)} |");
            }
            return string.Join("\n", rows);
        }
    }
}
