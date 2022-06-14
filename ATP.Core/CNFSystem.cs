namespace ATP.Core
{
    public class CNFSystem
    {
        public List<CNF.Literal> Literals;
        public Dictionary<string,CNF.Literal> LiteralMap;
        public CNFSystem()
        {
            Literals = new();
            LiteralMap = new();
        }
        public CNF.Literal Literal(string name,bool @true)
        {
            if(!LiteralMap.ContainsKey(name))
            {
                CNF.Literal t = new(name, Literals.Count, true);
                Literals.Add(t);
                CNF.Literal f = new(name, Literals.Count, false);
                Literals.Add(f);
                LiteralMap[name] = t;
                t.Mirror = f;
                f.Mirror = t;
            }
            return @true ? LiteralMap[name] : LiteralMap[name].Mirror;
        }
        public string Format(CNF cnf)
            => string.Join(" , ", cnf.Values.Select(clause => $"\\{{{string.Join(" , ", clause)}\\}}"));
    }
}
