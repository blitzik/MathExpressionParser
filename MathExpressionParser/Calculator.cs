using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
    public class Calculator
    {
        private readonly IInfixToPostfixNotationConverter _infixToRpnConverter;
        private readonly IPostfixNotationParser _rpnParser;

        public Calculator(IInfixToPostfixNotationConverter infixToRpnConverter, IPostfixNotationParser rpnParser)
        {
            _infixToRpnConverter = infixToRpnConverter;
            _rpnParser = rpnParser;
        }


        public double Calculate(string expression)
        {
            return _rpnParser.Parse(_infixToRpnConverter.Convert(expression));
        }
    }
}
