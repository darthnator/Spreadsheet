using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        // UnitOfWork_StateUnderTest_ExpectedBehavior

        [TestMethod]
        public void EvaluateMethod_SimpleArithmeticEquation01_four()
        {
            Formula f = new Formula("2+2", s => s.ToUpper(), s => true);
            Assert.AreEqual(4, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void EvaluateMethod_SimpleArithmeticEquation02_DecimalValue()
        {
            Formula f = new Formula("5*5+2/4", s => s.ToUpper(), s => true);
            Assert.AreEqual(25.5, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void EvaluateMethod_MoreComplexEqutation01_IntegerValue()
        {
            Formula f = new Formula("((5+2)*2+6)/10+(64/8)", s => s.ToUpper(), s => true);
            Assert.AreEqual(10, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void EvaluateMethod_MoreComplexEquation02_NumericValue()
        {
            Formula f = new Formula("(5+2-1/2*10)+100", s => s.ToUpper(), s => true);
            Assert.AreEqual(102, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void EvalutateMethod_ValidVariables01_NumericValue()
        {
            Formula f = new Formula("(a1+b1)+5", s => s.ToUpper(), s => true);
            Assert.AreEqual(7, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod]
        public void EvaluateMethod_ValidVariables02_NumbericValue()
        {
            Formula f = new Formula("(5+2-B24/2*10)+100", s => s.ToUpper(), s => true);
            Assert.AreEqual(102, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule1SpecialCharacters01_FormulaFormatException()
        {
            Formula f = new Formula("5!", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule1SpecialCharacters02_FormulaFormatException()
        {
            Formula f = new Formula("2^6", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule1SpecialCharacters03_FormulaFormatException()
        {
            Formula f = new Formula("1+5#", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule1SpecialCharacters04_FormulaFormatException()
        {
            Formula f = new Formula("1.5+1.5&", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule2AtLeastOneToken_FormulaFormatException()
        {
            Formula f = new Formula("", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule3RightParentheses_FormulaFormatException()
        {
            Formula f = new Formula("(3+4)+5)", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule4BalancedParentheses_FormulaFormatException()
        {
            Formula f = new Formula("((3+4)+5", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule5StaringToken01_FormulaFormatException()
        {
            Formula f = new Formula("+5-2", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule5StaringToken02_FormulaFormatException()
        {
            Formula f = new Formula("-5-2", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule5StaringToken03_FormulaFormatException()
        {
            Formula f = new Formula("/5-2", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule5StaringToken04_FormulaFormatException()
        {
            Formula f = new Formula("*5+2", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule5StaringToken05_FormulaFormatException()
        {
            Formula f = new Formula(")5-2", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule6EndingToken01_FormulaFormatException()
        {
            Formula f = new Formula("1+1+", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule6EndingToken02_FormulaFormatException()
        {
            Formula f = new Formula("1+1-", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule6EndingToken03_FormulaFormatException()
        {
            Formula f = new Formula("1+1/", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule6EndingToken04_FormulaFormatException()
        {
            Formula f = new Formula("1+1*", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule6EndingToken05_FormulaFormatException()
        {
            Formula f = new Formula("1+1(", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule7Following01_FormulaFormatException()
        {
            Formula f = new Formula("(+5+6)", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule7Following02_FormulaFormatException()
        {
            Formula f = new Formula("5(*6)", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule7Following03_FormulaFormatException()
        {
            Formula f = new Formula("(5+6+)1", s => s.ToUpper(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_Rule7Following04_FormulaFormatException()
        {
            Formula f = new Formula("(/5+6)", s => s.ToUpper(), s => true);
        }


      
    }
}
