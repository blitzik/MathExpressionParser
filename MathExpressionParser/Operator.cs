using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public enum Associativity
    {
        LEFT, RIGHT
    }


    public abstract class Operator : Function
    {
        public Operator(string symbol, int precedence, Associativity associativity) : base(symbol)
        {
            Precedence = precedence;
            Associativity = associativity;
        }
    }
}
