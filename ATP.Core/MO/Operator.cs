namespace ATP.Core.MO
{
    public class Operator
    {
        public int Index;
        public string Name;
        public Sort[] Parameters;
        public Sort Return;
        public Operator(int index, string name, Sort @return, Sort[] parameters)
        {
            Index = index;
            Name = name;
            Parameters = parameters;
            Return = @return;
        }
        public ITerm this[params ITerm[] terms]
            =>Return.Call(this,terms);
    }
}
