using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser.Constants
{
    public class PI : Constant
    {
        public PI()
        {
            Symbol = "PI";
            Value = Math.PI;
        }
    }
}
