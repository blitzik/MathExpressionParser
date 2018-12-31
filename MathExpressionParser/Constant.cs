using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public abstract class Constant : Token
    {
        private double _value;
        public double Value
        {
            get { return _value; }
            protected set { _value = value; }
        }
    }
}
