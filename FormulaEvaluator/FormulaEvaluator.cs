using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Breaks a formula down token by token and performs the arithmetic as it goes.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns>A whole number</returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            // README: Regex substring provided by instructor https://utah.instructure.com/courses/754704/assignments/10141496?module_item_id=16487969
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            String validVariableNames = @"[a-zA-Z]+[0-9]+";
            // README: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1?view=net-6.0
            Stack<string> operations = new Stack<string>();
            Stack<int> values = new Stack<int>();

            if (expression == "")
            {
                throw new ArgumentException("Bruh, give us something to work with.");
            }

            // Iterate through each token in substrings and perform arithmetic as needed.
            foreach(String t in substrings)
            {
                string token = t.Trim();
                if (token.Length == 0)
                {
                    continue;
                }
                // token is an integer
                // README: https://stackoverflow.com/questions/1752499/c-sharp-testing-to-see-if-a-string-is-an-integer/1752505
                if (int.TryParse(token, out int number))
                {
                    if(operations.Count > 0 && (operations.Peek() == "*" || operations.Peek() == "/"))
                    {
                        
                        int a = values.Pop(); // Retreive the other value needed to perform the arimetic

                        // for multiplication
                        if (operations.Peek() == "*")
                        {
                            values.Push(Multiply(a, number));
                        }
                        // for division
                        else
                        {
                            values.Push(Divide(a, number));
                        }
                        operations.Pop();

                    }
                    else
                    {
                        values.Push(number);
                    }
                }

                // token is a variable
                // README: https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.ismatch?view=net-6.0
                else if (Regex.IsMatch(token, validVariableNames))
                {
                    if (operations.Peek() == "*" || operations.Peek() == "/")
                    {
                        try
                        {
                            int a = values.Pop(); // Retreive the other value needed to perform the arimetic

                            // for multiplication
                            if (operations.Peek() == "*")
                            {
                                values.Push(Multiply(a, variableEvaluator(token)));
                            }
                            // for division
                            else values.Push(Divide(a, variableEvaluator(token)));
                            operations.Pop();
                        }
                        catch (Exception)
                        {
                            throw new Exception("There are no values in the values stack...");
                        }

                    }
                    else
                    {
                        values.Push(variableEvaluator(token));
                    }
                }

                // token is + or -
                else if (token == "+" || token == "-")
                {
                    if (operations.Count > 0 && (operations.Peek() == "+" || operations.Peek() == "-"))
                    {
                        
                        int b = values.Pop();
                        int a = values.Pop();
                        // for addition
                        if (operations.Peek() == "+")
                        {
                            values.Push(Add(a, b));
                        }
                        // for subtraction
                        else
                        {
                            values.Push(Subtract(a, b));
                        }

                    }
                    operations.Push(token);
                }

                // token is * or /
                else if (token == "*" || token == "/")
                {
                    operations.Push(token);
                }

                // token is a left parenthesis "("
                else if (token == "(")
                {
                    operations.Push(token);
                }

                // token is a right parenthesis ")"
                else if (token == ")")
                {
                    if (operations.Count > 0 && (operations.Peek() == "+" || operations.Peek() == "-"))
                    {

                        int b = values.Pop();
                        int a = values.Pop();
                        // for addition
                        if (operations.Peek() == "+")
                        {
                            values.Push(Add(a, b));
                        }
                        // for subtraction
                        else
                        {
                            values.Push(Subtract(a, b));
                        }


                    operations.Pop();
                    }

                    // get rid of the left parenthesis
                    if (operations.Count > 0  && operations.Peek() == "(")
                    {
                        operations.Pop();
                    }
                    else
                    {
                        throw new ArgumentException("There wasn't an accompanying left parenthesis to your right parenthesis...");
                    }

                    // Step three: * or /
                    if (operations.Count > 0 && (operations.Peek() == "*" || operations.Peek() == "/"))
                    {
                        try
                        {
                            int b = values.Pop();
                            int a = values.Pop();
                            // for multiplication
                            if (operations.Peek() == "*")
                            {
                                values.Push(Multiply(a, b));
                            }
                            // for Divide
                            else values.Push(Divide(a, b));
                            operations.Pop();
                        }
                        catch
                        {
                            throw new ArgumentException("something went wrong when you tried to read a + or -...");
                        }

                    }

                }

                // token is anything else
                else throw new ArgumentException("This formula can not be evaluated because there is an invalid token: " + token);
            }

            // Now that the we've gone through each token in our formula, it's time to return the evaluated value...

            // Operator stack is empty
            if (operations.Count == 0)
            {
                if (values.Count == 1)
                {
                    return values.Pop();
                }
                else throw new ArgumentException("Why doesn't values.Count equal 1?");

            }
            // Operator stack is not empty
            else
            {
                // verify that there is only one operator on the operator stack and that it is + or -.
                if (operations.Peek() != "+" && operations.Peek() != "-") throw new Exception("Why is there not a plus or minus in the operations stack?");
                // verif that there are exactly two values on the values stack.
                if (values.Count != 2)
                {
                    throw new Exception("Why is there not exactly two values in the value stack?");
                }

                // perform the arithmetic on the 
                int b = values.Pop();
                int a = values.Pop();

                if (operations.Peek() == "+")
                {
                    return Add(a, b);
                }
                else return Subtract(a, b);
            }

        }

        /// <summary>
        /// Multiplies two numbers together.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="number"></param>
        /// <returns>A whole number</returns>
        private static int Multiply(int a, int number)
        {
            return a * number;
        }

        /// <summary>
        /// Divides two numbers together.  
        /// </summary>
        /// <param name="a"></param>
        /// <param name="number"></param>
        /// <returns>A whole number or a DivideByZeroException</returns>
        private static int Divide(int a, int number)
        {
            if (number == 0)
            {
                throw new DivideByZeroException("This formula can not be evaluated because it divides by zero.");
            }
            else return a / number;
        }

        /// <summary>
        /// Adds two numbers together.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>A whole number</returns>
        private static int Add(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Subtracts two numbers together.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>A whole number</returns>
        private static int Subtract(int a, int b)
        {
            return a - b;
        }
    }
}
