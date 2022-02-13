using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SS
{
    [TestClass]
    public class SpreadsheetTests
    {
        // TESTS FOR GetCellContents

        /// <summary>
        /// Tests the spreadsheet's GetCellContents method when a cell's content is a string.
        /// </summary>
        [TestMethod]
        public void GetCellContents_WhenTheContentsIsAString_Text()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "Hey guys!");
            Object contents = s.GetCellContents("A1");
            Assert.AreEqual(contents, "Hey guys!");
        }

        /// <summary>
        /// Tests the spreadsheet's GetCellContents method when a cell's content is a formula.
        /// </summary>
        [TestMethod]
        public void GetCellContents_WhenTheContentsIsAFormula_Formula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("1+1");
            s.SetCellContents("A1", f);
            Object contents = s.GetCellContents("A1");
            Assert.AreEqual(contents, "1+1");
        }

        /// <summary>
        /// Tests the spreadsheet's GetCellContents method when a cell's content is a double/number.
        /// </summary>
        [TestMethod]
        public void GetCellContents_WhenTheContentsIsADouble_Number()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 3.2);
            Object contents = s.GetCellContents("A1");
            Assert.AreEqual(contents, 3.2);
        }

        /// <summary>
        /// Tests the spreadsheet's GetCellContents method when a cell's content is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents_WhenNameIsNull_ThrowInvalidNameExpection()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        /// <summary>
        /// Tests the spreadsheet's GetCellContents method when a cell's name is invalid.
        /// Test variation 1 of 3 other tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents_WhenNameIsInvalidTest01_ThrowInvalidNameExpection()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1a");
        }

        /// <summary>
        /// Tests the spreadsheet's GetCellContents method when a cell's name is invalid.
        /// Test variation 2 of 3 other tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents_WhenNameIsInvalidTest02_ThrowInvalidNameExpection()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("!");
        }

        /// <summary>
        /// Tests the spreadsheet's GetCellContents method when a cell's name is invalid.
        /// Test variation 3 of 3 other tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents_WhenNameIsInvalidTest03_ThrowInvalidNameExpection()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("A1A");
        }

        // TESTS FOR GetNamesOfAllNonemptyCells

        /// <summary>
        /// Tests the spreadsheet's GetNamesOfAllNonemptyCells method when there are no cells.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetNamesOfAllNonemptyCells_WhenThereAreZeroCells_NumberOfCellsEqualsZero()
        {
            Spreadsheet s = new Spreadsheet();
            int numberOfCells = 0;
            IEnumerable<string> cells = s.GetNamesOfAllNonemptyCells();
            foreach(String cell in cells)
            {
                numberOfCells++;
            }
            Assert.AreEqual(numberOfCells, 0);
        }

        /// <summary>
        /// Tests the spreadsheet's GetNamesOfAllNonemptyCells method when there is only one cell.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetNamesOfAllNonemptyCells_OnlyOneCell_A1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "I'm the only non empty cell in the spreadsheet!");
            int numberOfCells = 0;
            IEnumerable<string> cells = s.GetNamesOfAllNonemptyCells();
            foreach (String cell in cells)
            {
                numberOfCells++;
            }
            Assert.AreEqual(numberOfCells, 1);
        }

        /// <summary>
        /// Tests the spreadsheet's GetNamesOfAllNonemptyCells method when there are multiple cells.
        /// If successfull, the test contains a secret message!
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetNamesOfAllNonemptyCells_WhenThereAreMultipleCells_ListOfCellNames()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "Hello, ");
            s.SetCellContents("A2", "how ");
            s.SetCellContents("A3", "are ");
            s.SetCellContents("A4", "you ");
            s.SetCellContents("A5", "today?  ");
            s.SetCellContents("A6", "I ");
            s.SetCellContents("A7", "wish ");
            s.SetCellContents("A8", "you ");
            s.SetCellContents("A9", "the ");
            s.SetCellContents("A10", "best ");
            s.SetCellContents("A11", "this ");
            s.SetCellContents("A12", "world ");
            s.SetCellContents("A13", "has ");
            s.SetCellContents("A14", "to ");
            s.SetCellContents("A15", "offer!");
            int numberOfCells = 0;
            StringBuilder SecretMessage = new StringBuilder();
            IEnumerable<string> cells = s.GetNamesOfAllNonemptyCells();
            foreach (String cell in cells)
            {
                numberOfCells++;
                SecretMessage.Append(cell);

            }
            Assert.AreEqual(numberOfCells, 15);
            Assert.AreEqual(SecretMessage, "Hello, how are you today?  I wish you the best this world has to offer!");
        }

        // TESTS FOR SetCellContents
        
        /// <summary>
        /// Tests the spreadsheet's SetCellContent method when a cell is given a number.
        /// </summary>
        [TestMethod]
        public void SetCellContent_GiveANumber_A1()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> list = s.SetCellContents("A1", 1);
            Assert.IsTrue(list.Contains("A1"));
        }

        /// <summary>
        /// Tests the spreadsheet's SetCellContent method when a cell is given a string/text.
        /// </summary>
        [TestMethod]
        public void SetCellContent_Givetext_A1()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> list = s.SetCellContents("A1", "Hello");
            Assert.IsTrue(list.Contains("A1"));

        }

        /// <summary>
        /// Tests the spreadsheet's SetCellContent method when a cell is given a formula.
        /// </summary>
        [TestMethod]
        public void SetCellContent_GiveAFormula_A1()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("1+1");
            IList<string> list = s.SetCellContents("A1", f);
            Assert.IsTrue(list.Contains("A1"));

        }

        /// <summary>
        /// Tests the spreadsheet's SetCellConents method with
        /// multiple formulas, creating a network of dependents amongst
        /// the various cells.
        /// </summary>
        [TestMethod]
        public void SetCellContent_MultipleFormulas_CellwithDependents()
        {
            Spreadsheet s = new Spreadsheet();
            double contentOfA1 = 1;
            Formula contentOfB1 = new Formula("A1+1");
            Formula contentOfC1 = new Formula("B1+1");
            Formula contentOfD1 = new Formula("A1+B1+C1+1");

            IList<string> A1 = s.SetCellContents("A1", contentOfA1);
            IList<string> B1 = s.SetCellContents("B1", contentOfB1);
            IList<string> C1 = s.SetCellContents("C1", contentOfC1);
            IList<string> D1 = s.SetCellContents("D1", contentOfD1);

            Assert.AreEqual(A1.ToString(), "A1, B1, C1, D1");
        }

        // if a cellname is null

        /// <summary>
        /// Tests the spreadsheet's SetCellContents method to see if it will
        /// throw an InvalidNameException when the name is null and a number
        /// is given.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents_CellnameIsNullWithNumber_InvalidNameException()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, 1);
        }

        /// <summary>
        /// Tests the spreadsheet's SetCellContents method to see if it will
        /// throw an InvalidNameException when the name is null and a string 
        /// is given.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents_CellnameIsNullWithText_InvalidNameException()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, "null name...");
        }

        /// <summary>
        /// Tests the spreadsheet's SetCellContents method to see if it will
        /// throw an InvalidNameException when the name is null and a formula
        /// is given.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents_CellnameIsNullWithFormula_InvalidNameException()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("1+1");
            s.SetCellContents(null, f);
        }

        // if a cellname is invalid

        /// <summary>
        /// Tests the spreadsheet's SetCellContents method to see if it will
        /// throw an InvalidNameException when you give it an invalid name and
        /// a number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents_CellnameIsInvalidWithNumber_InvalidNameException()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A!", 1);
        }

        /// <summary>
        /// Tests the spreadsheet's SetCellContents method to see if it will
        /// throw an InvalidNameException when you give it an invalid name and 
        /// a string.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents_CellnameIsInvalidWithText_InvalidNameException()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("!a", "error");
        }

        /// <summary>
        /// Tests the spreadsheet's SetCellContents method to see if it will
        /// throw an InvalidNameException when you give it an invalid name with
        /// a formula.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents_CellnameIsInvalidWithFormula_InvalidNameException()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("1+1");
            s.SetCellContents("j2j2", f);
        }

        // if a circular dependency occurrs (self reference)

        /// <summary>
        /// Tests the spreadsheet's SetCellContents method to see if it will
        /// throw a CircularException when a cell tries to reference itself.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents_SelfReferencingCell_CircularException()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("1+A1");
            s.SetCellContents("A1", f);
        }

        // if a circular dependency occurs (linked chain of cells)

        /// <summary>
        /// Tests the spreadsheet's SetCellContents method to see if 
        /// if throws a CircularExeption when it tries to make a chain of
        /// cells that reference the first and end one together, thus 
        /// creating a circular dependency.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents_LinkedChainOfCells_CircularException()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("B1");
            Formula f2 = new Formula("C1");
            Formula f3 = new Formula("A1");

            s.SetCellContents("A1", f1);
            s.SetCellContents("B1", f2);
            s.SetCellContents("C1", f3);
        }

            // TESTS FOR GetDirectDependents



        }  
}      
       