using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathExpressionParser;

namespace Tests
{
    [TestClass]
    public class MathExpressionParser
    {
        private Calculator _calc;


        [TestInitialize]
        public void Initialize()
        {
            Tokenizer t = new Tokenizer();
            InfixToPostfixConverter i2pConverter = new InfixToPostfixConverter(t);
            PostfixNotationParser rpnParser = new PostfixNotationParser();
            _calc = new Calculator(i2pConverter, rpnParser);
        }


        [TestMethod]
        public void Literal()
        {
            Assert.AreEqual(5, _calc.Calculate("5"));
        }


        [TestMethod]
        public void NoWhiteSpace()
        {
            Assert.AreEqual(0, _calc.Calculate("5-5"));
        }


        [TestMethod]
        public void Addition()
        {
            Assert.AreEqual(10, _calc.Calculate("5 + 5"));
        }


        [TestMethod]
        public void Subtraction()
        {
            Assert.AreEqual(0, _calc.Calculate("5 - 5"));
        }


        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(25, _calc.Calculate("5 * 5"));
        }


        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(1, _calc.Calculate("5 / 5"));
        }


        [TestMethod]
        public void Power()
        {
            Assert.AreEqual(3125, _calc.Calculate("5 ^ 5"));
        }


        [TestMethod]
        public void OperatorPrecedence()
        {
            Assert.AreEqual(5, _calc.Calculate("2 + 3 * 2 - 3"));
            Assert.AreEqual(0, _calc.Calculate("2 + 16 / 4 - 6"));
            Assert.AreEqual(4, _calc.Calculate("3 * 2 ^ 2 / 3"));
            Assert.AreEqual(9, _calc.Calculate("12 / 2 ^ 2 * 3"));
        }


        [TestMethod]
        public void Associativity_Addition()
        {
            Assert.AreEqual(6, _calc.Calculate("3 + 2 + 1"));
        }


        [TestMethod]
        public void Associativity_Subtraction()
        {
            Assert.AreEqual(0, _calc.Calculate("3 - 2 - 1"));
        }


        [TestMethod]
        public void Associativity_Multiplication()
        {
            Assert.AreEqual(6, _calc.Calculate("3 * 2 * 1"));
        }


        [TestMethod]
        public void Associativity_Division()
        {
            Assert.AreEqual(1.5, _calc.Calculate("3 / 2 / 1"));
        }


        [TestMethod]
        public void Associativity_Power()
        {
            Assert.AreEqual(6561, _calc.Calculate("3 ^ 2 ^ 3"));
            Assert.AreEqual(-8, _calc.Calculate("-2^3"));
            Assert.AreEqual(0.125, _calc.Calculate("2^-3"));
        }


        [TestMethod]
        public void Grouping()
        {
            Assert.AreEqual(25, _calc.Calculate("(3 + 2) * 5"));
            Assert.AreEqual(729, _calc.Calculate("(3 ^ 2) ^ 3"));
        }


        [TestMethod]
        public void Negation_Literal()
        {
            Assert.AreEqual(-5, _calc.Calculate("-5"));
        }


        [TestMethod]
        public void Negation_FirstPosition()
        {
            Assert.AreEqual(-25, _calc.Calculate("-(3 + 2) * 5"));
        }


        [TestMethod]
        public void Negation_FirstPositionAndWhiteSpace()
        {
            Assert.AreEqual(-25, _calc.Calculate("- (3 + 2) * 5"));
        }


        [TestMethod]
        public void Negation_AfterOperator()
        {
            Assert.AreEqual(-25, _calc.Calculate("(3 + 2) * -5"));
            Assert.AreEqual(0.5, _calc.Calculate("2^-1"));
        }


        [TestMethod]
        public void Negation_AfterParameterSeparator()
        {
            Assert.AreEqual(0.125, _calc.Calculate("pow(2, -3)"));
            Assert.AreEqual(0.125, _calc.Calculate("pow(2, -pow(3, 1))"));
        }


        [TestMethod]
        public void Negation_AfterOperator_InParenthesis()
        {
            Assert.AreEqual(-25, _calc.Calculate("(3 + 2) * (-5)"));
            Assert.AreEqual(0.5, _calc.Calculate("2^(-1)"));
        }


        [TestMethod]
        public void Negation_AfterOperatorAndWhiteSpace()
        {
            Assert.AreEqual(-25, _calc.Calculate("(3 + 2) * - 5"));
        }


        [TestMethod]
        public void Negation_AfterOperatorAndWhiteSpace_InParenthesis()
        {
            Assert.AreEqual(-25, _calc.Calculate("(3 + 2) * (- 5)"));
        }


        [TestMethod]
        public void Negation_AfterLeftParenthesis()
        {
            Assert.AreEqual(-5, _calc.Calculate("(-3 + 2) * 5"));
        }


        [TestMethod]
        public void MinusAfterRightParenthesis()
        {
            Assert.AreEqual(0, _calc.Calculate("(3 + 2) - 5"));
        }


        [TestMethod]
        public void ImplicitLiteralMultiplication()
        {
            Assert.AreEqual(9, _calc.Calculate("3 3"));
        }


        [TestMethod]
        public void ImplicitMultiplication_Left()
        {
            Assert.AreEqual(25, _calc.Calculate("5(3 + 2)"));
        }


        [TestMethod]
        public void ImplicitMultiplication_Right()
        {
            Assert.AreEqual(25, _calc.Calculate("(3 + 2)5"));
        }


        [TestMethod]
        public void ImplicitMultiplicationWithNegation_Left()
        {
            Assert.AreEqual(-25, _calc.Calculate("-5(3 + 2)"));
        }


        [TestMethod]
        public void ImplicitMultiplicationWithNegation_Right()
        {
            Assert.AreEqual(0, _calc.Calculate("(3 + 2)-5")); // not a multiplication
            Assert.AreEqual(-25, _calc.Calculate("(3 + 2)(-5)"));
        }


        [TestMethod]
        public void Function_Pow()
        {
            Assert.AreEqual(8, _calc.Calculate("pow(2, 3)"));
            Assert.AreEqual(-8, _calc.Calculate("pow(-2, 3)"));
        }


        [TestMethod]
        public void Function_FunctionInFunction()
        {
            Assert.AreEqual(8, _calc.Calculate("pow(2, pow(3, 1))"));
        }


        [TestMethod]
        public void ComplexExpressions()
        {
            Assert.AreEqual(0.125, _calc.Calculate("pow(-(-5 + 3) , -abs(-3))"));
        }
    }
}
