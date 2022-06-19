namespace ATP.Core.PL
{
    public class SLD_Test
    {
        static void Main(string[] args)
        {
            SLD_System system = new();
            State state = new();
            int p1 = system.GetVariable("p_1");
            int p2 = system.GetVariable("p_2");
            int p3 = system.GetVariable("p_3");
            int p4 = system.GetVariable("p_4");
            state.Register(system.GetHorn(p3));
            state.Register(system.GetHorn(p4));
            state.Register(system.GetHorn(p1, p3, p4));
            state.Register(system.GetHorn(p2, p3));
            state.Register(system.GetGoal(p1, p2));
            Console.WriteLine(system.Format(state));
            system.Test(state);
            Console.WriteLine(state.Mode);
            Console.WriteLine(system.Trace(state));

        }
    }
}
