using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace SS
{
    [TestClass]
    public class SpreadsheetTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents_WhenNameIsNull_ThrowInvalidNameExpection()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }
    }
}
