﻿using MathExpressionParser.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Console.WriteLine("Simple math expression parser using shunting-yard algorithm.");
            Console.WriteLine("Supported unary operators: +  -");
            Console.WriteLine("Supported binary operators: +  -  *  /  ^");
            Console.WriteLine("Supported grouping operators: (  )");
            Console.WriteLine("Supported functions: pow, abs");
            Console.WriteLine("Supported constants: PI");
            Console.WriteLine("Created by Aleš Tichava, December 2018");
            Console.WriteLine();

            ITokenizer t = new Tokenizer();
            t.OnCreatedToken += (Token token) => {
                Console.WriteLine(string.Format("({0}) {1}", token, token is BaseFunction ? "Function" : token.GetType().Name));
            };

            /*string expression = "-abs-3";
            List<Token> tokens = t.Tokenize(expression);
            IInfixToPostfixNotationConverter c = new InfixToPostfixConverter(t);
            List<Token> convertedTokens = c.Convert(expression);
            PostfixNotationParser p = new PostfixNotationParser();
            double result = p.Parse(convertedTokens);*/

            IInfixToPostfixNotationConverter c = new InfixToPostfixConverter(t);
            c.OnFinishedConversion += (resultCollection) => {
                Console.Write("Reverse-Polish Notation: ");
                foreach (Token tok in resultCollection) {
                    Console.Write(string.Format("{0}{1} ", tok, tok is UnaryOperator ? "u" : string.Empty));
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
