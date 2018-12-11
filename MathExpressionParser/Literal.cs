using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class Literal : Token
    {
        public Literal(string value) : base(value)
        {
        }
    }
}
