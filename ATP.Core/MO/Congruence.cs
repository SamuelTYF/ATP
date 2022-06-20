namespace ATP.Core.MO
{
    public class Congruence
    {
        public Sort Sort;
        public int[] Father;
        public Congruence(Sort s)
        {
            Sort = s;
            Father = new int[s.Terms.Count];
            Array.Fill(Father, -1);
        }
        public int Find(int x) => Father[x] < 0 ? x : Find(Father[x]);
        public void Union(int x, int y)
        {
            x = Find(x);
            y = Find(y);
            if (x == y) return;
            Father[x] = y;
        }
    }
}
