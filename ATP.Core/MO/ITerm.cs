namespace ATP.Core.MO
{
    public class ITerm
    {
        public int Index;
        public Sort Sort;
        public ITerm[] Childrens;
        public List<ITerm> Parents;
        public ITerm(int index,Sort sort, ITerm[] childrens)
        {
            Index = index;
            Sort = sort;
            Childrens = childrens;
            Parents = new();
            foreach (ITerm child in childrens)
                child.Parents.Add(this);
        }
    }
}
