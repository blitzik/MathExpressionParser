using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser.Functions
{
    public class Abs : BaseFunction
    {
        public Abs(string symbol) : base(symbol)
        {
            NumberOfParameters = 1;
        }


        public override double Process(params double[] args)
        {
            return Math.Abs(args[0]);
        }
    }
}
