using System;

namespace BinaryOperations
{
    public class Program
    {
        const int NumericalBase = 2;

        const int OperationConvertToBinary = 1;
        const int OperationConvertFromBinary = 2;
        const int OperationBinaryNot = 3;
        const int OperationBinaryOr = 4;
        const int OperationBinaryAnd = 5;
        const int OperationBinaryXor = 6;
        const int OperationBinaryShiftLeft = 7;
        const int OperationBinaryShiftRight = 8;
        const int OperationBinaryLessThan = 9;
        const int OperationBinaryGreaterThan = 10;
        const int OperationBinaryEqual = 11;
        const int OperationBinaryNotEqual = 12;
        const int OperationBinaryAdd = 13;
        const int OperationBinarySubstract = 14;
        const int OperationBinaryMultiply = 15;
        const int OperationBinaryDivide = 16;

        public static bool BinaryLessThan(byte[] firstBinaryNumber, byte[] secondBinaryNumber)
        {
            if (firstBinaryNumber == null)
            {
                throw new ArgumentNullException(nameof(firstBinaryNumber));
            }

            if (secondBinaryNumber == null)
            {
                throw new ArgumentNullException(nameof(secondBinaryNumber));
            }

            int firstLength = GetBinaryNumberRealLength(firstBinaryNumber);
            int secondLength = GetBinaryNumberRealLength(secondBinaryNumber);

            if (firstLength < secondLength)
            {
                return true;
            }

            if (firstLength > secondLength)
            {
                return false;
            }

            for (int i = secondLength - 1; i >= 0; i--)
            {
                if (firstBinaryNumber[i] < secondBinaryNumber[i])
                {
                    return true;
                }
                else if (firstBinaryNumber[i] > secondBinaryNumber[i])
                {
                    return false;
                }
            }

            return false;
        }

        static void Main()
        {
            if (int.TryParse(Console.ReadLine(), out int operation) && operation >= OperationConvertToBinary && operation <= OperationBinaryDivide)
            {
                ExecuteOperation(operation);
            }
            else
            {
                Console.WriteLine("Operatie invalida.");
            }

            Console.Read();
        }

        static void ExecuteOperation(int operation)
        {
            if (operation >= OperationConvertToBinary && operation <= OperationConvertFromBinary)
            {
                ExecuteConversionOperation(operation);
                return;
            }

            if (operation >= OperationBinaryNot && operation <= OperationBinaryXor)
            {
                ExecuteLogicalOperation(operation);
                return;
            }

            if (operation >= OperationBinaryShiftLeft && operation <= OperationBinaryShiftRight)
            {
                ExecuteShiftOperation(operation);
                return;
            }

            if (operation >= OperationBinaryLessThan && operation <= OperationBinaryNotEqual)
            {
                ExecuteComparisonOperation(operation);
                return;
            }

            ExecuteArithmeticalOperation(operation);
        }

        private static void ExecuteArithmeticalOperation(int operation)
        {
            if (operation == OperationBinaryDivide)
            {
                BinaryDivideOperation();
            }
            else
            {
                BaseBinaryOperation(operation);
            }
        }

        private static void ExecuteLogicalOperation(int operation)
        {
            if (operation == OperationBinaryNot)
            {
                BinaryNotOperation();
            }
            else
            {
                BaseBinaryOperation(operation);
            }
        }

        private static void ExecuteConversionOperation(int operation)
        {
            if (operation == OperationConvertToBinary)
            {
                ConvertToBinaryOperation();
            }
            else
            {
                ConvertFromBinaryOperation();
            }
        }

        static void BinaryDivideOperation()
        {
            if (!ReadBinaryNumber(out byte[] firstBinaryNumber))
            {
                return;
            }

            if (!ReadBinaryNumber(out byte[] secondBinaryNumber))
            {
                return;
            }

            if (BinaryComparison(secondBinaryNumber, new byte[] { 0 }, OperationBinaryEqual))
            {
                Console.WriteLine("Nu se poate imparti la 0!");
                return;
            }

            byte[] resultedBinaryNumber = BinaryDivide(firstBinaryNumber, secondBinaryNumber);
            Console.WriteLine(BinaryNumberToString(resultedBinaryNumber));
        }

        static byte[] BinaryAdd(byte[] firstBinaryNumber, byte[] secondBinaryNumber)
        {
            var (shorter, longer) = GetShorterAndLongerNumber(firstBinaryNumber, secondBinaryNumber);
            byte[] result = new byte[longer.Length];
            int reminder = 0;

            for (int i = 0; i < shorter.Length; i++)
            {
                result[i] = (byte)((shorter[i] + longer[i] + reminder) % NumericalBase);
                reminder = (shorter[i] + longer[i] + reminder) / NumericalBase;
            }

            for (int i = shorter.Length; i < longer.Length; i++)
            {
                result[i] = (byte)((longer[i] + reminder) % NumericalBase);
                reminder = (longer[i] + reminder) / NumericalBase;
            }

            if (reminder == 1)
            {
                result = AddBinaryDigit(result, 1);
            }

            return result;
        }

        static void ExecuteComparisonOperation(int comparisonOperator)
        {
            if (!ReadBinaryNumber(out byte[] firstBinaryNumber))
            {
                return;
            }

            if (!ReadBinaryNumber(out byte[] secondBinaryNumber))
            {
                return;
            }

            Console.WriteLine(BinaryComparison(firstBinaryNumber, secondBinaryNumber, comparisonOperator));
        }

        static bool BinaryComparison(byte[] firstNumber, byte[] secondNumber, int comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case OperationBinaryLessThan:
                    return BinaryLessThan(firstNumber, secondNumber);
                case OperationBinaryGreaterThan:
                    return BinaryLessThan(secondNumber, firstNumber);
                case OperationBinaryEqual:
                    return !BinaryLessThan(firstNumber, secondNumber) && !BinaryLessThan(secondNumber, firstNumber);
                case OperationBinaryNotEqual:
                    return BinaryLessThan(firstNumber, secondNumber) || BinaryLessThan(secondNumber, firstNumber);
            }

            return false;
        }

        static int GetBinaryNumberRealLength(byte[] binaryNumber)
        {
            int result = binaryNumber.Length;
            while (result > 1 && binaryNumber[result - 1] == 0)
            {
                result--;
            }

            return result;
        }

        static void ExecuteShiftOperation(int shiftType)
        {
            if (!ReadBinaryNumber(out byte[] binaryNumber))
            {
                return;
            }

            if (int.TryParse(Console.ReadLine(), out int positions) && positions < 0)
            {
                Console.WriteLine("Numarul de pozitii trebuie sa fie intreg si pozitiv.");
                return;
            }

            byte[] resultedBinaryNumber = ApplyShiftOperation(binaryNumber, shiftType, positions);
            Console.WriteLine(BinaryNumberToString(resultedBinaryNumber));
        }

        static byte[] ApplyShiftOperation(byte[] binaryNumber, int shiftType, int positions)
        {
            int newLength = shiftType == OperationBinaryShiftLeft ?
                binaryNumber.Length + positions :
                binaryNumber.Length - positions;

            if (newLength < 1)
            {
                return new byte[] { 0 };
            }

            Array.Reverse(binaryNumber);
            Array.Resize(ref binaryNumber, newLength);
            Array.Reverse(binaryNumber);
            return binaryNumber;
        }

        static void BaseBinaryOperation(int operationType)
        {
            if (!ReadBinaryNumber(out byte[] firstBinaryNumber))
            {
                return;
            }

            if (!ReadBinaryNumber(out byte[] secondBinaryNumber))
            {
                return;
            }

            byte[] resultedBinaryNumber = ApplyBinaryOperation(firstBinaryNumber, secondBinaryNumber, operationType);
            Console.WriteLine(BinaryNumberToString(resultedBinaryNumber));
        }

        static byte[] ApplyBinaryOperation(byte[] firstBinaryNumber, byte[] secondBinaryNumber, int operationType)
        {
            switch (operationType)
            {
                case OperationBinaryOr:
                case OperationBinaryAnd:
                case OperationBinaryXor:
                    return BinaryLogicalOperation(firstBinaryNumber, secondBinaryNumber, operationType);
                case OperationBinaryAdd:
                    return BinaryAdd(firstBinaryNumber, secondBinaryNumber);
                case OperationBinarySubstract:
                    return BinarySubtract(firstBinaryNumber, secondBinaryNumber);
                case OperationBinaryMultiply:
                    return BinaryMultiply(firstBinaryNumber, secondBinaryNumber);
            }

            return null;
        }

        static byte[] BinaryDivide(byte[] dividend, byte[] divisor)
        {
            byte[] result = { 0 };
            while (BinaryLessThan(divisor, dividend) ||
                   BinaryComparison(divisor, dividend, OperationBinaryEqual))
            {
                dividend = BinarySubtract(dividend, divisor);
                result = BinaryAdd(result, new byte[] { 1 });
            }

            return result;
        }

        static byte[] BinaryMultiply(byte[] firstBinaryNumber, byte[] secondBinaryNumber)
        {
            var (bigger, smaller) = GetBiggerAndSmallerNumber(firstBinaryNumber, secondBinaryNumber);
            byte[] result = { 0 };
            for (byte[] index = { 0 }; BinaryLessThan(index, smaller); index = BinaryAdd(index, new byte[] { 1 }))
            {
                result = BinaryAdd(result, bigger);
            }

            return result;
        }

        static byte[] BinarySubtract(byte[] firstBinaryNumber, byte[] secondBinaryNumber)
        {
            var (bigger, smaller) = GetBiggerAndSmallerNumber(firstBinaryNumber, secondBinaryNumber);
            byte[] result = new byte[GetBinaryNumberRealLength(bigger)];
            int smallerLength = GetBinaryNumberRealLength(smaller);
            int reminder = 0;

            for (int i = 0; i < result.Length; i++)
            {
                int digit = i < smallerLength ?
                    bigger[i] - smaller[i] - reminder :
                    bigger[i] - reminder;

                if (digit < 0)
                {
                    result[i] = (byte)(digit * -1 % NumericalBase);
                    reminder = 1;
                }
                else
                {
                    result[i] = (byte)digit;
                    reminder = 0;
                }
            }

            return result;
        }

        static byte[] BinaryLogicalOperation(byte[] firstBinaryNumber, byte[] secondBinaryNumber, int operationType)
        {
            var (shorter, longer) = GetShorterAndLongerNumber(firstBinaryNumber, secondBinaryNumber);
            byte[] result = new byte[longer.Length];

            for (int i = 0; i < shorter.Length; i++)
            {
                result[i] = CalculateLogicalOperationResult(shorter[i], longer[i], operationType);
            }

            for (int i = shorter.Length; i < longer.Length; i++)
            {
                result[i] = CalculateLogicalOperationResult(0, longer[i], operationType);
            }

            return result;
        }

        static byte CalculateLogicalOperationResult(byte firstDigit, byte secondDigit, int operationType)
        {
            switch (operationType)
            {
                case OperationBinaryOr:
                    return (byte)(firstDigit + secondDigit > 0 ? 1 : 0);
                case OperationBinaryAnd:
                    return (byte)(firstDigit + secondDigit == NumericalBase ? 1 : 0);
                case OperationBinaryXor:
                    return (byte)(firstDigit + secondDigit == 1 ? 1 : 0);
            }

            return 0;
        }

        static (byte[] shorter, byte[] longer) GetShorterAndLongerNumber(byte[] firstBinaryNumber, byte[] secondBinaryNumber)
        {
            byte[] shorter;
            byte[] longer;

            if (firstBinaryNumber.Length > secondBinaryNumber.Length)
            {
                shorter = secondBinaryNumber;
                longer = firstBinaryNumber;
            }
            else
            {
                shorter = firstBinaryNumber;
                longer = secondBinaryNumber;
            }

            return (shorter, longer);
        }

        static (byte[] bigger, byte[] smaller) GetBiggerAndSmallerNumber(byte[] firstBinaryNumber, byte[] secondBinaryNumber)
        {
            byte[] bigger;
            byte[] smaller;

            if (BinaryLessThan(firstBinaryNumber, secondBinaryNumber))
            {
                bigger = secondBinaryNumber;
                smaller = firstBinaryNumber;
            }
            else
            {
                bigger = firstBinaryNumber;
                smaller = secondBinaryNumber;
            }

            return (bigger, smaller);
        }

        static void BinaryNotOperation()
        {
            if (!ReadBinaryNumber(out byte[] binaryNumber))
            {
                return;
            }

            binaryNumber = ApplyBinaryNot(binaryNumber);
            Console.WriteLine(BinaryNumberToString(binaryNumber));
        }

        static byte[] ApplyBinaryNot(byte[] binaryNumber)
        {
            for (int i = 0; i < binaryNumber.Length; i++)
            {
                binaryNumber[i] = (byte)((binaryNumber[i] + 1) % NumericalBase);
            }

            return binaryNumber;
        }

        static void ConvertFromBinaryOperation()
        {
            if (!ReadBinaryNumber(out byte[] binaryNumber))
            {
                return;
            }

            Console.WriteLine(ConvertToDecimal(binaryNumber));
        }

        static bool ReadBinaryNumber(out byte[] binaryNumber)
        {
            bool result = TryParseBinaryNumberFromString(Console.ReadLine(), out binaryNumber);
            if (!result)
            {
                Console.WriteLine("Nu s-a introdus un numar binar valid (format doar din 0 si 1).");
            }

            return result;
        }

        static int ConvertToDecimal(byte[] binaryNumber)
        {
            int result = 0;
            int power = 1;
            for (int i = 0; i < binaryNumber.Length; i++)
            {
                result += binaryNumber[i] * power;
                power *= NumericalBase;
            }

            return result;
        }

        static bool TryParseBinaryNumberFromString(string binaryNumberText, out byte[] binaryNumber)
        {
            binaryNumber = new byte[binaryNumberText.Length];
            if (binaryNumberText.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < binaryNumberText.Length; i++)
            {
                if (binaryNumberText[i] != '0' && binaryNumberText[i] != '1')
                {
                    return false;
                }

                binaryNumber[binaryNumber.Length - i - 1] = Convert.ToByte(binaryNumberText[i].ToString());
            }

            return true;
        }

        static void ConvertToBinaryOperation()
        {
            if (int.TryParse(Console.ReadLine(), out int number) && number > 0)
            {
                byte[] binaryNumber = ConvertToBinary(number);
                Console.WriteLine(BinaryNumberToString(binaryNumber));
            }
            else
            {
                Console.WriteLine("Programul converteste doar numere intregi pozitive.");
            }
        }

        static string BinaryNumberToString(byte[] binaryNumber)
        {
            string result = "";
            bool initialZeros = true;
            for (int i = binaryNumber.Length - 1; i >= 0; i--)
            {
                if (binaryNumber[i] == 0 && initialZeros)
                {
                    continue;
                }

                initialZeros = false;
                result += binaryNumber[i];
            }

            if (result == "")
            {
                result = "0";
            }

            return result;
        }

        static byte[] ConvertToBinary(int number)
        {
            byte[] result = null;
            while (number != 0)
            {
                result = AddBinaryDigit(result, (byte)(number % NumericalBase));
                number /= NumericalBase;
            }

            return result;
        }

        static byte[] AddBinaryDigit(byte[] binaryNumber, byte digit)
        {
            byte[] result = binaryNumber ?? (new byte[0]);
            Array.Resize(ref result, result.Length + 1);

            result[result.Length - 1] = digit;
            return result;
        }
    }
}
