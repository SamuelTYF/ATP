namespace ATP.Core.MO
{
    public class Functor:ITerm
    {
        public Operator Operator;
        public Functor(int index, Operator @operator, ITerm[] childrens) : base(index, @operator.Return, childrens)
        {
            Operator = @operator;
        }
    }
}
