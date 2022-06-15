using ATP.Core.PL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ATP.Core.Tests
{
    [TestClass()]
    public class FormalSystemTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            Operator and = new(2, "And");
            Operator or = new(2, "Or");
            Operator not = new(1, "Not");
            Operator implies = new(2, "Implies");
            FormalSystem system = new(and, or, not, implies);
            string source = "Implies[Or[Implies[p,r],Implies[q,r]],Implies[And[p,q],r]]";
            ITerm term=system.Parse(source);
            Assert.AreEqual(term.ToString(), source);
        }
    }
}