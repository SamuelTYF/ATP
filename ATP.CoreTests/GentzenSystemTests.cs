using Microsoft.VisualStudio.TestTools.UnitTesting;
using ATP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP.Core.Tests
{
    [TestClass()]
    public class GentzenSystemTests
    {
        [TestMethod()]
        public void SuccessTest()
        {
            GentzenSystem system = new();
            string source = "Implies[Or[Implies[p,r],Implies[q,r]],Implies[And[p,q],r]]";
            ITerm term = system.Parse(source);
            Assert.AreEqual(term.ToString(), source);
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            system.Test(sequent);
            Console.WriteLine(system.Trace(sequent));
            Assert.AreEqual(sequent.Mode, Mode.Success);
        }
        [TestMethod()]
        public void FailTest()
        {
            GentzenSystem system = new();
            string source = "Implies[Or[Implies[p,r],Implies[q,r]],Implies[Or[p,q],r]]";
            ITerm term = system.Parse(source);
            Assert.AreEqual(term.ToString(), source);
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            system.Test(sequent);
            Console.WriteLine(system.Trace(sequent));
            Assert.AreEqual(sequent.Mode, Mode.Fail);
        }
        [TestMethod()]
        public void FastSuccessTest()
        {
            GentzenSystem system = new();
            string source = "Implies[Or[Implies[p,r],Implies[q,r]],Implies[And[p,q],r]]";
            ITerm term = system.Parse(source);
            Assert.AreEqual(term.ToString(), source);
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            FiniteSequent next = new();
            foreach (int left in sequent.Left)
                system.InsertLeft(next, system.Terms[left]);
            foreach (int right in sequent.Right)
                system.InsertRight(next, system.Terms[right]);
            sequent.Nodes = new();
            sequent.Nodes.Add(next);
            system.FastTest(next);
            sequent.Mode = next.Mode;
            Console.WriteLine(system.Trace(sequent));
            Assert.AreEqual(sequent.Mode, Mode.Success);
        }
        [TestMethod()]
        public void FastFailTest()
        {
            GentzenSystem system = new();
            string source = "Implies[Or[Implies[p,r],Implies[q,r]],Implies[Or[p,q],r]]";
            ITerm term = system.Parse(source);
            Assert.AreEqual(term.ToString(), source);
            FiniteSequent sequent = new();
            sequent.RegisterRight(term.Index);
            FiniteSequent next = new();
            foreach (int left in sequent.Left)
                system.InsertLeft(next, system.Terms[left]);
            foreach (int right in sequent.Right)
                system.InsertRight(next, system.Terms[right]);
            sequent.Nodes = new();
            sequent.Nodes.Add(next);
            system.FastTest(next);
            sequent.Mode = next.Mode;
            Console.WriteLine(system.Trace(sequent));
            Assert.AreEqual(sequent.Mode, Mode.Fail);
        }
    }
}