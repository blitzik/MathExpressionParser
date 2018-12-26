using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser.Functions
{
    public class Sin : Function
    {
        public Sin(string value) : base(value)
        {
        }


        public override double Process(params double[] args)
        {
            return Math.Sin(args[0]);
        }
    }
}
