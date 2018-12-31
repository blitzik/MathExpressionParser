using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser.Functions
{
    public class Pow : BaseFunction
    {
        public Pow(string symbol) : base(symbol)
        {
            NumberOfParameters = 2;
        }


        public override double Process(params double[] args)
        {
            return Math.Pow(args[1], args[0]);
        }
    }
}
