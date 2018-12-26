using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser.Functions
{
    public class Pow : Function
    {
        public Pow(string value) : base(value)
        {
        }


        public override double Process(params double[] args)
        {
            return Math.Pow(args[0], args[1]);
        }
    }
}
