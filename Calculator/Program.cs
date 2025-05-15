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
            Expression exp = new Expression("3*4+3*2");
            float total = exp.Evaluate("+-");
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
            oplist = new string[] { "+-", "*/", "^", "" };
        }

        static void PrintSteps(string input) { if (true) { Console.WriteLine(input); } }

        public Expression(string expression)
        {
            this.expression = expression;
        }

        public float Evaluate(string curr_op)
        {
            PrintSteps($"Evaluating {this.expression} for {curr_op}");

            float total = 0;
            bool first = true;
            string sub_expression = "";
            Expression new_expression = new Expression(sub_expression);
            char recent_op = curr_op[0];
            for (int i = 0; i < this.expression.Length; i++)
            {
                PrintSteps($"{this.expression[i]}");
                bool operated = false;
                foreach (char op in curr_op)
                {
                    if (op == this.expression[i])
                    {
                        operated = true;
                        PrintSteps($"{sub_expression}");
                        new_expression = new Expression(sub_expression);

                        if (first)
                        {
                            if (NumCheck(new_expression)){
                                total = float.Parse(new_expression.expression);
                            }
                            else
                            {
                                total = new_expression.Operate('+', 0);
                            }
                            PrintSteps($"Total So Far: {total}");
                            first = false;
                        }
                        else
                        {
                            if (NumCheck(new_expression))
                            {
                                total = float.Parse(new_expression.expression);
                            }
                            else
                            {
                                total = new_expression.Operate(recent_op, total);
                            }
                            PrintSteps($"Total So Far: {total}");
                        }
                        sub_expression = "";
                    }
                }

                if (operated)
                {
                    recent_op = this.expression[i];
                }
                else
                {
                    sub_expression += this.expression[i];
                }
            }
            PrintSteps($"{sub_expression}");
            new_expression = new Expression(sub_expression);

            if (NumCheck(new_expression))
            {
                total = float.Parse(new_expression.expression);
            }
            else
            {
                total = this.Operate(recent_op, total);
            }
            PrintSteps($"Total So Far: {total}");

            return total;


        }

        public float Operate(char operation, float total)
        {
            if (NumCheck(this))
            {
                return float.Parse(this.expression);
            }
            else
            {
                switch (operation)
                {
                    case '+':
                        PrintSteps($"Adding {this.expression}");
                        return total + this.Evaluate("*/");
                    case '-':
                        PrintSteps($"Subtracting {this.expression}");
                        return total - this.Evaluate("*/");
                    case '*':
                        PrintSteps($"Multiplying {this.expression}");
                        return total * this.Operate(' ', 0);
                    case '/':
                        PrintSteps($"Dividing {this.expression}");
                        return total / this.Operate(' ', 0);
                    //case "^":
                    //    PrintSteps("Subtracting");
                    //    break;
                    case ' ':
                        PrintSteps($"Parse {this.expression}");
                        return float.Parse(this.expression);
                }
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
