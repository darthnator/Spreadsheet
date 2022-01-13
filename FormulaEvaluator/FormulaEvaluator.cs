using System;
using System.Text.RegularExpressions;
/// <summary>
/// Author: Nathan Hammond
/// Partner: Not aplicable
/// Date: 1/12/2022
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Nathan Hammond - This work may not be copied for use in Academic Coursework.
/// 
/// I, Nathan Hammond, certify that I wrote this code from scratch and did not copy it in part or whole from another source.  All references used in the completion of the assignment are cited in my README file.
/// 
/// File Contents
/// 
/// This is an algorithm that takes a an arithmetic expression (or formula) and evaluates it following the starndard infix notation.  
/// </summary>
/// 

namespace FormulaEvaluator
{
    public class FormulaEvaluator
    {
        public delegate int Lookup(String cellname);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            int result=0;
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            return result;
        }
    }
}
