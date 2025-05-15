using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Expression exp = new Expression("((2*2+1^2)+2)*3^2-(4)*5");
            float total = exp.Evaluate(0);
            Console.WriteLine(total);
            //int a = 0, b = 0;
            //Console.WriteLine(" - Calculator - ");
            //Console.WriteLine("Type an expression with many numbers");
            //string input_str = Console.ReadLine();
            //string[] parts = Console.ReadLine().Split(' '); // problem here being that you don't know the size.
            //// Input Check
            //for (int i = 0; i < parts.Length; i++){
            //    Console.WriteLine($"{parts[i]}");

            //}



            //Console.WriteLine("\nLoop a different way");
            //foreach (string i in parts)
            //{
            //    Console.WriteLine(i);
            //}

            //Console.WriteLine(a);

        }
    }

    public class Expression
    {
        public static string[] oplist;
        public string expression;

        static Expression()
        {
            oplist = new string[] { "+-", "*/", "^", " " };
        }

        static void PrintSteps(string input) { if (false) { Console.WriteLine(input); } }

        public Expression(string expression)
        {
            this.expression = expression;
        }

        public float Evaluate(int curr_op_Index)
        {
            if (NumCheck(this))
            {
                return float.Parse(this.expression);
            }

            string curr_op = oplist[curr_op_Index];
            PrintSteps($"Evaluating {this.expression} for {curr_op}");
            float total = 0;
            bool first = true;
            int brackets = 0;
            string sub_expression = "";
            Expression new_expression;
            char recent_op = curr_op[0];
            string expr = this.expression + recent_op;


            for (int i = 0; i < expr.Length; i++)
            {
                PrintSteps($"{expr[i]}");
                bool operated = false;

                if (expr[i] == '(') { brackets++; sub_expression += expr[i]; continue; } // checks if whole expression is in brackets
                else if (expr[i] == ')') { brackets--; sub_expression += expr[i]; continue; }
                if (brackets > 0) { sub_expression += expr[i]; continue; }

                foreach (char op in curr_op)
                {
                    if (op == expr[i])
                    {
                        operated = true;
                        PrintSteps($"New Sub Expression: {sub_expression}");
                        new_expression = new Expression(sub_expression);

                        if (curr_op_Index == 2 && sub_expression[0] == '(')
                        {
                            new_expression = new Expression(sub_expression.Substring(1, sub_expression.Length - 2));
                            return new_expression.Evaluate(0);
                        }

                        if (first)
                        {
                            total = new_expression.Evaluate(curr_op_Index + 1);
                            PrintSteps($"Total So Far: {total}");
                            first = false;
                        }
                        else
                        {
                            total = new_expression.Operate(recent_op, total, curr_op_Index);
                            PrintSteps($"Total So Far: {total}, Hmm");
                        }
                        sub_expression = "";
                    }
                }

                if (operated)
                {
                    recent_op = expr[i];
                }
                else
                {
                    sub_expression += expr[i];
                }
            }
            PrintSteps($"End of Evaluation, Total So Far: {total}, For: {this.expression} and Operation: {curr_op}\n");

            return total;


        }

        public float Operate(char operation, float total, int curr_op_index)
        {
            float curr;
            if (NumCheck(this))
            {
                curr = float.Parse(this.expression);
            }
            else
            {
                curr = Evaluate(curr_op_index + 1);
            }
            switch (operation)
            {
                case '+':
                    PrintSteps($"Adding {this.expression}");
                    return total + curr;
                case '-':
                    PrintSteps($"Subtracting {this.expression}");
                    return total - curr;
                case '*':
                    PrintSteps($"Multiplying {this.expression}");
                    return total * curr;
                case '/':
                    PrintSteps($"Dividing {this.expression}");
                    return total / curr;
                case '^':
                    PrintSteps($"Exponent {this.expression}");
                    return (float) Math.Pow(total, curr);
                case ' ':
                    PrintSteps($"Parse {this.expression}");
                    return float.Parse(this.expression);
            }
            return 0;

        }

        public bool NumCheck(Expression expression)
        {
            float value;
            return float.TryParse(expression.expression, out value);
        }
    }
}
