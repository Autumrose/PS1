using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

namespace FormulaEvaluatorApp
{
    /// <summary>
    /// This class provides tests for the evaluator.
    ///</summary>
    class Program
    {
        public static int returnVarValue(string variable)
        {
            if (variable == "X1")
            {
                return 1;
            } else
            {
                throw new ArgumentException();
            }
        }

        static Lookup varEvaluator = new Lookup(returnVarValue);

        static void Main(string[] args)
        {
            // TestPlus
            if (Evaluator.Evaluate("2+2", varEvaluator) == 4)
            {
                Console.WriteLine("Testing plus: PASSED");
            } else
            {
                Console.WriteLine("Testing plus: FAILED");
            }

            // TestPlusWithWhitespace
            if (Evaluator.Evaluate("2 + 2", varEvaluator) == 4)
            {
                Console.WriteLine("Testing plus with whitespace: PASSED");
            }
            else
            {
                Console.WriteLine("Testing plus with whitespace: FAILED");
            }

            // TestMinus
            if (Evaluator.Evaluate("4-2", varEvaluator) == 2)
            {
                Console.WriteLine("Testing plus: PASSED");
            }
            else
            {
                Console.WriteLine("Testing plus: FAILED");
            }

            // TestPlusWithVariable
            if (Evaluator.Evaluate("2+X1", varEvaluator) == 3)
            {
                Console.WriteLine("Testing plus with variable: PASSED");
            }
            else
            {
                Console.WriteLine("Testing plus with variable: FAILED");
            }

            // TestInvalidVariable
            try {
                Evaluator.Evaluate("2+AB", varEvaluator);
                Console.WriteLine("Testing invalid variable error: FAILED");
            } catch (ArgumentException)
            {
                Console.WriteLine("Testing invalid variable error: PASSED");
            }

            // TestMultiply
            if (Evaluator.Evaluate("2*2", varEvaluator) == 4)
            {
                Console.WriteLine("Testing multiply: PASSED");
            }
            else
            {
                Console.WriteLine("Testing multiply: FAILED");
            }

            // TestDivide
            if (Evaluator.Evaluate("4/2", varEvaluator) == 2)
            {
                Console.WriteLine("Testing divide: PASSED");
            }
            else
            {
                Console.WriteLine("Testing divide: FAILED");
            }

            // TestDivideByZero
            try
            {
                Evaluator.Evaluate("2/0", varEvaluator);
                Console.WriteLine("Testing divide by 0 error: FAILED");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Testing divide by 0 error: PASSED");
            }

            // TestParentheses
            if (Evaluator.Evaluate("(4/2)+2", varEvaluator) == 4)
            {
                Console.WriteLine("Testing parentheses: PASSED");
            }
            else
            {
                Console.WriteLine("Testing parentheses: FAILED");
            }

            // TestInvalidExpression
            try
            {
                Evaluator.Evaluate("2++", varEvaluator);
                Console.WriteLine("Testing invalid expression error: FAILED");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Testing invalid expression error: PASSED");
            }

            Console.ReadLine();
        }
       
    }
}
