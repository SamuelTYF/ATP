namespace ATP.Core
{
    public class Program
    {
        static void Main(string[] args)
        {
            GentzenSystem system = new();
            string source = "And[And[Or[P,Not[Q]],Or[Not[P],Not[S]]],And[Or[S,Not[Q]],Q]";
            ITerm term = system.Parse(source);
            CNFSystem csys = new();
            CNF cnf = system.ToCNF(csys, term, true);
            Console.WriteLine(csys.Format(cnf));
            csys.Test(cnf);
        }
    }
}
