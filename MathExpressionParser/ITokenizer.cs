using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public interface ITokenizer
    {
        event Action<Token> OnCreatedToken;
        List<Token> Tokenize(string str);
    }
}
