namespace ATP
{
    public class Variable:ITerm
    {
        public string Name;
        public int Deep => 1;
        public Variable(string name) => Name = name;
        public int CompareTo(ITerm other)
        {
            if (other is Variable variable)
                return Name.CompareTo(variable.Name);
            else return -1;
        }
        public override string ToString() => Name;
    }
}
