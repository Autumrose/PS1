using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public delegate int Lookup(String v);

    /// <summary>
    /// Evaluate arithmetic expressions using standard infix notation.
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// This function takes an expression as a String and returns the computed value in an int, or throw an ArgumentException if the expression is not computable.
        /// </summary>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            Stack<string> operatorStack = new Stack<string>();
            Stack<int> valueStack = new Stack<int>();

            exp = exp.Trim();
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (String token in substrings)
            {
                if (String.IsNullOrEmpty(token))
                {
                    continue;
                }

                int intValue;
                int computed;
                if (int.TryParse(token, out intValue)) // if token is int
                {
                    if (operatorStack.Count > 0)
                    {
                        if (Equals(operatorStack.Peek(), "*") || Equals(operatorStack.Peek(), "/")) // if operator is * or /
                        {
                            if (valueStack.Count == 0) // value stack empty error
                            {
                                throw new ArgumentException("Value stack is empty!");
                            }

                            string op = operatorStack.Pop();
                            int val = valueStack.Pop();

                            if (intValue == 0 && Equals(op, "/")) // divide by 0 error
                            {
                                throw new ArgumentException();
                            }
                            else
                            {
                                if (Equals(op, "*"))
                                {
                                    computed = val * intValue;
                                    valueStack.Push(computed); // apply *
                                }
                                else if (Equals(op, "/"))
                                {
                                    computed = val / intValue;
                                    valueStack.Push(computed); // apply /
                                }
                            }
                        }
                        else
                        {
                            valueStack.Push(intValue); // else push to value stack
                        }
                    }
                    else
                    {
                        valueStack.Push(intValue); // else push to value stack
                    }
                }
                else if (Regex.IsMatch(token, @"[a-zA-Z]+\d+")) // if token is a variable
                {
                    int varVal = variableEvaluator(token);
                    if (operatorStack.Count > 0)
                    {
                        if (Equals(operatorStack.Peek(), "*") || Equals(operatorStack.Peek(), "/")) // if operator is * or /
                        {
                            if (valueStack.Count == 0) // value stack empty error
                            {
                                throw new ArgumentException("Value stack is empty!");
                            }

                            string op = operatorStack.Pop();
                            int val = valueStack.Pop();

                            if (varVal == 0 && Equals(op, "/")) // divide by 0 error
                            {
                                throw new ArgumentException();
                            }
                            else
                            {
                                if (Equals(op, "*"))
                                {
                                    computed = val * varVal;
                                    valueStack.Push(computed); // apply *
                                }
                                else if (Equals(op, "/"))
                                {
                                    computed = val / varVal;
                                    valueStack.Push(computed); // apply /
                                }
                            }
                        }
                        else
                        {
                            valueStack.Push(varVal); // else push to value stack
                        }
                    }
                    else
                    {
                        valueStack.Push(varVal); // else push to value stack
                    }
                }
                else if (Equals(token, "+") || Equals(token, "-")) // if token is + or -
                {
                    if (operatorStack.Count > 0)
                    {
                        if (Equals(operatorStack.Peek(), "+") || Equals(operatorStack.Peek(), "-"))
                        {
                            if (valueStack.Count < 2) // value stack less than 2
                            {
                                throw new ArgumentException();
                            }

                            string op = operatorStack.Pop();
                            int firstVal = valueStack.Pop();
                            int secondVal = valueStack.Pop();

                            if (Equals(op, "-"))
                            {
                                computed = secondVal - firstVal;
                                valueStack.Push(computed); // apply /
                            }
                            else if (Equals(op, "+"))
                            {
                                computed = firstVal + secondVal;
                                valueStack.Push(computed); // apply /
                            }
                        }
                    }
                    else if (valueStack.Count < 1)
                    {
                        throw new ArgumentException();
                    }
                    operatorStack.Push(token);
                }
                else if (Equals(token, "*") || Equals(token, "/")) // if token is * or /
                {
                    if (valueStack.Count < 1)
                    {
                        throw new ArgumentException();
                    }
                    operatorStack.Push(token);
                }
                else if (Equals(token, "(")) // if token is (
                {
                    operatorStack.Push(token);
                }
                else if (Equals(token, ")")) // if token is ) 
                {
                    if (operatorStack.Count > 0)
                    {
                        if (Equals(operatorStack.Peek(), "+") || Equals(operatorStack.Peek(), "-"))
                        {
                            if (valueStack.Count < 2) // value stack less than 2
                            {
                                throw new ArgumentException();
                            }

                            string op = operatorStack.Pop();
                            int firstVal = valueStack.Pop();
                            int secondVal = valueStack.Pop();

                            if (Equals(op, "-"))
                            {
                                computed = secondVal - firstVal;
                                valueStack.Push(computed); // apply /
                            }
                            else if (Equals(op, "+"))
                            {
                                computed = firstVal + secondVal;
                                valueStack.Push(computed); // apply /
                            }

                            if (operatorStack.Count < 1)
                            {
                                throw new ArgumentException();
                            }
                            else if (!operatorStack.Pop().Equals("("))
                            {
                                throw new ArgumentException();
                            }
                            else if (operatorStack.Count > 0)
                            {
                                if (Equals(operatorStack.Peek(), "*") || Equals(operatorStack.Peek(), "/"))
                                {
                                    if (valueStack.Count < 2) // value stack less than 2
                                    {
                                        throw new ArgumentException();
                                    }

                                    string op2 = operatorStack.Pop();
                                    int firstVal2 = valueStack.Pop();
                                    int secondVal2 = valueStack.Pop();

                                    if (Equals(op2, "*"))
                                    {
                                        computed = firstVal2 * secondVal2;
                                        valueStack.Push(computed); // apply *
                                    }
                                    else if (Equals(op2, "/"))
                                    {
                                        computed = secondVal2 / firstVal2;
                                        if (firstVal2 == 0)
                                        {
                                            throw new ArgumentException();
                                        }
                                        valueStack.Push(computed); // apply /
                                    }

                                }
                            }
                        }else if (!operatorStack.Pop().Equals("("))
                        {
                            throw new ArgumentException();
                        }
                    }
                }
                else // token did not match any acceptable conditions
                {
                    throw new ArgumentException();
                }
            }

            // After all tokens are processed
            if (operatorStack.Count == 0)
            {
                if (valueStack.Count != 1)
                {
                    throw new ArgumentException();
                }
                else
                {
                    return valueStack.Pop();
                }
            }
            else
            {
                if ((operatorStack.Count == 1 && valueStack.Count != 2) || (operatorStack.Count != 1 && valueStack.Count == 2))
                {
                    throw new ArgumentException();
                }
                else
                {
                    string op1 = operatorStack.Pop();
                    int val1 = valueStack.Pop();
                    int val2 = valueStack.Pop();

                    if (Equals(op1, "-"))
                    {
                        return val2 - val1;
                    }
                    else if (Equals(op1, "+"))
                    {
                        return val2 + val1;
                    }
                }
            }

            return 0;
        }
    }
}

