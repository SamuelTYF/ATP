using ATP.Core.PL;

namespace ATP.Core.FirstOrder
{
    public class Program
    {
        static void Main()
        {
            FirstOrderGentzenSystem system = new();
            system.GetOperator("P", 2);
            Term t=system.Parse("Exist[y,Forall[z,And[Or[Not[P[z,y]],Not[Exist[x,And[P[z,x],P[x,z]]]]],Or[Exist[x,And[P[z,x],P[x,z]]],P[z,y]]]]]");
            Console.WriteLine(system.Format(t));
            //Console.WriteLine(string.Join(",", t.BoundLiterals));
            //Console.WriteLine(string.Join(",", t.Literals));
            //FiniteSequent sequent = new();
            //sequent.RegisterRight(t.Index);
            //system.Test(sequent,10);
            //Console.Clear();
            //Console.WriteLine(system.Trace(sequent));
            PrenexForm pf = system.ToPrenexForm(t);
            //Console.Clear();
            Console.WriteLine(system.Format(pf));
            SkolemSystem ssystem = new();
            SkolemForm sf= ssystem.FromPrenexForm(pf,system);
            Console.WriteLine(ssystem.Format(sf));
            DAG_Dictionary<int, HashSet<SkolemTerm>, (HashSet<SkolemTerm>, HashSet<SkolemTerm>)> dag = new();
            Console.WriteLine(ssystem.Test(sf.Clauses, dag, out string tree));
            Console.WriteLine(tree);
            Console.WriteLine(ssystem.PrintTable(dag));
        }
    }
}
