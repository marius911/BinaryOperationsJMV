using System;
using Xunit;

namespace BinaryOperations.Tests
{
    public class LessThanTests
    {
        [Fact]
        public void LessThanWorksWhenFirstNumberisLonger()
        {
            byte[] firstNumber = { 1, 1 };
            byte[] secondNumber = {0,  1, 1, 1 };

            Assert.True(Program.BinaryLessThan(firstNumber, secondNumber));

        }

        [Fact]
        public void LessThanWorksWhenSecondNumberisLonger()
        {
            byte[] firstNumber = { 0, 1, 1, 0 };
            byte[] secondNumber = { 1, 1 };

            Assert.False(Program.BinaryLessThan(firstNumber, secondNumber));

        }

        [Fact]
        public void LessThanWorksWhenFirstNumberIsSmaller()
        {
            byte[] firstNumber = { 1, 1, 0 };
            byte[] secondNumber = { 1, 1, 1 };

            Assert.True(Program.BinaryLessThan(firstNumber, secondNumber));

        }

        [Fact]
        public void LessThanWorksWhenNumbersAreEquals()
        {
            byte[] firstNumber = { 1, 1, 1 };
            byte[] secondNumber = { 1, 1, 1 };

            Assert.False(Program.BinaryLessThan(firstNumber, secondNumber));

        }
    }
}
