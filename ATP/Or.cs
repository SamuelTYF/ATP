using System;

namespace ATP
{
    public class Or : ITerm
    {
        public ITerm[] Terms;
        public int Deep { get; private set; }
        public Or(params ITerm[] terms)
        {
            Terms = terms;
            Array.Sort(Terms, (a, b) => b.Deep.CompareTo(a.Deep));
            Deep = terms[0].Deep + 1;
        }
        public int CompareTo(ITerm other)
        {
            if (other is Or or)
            {
                if (Deep != or.Deep) return Deep > or.Deep ? 1 : -1;
                if (Terms.Length != or.Terms.Length) return Terms.Length > or.Terms.Length ? 1 : -1;
                for (int i = 0; i < Terms.Length; i++)
                {
                    int r = Terms[i].CompareTo(or.Terms[i]);
                    if (r != 0) return r;
                }
                return 0;
            }
            else return -1;
        }
        public override string ToString() => string.Join<ITerm>(" \\vee ", Terms);
    }
}
