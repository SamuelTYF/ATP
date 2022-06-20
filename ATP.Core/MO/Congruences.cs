namespace ATP.Core.MO
{
    public class Congruences
    {
        public DAG Graph;
        public Congruence[] Sorts;
        public Congruences(DAG dag)
        {
            Graph = dag;
            Sorts = new Congruence[dag.Sorts.Count];
            for (int i = 0; i < Sorts.Length; i++)
                Sorts[i] = new(dag.Sorts[i]);
        }
        public ITerm Find(ITerm u)
        {
            Sort s = u.Sort;
            return s.Terms[Sorts[s.Index].Find(u.Index)];
        }
        public void Union(ITerm u,ITerm v)
        {
            Sort s = u.Sort;
            Sorts[s.Index].Union(u.Index, v.Index);
        }
        public bool Congruent(ITerm u,ITerm v)
        {
            if (u == v) return true;
            if (u.Sort != v.Sort) return false;
            if (Find(u) == Find(v)) return true;
            if (u is Functor fu && v is Functor fv)
            {
                if (fu.Operator != fv.Operator) return false;
                for (int i = 0; i < fu.Childrens.Length; i++)
                    if (!Congruent(fu.Childrens[i],fv.Childrens[i])) return false;
                Merge(u, v);
                return true;
            }
            else return false;
        }
        public void Merge(ITerm u,ITerm v)
        {
            if(Find(u)!=Find(v))
            {
                Union(u, v);
                foreach (ITerm x in u.Parents)
                    foreach (ITerm y in v.Parents)
                        if (Find(x) != Find(y) && Congruent(x, y))
                            Merge(x, y);
            }
        }
    }
}
