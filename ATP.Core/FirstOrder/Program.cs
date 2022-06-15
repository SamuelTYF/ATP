namespace ATP.Core.FirstOrder
{
    public class Program
    {
        static void Main(string[] args)
        {
            FirstOrderGentzenSystem system = new();
            system.GetOperator("Q", 1);
            Term t=system.Parse("Implies[Exist[_x,Implies[P,Q[_x]]],Implies[P,Exist[_z,Q[_z]]]]");
            Console.WriteLine(system.Format(t));
            Console.WriteLine(string.Join(",", t.BoundLiterals));
            Console.WriteLine(string.Join(",", t.Literals));
        }
    }
}
