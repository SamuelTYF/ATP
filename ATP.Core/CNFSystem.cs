using static ATP.Core.CNF;

namespace ATP.Core
{
    public class CNFSystem
    {
        public List<Literal> Literals;
        public Dictionary<string,Literal> LiteralMap;
        public List<HashSet<Literal>> Clauses;
        public CNFSystem()
        {
            Literals = new();
            LiteralMap = new();
            Clauses = new();
        }
        public Literal Literal(string name,bool @true)
        {
            if(!LiteralMap.ContainsKey(name))
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
        public int Find(HashSet<Literal> raw)
        {
            HashSet<int> cs = new();
            for (int i = 0; i < Clauses.Count; i++)
                if (Clauses[i].Count == raw.Count)
                    cs.Add(i);
            foreach(Literal l in raw)
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
        public bool Test(CNF cnf)
        {
            List<int> clauses = new(cnf.Values);
            for(int i=1;i<clauses.Count;i++)
                for(int j=0;j<i;j++)
                {
                    HashSet<Literal> c1 = Clauses[clauses[i]];
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
                    if (c.Count == 0) return false;
                    int p = Find(c);
                    if (!clauses.Contains(p)) clauses.Add(p);
                }
            return true;
        }
    }
}
