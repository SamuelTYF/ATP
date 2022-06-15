namespace ATP.Core.FirstOrder
{
    public class FirstOrderGentzenSystem:FirstOrderSystem
    {
        public Operator And;
        public Operator Or;
        public Operator Implies;
        public Operator Forall;
        public Operator Exist;
        public FirstOrderGentzenSystem():base()
        {
            And = GetOperator("And", 2);
            Or = GetOperator("Or", 2);
            Implies = GetOperator("Implies", 2);
            Forall = GetOperator("ForAll", 2);
            Exist = GetOperator("Exist", 2);
        }
        public string Format(Term term)
        {
            if (term is Literal literal) return literal.ToString();
            else if (term is BoundLiteral boundliteral) return boundliteral.ToString();
            else if (term is Functor functor)
            {
                string text;
                if (functor.Operator == And) text = $"{Format(functor.Terms[0])} \\wedge {Format(functor.Terms[1])}";
                else if (functor.Operator == Or) text = $"{Format(functor.Terms[0])} \\vee {Format(functor.Terms[1])}";
                else if (functor.Operator == Implies) text = $"({Format(functor.Terms[0])})\\to({Format(functor.Terms[1])})";
                else if (functor.Operator == Forall) text = $"\\forall {Format(functor.Terms[0])},({Format(functor.Terms[1])})";
                else if (functor.Operator == Exist) text = $"\\exist {Format(functor.Terms[0])},({Format(functor.Terms[1])})";
                else text = $"{functor.Operator.Name}({string.Join(",", functor.Terms.Select(term => Format(term)))})";
                return functor.True ? text : $"\\lnot ({text})";
            }
            else throw new Exception();
        }
    }
}
