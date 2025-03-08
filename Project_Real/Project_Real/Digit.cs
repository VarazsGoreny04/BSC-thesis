using System.Collections.Immutable;

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

	public readonly ImmutableArray<bool> Bits;

	private static readonly ImmutableArray<bool> TEN = [false, true, false, true];

	#endregion

	#region Properties

	public readonly bool this[Index i] => Bits[i];

	#endregion

	#region Constructors

	public Digit()
	{
		Bits = ImmutableArray.Create(new bool[LENGTH]);
	}

	public Digit(char character)
	{
		short number = (short)(character - '0');

		if (number < 0 || number > 9)
			throw new ValueOutOfRangeException();

		bool[] bits = new bool[LENGTH];
		short pow = 0;

		do
		{
			bits[pow++] = number % 2 == 1;
			number /= 2;
		} while (number > 0);

		Bits = ImmutableArray.Create(bits);
	}
	public Digit(bool[] bitArray)
	{
		ImmutableArray<bool> immutableBitArray = ImmutableArray.Create(bitArray);

		Bits = BitGreaterThan(TEN, immutableBitArray) ? immutableBitArray : throw new ValueOutOfRangeException();
	}

	public Digit(ImmutableArray<bool> bitArray)
	{
		Bits = BitGreaterThan(TEN, bitArray) ? bitArray : throw new ValueOutOfRangeException();
	}

	#endregion

	#region Private methods

	private static bool BitEquals(ImmutableArray<bool> b1, ImmutableArray<bool> b2)
	{
		if (b1.Length != b2.Length)
			throw new UnmatchingArrayLengthException();

		short i = 0;
		while (++i < b1.Length && b1[^i] == b2[^i]) { }

		return i == b1.Length && b1[^i] == b2[^i];
	}

	private static bool BitGreaterThan(ImmutableArray<bool> b1, ImmutableArray<bool> b2)
	{
		if (b1.Length != b2.Length)
			throw new UnmatchingArrayLengthException();

		short i = 0;
		while (++i < b1.Length && b1[^i] == b2[^i]) { }

		return b1[^i] && !b2[^i];
	}

	private static ImmutableArray<bool> TwosComplement(ImmutableArray<bool> b)
	{
		bool[] result = new bool[b.Length];

		for (short i = 0; i < b.Length; ++i)
			result[i] = !b[i];

		return BitAdd(ImmutableArray.Create(result), ImmutableArray.Create(new bool[b.Length]), true).Bits;
	}

	private static (bool OverFlow, ImmutableArray<bool> Bits) BitAdd(ImmutableArray<bool> b1, ImmutableArray<bool> b2, bool carry = false)
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

		return (carry, ImmutableArray.Create(result));
	}

	private static ImmutableArray<bool> BitSubstract(ImmutableArray<bool> b1, ImmutableArray<bool> b2)
	{
		if (BitGreaterThan(b2, b1))
			throw new SecondValueGreaterException();

		return BitAdd(b1, TwosComplement(b2)).Bits;
	}

	private static ImmutableArray<bool> BitMultiply(ImmutableArray<bool> b1, ImmutableArray<bool> b2)
	{
		ImmutableArray<bool> temp;
		bool[] result = new bool[b1.Length + b2.Length - 1];
		int resultCutLength = result.Length - b1.Length;

		for (short i = 0; i < b2.Length; ++i)
		{
			if (b2[^(i + 1)])
			{
				temp = BitAdd(ImmutableArray.Create(result[resultCutLength..]), ImmutableArray.Create([.. b1, .. new bool[i]])).Bits;
				Array.Copy(temp.ToArray(), 0, result, resultCutLength, temp.Length);
			}
			--resultCutLength;
		}

		return ImmutableArray.Create(result);
	}

	private static (ImmutableArray<bool> Whole, ImmutableArray<bool> Remainder) BitDivide(ImmutableArray<bool> b1, ImmutableArray<bool> b2)
	{
		short trueIndex = (short)b2.Length;

		while (--trueIndex >= 0 && !b2[trueIndex]) { }

		if (trueIndex < 0)
			throw new DivideByZeroException();

		ImmutableArray<bool> minuend, suprahend, difference;
		bool[] denominatorEnd = b2.Take(trueIndex + 1).ToArray();
		short lenDiff = (short)(b1.Length - denominatorEnd.Length);
		bool[] whole = new bool[lenDiff + 1];
		bool[] remainder = [.. b1];

		for (short i = lenDiff; i >= 0; --i)
		{
			minuend = ImmutableArray.Create(remainder[i..]);
			suprahend = [.. denominatorEnd, .. new bool[lenDiff - i]];

			if (!BitGreaterThan(suprahend, minuend))
			{
				difference = BitAdd(minuend, TwosComplement(suprahend)).Bits;
				Array.Copy(difference.ToArray(), 0, remainder, i, difference.Length);
				whole[i] = true;
			}
		}

		return (ImmutableArray.Create(whole), ImmutableArray.Create(remainder));
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return ToChar(this).ToString();
	}

	public static char ToChar(Digit d)
	{
		char number = '0';

		for (byte i = 0; i < LENGTH; ++i)
		{
			if (d.Bits[i])
				number += (char)Math.Pow(2d, i);
		}

		return number;
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
		(carry, ImmutableArray<bool> result) = BitAdd(d1.Bits, d2.Bits, carry);

		if (carry || !BitGreaterThan(TEN, result))
			return (true, new Digit(BitAdd(result, [false, true, true, false]).Bits));
		else
			return (false, new Digit(result));
	}

	public static (bool Borrow, Digit Digit) Substract(Digit d1, Digit d2, bool carry = false)
	{
		ImmutableArray<bool> d2PlusCarry = carry ? BitAdd(d2.Bits, ImmutableArray.Create(new bool[d2.Bits.Length]), carry).Bits : d2.Bits;

		if (!BitGreaterThan(d2PlusCarry, d1.Bits))
			return (false, new Digit(BitSubstract(d1.Bits, d2PlusCarry)));
		else
			return (true, new Digit(BitSubstract(TEN, BitSubstract(d2PlusCarry, d1.Bits))));
	}

	public static (Digit OverFlow, Digit Digit) Multiply(Digit d1, Digit d2)
	{
		(ImmutableArray<bool> whole, ImmutableArray<bool> remainder) = BitDivide(BitMultiply(d1.Bits, d2.Bits), TEN);

		return (new Digit(ImmutableArray.Create([.. whole, .. new bool[LENGTH - whole.Length]])), new Digit(remainder.Take(LENGTH).ToArray()));
	}

	public static (Digit Whole, Digit Remainder) Divide(Digit d1, Digit d2)
	{
		(ImmutableArray<bool> whole, ImmutableArray<bool> remainder) = BitDivide(d1.Bits, d2.Bits);

		return (new Digit(ImmutableArray.Create([.. whole, .. new bool[LENGTH - whole.Length]])), new Digit(remainder));
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