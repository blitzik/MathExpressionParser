using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    // Class that converts expression in infix notation into Reverse Polish Notation (otherwise known as postfix notation)
    public class InfixToPostfixConverter : IInfixToPostfixNotationConverter
    {
        private readonly ITokenizer _tokenizer;

        public InfixToPostfixConverter(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }


        public event Action<IReadOnlyCollection<Token>> OnFinishedConversion;
        public List<Token> Convert(string expression)
        {
            List<Token> queue = new List<Token>();
            Stack<Function> operatorStack = new Stack<Function>();

            List<Token> tokens = _tokenizer.Tokenize(expression);
            foreach (Token t in tokens) {
                if (t is Literal) {
                    queue.Add(t);

                } else if (t is ParameterSeparator) {
                    while (operatorStack.Count > 0 && !operatorStack.Peek().Value.Equals("(")) {
                        queue.Add(operatorStack.Pop());
                    }

                } else if (t is Parenthesis p) {
                    if (p.Associativity == Associativity.LEFT) { // left bracket
                        operatorStack.Push(p);

                    } else { // right bracket
                        while (operatorStack.Count > 0 && !operatorStack.Peek().Value.Equals("(")) {
                            queue.Add(operatorStack.Pop());
                        }
                        if (operatorStack.Count > 0) {
                            operatorStack.Pop(); // removal of opening bracket

                        } else {
                            throw new Exception("An Opening Parenthesis is missing");
                        }
                    }

                } else if (t is Operator) {
                    Operator o = (Operator)t;
                    while (operatorStack.Count > 0 && (operatorStack.Peek().Precedence > o.Precedence || operatorStack.Peek().Precedence == o.Precedence && o.Associativity == Associativity.LEFT)) {
                        queue.Add(operatorStack.Pop());
                    }
                    operatorStack.Push(o);

                } else { // function
                    operatorStack.Push((Function)t);
                }
            }

            while (operatorStack.Count > 0) {
                if (operatorStack.Peek().Value.Equals("(")) {
                    throw new Exception("A Closing Parenthesis is missing.");
                }
                queue.Add(operatorStack.Pop());
            }

            Action<IReadOnlyList<Token>> handler = OnFinishedConversion;
            handler?.Invoke(new ReadOnlyCollection<Token>(queue));

            return queue;
        }
    }
}
