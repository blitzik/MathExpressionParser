using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class ParameterSeparator : Token
    {
        public ParameterSeparator(string value) : base(value)
        {
        }
    }
}
