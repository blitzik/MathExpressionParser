using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public interface IInfixToPostfixNotationConverter
    {
        event Action<IReadOnlyCollection<Token>> OnFinishedConversion;

        List<Token> Convert(string expression);
    }
}
