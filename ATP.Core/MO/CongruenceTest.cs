namespace ATP.Core.MO
{
    public class CongruenceTest
    {
        static void Main()
        {
            DAG dag = new();
            Sort s_bool = dag.GetSort("bool");
            Operator f = s_bool.GetOperator("f", s_bool);
            Operator p = s_bool.GetOperator("p", s_bool);
            ITerm @true = s_bool.GetConstant("true");
            ITerm a = s_bool.GetVariable("a");
            ITerm f3 = f[f[f[a]]];
            ITerm f5 = f[f[f[f[f[a]]]]];
            ITerm pa = p[a];
            ITerm pfa = p[f[a]];
            Congruences cs = new(dag);
            cs.Merge(f3, a);
            cs.Merge(f5, a);
            cs.Merge(pa, @true);
            Console.WriteLine(cs.Congruent(f3, f[f3]));
            Console.WriteLine(cs.Congruent(pfa,@true));
        }
    }
}
