using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ATP.Core.Tests
{
    [TestClass()]
    public class FormalSystemTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            Operator1 and = new(2, "And");
            Operator1 or = new(2, "Or");
            Operator1 not = new(1, "Not");
            Operator1 implies = new(2, "Implies");
            FormalSystem system = new(and, or, not, implies);
            string source = "Implies[Or[Implies[p,r],Implies[q,r]],Implies[And[p,q],r]]";
            ITerm term=system.Parse(source);
            Assert.AreEqual(term.ToString(), source);
        }
    }
}