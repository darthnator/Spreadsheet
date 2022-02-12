using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {

        private Dictionary<string, Cell> Cells = new Dictionary<string, Cell>();
        private DependencyGraph connections = new DependencyGraph();
        private String validCellNames = @"[a-zA-Z]+[0-9]+";

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            if (name is null || !IsValidName(name)) throw new InvalidNameException();
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
            if (name == null || !IsValidName(name)) throw new InvalidNameException();

            // in the event that this cell already existed and had other content before, we need to "clean it out"
            CleanOutOldCell(name);
                      
            // We're now ready to add the new cell to Cells
            Cell cell = new Cell(number);
            Cells.Add(name, cell);
            
            // because it's a number, we don't have to worry about dependees (only formulas have dependees)
            // but still we need to worry about potential dependents
            return GetCellAndDependents(name);
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetCellContents(string name, string text)
        {
            if (text == null) throw new ArgumentException();
            if (name == null || !IsValidName(name)) throw new InvalidNameException();

            // in the event that this cell already existed and had other content before, we need to "clean it out"
            CleanOutOldCell(name);

            // We're now ready to add the new cell to Cells
            Cell cell = new Cell(text);
            Cells.Add(name, cell);

            // because it's a number, we don't have to worry about dependees (only formulas have dependees)
            // but still we need to worry about potential dependents
            return GetCellAndDependents(name);
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null) throw new ArgumentException();
            if (name == null || !IsValidName(name)) throw new InvalidNameException();
            if (CreatesCircularDependency(name, formula)) throw new CircularException();

            CleanOutOldCell(name);

            Cell cell = new Cell(formula);
            Cells.Add(name, cell);

            // TODO: take care of potential dependees

            return GetCellAndDependents(name);
            
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return connections.GetDependents(name);
        }

        /// <summary>
        /// Verifies that a cell's name is legitimate and not malformed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsValidName(String name)
        {
            if (Regex.IsMatch(name, validCellNames)) return true;
            else return false;
        }

        /// <summary>
        /// Erases every connect a cell had with all other cells.
        /// </summary>
        /// <param name="name"></param>
        private void CleanOutOldCell(String name)
        {
            if (Cells.ContainsKey(name))
            {
                // the old cell could have had dependents and dependees
                if (connections.HasDependents(name))
                {
                    foreach (String dependent in connections.GetDependents(name))
                    {
                        connections.RemoveDependency(name, dependent);
                    }
                }
                if (connections.HasDependees(name))
                {
                    foreach (String dependee in connections.GetDependees(name))
                    {
                        connections.RemoveDependency(dependee, name);
                    }
                }

            }

            // Now that we've stripped the cell of its old dependents and dependees, we can remove the old cell.
            Cells.Remove(name);
        }

        private bool CreatesCircularDependency(String name, Formula formula)
        {
            foreach(String variable in formula.GetVariables())
            {
                // Case 1: if a formula references itself.  For example: A1 = A1.
                if (variable == name) return true;
                
                // Case 2: check if one of its variables is in 
                foreach (String dependee in connections.GetDependees(variable))
                {
                    if (name == dependee) return true;
                }
            }
            //Case 2: if any of its variables
            return false;
        }

        private IList<string> GetCellAndDependents(String name)
        {
            IList<string> cellAndDependents = new List<string>();

            cellAndDependents.Add(name);

            // Get all the dependents to the cell and include them in the list.
            if (connections.HasDependents(name))
            {
                foreach(String dependent in GetDirectDependents(name))
                {
                    cellAndDependents.Add(dependent);
                }
            }

            return cellAndDependents;
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
