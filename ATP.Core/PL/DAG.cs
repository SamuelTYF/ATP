namespace ATP.Core.PL
{
    public interface IDAG<TKey, TVertex, TEdge>
    {
        public IEnumerable<TKey> Keys { get; }
        public IEnumerable<TVertex> Values { get; }
        public TVertex GetVertex(TKey key);
        public void RegisterVertex(TKey key, TVertex value);
        public void RegisterEdge(TKey from, TKey to, TEdge value);
        public bool GetEdge(TKey from, TKey to, out TEdge? edge);
    }
}
