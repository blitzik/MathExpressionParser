using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public abstract class Function : Token
    {
        public Function(string value) : base(value)
        {
        }


        public abstract double Process(params double[] args);
    }
}
