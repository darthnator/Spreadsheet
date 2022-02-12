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

        // TESTS FOR SetCellContents- numbers

        
        // TESTS FOR SetCellContents- text

        
        // TESTS FOR SetCellContents- formula


        // TESTS FOR GetDirectDependents


    }
}
