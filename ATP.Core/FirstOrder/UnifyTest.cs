namespace ATP.Core.FirstOrder
{
    public class UnifyTest
    {
        static void Main(string[] args)
        {
            SkolemSystem system = new();
            SkolemTerm a = system.Call("F", system.GetVariable("x"), system.Call("F", system.GetVariable("x"), system.GetVariable("y")));
            SkolemTerm b = system.Call("F", system.Call("G",system.GetVariable("y")), system.Call("F", system.Call("G", system.GetConstant("a")), system.GetVariable("z")));
            SkolemTerm c = system.Call("F", system.Call("G", system.GetVariable("y")), system.Call("F", system.Call("G", system.GetConstant("a")), system.Call("H", system.GetConstant("a"))));
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);
            Console.WriteLine(system.Combine(new SkolemTerm[] { a, b },new SkolemTerm[] { c},out SkolemTerm[] r1,out SkolemTerm[] r2,out SkolemTerm unify));
            Console.WriteLine(r1[0]);
            Console.WriteLine(r1[1]);
            Console.WriteLine(r2[0]);
            Console.WriteLine(unify);
        }
    }
}
