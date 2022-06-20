namespace ATP.Core.MO
{
    public class Constant:ITerm
    {
        public string Name;
        public Constant(int index, Sort sort, string name):base(index,sort,Array.Empty<ITerm>())
        {
            Name = name;
        }
    }
}
