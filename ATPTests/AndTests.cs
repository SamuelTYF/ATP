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
    public class AndTests
    {
        [TestMethod()]
        public void AndTest()
        {
            And and = new And(
                new Variable("a"),
                new Variable("b"),
                new Implies(new Variable("a"), new Variable("b"))
                );
            Assert.AreEqual(and.Terms[0].Deep, 2);
        }
    }
}