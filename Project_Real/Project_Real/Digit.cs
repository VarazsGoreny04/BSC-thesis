using System;
using System.Collections.Immutable;
using System.Linq;

namespace Project_Real;

public readonly struct Digit
{
	#region Exceptions

	public class ValueOutOfRangeException() : Exception() { }
	public class UnmatchingArrayLengthException() : Exception() { }
	public class SecondValueGreaterException() : Exception() { }

	#endregion

	#region Constants

	/// <value>Constant <see cref="LENGTH"/> represents the number of booleans used to create a <see cref="Digit"/>.</value>
	public const byte LENGTH = 4;

	public static readonly Digit ZERO = new();
	public static readonly Digit ONE = new('1');
	public static readonly Digit TWO = new('2');
	public static readonly Digit THREE = new('3');
	public static readonly Digit FOUR = new('4');
	public static readonly Digit FIVE = new('5');
	public static readonly Digit SIX = new('6');
	public static readonly Digit SEVEN = new('7');
	public static readonly Digit EIGHT = new('8');
	public static readonly Digit NINE = new('9');

	#endregion

	#region Fields

	private readonly ImmutableArray<bool> bits;

	private static readonly ImmutableArray<bool> TEN = [false, true, false, true];

	#endregion

	#region Properties

	/// <returns>The booleans used to represent a <see cref="Digit"/>.</returns>
	public readonly ImmutableArray<bool> Bits => bits;

	/// <returns>The boolean at the specified <see cref="Index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> cannot be less than 0.</exception>
	public readonly bool this[Index index] => bits[index];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Digit"/> with a value of 0.
	/// </summary>
	public Digit() => bits = ImmutableArray.Create(new bool[LENGTH]);

	/// <summary>
	/// Constructs a <see cref="Digit"/> by the given <paramref name="character"/>.
	/// </summary>
	/// <param name="character">A number character.</param>
	/// <exception cref="ValueOutOfRangeException"><paramref name="character"/> must be a number character.</exception>
	public Digit(char character)
	{
		sbyte number = (sbyte)(character - '0');

		if (number < 0 || number > 9)
			throw new ValueOutOfRangeException();

		bool[] bits = new bool[LENGTH];
		sbyte pow = 0;

		do
		{
			bits[pow++] = number % 2 == 1;
			number /= 2;
		} while (number > 0);

		this.bits = ImmutableArray.Create(bits);
	}

	/// <summary>
	/// Constructs a <see cref="Digit"/> by the given <paramref name="bitArray"/>.
	/// </summary>
	/// <param name="bitArray">The binary representation of the number, where the first index represents the lowest value.</param>
	/// <exception cref="UnmatchingArrayLengthException"><paramref name="bitArray"/> does not match the specified <see cref="LENGTH"/> length.</exception>
	/// <exception cref="ValueOutOfRangeException"><paramref name="bitArray"/> cannot represent a number more than 9.</exception>
	public Digit(bool[] bitArray)
	{
		if (bitArray.Length != LENGTH)
			throw new UnmatchingArrayLengthException();

		ImmutableArray<bool> immutableBitArray = ImmutableArray.Create(bitArray);

		bits = BitGreaterThan(TEN, immutableBitArray) ? immutableBitArray : throw new ValueOutOfRangeException();
	}

	/// <summary>
	/// Constructs a <see cref="Digit"/> by the given <paramref name="bitArray"/>.
	/// </summary>
	/// <param name="bitArray">The binary representation of the number, where the first index represents the lowest value.</param>
	/// <exception cref="UnmatchingArrayLengthException"><paramref name="bitArray"/> does not match the specified <see cref="LENGTH"/> length.</exception>
	/// <exception cref="ValueOutOfRangeException"><paramref name="bitArray"/> cannot represent a number more than 9.</exception>
	public Digit(ImmutableArray<bool> bitArray)
	{
		if (bitArray.Length != LENGTH)
			throw new UnmatchingArrayLengthException();

		bits = BitGreaterThan(TEN, bitArray) ? bitArray : throw new ValueOutOfRangeException();
	}

	#endregion

	#region Private methods

	/// <summary>
	/// Compares two <see cref="ImmutableArray{bool}"/> structs of the same length.
	/// </summary>
	/// <param name="left">The first <see cref="ImmutableArray{bool}"/> struct.</param>
	/// <param name="right">The second <see cref="ImmutableArray{bool}"/> struct.</param>
	/// <returns>
	/// <see langword="true"/> if every index from <paramref name="left"/> matched the corresponding index in <paramref name="right"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="UnmatchingArrayLengthException">Length of <paramref name="left"/> does not match the length of <paramref name="right"/>.</exception>
	private static bool BitEquals(ImmutableArray<bool> left, ImmutableArray<bool> right)
	{
		if (left.Length != right.Length)
			throw new UnmatchingArrayLengthException();

		sbyte i = 0;
		while (++i < left.Length && left[^i] == right[^i]) { }

		return i == left.Length && left[^i] == right[^i];
	}

	/// <summary>
	/// Compares two <see cref="ImmutableArray{bool}"/> structs of the same length.
	/// </summary>
	/// <param name="left">The first <see cref="ImmutableArray{bool}"/> struct.</param>
	/// <param name="right">The second <see cref="ImmutableArray{bool}"/> struct.</param>
	/// <returns>
	/// <see langword="true"/> if the two arrays are not identical and at the rightmost difference <paramref name="left"/> is <see langword="true"/> 
	/// and  <paramref name="right"/> is <see langword="false"/>; otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="UnmatchingArrayLengthException">Length of <paramref name="left"/> does not match the length of <paramref name="right"/>.</exception>
	private static bool BitGreaterThan(ImmutableArray<bool> left, ImmutableArray<bool> right)
	{
		if (left.Length != right.Length)
			throw new UnmatchingArrayLengthException();

		sbyte i = 0;
		while (++i < left.Length && left[^i] == right[^i]) { }

		return left[^i] && !right[^i];
	}

	/// <summary>
	/// Constructs the two's complement of a <see cref="ImmutableArray{bool}"/> struct.
	/// </summary>
	/// <param name="value">The <see cref="ImmutableArray{bool}"/> struct.</param>
	/// <returns>The two's complement of <paramref name="value"/> in a new <see cref="ImmutableArray{bool}"/> struct.</returns>
	private static ImmutableArray<bool> TwosComplement(ImmutableArray<bool> value)
	{
		bool[] result = new bool[value.Length];

		for (sbyte i = 0; i < value.Length; ++i)
			result[i] = !value[i];

		return BitAdd(ImmutableArray.Create(result), ImmutableArray.Create(new bool[value.Length]), true).Bits;
	}

	/// <summary>
	/// Adds two <see cref="ImmutableArray{bool}"/> structs of the same length.
	/// </summary>
	/// <param name="left">The first <see cref="ImmutableArray{bool}"/> struct to add.</param>
	/// <param name="right">The second <see cref="ImmutableArray{bool}"/> struct to add.</param>
	/// <param name="carry">The carry bit.</param>
	/// <returns>The bits of the operation and if there was an overflow in a tuple.</returns>
	/// <exception cref="UnmatchingArrayLengthException">Length of <paramref name="left"/> does not match the length of <paramref name="right"/>.</exception>
	private static (bool OverFlow, ImmutableArray<bool> Bits) BitAdd(ImmutableArray<bool> left, ImmutableArray<bool> right, bool carry = false)
	{
		if (left.Length != right.Length)
			throw new UnmatchingArrayLengthException();

		bool iLeft, iRight;
		bool[] result = new bool[left.Length];

		for (sbyte i = 0; i < result.Length; ++i)
		{
			iLeft = left[i];
			iRight = right[i];

			if (iLeft && iRight && carry)
				result[i] = true;
			else if (iLeft && iRight)
				carry = true;
			else if ((iLeft || iRight) && carry) { }
			else if (iLeft || iRight || carry)
			{
				result[i] = true;
				carry = false;
			}
		}

		return (carry, ImmutableArray.Create(result));
	}

	/// <summary>
	/// Subtracts two <see cref="ImmutableArray{bool}"/> structs of the same length.
	/// </summary>
	/// <param name="left">The <see cref="ImmutableArray{bool}"/> struct that represents the minuend.</param>
	/// <param name="right">The <see cref="ImmutableArray{bool}"/> struct that represents the subtrahend.</param>
	/// <returns>The bits of the operation.</returns>
	/// <exception cref="UnmatchingArrayLengthException">Length of <paramref name="left"/> does not match the length of <paramref name="right"/>.</exception>
	/// <exception cref="SecondValueGreaterException"><paramref name="left"/> cannot be bigger than <paramref name="right"/>.</exception>
	private static ImmutableArray<bool> BitSubtract(ImmutableArray<bool> left, ImmutableArray<bool> right)
	{
		if (BitGreaterThan(right, left))
			throw new SecondValueGreaterException();

		return BitAdd(left, TwosComplement(right)).Bits;
	}

	/// <summary>
	/// Multiplies two <see cref="ImmutableArray{bool}"/> structs.
	/// </summary>
	/// <param name="left">The <see cref="ImmutableArray{bool}"/> struct that represents the multiplier.</param>
	/// <param name="right">The <see cref="ImmutableArray{bool}"/> struct that represents the multiplicand.</param>
	/// <returns>
	/// The bits of the operation. The length of the result array is <c>(<paramref name="left"/>.Length + <paramref name="right"/>.Length - 1)</c>.
	/// </returns>
	private static ImmutableArray<bool> BitMultiply(ImmutableArray<bool> left, ImmutableArray<bool> right)
	{
		ImmutableArray<bool> temp;
		bool[] result = new bool[left.Length + right.Length - 1];
		int resultCutLength = result.Length - left.Length;

		for (sbyte i = 0; i < right.Length; ++i)
		{
			if (right[^(i + 1)])
			{
				temp = BitAdd(ImmutableArray.Create(result[resultCutLength..]), ImmutableArray.Create([.. left, .. new bool[i]])).Bits;
				Array.Copy(temp.ToArray(), 0, result, resultCutLength, temp.Length);
			}
			--resultCutLength;
		}

		return ImmutableArray.Create(result);
	}

	/// <summary>
	/// Divides two <see cref="ImmutableArray{bool}"/> structs.
	/// </summary>
	/// <param name="left">The <see cref="ImmutableArray{bool}"/> struct that represents the numerator.</param>
	/// <param name="right">The <see cref="ImmutableArray{bool}"/> struct that represents the denominator.</param>
	/// <returns>The bits of the operation in two parts: the whole and the remainder in a tuple.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	private static (ImmutableArray<bool> Whole, ImmutableArray<bool> Remainder) BitDivide(ImmutableArray<bool> left, ImmutableArray<bool> right)
	{
		sbyte trueIndex = (sbyte)right.Length;

		while (--trueIndex >= 0 && !right[trueIndex]) { }

		if (trueIndex < 0)
			throw new DivideByZeroException();

		ImmutableArray<bool> minuend, subtrahend, difference;
		bool[] denominatorEnd = right.ToArray()[..(trueIndex + 1)];
		sbyte lenDiff = (sbyte)(left.Length - denominatorEnd.Length);
		bool[] whole = new bool[lenDiff + 1];
		bool[] remainder = [.. left];

		for (sbyte i = lenDiff; i >= 0; --i)
		{
			minuend = ImmutableArray.Create(remainder[i..]);
			subtrahend = [.. denominatorEnd, .. new bool[lenDiff - i]];

			if (!BitGreaterThan(subtrahend, minuend))
			{
				difference = BitAdd(minuend, TwosComplement(subtrahend)).Bits;
				Array.Copy(difference.ToArray(), 0, remainder, i, difference.Length);
				whole[i] = true;
			}
		}

		return (ImmutableArray.Create(whole), ImmutableArray.Create(remainder));
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Constructs an array of <see cref="Digit"/>s with the value of 0.
	/// </summary>
	/// <param name="length">The length of the created array.</param>
	/// <returns>The constructed array.</returns>
	public static Digit[] CreateArray(int length) => CreateArray(length, ZERO);

	/// <summary>
	/// Constructs an array of <see cref="Digit"/>s with the value of the specified <paramref name="sample"/>.
	/// </summary>
	/// <param name="length">The length of the created array.</param>
	/// <param name="sample">The value every index of the array will be set to.</param>
	/// <returns>The constructed array.</returns>
	public static Digit[] CreateArray(int length, Digit sample)
	{
		if (length < 1)
			return [];

		Digit[] array = new Digit[length];
		Array.Fill(array, sample);

		return array;
	}

	/// <summary>
	/// Removes the tailing 0 values from the given <see cref="Digit"/> array.
	/// </summary>
	/// <param name="digits">The array to trim.</param>
	/// <returns>The trimmed array.</returns>
	public static Digit[] TrimEnd(Digit[] digits)
	{
		int i = 0;

		while (++i < digits.Length && digits[^i] == ZERO) { }

		return i == 1 ? digits : digits[..^(i - 1)];
	}

	/// <summary>
	/// Returns a string that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A character form 0 to 9 as a <see langword="string"/>.</returns>
	public override string ToString() => ToChar(this).ToString();

	/// <summary>
	/// Converts the numeric value of the given <see cref="Digit"/> instance to its equivalent <see langword="char"/> representation.
	/// </summary>
	/// <param name="value"></param>
	/// <returns>A character form 0 to 9.</returns>
	public static char ToChar(Digit value) 
	{
		char number = '0';

		for (sbyte i = 0; i < LENGTH; ++i)
		{
			if (value.bits[i])
				number += (char)Math.Pow(2d, i);
		}

		return number;
	}

	/// <summary>
	/// Compares two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Digit"/> to compare.</param>
	/// <param name="right">The second <see cref="Digit"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is equal to the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(Digit left, Digit right) => BitEquals(left.bits, right.bits);

	/// <summary>
	/// Compares two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Digit"/> to compare.</param>
	/// <param name="right">The second <see cref="Digit"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref name="right"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(Digit left, Digit right) => BitGreaterThan(left.bits, right.bits);

	/// <summary>
	/// Adds two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Digit"/> to add.</param>
	/// <param name="right">The second <see cref="Digit"/> to add.</param>
	/// <param name="carry">The carry value.</param>
	/// <returns>The result value and if there was an overflow in a tuple.</returns>
	public static (bool Overflow, Digit Digit) Add(Digit left, Digit right, bool carry = false)
	{
		(carry, ImmutableArray<bool> result) = BitAdd(left.bits, right.bits, carry);

		if (carry || !BitGreaterThan(TEN, result))
			return (true, new Digit(BitAdd(result, [false, true, true, false]).Bits));
		else
			return (false, new Digit(result));
	}

	/// <summary>
	/// Subtracts two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Digit"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Digit"/> that represents the subtrahend.</param>
	/// <param name="carry">The carry value.</param>
	/// <returns>The result value and if there was a borrow in a tuple.</returns>
	public static (bool Borrow, Digit Digit) Subtract(Digit left, Digit right, bool carry = false)
	{
		ImmutableArray<bool> d2PlusCarry = carry ? BitAdd(right.bits, ImmutableArray.Create(new bool[right.bits.Length]), carry).Bits : right.bits;

		if (!BitGreaterThan(d2PlusCarry, left.bits))
			return (false, new Digit(BitSubtract(left.bits, d2PlusCarry)));
		else
			return (true, new Digit(BitSubtract(TEN, BitSubtract(d2PlusCarry, left.bits))));
	}

	/// <summary>
	/// Multiplies two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Digit"/> that represents the multiplier.</param>
	/// <param name="right">The <see cref="Digit"/> that represents the multiplicand.</param>
	/// <returns>The result value and the amount of overflow in a tuple.</returns>
	public static (Digit Overflow, Digit Digit) Multiply(Digit left, Digit right)
	{
		(ImmutableArray<bool> whole, ImmutableArray<bool> remainder) = BitDivide(BitMultiply(left.bits, right.bits), TEN);

		return (new Digit(ImmutableArray.Create([.. whole, .. new bool[LENGTH - whole.Length]])), new Digit(remainder.Take(LENGTH).ToArray()));
	}

	/// <summary>
	/// Divides two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Digit"/> that represents the numerator.</param>
	/// <param name="right">The <see cref="Digit"/> that represents the denominator.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static (Digit Whole, Digit Remainder) Divide(Digit left, Digit right)
	{
		(ImmutableArray<bool> whole, ImmutableArray<bool> remainder) = BitDivide(left.bits, right.bits);

		return (new Digit(ImmutableArray.Create([.. whole, .. new bool[LENGTH - whole.Length]])), new Digit(remainder));
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Digit"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Digit digit && this == digit;

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Digit(char value) => new(value);
	public static bool operator ==(Digit left, Digit right) => Equals(left, right);
	public static bool operator !=(Digit left, Digit right) => !Equals(left, right);
	public static bool operator >(Digit left, Digit right) => GreaterThan(left, right);
	public static bool operator <(Digit left, Digit right) => GreaterThan(right, left);
	public static bool operator >=(Digit left, Digit right) => !GreaterThan(right, left);
	public static bool operator <=(Digit left, Digit right) => !GreaterThan(left, right);
	public static Digit operator +(Digit left, Digit right) => Add(left, right).Digit;
	public static Digit operator -(Digit left, Digit right) => Subtract(left, right).Digit;
	public static Digit operator *(Digit left, Digit right) => Multiply(left, right).Digit;
	public static Digit operator /(Digit left, Digit right) => Divide(left, right).Whole;
	public static Digit operator %(Digit left, Digit right) => Divide(left, right).Remainder;

	#endregion
}