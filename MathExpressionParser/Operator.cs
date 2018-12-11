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


    public abstract class Operator : Token
    {
        private int _precedence;
        public int Precedence
        {
            get { return _precedence; }
            private set { _precedence = value; }
        }


        private Associativity _associativity;
        public Associativity Associativity
        {
            get { return _associativity; }
            private set { _associativity = value; }
        }


        public Operator(string value, int precedence, Associativity associativity) : base(value)
        {
            Precedence = precedence;
            Associativity = associativity;
        }
    }
}
