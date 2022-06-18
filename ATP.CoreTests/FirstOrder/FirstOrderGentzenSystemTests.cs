using Microsoft.VisualStudio.TestTools.UnitTesting;
using ATP.Core.FirstOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP.Core.FirstOrder.Tests
{
    [TestClass()]
    public class FirstOrderGentzenSystemTests
    {
        [TestMethod()]
        [JsonDataSource("FirstOrderGentzenSystemTests.json", "SearchingTest")]
        public void SearchingTest(string source,Mode mode, string[] operators)
        {
            FirstOrderGentzenSystem system = new();
            foreach(string op in operators)
            {
                string[] ss = op.Split("_");
                system.GetOperator(ss[0], int.Parse(ss[1]));
            }
            Term t = system.Parse(source);
            Assert.IsNotNull(t);
            //Console.WriteLine(system.Format(t));
            //Console.WriteLine(string.Join(",", t.BoundLiterals));
            //Console.WriteLine(string.Join(",", t.Literals));
            FiniteSequent sequent = new();
            sequent.RegisterRight(t.Index);
            system.Test(sequent);
            Console.WriteLine(system.Trace(sequent));
            Assert.AreEqual(sequent.Mode, mode);
        }
    }
}