namespace ATP.Core
{
    public class Program
    {
        static void Main(string[] args)
        {
            GentzenSystem system = new();
            string source = "Implies[And[Or[A,P],Or[B,Not[P]]],Or[A,B]]";
            ITerm term = system.Parse(source);
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            system.Test(sequent);
            Console.WriteLine(system.Trace(sequent));
        }
    }
}
