using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public abstract class Function : Token
    {
        private int _precedence;
        public int Precedence
        {
            get { return _precedence; }
            protected set { _precedence = value; }
        }


        private Associativity _associativity;
        public Associativity Associativity
        {
            get { return _associativity; }
            protected set { _associativity = value; }
        }


        private int _numberOfParameters;
        public int NumberOfParameters
        {
            get { return _numberOfParameters; }
            protected set { _numberOfParameters = value; }
        }


        public Function(string value) : base(value)
        {
        }


        public abstract double Process(params double[] args);
    }
}
