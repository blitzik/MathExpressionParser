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

        public BinaryOperator(string value, int precedence, Associativity associativity, Func<double, double, double> action) : base(value, precedence, associativity)
        {
            _action = action;
        }


        public override double Process(params double[] args)
        {
            return _action.Invoke(args[0], args[1]);
        }
    }
}
