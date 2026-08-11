using System;

namespace ProjectReal.Number;

/// <summary>
/// Represents a decimal digit.
/// </summary>
public static class Digit
{
	#region Public methods

	/// <summary>
	/// Constructs a <see langword="byte"/> by the given <paramref name="character"/>.
	/// </summary>
	/// <param name="character">A number character.</param>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="character"/> must be a number character.</exception>
	public static byte Create(char character)
	{
		if (!char.IsDigit(character))
			throw new ArgumentOutOfRangeException(nameof(character), character, "The given parameter must be a number character!");

		return (byte)(character - '0');
	}

	/// <summary>
	/// Constructs a <see langword="byte"/> by the given <paramref name="bits"/>.
	/// </summary>
	/// <param name="bits">The binary representation of the number, where the first index represents the lowest value.</param>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> cannot represent a number more than 9.</exception>
	public static byte Create(byte bits)
	{
		return 10 > bits ? bits : throw new ArgumentOutOfRangeException(nameof(bits), bits, "The given parameter cannot represent a number more than 9!");
	}

	/// <summary>
	/// Constructs an array of <see ="byte"/>s with the value of 0.
	/// </summary>
	/// <param name="length">The length of the created array.</param>
	/// <returns>The constructed array.</returns>
	public static byte[] CreateArray(int length) => new byte[length < 1 ? 0 : length];

	/// <summary>
	/// Constructs an array of <see langword="byte"/>s with the value of the specified <paramref name="sample"/>.
	/// </summary>
	/// <param name="length">The length of the created array.</param>
	/// <param name="sample">The value every index of the array will be set to.</param>
	/// <returns>The constructed array.</returns>
	public static byte[] CreateArray(int length, byte sample)
	{
		if (length < 1)
			return [];

		byte[] array = new byte[length];
		Array.Fill(array, sample);

		return array;
	}

	/// <summary>
	/// Removes the tailing 0 values from the given <see langword="byte"/> array.
	/// </summary>
	/// <param name="bytes">The array to trim.</param>
	/// <returns>The trimmed array.</returns>
	public static byte[] TrimEnd(byte[] bytes)
	{
		int i = 0;

		while (++i < bytes.Length && bytes[^i] == 0) { }

		return i == 1 ? bytes : bytes[..^(i - 1)];
	}

	/// <summary>
	/// Compares two <see langword="byte"/>s.
	/// </summary>
	/// <param name="left">The first <see langword="byte"/> to compare.</param>
	/// <param name="right">The second <see langword="byte"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is equal to the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(byte left, byte right) => left == right;

	/// <summary>
	/// Compares two <see langword="byte"/>s.
	/// </summary>
	/// <param name="left">The first <see langword="byte"/> to compare.</param>
	/// <param name="right">The second <see langword="byte"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref name="right"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(byte left, byte right) => left > right;

	/// <summary>
	/// Adds two <see langword="byte"/>s.
	/// </summary>
	/// <param name="left">The first <see langword="byte"/> to add.</param>
	/// <param name="right">The second <see langword="byte"/> to add.</param>
	/// <param name="carry">The carry value.</param>
	/// <returns>The result value and if there was an overflow in a tuple.</returns>
	public static (bool Overflow, byte Digit) Add(byte left, byte right, bool carry = false)
	{
		int result = carry ? left + right + 1 : left + right; 
		bool overflow = 10 <= result;

		return (overflow, (byte)(overflow ? result - 10 : result));
	}

	/// <summary>
	/// Adds one to the given <see langword="byte"/>.
	/// </summary>
	/// <param name="value">The <see langword="byte"/> to add to.</param>
	/// <returns>The result value and if there was an overflow in a tuple.</returns>
	public static (bool Overflow, byte Digit) AddOne(byte value)
	{
		bool overflow = 10 <= ++value;

		return (overflow, (byte)(overflow ? value - 10 : value));
	}

	/// <summary>
	/// Subtracts two <see langword="byte"/>s.
	/// </summary>
	/// <param name="left">The <see langword="byte"/> that represents the minuend.</param>
	/// <param name="right">The <see langword="byte"/> that represents the subtrahend.</param>
	/// <param name="carry">The carry value.</param>
	/// <returns>The result value and if there was a borrow in a tuple.</returns>
	public static (bool Borrow, byte Digit) Subtract(byte left, byte right, bool carry = false)
	{
		int rightBitsPlusCarry = carry ? right + 1 : right;
		bool borrow = rightBitsPlusCarry > left;

		return (borrow, (byte)(borrow ? 10 - (rightBitsPlusCarry - left) : left - rightBitsPlusCarry));
	}

	/// <summary>
	/// Subtracts one from the given <see langword="byte"/>.
	/// </summary>
	/// <param name="value">The <see langword="byte"/> to subtract from.</param>
	/// <returns>The result value and if there was an overflow in a tuple.</returns>
	public static (bool Overflow, byte Digit) SubtractOne(byte value)
	{
		bool overflow = value == 0;

		return (overflow, (byte)(overflow ? 9 : --value));
	}

	/// <summary>
	/// Multiplies two <see langword="byte"/>s.
	/// </summary>
	/// <param name="left">The <see langword="byte"/> that represents the multiplier.</param>
	/// <param name="right">The <see langword="byte"/> that represents the multiplicand.</param>
	/// <returns>The result value and the amount of overflow in a tuple.</returns>
	public static (byte Overflow, byte Digit) Multiply(byte left, byte right)
	{
		int whole = left * right;
		int remainder = whole % 10;
		whole /= 10;

		return ((byte)whole, (byte)remainder);
	}

	/// <summary>
	/// Divides two <see langword="byte"/>s.
	/// </summary>
	/// <param name="left">The <see langword="byte"/> that represents the numerator.</param>
	/// <param name="right">The <see langword="byte"/> that represents the denominator.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="DivideBy0Exception"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static (byte Whole, byte Remainder) Divide(byte left, byte right)
	{
		int whole = left / right;
		int remainder = left % right;

		return ((byte)whole, (byte)remainder);
	}

	#endregion
}