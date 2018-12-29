using MathExpressionParser.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class Tokenizer : ITokenizer
    {
        private ReadOnlyDictionary<string, BinaryOperator> _binaryOperators;
        public ReadOnlyDictionary<string, BinaryOperator> BinaryOperators
        {
            get { return _binaryOperators; }
            private set { _binaryOperators = value; }
        }


        private ReadOnlyDictionary<string, UnaryOperator> _unaryOperators;
        public ReadOnlyDictionary<string, UnaryOperator> UnaryOperators
        {
            get { return _unaryOperators; }
            private set { _unaryOperators = value; }
        }


        private ReadOnlyDictionary<string, Parenthesis> _parenthesis;
        public ReadOnlyDictionary<string, Parenthesis> Parenthesis
        {
            get { return _parenthesis; }
            private set { _parenthesis = value; }
        }


        private Dictionary<string, Function> _functionsStorage;
        private ReadOnlyDictionary<string, Function> _functions;
        public ReadOnlyDictionary<string, Function> Functions
        {
            get { return _functions; }
            private set { _functions = value; }
        }


        private string _numberDecimalSeparator;
        public string NumberDecimalSeparator
        {
            get { return _numberDecimalSeparator; }
            private set { _numberDecimalSeparator = value; }
        }


        public Tokenizer()
        {
            NumberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            UnaryOperators = new ReadOnlyDictionary<string, UnaryOperator>(
                new Dictionary<string, UnaryOperator>() {
                    { "-", new UnaryOperator("-", 4, Associativity.LEFT, new Func<double, double>((a) => { return a * (-1); })) },
                    { "+", new UnaryOperator("+", 4, Associativity.LEFT, new Func<double, double>((a) => { return a; })) }
                }
            );

            BinaryOperators = new ReadOnlyDictionary<string, BinaryOperator>(
                new Dictionary<string, BinaryOperator>() {
                    { "+", new BinaryOperator("+", 1, Associativity.LEFT, new Func<double, double, double>((a, b) => { return b + a; })) },
                    { "-", new BinaryOperator("-", 1, Associativity.LEFT, new Func<double, double, double>((a, b) => { return b - a; })) },
                    { "*", new BinaryOperator("*", 2, Associativity.LEFT, new Func<double, double, double>((a, b) => { return b * a; })) },
                    { "/", new BinaryOperator("/", 2, Associativity.LEFT, new Func<double, double, double>((a, b) => {
                        if (a == 0) {
                            throw new DivideByZeroException("Expression cannot be parsed. Divison by zero.");
                        }
                        return b / a;
                        }))
                    },
                    { "^", new BinaryOperator("^", 3, Associativity.RIGHT, new Func<double, double, double>((a, b) => { return Math.Pow(b, a); })) }
                }
            );

            Parenthesis = new ReadOnlyDictionary<string, Parenthesis>(
                new Dictionary<string, Parenthesis>() {
                    { "(", new Parenthesis("(", 0, Associativity.LEFT) },
                    { ")", new Parenthesis(")", 0, Associativity.RIGHT) }
                }
            );


            _functionsStorage = new Dictionary<string, Function>() {
                { "pow", new Pow("pow") },
                { "abs", new Abs("abs") }
            };
            Functions = new ReadOnlyDictionary<string, Function>(_functionsStorage);
        }


        public void AddFunction(Function func)
        {
            _functionsStorage.Add(func.Value, func);
        }

        /*
            // separators (check for literal & function completion)
            [  ] -> whitespace --done
            [ + - / * ^ ) ( ] -> operators --done

            number letter -> end of a literal --done
            letter number -> end of a function name | sin90 --done
            letter UnaryOperator -> end of a function name | sin-90 --done
            letter OpeninParenthesis -> end of a function name --done
            
            // implicit multiplication
            number whitespace number | 3 3 -- done
            number OpeningParentehsis | 5 (2 + 3) -- done
            ClosingParenthesis number | (2 + 3) 5 -- done
            ClosingParenthesis OpeningParenthesis | (3 + 2)(3 + 2); (-5)(3 + 2) -- done
            number function | 5sin(90); -- done
            ClosingParenthesis function | sin(90)sin(92); (3 + 2)sin(90); -- done
            function function | sin(90)sin(90); sin90sin90; sin-90sin90 -- done

            // unary operators
            +;- at the start of an expression (lookBehind == null) | -(3 + 2); -5 + 2 -- done
            BinaryOperator +;- | 3 + -2 -- done
            OpeningParenthesis +;- | (-3 + 2)5 -- done
            ParameterSeparator +;- number | pow(2, -3); -- done
        */

        public event Action<Token> OnCreatedToken;
        public List<Token> Tokenize(string str)
        {
            Action<Token> onCreatedTokenHandler = OnCreatedToken;

            List<Token> tokens = new List<Token>();

            StringBuilder literalBuilder = new StringBuilder();
            StringBuilder functionBuilder = new StringBuilder();
            Token lookBehind;
            foreach (char c in str) {
                if (char.IsLetter(c)) {
                    AddLiteral(literalBuilder, tokens, onCreatedTokenHandler);
                    functionBuilder.Append(c);
                    continue;
                }

                if (char.IsDigit(c) || c.Equals('.')) {
                    AddFunction(functionBuilder, tokens, onCreatedTokenHandler);
                    literalBuilder.Append(c);
                    continue;
                }

                AddLiteral(literalBuilder, tokens, onCreatedTokenHandler);
                AddFunction(functionBuilder, tokens, onCreatedTokenHandler);

                if (c.Equals(' ')) {
                    continue;
                }

                if (c.Equals(',')) {
                    tokens.Add(new ParameterSeparator(","));
                    continue;
                }

                string oValue = c.ToString(); // operator symbol

                // unary operators
                lookBehind = tokens.LastOrDefault();
                if (UnaryOperators.ContainsKey(oValue)) {
                    if (lookBehind == null) { // unary operator at the start of an expression
                        tokens.Add(UnaryOperators[oValue]);
                        onCreatedTokenHandler?.Invoke(UnaryOperators[oValue]);
                        continue;

                    } else {
                        // if the previous token is a BinaryOperator or left parenthesis and the current operator is a supported unary operator
                        // then we can add current operator as a unary operator token
                        Type lbType = lookBehind.GetType();
                        if ((lbType == typeof(BinaryOperator) ||
                             lbType == typeof(Function) ||
                             lbType == typeof(ParameterSeparator) ||
                             lbType == typeof(Parenthesis) && ((Parenthesis)lookBehind).Associativity == Associativity.LEFT) &&
                             UnaryOperators.ContainsKey(oValue)) {
                            tokens.Add(UnaryOperators[oValue]);
                            onCreatedTokenHandler?.Invoke(UnaryOperators[oValue]);
                            continue;
                        }
                    }
                }

                // parenthesis & binary operators
                if (Parenthesis.ContainsKey(oValue)) {
                    // implicit multiplication(left side) between literal <-> polynomial && polynomial <-> polynomial e.g 5(3+2); (3+2)(3+2)
                    if (Parenthesis[oValue].Associativity == Associativity.LEFT && (lookBehind is Literal || (lookBehind is Parenthesis && ((Parenthesis)lookBehind).Associativity == Associativity.RIGHT))) {
                        tokens.Add(BinaryOperators["*"]);
                        onCreatedTokenHandler?.Invoke(BinaryOperators["*"]);
                    }
                    tokens.Add(Parenthesis[oValue]);
                    onCreatedTokenHandler?.Invoke(Parenthesis[oValue]);

                } else if (BinaryOperators.ContainsKey(oValue)) {
                    tokens.Add(BinaryOperators[oValue]);
                    onCreatedTokenHandler?.Invoke(BinaryOperators[oValue]);

                } else {
                    throw new Exception("Unsupported operator");
                }
            }

            AddLiteral(literalBuilder, tokens, onCreatedTokenHandler);
            AddFunction(functionBuilder, tokens, onCreatedTokenHandler);

            return tokens;
        }


        private void AddLiteral(StringBuilder literalBuilder, List<Token> tokenStorage, Action<Token> onCreatedTokenHandler)
        {
            if (literalBuilder.Length > 0) {
                Token lookBehind = tokenStorage.LastOrDefault();
                Literal n = new Literal(literalBuilder.ToString());
                // implicit multiplication (right side); e.g. (3+2)5
                if (lookBehind is Literal || (lookBehind is Parenthesis && ((Parenthesis)lookBehind).Associativity == Associativity.RIGHT)) {
                    tokenStorage.Add(BinaryOperators["*"]);
                    onCreatedTokenHandler?.Invoke(BinaryOperators["*"]);
                }

                tokenStorage.Add(n);
                onCreatedTokenHandler?.Invoke(n);

                literalBuilder.Clear();
            }
        }


        private void AddFunction(StringBuilder functionBuilder, List<Token> tokenStorage, Action<Token> onCreatedTokenHandler)
        {
            if (functionBuilder.Length > 0) {
                string func = functionBuilder.ToString();
                if (!Functions.ContainsKey(func)) {
                    throw new Exception("Unsupported function");
                }

                // 5sin90; sin90sin90; sin(90)sin(90);
                Token lookBehind = tokenStorage.LastOrDefault();
                if (lookBehind is Literal || (lookBehind is Parenthesis && ((Parenthesis)lookBehind).Associativity == Associativity.RIGHT)) {
                    tokenStorage.Add(BinaryOperators["*"]);
                    onCreatedTokenHandler?.Invoke(BinaryOperators["*"]);
                }

                tokenStorage.Add(Functions[func]);
                onCreatedTokenHandler?.Invoke(Functions[func]);
                functionBuilder.Clear();
            }
        }
    }
}
