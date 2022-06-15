namespace ATP.Core.PL
{
    public class Term : ITerm
    {
        public IOperator Operator;
        public ITerm[] Terms;
        public Term(IOperator @operator, ITerm[] terms, int deep, int index, IOperator[] operators) : base(deep, index, operators)
        {
            Operator = @operator;
            Terms = terms;
            //if (@operator.Count != terms.Length) throw new Exception();
            for (int i = 0; i < terms.Length; i++)
                terms[i].OperatorBacks[@operator.Index].BackInfos[i].Add(this);
        }
        public override string ToString() => $"{Operator.Name}[{string.Join<ITerm>(",", Terms)}]";
    }
}
