namespace ATP.Core.PL
{
    public class DAG_Dictionary<TKey, TVertex, TEdge> : IDAG<TKey, TVertex, TEdge>
    {
        public Dictionary<TKey, TVertex> Vertexs;
        public Dictionary<TKey, Dictionary<TKey, TEdge>> Edges;
        public IEnumerable<TKey> Keys => Vertexs.Keys;
        public IEnumerable<TVertex> Values => Vertexs.Values;
        public DAG_Dictionary()
        {
            Vertexs = new();
            Edges = new();
        }
        public TVertex GetVertex(TKey key) => Vertexs[key];
        public void RegisterVertex(TKey key, TVertex value)
        {
            Vertexs[key] = value;
            Edges[key] = new();
        }
        public void RegisterEdge(TKey from, TKey to, TEdge value)
            => Edges[from][to] = value;
        public bool GetEdge(TKey from, TKey to, out TEdge? edge)
        {
            if (Edges[from].ContainsKey(to))
            {
                edge = Edges[from][to];
                return true;
            }
            else
            {
                edge = default;
                return false;
            }
        }
    }
}
