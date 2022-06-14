namespace ATP.Core
{
    public class CNF
    {
        public class Literal
        {
            public string Name;
            public int Index;
            public bool True;
            public Literal Mirror;
            public Literal(string name,int index, bool @true)
            {
                Name = name;
                Index = index;
                True = @true;
            }
            public override string ToString() => True ? Name : $"\\lnot {Name}";
        }
        public List<HashSet<Literal>> Values;
        public CNF() => Values = new();
        public static CNF operator&(CNF a,CNF b)
        {
            CNF cnf = new();
            cnf.Values.AddRange(a.Values);
            cnf.Values.AddRange(b.Values);
            return cnf;
        }
        public static CNF operator|(CNF a, CNF b)
        {
            CNF cnf = new();
            foreach (HashSet<Literal> t1 in a.Values)
            {
                HashSet<Literal> t = new();
                bool success = false;
                foreach (Literal l1 in t1)
                {
                    t.Add(l1);
                    if (t.Contains(l1.Mirror))
                    {
                        success = true;
                        break;
                    }
                }
                if (success) continue;
                foreach (HashSet<Literal> t2 in b.Values)
                {
                    HashSet<Literal> r = new(t);
                    success = false;
                    foreach(Literal l2 in t2)
                    {
                        r.Add(l2);
                        if (r.Contains(l2.Mirror))
                        {
                            success = true;
                            break;
                        }
                    }
                    if (success) continue;
                    cnf.Values.Add(r);
                }
            }
            return cnf;
        }
    }
}
