using System.Text;

namespace ATP.Core.FirstOrder
{
    public class FirstOrderSystem
    {
        public List<Term> Terms;
        public Dictionary<string, Literal> Literals;
        public Dictionary<string, BoundLiteral> BoundLiterals;
        public List<Operator> Operators;
        public Dictionary<string, Operator> OperatorMap;
        public FirstOrderSystem()
        {
            Terms = new();
            Literals = new();
            BoundLiterals = new();
            Operators = new();
            OperatorMap = new();
        }
        public Operator GetOperator(string name, int count)
        {
            if (!OperatorMap.ContainsKey(name))
            {
                Operator @operator = new(name, count, Operators.Count);
                OperatorMap[name] = @operator;
                Operators.Add(@operator);
            }
            return OperatorMap[name];
        }
        public Term GetLiteral(string name, bool @true = true)
        {
            if (!Literals.ContainsKey(name))
            {
                Literal t = new(name, true);
                Literals[name] = t;
                Literal f = new(name, false);
                t.Mirror = f;
                f.Mirror = t;
            }
            return @true ? Literals[name] : Literals[name].Mirror;
        }
        public Term GetBoundLiteral(string name, bool @true = true)
        {
            if (!BoundLiterals.ContainsKey(name))
            {
                BoundLiteral t = new(name, true);
                BoundLiterals[name] = t;
                BoundLiteral f = new(name, false);
                t.Mirror = f;
                f.Mirror = t;
            }
            return @true ? BoundLiterals[name] : BoundLiterals[name].Mirror;
        }
        public Term Call(string name, params Term[] terms)
            => Call(GetOperator(name, terms.Length), terms);
        public Term Call(Operator @operator, params Term[] terms)
        {
            HashSet<int> funcs = terms[0].GetBack(@operator.Index, 0);
            for (int i = 1; i < terms.Length; i++)
                funcs.IntersectWith(terms[1].GetBack(@operator.Index, i));
            List<Functor> functors = new(funcs.Select(i => Terms[i] as Functor));
            if (functors.Count > 1) throw new Exception();
            if (functors.Count == 0)
            {
                Functor t = new(@operator, terms, true, Terms.Count);
                Terms.Add(t);
                Functor f = new(@operator, terms, false, Terms.Count);
                Terms.Add(f);
                functors.Add(t);
            }
            return functors[0];
        }
        public Term Parse(string value)
        {
            value = value.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            int index = 0;
            return Parse(value.ToCharArray(), ref index);
        }
        public Term Parse(char[] values, ref int index)
        {
            StringBuilder builder = new();
            for (; index < values.Length && values[index] is not ('[' or ']' or ','); index++)
                builder.Append(values[index]);
            string name = builder.ToString();
            if (index == values.Length || values[index] is ']' or ',')
            {
                if (name.StartsWith("_")) return GetBoundLiteral(name[1..], true);
                else return GetLiteral(name, true);
            }
            else if (values[index] is '[')
            {
                if (name == "Not")
                {
                    index++;
                    Term term = Parse(values, ref index);
                    index++;
                    return term.Mirror;
                }
                else
                {
                    Operator @operator = OperatorMap[name];
                    Term[] terms = new Term[@operator.Count];
                    for (int i = 0; i < terms.Length; i++)
                    {
                        index++;
                        terms[i] = Parse(values, ref index);
                    }
                    index++;
                    return Call(@operator, terms);
                }
            }
            else throw new Exception();
        }
    }
}
