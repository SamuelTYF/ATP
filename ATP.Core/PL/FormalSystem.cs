using System.Text;

namespace ATP.Core.PL
{
    public class FormalSystem
    {
        public IOperator[] Operators;
        public Dictionary<string, IOperator> OperatorMap;
        public List<ITerm> Terms;
        public Dictionary<string, int> Variables;
        public FormalSystem(params IOperator[] operators)
        {
            Operators = operators;
            OperatorMap = new();
            for (int i = 0; i < operators.Length; i++)
            {
                operators[i].Index = i;
                OperatorMap[operators[i].Name] = operators[i];
            }
            Terms = new();
            Variables = new();
        }
        public ITerm Variable(string name)
        {
            if (!Variables.ContainsKey(name))
            {
                Variables[name] = Terms.Count;
                Terms.Add(new Variable(name, Terms.Count, Operators));
            }
            return Terms[Variables[name]];
        }
        public ITerm Call(string name, params ITerm[] terms)
            => Call(OperatorMap[name], terms);
        public ITerm Call(IOperator @operator, params ITerm[] terms)
        {
            if (@operator.Count != terms.Length) throw new Exception();
            HashSet<int> all = new(terms[0].OperatorBacks[@operator.Index].BackInfos[0].Select(info => info.Index));
            for (int i = 0; i < terms.Length; i++)
                all.IntersectWith(terms[i].OperatorBacks[@operator.Index].BackInfos[i].Select(info => info.Index));
            if (all.Count > 1) throw new Exception();
            if (all.Count == 0)
            {
                all.Add(Terms.Count);
                int d = terms.Select(term => term.Deep).Max() + 1;
                Terms.Add(new Term(@operator, terms, d, Terms.Count, Operators));
            }
            return Terms[all.First()];
        }
        public ITerm Parse(string value)
        {
            value = value.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            int index = 0;
            return Parse(value.ToCharArray(), ref index);
        }
        public ITerm Parse(char[] values, ref int index)
        {
            StringBuilder builder = new();
            for (; index < values.Length && values[index] is not ('[' or ']' or ','); index++)
                builder.Append(values[index]);
            string name = builder.ToString();
            if (index == values.Length || values[index] is ']' or ',') return Variable(name);
            else if (values[index] is '[')
            {
                IOperator @operator = OperatorMap[name];
                ITerm[] terms = new ITerm[@operator.Count];
                for (int i = 0; i < terms.Length; i++)
                {
                    index++;
                    terms[i] = Parse(values, ref index);
                }
                index++;
                return Call(@operator, terms);
            }
            else throw new Exception();
        }
    }
}
