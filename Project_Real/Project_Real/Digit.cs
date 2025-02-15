namespace Project_Real;

public readonly struct Digit
{
	#region Exceptions

	public class ValueOutOfRangeException() : Exception() { }
	public class UnmatchingArrayLengthException() : Exception() { }
	public class SecondValueGreaterException() : Exception() { }

	#endregion

	#region Fields

	public const short LENGTH = 4;
	public static readonly Digit ZERO = new();

	public readonly bool[] Bits;

	private static readonly bool[] TEN = [false, true, false, true];

	#endregion

	#region Properties

	public readonly bool this[Index i] => Bits[i];

	#endregion

	#region Constructors

	public Digit()
	{
		Bits = new bool[LENGTH];
	}

	public Digit(char character)
	{
		short number = (short)(character - '0');

		if (number < 0 || number > 9)
			throw new ValueOutOfRangeException();

		Bits = new bool[LENGTH];
		short pow = 0;

		do
		{
			Bits[pow++] = number % 2 == 1;
			number /= 2;
		} while (number > 0);
	}

	public Digit(bool[] bitArray)
	{
		Bits = BitGreaterThan(TEN, bitArray) ? bitArray : throw new ValueOutOfRangeException();
	}

	#endregion

	#region Private methods

	private static bool BitEquals(bool[] b1, bool[] b2)
	{
		if (b1.Length != b2.Length)
			throw new UnmatchingArrayLengthException();

		short i = 0;
		while (++i < b1.Length && b1[^i] == b2[^i]) { }

		return i == b1.Length && b1[^i] == b2[^i];
	}

	private static bool BitGreaterThan(bool[] b1, bool[] b2)
	{
		if (b1.Length != b2.Length)
			throw new UnmatchingArrayLengthException();

		short i = 0;
		while (++i < b1.Length && b1[^i] == b2[^i]) { }

		return b1[^i] && !b2[^i];
	}

	private static bool[] TwosComplement(bool[] b)
	{
		bool[] result = new bool[b.Length];

		for (short i = 0; i < b.Length; ++i)
			result[i] = !b[i];

		return BitAdd(result, new bool[b.Length], true).Bits;
	}

	private static (bool OverFlow, bool[] Bits) BitAdd(bool[] b1, bool[] b2, bool carry = false)
	{
		if (b1.Length != b2.Length)
			throw new UnmatchingArrayLengthException();

		bool b1i, b2i;
		bool[] result = new bool[b1.Length];

		for (short i = 0; i < result.Length; ++i)
		{
			b1i = b1[i];
			b2i = b2[i];

			if (b1i && b2i && carry)
				result[i] = true;
			else if (b1i && b2i)
				carry = true;
			else if ((b1i || b2i) && carry) { }
			else if (b1i || b2i || carry)
			{
				result[i] = true;
				carry = false;
			}
		}

		return (carry, result);
	}

	private static bool[] BitSubstract(bool[] b1, bool[] b2)
	{
		if (BitGreaterThan(b2, b1))
			throw new SecondValueGreaterException();

		return BitAdd(b1, TwosComplement(b2)).Bits;
	}

	private static bool[] BitMultiply(bool[] b1, bool[] b2)
	{
		bool[] temp;
		bool[] result = new bool[b1.Length + b2.Length - 1];
		int resultCutLength = result.Length - b1.Length;

		for (short i = 0; i < b2.Length; ++i)
		{
			if (b2[^(i + 1)])
			{
				temp = BitAdd(result[resultCutLength..], [.. b1, .. new bool[i]]).Bits;
				Array.Copy(temp, 0, result, resultCutLength, temp.Length);
			}
			--resultCutLength;
		}

		return result;
	}

	private static (bool[] Whole, bool[] Remainder) BitDivide(bool[] b1, bool[] b2)
	{
		short trueIndex = (short)b2.Length;

		while (--trueIndex >= 0 && !b2[trueIndex]) { }

		if (trueIndex < 0)
			throw new DivideByZeroException();

		bool[] minuend, suprahend, difference;
		bool[] denumeratorEnd = b2.Take(trueIndex + 1).ToArray();
		short lenDiff = (short)(b1.Length - denumeratorEnd.Length);
		bool[] whole = new bool[lenDiff + 1];

		for (short i = lenDiff; i >= 0; --i)
		{
			minuend = b1[i..];
			suprahend = [.. denumeratorEnd, .. new bool[lenDiff - i]];

			if (!BitGreaterThan(suprahend, minuend))
			{
				difference = BitAdd(minuend, TwosComplement(suprahend)).Bits;
				Array.Copy(difference, 0, b1, i, difference.Length);
				whole[i] = true;
			}
		}

		return (whole, b1);
	}

	#endregion

	#region Public methods

	public override readonly string ToString()
	{
		short number = 0;

		for (short i = 0; i < LENGTH; ++i)
		{
			if (Bits[i])
				number += (short)Math.Pow(2d, i);
		}

		return number.ToString();
	}

	public static Digit[] CreateArray(int length)
	{
		return CreateArray(length, ZERO);
	}

	public static Digit[] CreateArray(int length, Digit sample)
	{
		if (length < 1)
			return [];

		Digit[] array = new Digit[length];
		Array.Fill(array, sample);

		return array;
	}

	public static Digit[] TrimEnd(Digit[] digits)
	{
		int i = 0;

		while (++i < digits.Length && Equals(digits[^i], ZERO)) { }

		return i == 1 ? digits : digits[..^(i - 1)];
	}

	public static bool Equals(Digit d1, Digit d2)
	{
		return BitEquals(d1.Bits, d2.Bits);
	}

	public static bool GreaterThan(Digit d1, Digit d2)
	{
		return BitGreaterThan(d1.Bits, d2.Bits);
	}

	public static (bool OverFlow, Digit Digit) Add(Digit d1, Digit d2, bool carry = false)
	{
		(carry, bool[] result) = BitAdd(d1.Bits, d2.Bits, carry);

		if (carry || !BitGreaterThan(TEN, result))
			return (true, new Digit(BitAdd(result, TwosComplement(TEN)).Bits));
		else
			return (false, new Digit(result));
	}

	public static (bool Borrow, Digit Digit) Substract(Digit d1, Digit d2, bool carry = false)
	{
		bool[] d2PlusCarry = carry ? BitAdd(d2.Bits, new bool[d2.Bits.Length], carry).Bits : d2.Bits;

		if (!BitGreaterThan(d2PlusCarry, d1.Bits))
			return (false, new Digit(BitSubstract(d1.Bits, d2PlusCarry)));
		else
			return (true, new Digit(BitSubstract(TEN, BitSubstract(d2PlusCarry, d1.Bits))));
	}

	public static (Digit OverFlow, Digit Digit) Multiply(Digit d1, Digit d2)
	{
		(bool[] whole, bool[] remainder) = BitDivide(BitMultiply(d1.Bits, d2.Bits), TEN);

		return (new Digit([.. whole, .. new bool[LENGTH - whole.Length]]), new Digit(remainder.Take(LENGTH).ToArray()));
	}

	public static (Digit Whole, Digit Remainder) Divide(Digit d1, Digit d2)
	{
		(bool[] whole, bool[] remainder) = BitDivide(d1.Bits, d2.Bits);

		return (new Digit([.. whole, .. new bool[LENGTH - whole.Length]]), new Digit(remainder));
	}

	public override bool Equals(object? obj)
	{
		return obj is Digit digit && Equals(this, digit);
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Digit(char num) => new(num);
	public static bool operator ==(Digit f1, Digit f2) => Equals(f1, f2);
	public static bool operator !=(Digit f1, Digit f2) => !Equals(f1, f2);
	public static bool operator >(Digit f1, Digit f2) => GreaterThan(f1, f2);
	public static bool operator <(Digit f1, Digit f2) => GreaterThan(f2, f1);
	public static bool operator >=(Digit f1, Digit f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Digit f1, Digit f2) => !GreaterThan(f1, f2);
	public static Digit operator +(Digit f1, Digit f2) => Add(f1, f2).Digit;
	public static Digit operator -(Digit f1, Digit f2) => Substract(f1, f2).Digit;
	public static Digit operator *(Digit f1, Digit f2) => Multiply(f1, f2).Digit;
	public static Digit operator /(Digit f1, Digit f2) => Divide(f1, f2).Whole;
	public static Digit operator %(Digit f1, Digit f2) => Divide(f1, f2).Remainder;

	#endregion
}