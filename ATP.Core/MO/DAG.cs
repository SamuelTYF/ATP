namespace ATP.Core.MO
{
    public class DAG
    {
        public List<Sort> Sorts;
        public Dictionary<string, int> SortMap;
        public DAG()
        {
            Sorts = new();
            SortMap = new();
        }
        public Sort GetSort(string name)
        {
            if(!SortMap.ContainsKey(name))
            {
                Sort s = new(Sorts.Count, name);
                SortMap[name] = Sorts.Count;
                Sorts.Add(s);
                return s;
            }
            return Sorts[SortMap[name]];
        }
    }
}
