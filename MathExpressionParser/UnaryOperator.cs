using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class UnaryOperator : Operator
    {
        private Func<double, double> _action;

        public UnaryOperator(string value, int precedence, Associativity associativity, Func<double, double> action) : base(value, precedence, associativity)
        {
            _action = action;
        }


        public double Calc(double a)
        {
            return _action.Invoke(a);
        }
    }
}
