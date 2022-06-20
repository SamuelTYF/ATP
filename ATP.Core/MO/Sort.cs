namespace ATP.Core.MO
{
    public class Sort
    {
        public int Index;
        public string Name;
        public List<ITerm> Terms;
        public List<Operator> Operators;
        public Dictionary<string, int> OperatorMap;
        public Dictionary<string, int> Variables;
        public Dictionary<string, int> Constants;
        public Sort(int index,string name)
        {
            Index = index;
            Name = name;
            Terms = new();
            Operators = new();
            OperatorMap = new();
            Variables = new();
            Constants = new();
        }
        public Operator GetOperator(string name, params Sort[] parameters)
        {
            if (parameters.Length == 0) throw new Exception();
            if (!OperatorMap.ContainsKey(name))
            {
                Operator @operator = new(Operators.Count, name, this, parameters);
                OperatorMap[name] = Operators.Count;
                Operators.Add(@operator);
                return @operator;
            }
            else
            {
                Operator @operator = Operators[OperatorMap[name]];
                if (@operator.Return != this || @operator.Parameters != parameters) throw new Exception();
                return @operator;
            }
        }
        public ITerm GetConstant(string name)
        {
            if (!Constants.ContainsKey(name))
            {
                Constant constant = new(Terms.Count, this, name);
                Constants[name] = Terms.Count;
                Terms.Add(constant);
                return constant;
            }
            else return Terms[Constants[name]];
        }
        public ITerm GetVariable(string name)
        {
            if (!Constants.ContainsKey(name))
            {
                Variable variable = new(Terms.Count, this, name);
                Variables[name] = Terms.Count;
                Terms.Add(variable);
                return variable;
            }
            else return Terms[Variables[name]];
        }
        public ITerm Call(Operator @operator,params ITerm[] parameters)
        {
            if (@operator.Parameters.Length != parameters.Length) throw new Exception();
            for (int i = 0; i < parameters.Length; i++)
                if (@operator.Parameters[i] != parameters[i].Sort) throw new Exception();
            if (@operator.Return != this) throw new Exception();
            HashSet<ITerm> terms = new(parameters[0].Parents);
            for (int i = 1; i < parameters.Length; i++)
                terms.IntersectWith(parameters[i].Parents);
            List<Functor> fs = new(terms.Select(term => term as Functor).Where(func => func != null && func.Operator == @operator));
            if (fs.Count > 1) throw new Exception();
            else if(fs.Count==0)
            {
                Functor f = new(Terms.Count, @operator, parameters);
                Terms.Add(f);
                fs.Add(f);
            }
            return fs[0];
        }
    }
}
