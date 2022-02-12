using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {

        private Dictionary<string, Cell> Cells = new Dictionary<string, Cell>();
        private DependencyGraph CellNetwork = new DependencyGraph();

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            if (name is null) throw new InvalidNameException();
            //TODO: throw exception if name is invalid

            if (Cells.TryGetValue(name, out Cell cell)) return cell.GetContentsOfCell();
            else return "";  // All other cells are empty by default.   
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return Cells.Keys;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
    }

    public class Cell
    {
        private String text;
        private Formula formula;
        private Double number;

        /// <summary>
        /// Initializes a cell when its contents are just text.
        /// </summary>
        /// <param name="content"></param>
        public Cell(String content)
        {
            this.text = content;
            this.formula = null;
        }

        /// <summary>
        /// Initializes a cell when its contents are a formula.
        /// </summary>
        /// <param name="content"></param>
        public Cell(Formula content)
        {
            this.text = null;
            this.formula = content;
        }

        /// <summary>
        /// Initializes a cell when its contents are a number.
        /// </summary>
        public Cell(Double content)
        {
            this.text = null;
            this.formula = null;
            this.number = content;
        }

        /// <summary>
        /// Retrieves the contents of a particular cell.
        /// </summary>
        /// <returns></returns>
        public object GetContentsOfCell()
        {
            if (text != null && text.Length > 0) return text;
            else if (formula is object) return formula;
            else return number;
        }
    }

}
