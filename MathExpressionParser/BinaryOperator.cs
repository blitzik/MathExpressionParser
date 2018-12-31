using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class BinaryOperator : Operator
    {
        private Func<double, double, double> _action;

        public BinaryOperator(string symbol, int precedence, Associativity associativity, Func<double, double, double> action) : base(symbol, precedence, associativity)
        {
            _action = action;
            NumberOfParameters = 2;
        }


        public override double Process(params double[] args)
        {
            return _action.Invoke(args[0], args[1]);
        }
    }
}
