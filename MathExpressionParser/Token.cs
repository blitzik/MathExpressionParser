using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public abstract class Token
    {
        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            protected set { _symbol = value; }
        }


        public override string ToString()
        {
            return Symbol;
        }
    }
}
