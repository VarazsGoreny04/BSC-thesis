using System;

namespace Project_Real;

/// <summary>
/// Represents a decimal digit using the BCD format.
/// </summary>
public readonly struct Digit
{
	#region Exceptions

	public class ValueOutOfRangeException() : Exception() { }
	public class UnmatchingArrayLengthException() : Exception() { }
	public class SecondValueGreaterException() : Exception() { }

	#endregion

	#region Constants

	public static readonly Digit ZERO = new();
	public static readonly Digit ONE = '1';
	public static readonly Digit TWO = '2';
	public static readonly Digit THREE = '3';
	public static readonly Digit FOUR = '4';
	public static readonly Digit FIVE = '5';
	public static readonly Digit SIX = '6';
	public static readonly Digit SEVEN = '7';
	public static readonly Digit EIGHT = '8';
	public static readonly Digit NINE = '9';

	private const byte TEN = 0b1010;

	#endregion

	#region Fields

	private readonly byte bits;

	#endregion

	#region Properties

	/// <returns>The <see cref="byte"/> that represents <see langword="this"/> <see cref="Digit"/> instance.</returns>
	public readonly byte Bits => bits;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Digit"/> with a value of 0.
	/// </summary>
	public Digit() { }

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

		bits = (byte)number;
	}

	/// <summary>
	/// Constructs a <see cref="Digit"/> by the given <paramref name="bits"/>.
	/// </summary>
	/// <param name="bits">The binary representation of the number, where the first index represents the lowest value.</param>
	/// <exception cref="ValueOutOfRangeException"><paramref name="bits"/> cannot represent a number more than 9.</exception>
	public Digit(byte bits) => this.bits = TEN > bits ? bits : throw new ValueOutOfRangeException();

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
	public static char ToChar(Digit value) => (char)('0' + value.bits);

	/// <summary>
	/// Compares two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Digit"/> to compare.</param>
	/// <param name="right">The second <see cref="Digit"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is equal to the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(Digit left, Digit right) => left.bits == right.bits;

	/// <summary>
	/// Compares two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Digit"/> to compare.</param>
	/// <param name="right">The second <see cref="Digit"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref name="right"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(Digit left, Digit right) => left.bits > right.bits;

	/// <summary>
	/// Adds two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Digit"/> to add.</param>
	/// <param name="right">The second <see cref="Digit"/> to add.</param>
	/// <param name="carry">The carry value.</param>
	/// <returns>The result value and if there was an overflow in a tuple.</returns>
	public static (bool Overflow, Digit Digit) Add(Digit left, Digit right, bool carry = false)
	{
		int result = carry ? left.bits + right.bits + 0b0001 : left.bits + right.bits;

		bool overflow = TEN <= result;

		return (overflow, (byte)(overflow ? result - TEN : result));
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
		int rightBitsPlusCarry = carry ? right.bits + 0b0001 : right.bits;

		bool borrow = rightBitsPlusCarry > left.bits;

		return (borrow, (byte)(borrow ? TEN - (rightBitsPlusCarry - left.bits) : left.bits - rightBitsPlusCarry));
	}

	/// <summary>
	/// Multiplies two <see cref="Digit"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Digit"/> that represents the multiplier.</param>
	/// <param name="right">The <see cref="Digit"/> that represents the multiplicand.</param>
	/// <returns>The result value and the amount of overflow in a tuple.</returns>
	public static (Digit Overflow, Digit Digit) Multiply(Digit left, Digit right)
	{
		int whole = left.bits * right.bits;
		int remainder = whole % TEN;
		whole /= TEN;

		return ((byte)whole, (byte)remainder);
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
		int whole = left.bits / right.bits;
		int remainder = left.bits % right.bits;

		return ((byte)whole, (byte)remainder);
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
	/// Returns the hash value representing <see langword="this"/> instance.
	/// </summary>
	/// <returns>The hash value computed by the <see cref="Bits"/> property.</returns>
	public override int GetHashCode() => bits;

	#endregion

	#region Operators

	public static implicit operator Digit(char value) => new(value);
	public static implicit operator Digit(byte value) => new(value);
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