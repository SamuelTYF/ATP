using Microsoft.VisualStudio.TestTools.UnitTesting;
using ATP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP.Tests
{
    [TestClass()]
    public class SequentCalculusTests
    {
        [TestMethod()]
        public void ReduceTest()
        {
            ITerm term = new Implies(new Or(new Implies(new Variable("p"), new Variable("r")), new Implies(new Variable("q"), new Variable("r"))), new Implies(new And(new Variable("p"), new Variable("q")), new Variable("r")));
            SequentCalculus result = new SequentCalculus();
            result.RegisterRight(term);
            result.Reduce();
            Assert.AreEqual(result.Mode, Mode.Success);
            Console.WriteLine(result.FormatPrint());
        }
    }
}