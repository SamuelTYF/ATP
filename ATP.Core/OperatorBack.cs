namespace ATP.Core
{
    public class OperatorBack
    {
        public int Count;
        public HashSet<ITerm>[] BackInfos;
        public OperatorBack(int count)
        {
            Count = count;
            BackInfos = new HashSet<ITerm>[count];
            for(int i = 0; i < count; i++)
                BackInfos[i] = new();
        }
    }
}
