using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Simple math expression parser using shunting-yard algorithm.");
            Console.WriteLine("Supported unary operators: +  -");
            Console.WriteLine("Supported binary operators: +  -  *  /  ^");
            Console.WriteLine("Supported grouping operators: (  )");
            Console.WriteLine("Created by Aleš Tichava, December 2018");
            Console.WriteLine();

            ITokenizer t = new Tokenizer();
            t.OnCreatedToken += (Token token) => {
                Console.WriteLine(string.Format("({0}) {1}", token, token.GetType().Name));
            };

            IInfixToPostfixNotationConverter c = new InfixToPostfixConverter(t);
            c.OnFinishedConversion += (resultCollection) => {
                Console.Write("Reverse-Polish Notation: ");
                foreach (Token tok in resultCollection) {
                    if (tok is UnaryOperator) {
                        Console.Write(string.Format("{0}u ", tok));
                    } else {
                        Console.Write(string.Format("{0} ", tok));
                    }
                }
                Console.WriteLine();
            };

            Calculator calculator = new Calculator(c, new PostfixNotationParser());

            string input;
            do {
                input = Console.ReadLine();
                try {
                    Console.WriteLine(string.Format("Result: {0}", calculator.Calculate(input)));
                    Console.WriteLine();
                    Console.WriteLine("------");

                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                    continue;
                }

            } while (true);
        }
    }
}
