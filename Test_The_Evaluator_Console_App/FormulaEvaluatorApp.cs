using System;
using System.Collections.Generic;

namespace FormulaEvaluator
{
    class FormulaEvaluatorApp
    {
        static void Main(string[] args)
        {          
            
            //README: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-6.0
            List<String> formulas = new List<string>();

            //add formulas

            //simple aritmetic
            formulas.Add("1+2");  // = 3
            formulas.Add("2-1");  // = 1
            formulas.Add("2*2");  // = 4


            // longer formulas
            formulas.Add("1+2+3+4+5+6+7+8+9"); // = 45
            formulas.Add("100-10-50+50+10"); // = 100
            formulas.Add("1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1+1"); // = 30


            // complex formulas
            formulas.Add("(1-2*5)+(3*(2+3))/(3*1)*3");  // = 6
            formulas.Add("(((3+2)/5)+1)"); // 2
            formulas.Add("1/0"); // divide by zero error
            formulas.Add("1*2/2+9-10*10");
            formulas.Add("1+a1-b2+g5");
            formulas.Add("4*0)5)+13");
            formulas.Add("((4*0)5)+13");
            formulas.Add("");
            formulas.Add("2^2");  

            foreach (String formula in formulas)
            {
                try
                {

                    Console.WriteLine("formula: " + formula + " = " + FormulaEvaluator.Evaluate(formula, fakeLookup) + "\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid formula: " + formula + " because .... " + e.Message + "\n");
                    continue;
                }
            }

        }

        public static int fakeLookup(String cellname)
        {
            return 1;
        }
    }
}
