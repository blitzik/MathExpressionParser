using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class Parenthesis : Operator
    {
        public Parenthesis(string symbol, int precedence, Associativity associativity) : base(symbol, precedence, associativity)
        {
            Precedence = precedence;
            Associativity = associativity;
        }


        public override double Process(params double[] args)
        {
            throw new NotImplementedException();
        }
    }
}
