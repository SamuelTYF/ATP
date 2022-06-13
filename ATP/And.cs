using System;

namespace ATP
{
    public class And:ITerm
    {
        public ITerm[] Terms;
        public int Deep { get; private set; }
        public And(params ITerm[] terms)
        {
            Terms = terms;
            Array.Sort(Terms, (a, b) => b.Deep.CompareTo(a.Deep));
            Deep = terms[0].Deep + 1;
        }
        public int CompareTo(ITerm other)
        {
            if (other is And and)
            {
                if (Deep != and.Deep) return Deep > and.Deep ? 1 : -1;
                if (Terms.Length != and.Terms.Length) return Terms.Length > and.Terms.Length ? 1 : -1;
                for(int i=0;i<Terms.Length;i++)
                {
                    int r = Terms[i].CompareTo(and.Terms[i]);
                    if (r != 0) return r;
                }
                return 0;
            }
            else return -1;
        }
        public override string ToString() => string.Join<ITerm>(" \\wedge ",Terms);
    }
}
