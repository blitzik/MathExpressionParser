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
        public Operator(string value, int precedence, Associativity associativity) : base(value)
        {
            Precedence = precedence;
            Associativity = associativity;
        }
    }
}
