using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Formula f = new Formula("2+2", s => s.ToUpper(), s => true);
            Assert.AreEqual(4, (double)f.Evaluate(s => 0), 1e-9);
        }
    }
}
