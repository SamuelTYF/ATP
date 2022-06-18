namespace ATP.Core.FirstOrder
{
    public class PrenexForm
    {
        public List<bool> Quantifiers;
        public List<BoundLiteral> BoundLiterals;
        public Term Term;
        public PrenexForm(List<bool> quantifiers, List<BoundLiteral> boundLiterals, Term term)
        {
            Quantifiers = quantifiers;
            BoundLiterals = boundLiterals;
            Term = term;
        }
        public PrenexForm Mirror()
        {
            List<bool> qs = new(Quantifiers.Select(q=>!q));
            return new(qs, BoundLiterals, Term.Mirror);
        }
    }
}
