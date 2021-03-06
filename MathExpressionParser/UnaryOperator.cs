﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class UnaryOperator : Operator
    {
        private Func<double, double> _action;

        public UnaryOperator(string symbol, int precedence, Associativity associativity, Func<double, double> action) : base(symbol, precedence, associativity)
        {
            _action = action;
            NumberOfParameters = 1;
        }


        public override double Process(params double[] args)
        {
            return _action.Invoke(args[0]);
        }
    }
}
