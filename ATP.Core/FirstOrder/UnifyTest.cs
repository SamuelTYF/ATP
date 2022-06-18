namespace ATP.Core.FirstOrder
{
    public class UnifyTest
    {
        static void Main(string[] args)
        {
            SkolemSystem system = new();
            SkolemTerm a = system.Call("F", system.GetVariable("x"), system.Call("F", system.GetVariable("x"), system.GetVariable("y")));
            SkolemTerm b = system.Call("F", system.Call("G",system.GetVariable("y")), system.Call("F", system.Call("G", system.GetConstant("a")), system.GetVariable("z")));
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(system.Unify(a, b, out SkolemTerm ra, out SkolemTerm rb));
            Console.WriteLine(ra);
            Console.WriteLine(rb);

        }
    }
}
