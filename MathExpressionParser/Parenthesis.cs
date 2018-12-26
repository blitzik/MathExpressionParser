using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class Parenthesis : Operator
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

        public Parenthesis(string value, int precedence, Associativity associativity) : base(value, precedence, associativity)
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
