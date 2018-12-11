using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public interface IPostfixNotationParser
    {
        double Parse(IList<Token> tokens);
    }
}
