﻿// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private Func<string, string> normalize;
        private Func<string, bool> isValid;
        private String formula;
        private IEnumerable<String> tokens;
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            this.normalize = normalize;
            this.isValid = isValid;
            this.formula = formula;
            
            tokens = GetTokens(formula);

            String ValidTokens = @"[a-zA-Z0-9\+\-\/\*\(\)\.]";
            String ValidScientificNotation = @"[+-] ?\d(\.\d +)?[Ee][+-] ?\d +";  // Credit: https://regexlib.com/Search.aspx?k=scientific%20notation&AspxAutoDetectCookieSupport=1
            String ValidVariable = @"[a-zA-Z]+[0-9]+";
            String ValidStartingTokens = @"[\(a-zA-Z0-9]+";
            String ValidLastTokens = @"[\)a-zA-Z0-9]";
            String LeftParenthesisOrOperator = @"[\(\+\-\*\/]";
            //String LeftOrRightParenthesisOrOperator = @"[\)\(\+\-\*\/]";
            String RightParenthesisOrOperator = @"[\)\+\-\*\/]";
            String NumberVariableOrLeft = @"[0-9a-zA-Z\(]+";
            String NumberVariableOrRight = @"[0-9a-zA-Z\)]+";
            int LeftParentheses = 0;
            int RightParentheses = 0;
            //int NumberOfTokens = 0;
            String LastToken = "";

            
            foreach(String token in tokens)
            {
                // Rule 1: Specific Token Rule
                if (!Regex.IsMatch(token, ValidTokens) && !Regex.IsMatch(token, ValidScientificNotation)) throw new FormulaFormatException(token + " is an invalid token.");

                // Rule 3: Right Parentheses Rule
                if (token == ")") RightParentheses++;
                if (token == "(") LeftParentheses++;
                if (RightParentheses > LeftParentheses) throw new FormulaFormatException("The number of right parentheses cannot be greater than the left parentheses.");

                // Rule 7: Parenthesis/Operator Following Rule
                if (LastToken != "" && Regex.IsMatch(LastToken, LeftParenthesisOrOperator) && !Regex.IsMatch(token, NumberVariableOrLeft)) throw new FormulaFormatException("A left parenthesis or any operator must be followed by a number, variable, or a left parenthesis.");

                // Rule 8: Extra Following Rule
                if (LastToken != "" && Regex.IsMatch(LastToken, NumberVariableOrRight) && !Regex.IsMatch(token, RightParenthesisOrOperator)) throw new FormulaFormatException("A number, variable, or right parenthesis must be followed by right parenthesis or any operator.");

                LastToken = token;
            }

            // Rule 2: One Token Rule
            if (tokens.Count() == 0) throw new FormulaFormatException("There must be at least one token.");

            // Rule 4: Balanced Parentheses Rule
            if (LeftParentheses != RightParentheses) throw new FormulaFormatException("The total number of left parentheses must equal the total number of right parentheses.");

            // Rule 5: Starting Token Rule
            if (!Regex.IsMatch(tokens.ElementAt(0), ValidStartingTokens)) throw new FormulaFormatException("The first token of an expression must be a number, a variable, or an opening parenthesis.");

            // Rule 6: Ending Token Rule
            if (!Regex.IsMatch(LastToken, ValidLastTokens) && !Regex.IsMatch(LastToken, ValidVariable)) throw new FormulaFormatException("The last token must be a number, variable, or a right parentheses.");

        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            // README: Regex substring provided by instructor https://utah.instructure.com/courses/754704/assignments/10141496?module_item_id=16487969
            string[] substrings = Regex.Split(this.formula, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            // String validVariableNames = @"[a-zA-Z]+[0-9]+";
            // README: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1?view=net-6.0
            Stack<string> operations = new Stack<string>();
            Stack<double> values = new Stack<double>();

            if (this.formula == "")
            {
                throw new ArgumentException("Bruh, give us something to work with.");
            }

            // Iterate through each token in substrings and perform arithmetic as needed.
            foreach (String t in substrings)
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
                    if (operations.Count > 0 && (operations.Peek() == "*" || operations.Peek() == "/"))
                    {

                        double a = values.Pop(); // Retreive the other value needed to perform the arimetic

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
                else if (Char.IsLetter(token[0]))
                {
                    if (this.isValid(token))
                    {
                        if (operations.Peek() == "*" || operations.Peek() == "/")
                        {
                            try
                            {
                                double a = values.Pop(); // Retreive the other value needed to perform the arimetic

                                // for multiplication
                                if (operations.Peek() == "*")
                                {
                                    values.Push(Multiply(a, lookup(normalize(token))));
                                }
                                // for division
                                else values.Push(Divide(a, lookup(normalize(token))));
                                operations.Pop();
                            }
                            catch (Exception)
                            {
                                throw new Exception("There are no values in the values stack...");
                            }

                        }
                        else
                        {
                            values.Push(lookup(normalize(token)));
                        }
                    }
                    else return new FormulaError("There is an invalid varialbe.");
                        
                }

                // token is + or -
                else if (token == "+" || token == "-")
                {
                    if (operations.Count > 0 && (operations.Peek() == "+" || operations.Peek() == "-"))
                    {

                        double b = values.Pop();
                        double a = values.Pop();
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

                        double b = values.Pop();
                        double a = values.Pop();
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
                    if (operations.Count > 0 && operations.Peek() == "(")
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
                            double b = values.Pop();
                            double a = values.Pop();
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
                double b = values.Pop();
                double a = values.Pop();

                if (operations.Peek() == "+")
                {
                    return Add(a, b);
                }
                else return Subtract(a, b);
            }

        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            String ValidVariable = @"[a-zA-Z]+[0-9]+";
            foreach (string token in GetTokens(this.formula))
            {
                if (Regex.IsMatch(token, ValidVariable)) yield return this.normalize(token);
            }
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return this.formula;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Formula)) return false;
            else return this.ToString() == obj.ToString();
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (f1 is null) return f2 is null;
            else return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1.Equals(f2));
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// Multiplies two numbers together.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="number"></param>
        /// <returns>A whole number</returns>
        private static double Multiply(double a, double number)
        {
            return a * number;
        }

        /// <summary>
        /// Divides two numbers together.  
        /// </summary>
        /// <param name="a"></param>
        /// <param name="number"></param>
        /// <returns>A whole number or a DivideByZeroException</returns>
        private static double Divide(double a, double number)
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
        private static double Add(double a, double b)
        {
            return a + b;
        }

        /// <summary>
        /// Subtracts two numbers together.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>A whole number</returns>
        private static double Subtract(double a, double b)
        {
            return a - b;
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
