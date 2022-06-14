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
            public HashSet<int> Back;
            public Literal(string name,int index, bool @true)
            {
                Name = name;
                Index = index;
                True = @true;
                Back = new();
            }
            public override string ToString() => True ? Name : $"\\lnot {Name}";
        }
        public HashSet<int> Values;
        public CNF() => Values = new();
    }
}
