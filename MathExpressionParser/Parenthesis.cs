using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class Parenthesis : Operator
    {
        public Parenthesis(string value, int precedence, Associativity associativity) : base(value, precedence, associativity)
        {
        }
    }
}
