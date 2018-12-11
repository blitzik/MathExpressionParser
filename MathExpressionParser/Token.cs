using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public abstract class Token
    {
        private string _value;
        public string Value
        {
            get { return _value; }
            private set { _value = value; }
        }


        public Token(string value)
        {
            Value = value;
        }


        public override string ToString()
        {
            return Value;
        }
    }
}
