using static ATP.Core.PL.CNF;

namespace ATP.Core.PL
{
    public class CNFTest
    {
        static void Main(string[] args)
        {
            CNFSystem csys = new();
            CNF cnf = new();
            cnf.Values.Add(csys.Find(csys.Literal("R", false)));
            cnf.Values.Add(csys.Find(csys.Literal("Q", false)));
            cnf.Values.Add(csys.Find(csys.Literal("R", true), csys.Literal("P", false)));
            cnf.Values.Add(csys.Find(csys.Literal("P", true), csys.Literal("Q", true), csys.Literal("R", true)));
            Console.WriteLine(csys.Format(cnf));
            DAG_Dictionary<int, HashSet<Literal>, (HashSet<Literal>, HashSet<Literal>)> dag = new();
            csys.Test(cnf, dag, out string tree);
            Console.Clear();
            Console.WriteLine(csys.PrintTable(dag));
            Console.Clear();
            Console.WriteLine(tree);
        }
    }
}
