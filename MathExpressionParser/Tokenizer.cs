using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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


        public Tokenizer()
        {
            UnaryOperators = new ReadOnlyDictionary<string, UnaryOperator>(
                new Dictionary<string, UnaryOperator>() {
                    { "-", new UnaryOperator("-", 3, Associativity.RIGHT, new Func<double, double>((a) => { return a * (-1); })) },
                    { "+", new UnaryOperator("+", 3, Associativity.RIGHT, new Func<double, double>((a) => { return a; })) }
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
                    { "^", new BinaryOperator("^", 4, Associativity.RIGHT, new Func<double, double, double>((a, b) => { return Math.Pow(b, a); })) }
                }
            );

            Parenthesis = new ReadOnlyDictionary<string, Parenthesis>(
                new Dictionary<string, Parenthesis>() {
                    { "(", new Parenthesis("(", 0, Associativity.LEFT) },
                    { ")", new Parenthesis(")", 0, Associativity.RIGHT) }
                }
            );
        }


        public event Action<Token> OnCreatedToken;
        public List<Token> Tokenize(string str)
        {
            Action<Token> onCreatedTokenHandler = OnCreatedToken;

            List<Token> tokens = new List<Token>();

            StringBuilder literalBuilder = new StringBuilder();
            foreach (char c in str) {
                if (c.Equals(string.Empty) || c.Equals(' ') || c.Equals(" ")) {
                    continue;
                }

                if (char.IsDigit(c) || c.Equals('.') || c.Equals(',')) {
                    literalBuilder.Append(c);
                    continue;
                }

                AddLiteral(literalBuilder, tokens, onCreatedTokenHandler);

                string oValue = c.ToString(); // operator symbol
                
                // unary operators
                Token lookBehind = tokens.LastOrDefault();
                if (UnaryOperators.ContainsKey(oValue)) {
                    if (lookBehind == null) { // unary operator at the start of an expression
                        tokens.Add(UnaryOperators[oValue]);
                        onCreatedTokenHandler?.Invoke(UnaryOperators[oValue]);
                        continue;

                    } else {
                        // if the previous token is a BinaryOperator or left parenthesis and the current operator is a supported unary operator
                        // then we can add current operator as a unary operator token
                        if ((lookBehind.GetType() == typeof(BinaryOperator) ||
                             lookBehind.GetType() == typeof(Parenthesis) && ((Parenthesis)lookBehind).Associativity != Associativity.RIGHT) &&
                             UnaryOperators.ContainsKey(oValue)) {
                            tokens.Add(UnaryOperators[oValue]);
                            onCreatedTokenHandler?.Invoke(UnaryOperators[oValue]);
                            continue;
                        }
                    }
                }

                // parenthesis & binary operators
                if (Parenthesis.ContainsKey(oValue)) {
                    // implicit multiplication(left side) between literal <-> polynom && polynom <-> polynom e.g 5(3+2); (3+2)(3+2)
                    if (Parenthesis[oValue].Associativity == Associativity.LEFT && (lookBehind is Literal || (lookBehind is Parenthesis && ((Parenthesis)lookBehind).Associativity == Associativity.RIGHT))) {
                        tokens.Add(BinaryOperators["*"]);
                        onCreatedTokenHandler?.Invoke(BinaryOperators["*"]);
                    }
                    tokens.Add(Parenthesis[oValue]);
                    onCreatedTokenHandler?.Invoke(Parenthesis[oValue]);

                } else if (BinaryOperators.ContainsKey(oValue)) {
                    tokens.Add(BinaryOperators[oValue]);
                    onCreatedTokenHandler?.Invoke(BinaryOperators[oValue]);

                } else { // unsupported character
                    Literal n = new Literal(oValue);
                    tokens.Add(n);
                    onCreatedTokenHandler?.Invoke(n);
                }

                literalBuilder.Clear();
            }

            AddLiteral(literalBuilder, tokens, onCreatedTokenHandler);

            return tokens;
        }


        private void AddLiteral(StringBuilder literalBuilder, List<Token> tokenStorage, Action<Token> onCreatedTokenHandler)
        {
            if (literalBuilder.Length > 0) {
                Token lookBehind = tokenStorage.LastOrDefault();
                Literal n = new Literal(literalBuilder.ToString());
                // implicit multiplication (right side); e.g. (3+2)5
                if (lookBehind is Parenthesis && ((Parenthesis)lookBehind).Associativity == Associativity.RIGHT) {
                    tokenStorage.Add(BinaryOperators["*"]);
                    onCreatedTokenHandler?.Invoke(BinaryOperators["*"]);
                }
                tokenStorage.Add(n);
                onCreatedTokenHandler?.Invoke(n);
            }
        }
    }
}
