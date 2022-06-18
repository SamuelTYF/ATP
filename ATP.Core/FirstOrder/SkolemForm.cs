using System.Xml.Linq;

namespace ATP.Core.FirstOrder
{
    public class SkolemForm
    {
        public List<SkolemVariable> Variables;
        public List<SkolemConstant> Constants;
        public SkolemClauses Clauses;
        public SkolemForm(List<SkolemVariable> variables, List<SkolemConstant> constants, SkolemClauses clauses)
        {
            Variables = variables;
            Constants = constants;
            Clauses = clauses;
        }
    }
    public class SkolemClauses
    {
        public HashSet<int> Clauses;
        public SkolemClauses() => Clauses = new();
    }
}
