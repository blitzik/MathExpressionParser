using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser.Functions
{
    public abstract class BaseFunction : Function
    {
        public BaseFunction(string value) : base(value)
        {
            Precedence = 1000;
            Associativity = Associativity.LEFT;
        }
    }
}
