using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class PostfixNotationParser : IPostfixNotationParser
    {
        public PostfixNotationParser()
        {
        }


        public double Parse(IList<Token> tokens)
        {
            Stack<Literal> stack = new Stack<Literal>();
            try {
                foreach (Token t in tokens) {
                    Type tt = t.GetType();
                    if (tt == typeof(Literal)) {
                        stack.Push((Literal)t);

                    } else { // functions & operators
                        Function f = (Function)t;
                        double[] parameters = new double[f.NumberOfParameters];
                        for (int i = 0; i < f.NumberOfParameters; i++) {
                            parameters[i] = ParseNumber(stack.Pop());
                        }
                        stack.Push(new Literal(f.Process(parameters).ToString()));

                    }
                }

            } catch (DivideByZeroException e) {
                throw e;

            } catch (Exception e) {
                throw new Exception("Expression cannot be parsed. Too many operators.");
            }

            if (stack.Count > 1) {
                throw new Exception("TODO [PostfixCalculator]. Stack has more items than it should have");
            }

            return double.Parse(stack.Pop().Value);
        }


        private double ParseNumber(Literal number)
        {
            if (double.TryParse(number.Value, out double result)) {
                return result;
            }

            throw new Exception(string.Format("Expression cannot be parsed. \"{0}\" is not a number.", number));
        }
    }
}
