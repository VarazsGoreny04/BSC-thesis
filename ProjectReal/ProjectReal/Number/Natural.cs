using ProjectReal.NumberSet;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

namespace ProjectReal.Number;

/// <summary>
/// Represents a natural number.
/// </summary>
public class Natural : 
	IComparisonOperators<Natural, Natural, bool>, 
	IEqualityOperators<Natural, Natural, bool>, 
	IIncrementOperators<Natural>,
	IDecrementOperators<Natural>, 
	IAdditionOperators<Natural, Natural, Natural>,
	ISubtractionOperators<Natural, Natural, Natural>,
	IMultiplyOperators<Natural, Natural, Natural>,
	IDivisionOperators<Natural, Natural, Natural>, 
	IModulusOperators<Natural, Natural, Natural>,
	IPowerOperations<Natural, Natural, Natural>,
	IRootOperations<Natural, Natural, Natural>,
	IAdditiveIdentity<Natural, Natural>,
	IMultiplicativeIdentity<Natural, Natural>,
	IParsable<Natural>
{
	#region Fields

	private readonly int length;
	private readonly bool isZero;
	private readonly ImmutableArray<byte> digits;

	#endregion

	#region Properties

	public static Natural AdditiveIdentity => 0;

	public static Natural MultiplicativeIdentity => 1;

	/// <returns>The number of <see cref="byte"/>s used to represent <see langword="this"/> <see cref="Natural"/>.</returns>
	public int Length => length;

	/// <summary>
	/// Returns whether <see langword="this"/> is equal to 0.
	/// </summary>
	/// <returns><see langword="true"/> if <see langword="this"/> is equal to 0; otherwise, <see langword="false"/>.</returns>
	public bool IsZero => isZero;

	/// <returns>The <see cref="ImmutableArray{byte}"/> used to represent <see langword="this"/> <see cref="Natural"/>.</returns>
	public ImmutableArray<byte> Digits => digits;

	/// <returns>The <see cref="byte"/> at the specified <see cref="Index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> must be within the bounds of the digits.</exception>
	public byte this[Index index] => digits[index];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Natural"/> with a value of 0.
	/// </summary>
	public Natural()
	{
		length = 1;
		digits = [0];
		isZero = true;
	}

	/// <summary>
	/// Constructs a <see cref="Natural"/> by the given <see langword="string"/> parameter.
	/// </summary>
	/// <param name="number">A <see langword="string"/> of 0 to 9 characters.</param>
	/// <exception cref="ArgumentException"><paramref name="number"/> is not a valid number format.</exception>
	public Natural(string number)
	{
		if (number.Length < 1)
			throw new ArgumentException("The given string parameter must not be empty!", nameof(number));

		number = number.TrimStart('0');

		isZero = number.Length < 1;

		if (isZero)
			number = "0";

		length = number.Length;
		byte[] digits = new byte[length];

		try
		{
			for (int i = 0; i < length; ++i)
				digits[i] = Digit.Create(number[^(i + 1)]);
		}
		catch (ArgumentOutOfRangeException)
		{
			throw new ArgumentException("The given string parameter can only contain number characters of 0-9!", nameof(number));
		}

		this.digits = ImmutableArray.Create(digits);
	}

	/// <summary>
	/// Constructs a <see cref="Natural"/> by the given <paramref name="number"/>.
	/// </summary>
	/// <param name="number">An array of <see cref="byte"/>s, where the first index represents the lowest value.</param>
	/// <exception cref="ArgumentException"><paramref name="number"/> cannot be null or empty.</exception>
	public Natural(byte[] number)
	{
		if (number.Length < 1)
			throw new ArgumentException("The given array parameter must not be empty!", nameof(number));

		digits = ImmutableArray.Create(Digit.TrimEnd(number));
		length = digits.Length;
		isZero = length == 1 && digits[0] == 0;
	}

	/// <summary>
	/// Constructs a <see cref="Natural"/> by the given <see cref="byte"/>.
	/// </summary>
	public Natural(byte value) : this([value]) { }

	/// <summary>
	/// Constructs a <see cref="Natural"/> by the given <paramref name="number"/>.
	/// </summary>
	/// <param name="number">An unsigned integer value.</param>
	public Natural(uint number) : this(number.ToString()) { }

	#endregion

	#region Internal methods

	/// <summary>
	/// Calculates one iteration of the square root.
	/// </summary>
	/// <param name="root">The root from the previous calculations.</param>
	/// <param name="remainder">The remainder from the previous calculations.</param>
	/// <returns>The result of this iteration.</returns>
	internal static (Natural Root, Natural Remainder) CalculateTwoRootDigits(Natural root, Natural remainder)
	{
		byte xTry;
		Natural test;

		if (!remainder.IsZero)
		{
			Natural rootTimesTwo = root * 2;
			xTry = 5;

			test = new Natural([xTry, .. rootTimesTwo.digits]) * xTry;
			if (test <= remainder)
				xTry = 0;

			byte j = 0;
			do
			{
				xTry = Digit.SubtractOne(xTry).Digit;
				test = new Natural([xTry, .. rootTimesTwo.digits]) * xTry;
			} while (++j < 5 && test > remainder);

			remainder -= test;
		}
		else
			xTry = 0;

		return (new Natural([xTry, .. root.digits]), remainder);
	}

	/// <summary>
	/// Calculates one iteration of the <paramref name="degree"/>-th root.
	/// </summary>
	/// <param name="root">The root from the previous calculations.</param>
	/// <param name="remainder">The remainder from the previous calculations.</param>
	/// <param name="degree">The degree.</param>
	/// <param name="degree">The factorial of the degree.</param>
	/// <returns>The result of the calculation.</returns>
	internal static (Natural Root, Natural Remainder) CalculateNRootDigits(Natural root, Natural remainder, Natural degree, Natural degreeFactorial)
	{
		byte xTry;
		Natural test, degUp, binomial;

		if (!remainder.IsZero)
		{
			xTry = 5;
			test = new();

			Natural degDown = degree;
			while (!degDown.IsZero)
			{
				degDown -= 1;

				degUp = degree - degDown;
				binomial = degreeFactorial / (Factorial(degDown) * Factorial(degUp));

				test += new Natural([.. Digit.CreateArray((int)ToUInt32(degDown)), .. (binomial * (root ^ degDown) * (xTry ^ degUp)).digits]);
			}

			if (test <= remainder)
				xTry = 0;

			byte j = 0;
			do
			{
				xTry = Digit.SubtractOne(xTry).Digit;
				test = new();

				degDown = degree;
				while (!degDown.IsZero)
				{
					degDown -= 1;

					degUp = degree - degDown;
					binomial = degreeFactorial / (Factorial(degDown) * Factorial(degUp));

					test += new Natural([.. Digit.CreateArray((int)ToUInt32(degDown)), .. (binomial * (root ^ degDown) * (xTry ^ degUp)).digits]);
				}
			} while (++j < 10 && test > remainder);

			remainder -= test;
		}
		else
			xTry = 0;

		return (new Natural([xTry, .. root.digits]), remainder);
	}

	/// <summary>
	/// Calculates the greatest common divisor of the given two <see cref="Natural"/> numbers using the Euclidean algorithm.
	/// </summary>
	/// <param name="a">The first number.</param>
	/// <param name="b">The second number.</param>
	/// <returns>The greatest common divisor.</returns>
	public static Natural GreatestCommonDivisor(Natural a, Natural b)
	{
		Natural temp;

		while (!b.IsZero)
		{
			temp = b;
			b = a % b;
			a = temp;
		}

		return a;
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a <see cref="string"/> that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Natural"/> number as a <see langword="string"/>.</returns>
	public override string ToString()
	{
		string number = string.Empty;

		for (int i = digits.Length - 1; i >= 0; --i)
			number += digits[i];

		return number;
	}

	/// <summary>
	/// Parses a <see cref="string"/> into a <see cref="Natural"/> instance.
	/// </summary>
	/// <param name="s">The <see cref="string"/> to parse.</param>
	/// <param name="_">This parameter is unused.</param>
	/// <returns>The created instance.</returns>
	/// <exception cref="ArgumentException">The <see cref="string"/> must be accepted by the constructor.</exception>
	public static Natural Parse(string s, IFormatProvider? _ = null) => new(s);

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Natural result)
	{
		if (s is null)
		{
			result = null;
			return false;
		}

		try
		{
			result = Parse(s, provider);
			return true;
		}
		catch (Exception)
		{
			result = null;
			return false;
		}
	}

	/// <summary>
	/// Returns an unsigned integer that represents the given <paramref name="value"/>.
	/// </summary>
	/// <param name="value">The <see cref="Natural"/> to convert.</param>
	/// <returns>A <see cref="Natural"/> number as an <see langword="uint"/>.</returns>
	/// <exception cref="OverflowException"><paramref name="value"/> cannot be greater than than <see cref="uint.MaxValue"/>.</exception>
	public static uint ToUInt32(Natural value) => Convert.ToUInt32(value.ToString());

	/// <summary>
	/// Compares two <see cref="Natural"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Natural"/> to compare.</param>
	/// <param name="right">The second <see cref="Natural"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is equal to the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(Natural left, Natural right)
	{
		if (left.Length != right.Length)
			return false;

		int i = left.Length;

		while (--i > 0 && left[i] == right[i]) { }

		return i == 0 && left[0] == right[0];
	}

	/// <summary>
	/// Compares two <see cref="Natural"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Natural"/> to compare.</param>
	/// <param name="right">The second <see cref="Natural"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref name="right"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(Natural left, Natural right)
	{
		if (left.Length != right.Length)
			return left.Length > right.Length;

		int i = left.Length;

		while (--i > 0 && left[i] == right[i]) { }

		return left[i] > right[i];
	}

	/// <summary>
	/// Adds two <see cref="Natural"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Natural"/> to add.</param>
	/// <param name="right">The second <see cref="Natural"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Natural Add(Natural left, Natural right)
	{
		if (left.Length < right.Length)
			(left, right) = (right, left);

		bool carry = false;
		byte[] result = [.. left.digits];

		for (int i = 0; i < right.Length; ++i)
			(carry, result[i]) = Digit.Add(left[i], right[i], carry);

		for (int i = right.Length; i < left.length && carry; ++i)
			(carry, result[i]) = Digit.Add(left[i], 0, carry);

		return carry ? new Natural([.. result, 1]) : result;
	}

	/// <summary>
	/// Subtracts two <see cref="Natural"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Natural"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Natural"/> that represents the subtrahend.</param>
	/// <returns>The result value and if there was a swap in a tuple.</returns>
	public static (bool Swap, Natural Value) Subtract(Natural left, Natural right)
	{
		bool swap = right > left;

		if (swap)
			(left, right) = (right, left);

		bool carry = false;
		byte[] result = [.. left.digits];

		for (int i = 0; i < right.Length; ++i)
			(carry, result[i]) = Digit.Subtract(left[i], right[i], carry);

		for (int i = right.Length; i < left.length && carry; ++i)
			(carry, result[i]) = Digit.Subtract(left[i], 0, carry);

		return (swap, result);
	}

	/// <summary>
	/// Multiplies two <see cref="Natural"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Natural"/> that represents the multiplier.</param>
	/// <param name="right">The <see cref="Natural"/> that represents the multiplicand.</param>
	/// <returns>The result of the calculation.</returns>
	public static Natural Multiply(Natural left, Natural right)
	{
		if (right > left)
			(left, right) = (right, left);

		if (right.isZero)
			return new Natural();
		else if (right == 1)
			return left;

		Natural result = new();
		byte[] temp;
		byte overflowD, digit;
		bool overflowB;
		int addedIndex;

		for (int rightStep = 0; rightStep < right.Length; ++rightStep)
		{
			if (right[rightStep] == 0)
				continue;

			temp = new byte[left.Length + rightStep + 1];

			addedIndex = rightStep;

			for (int leftStep = 0; leftStep < left.Length; ++leftStep)
			{
				(overflowD, digit) = Digit.Multiply(left[leftStep], right[rightStep]);
				(overflowB, temp[addedIndex]) = Digit.Add(temp[addedIndex], digit);
				++addedIndex;
				temp[addedIndex] = Digit.Add(overflowD, 0, overflowB).Digit;
			}

			result += temp;
		}

		return result;
	}

	/// <summary>
	/// Divides two <see cref="Natural"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Natural"/> that represents the numerator.</param>
	/// <param name="right">The <see cref="Natural"/> that represents the denominator.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static (Natural Whole, Natural Remainder) Divide(Natural left, Natural right)
	{
		if (right.isZero)
			throw new DivideByZeroException("The divisor cannot be 0, as it is not mathematically meaningful!");
		else if (left.Length < right.Length)
			return (0, left);
		else if (right == 1)
			return (left, 0);

		int initialNextChunkLength;
		Natural nextChunk = new();
		byte[] remainder = [.. left.digits];
		byte[] result = Digit.CreateArray(left.Length - right.Length + 1);

		int stepBack = left.Length - right.Length;
		while (stepBack >= 0)
		{
			nextChunk = remainder.Skip(stepBack).ToArray();
			initialNextChunkLength = nextChunk.Length;

			while (right <= nextChunk)
			{
				nextChunk -= right;
				result[stepBack] += 1;
			}

			Array.Copy(nextChunk.digits.ToArray(), 0, remainder, stepBack, nextChunk.Length);
			Array.Fill(remainder, (byte)0, stepBack + nextChunk.Length, initialNextChunkLength - nextChunk.Length);

			if (nextChunk.isZero)
				while (--stepBack >= 0 && remainder[stepBack] == 0) { }
			else
				--stepBack;
		}

		return (result, nextChunk);
	}

	/// <summary>
	/// Raises the given <paramref name="value"/> to the second power.
	/// </summary>
	/// <param name="value">The <see cref="Natural"/> that represents the base.</param>
	/// <returns>The result of the calculation.</returns>
	public static Natural SecondPower(Natural value) => value * value;

	/// <summary>
	/// Raises the given base to the given power using a squaring algorithm.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/Exponentiation#Efficient_computation_with_integer_exponents"/></remarks>
	/// <param name="left">The <see cref="Natural"/> that represents the base.</param>
	/// <param name="right">The <see cref="Natural"/> that represents the exponent.</param>
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="NotSupportedException">
	/// <paramref name="right"/> cannot be higher than 999 as it would be too computationally expensive.
	/// </exception>
	public static Natural Power(Natural left, Natural right)
	{
		Natural result = 1;

		if (right < 3)
		{
			return right[0] switch
			{
				0 => result,
				1 => left,
				_ => left * left
			};
		}

		if (left < 2)
			return left;

		if (right.length > 3)
			throw new NotSupportedException("The exponent cannot be higher than 999 as it would be too computationally expensive!");

		Natural lastPowerCalculated = left;

		(Natural whole, Natural remainder) = Divide(right,2);

		if (!remainder.isZero)
			result = lastPowerCalculated;

		while (!whole.isZero)
		{
			lastPowerCalculated *= lastPowerCalculated;
			(whole, remainder) = Divide(whole, 2);

			if (!remainder.isZero)
				result *= lastPowerCalculated;
		}

		return result;
	}

	/// <summary>
	/// Raises the given radicand to the second degree.
	/// </summary>
	/// <param name="value">The <see cref="Natural"/> that represents the radicand.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	public static (Natural Whole, Natural Remainder) SquareRoot(Natural value)
	{
		if (value.isZero || value == 1)
			return (value, 0);

		Natural remainder = new();
		Natural root = new();

		for (int stepBack = value.length - ((value.Length - 1 & 1) + 1); stepBack >= 0; stepBack -= 2)
		{
			remainder = new Natural([value[stepBack], (stepBack + 1 < value.Length ? value[stepBack + 1] : (byte)0), .. remainder.digits]);

			(root, remainder) = CalculateTwoRootDigits(root, remainder);
		}

		return (root, remainder);
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Natural"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Natural"/> that represents the degree.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0 as is not mathematically meaningful.</exception>
	/// <exception cref="NotSupportedException">
	/// <paramref name="right"/> cannot be higher than 99 as it would be too computationally expensive.
	/// </exception>
	public static (Natural Whole, Natural Remainder) Root(Natural left, Natural right)
	{
		Natural remainder = new();

		if (right < 3)
		{
			return right[0] switch
			{
				0 => throw new DivideByZeroException("The degree cannot be 0, as it is not mathematically meaningful!"),
				1 => (left, remainder),
				_ => SquareRoot(left)
			};
		}

		if (left.isZero || left == 1)
			return (left, remainder);

		if (right.length > 2)
			throw new NotSupportedException("The degree cannot be higher than 99 as it would be too computationally expensive!");

		int degreeInt = (int)ToUInt32(right);
		byte[] digits = [.. left.digits, .. Digit.CreateArray(degreeInt - left.Length % degreeInt)];

		Natural degreeFactorial = Factorial(right);
		Natural root = new();

		for (int stepBack = digits.Length - degreeInt; stepBack >= 0; stepBack -= degreeInt)
		{
			remainder = new Natural([.. digits[stepBack..(stepBack + degreeInt)], .. remainder.digits]);

			(root, remainder) = CalculateNRootDigits(root, remainder, right, degreeFactorial);
		}

		return (root, remainder);
	}

	/// <summary>
	/// Calculates the factorial of the given base using binary splitting.
	/// </summary>
	/// <param name="value">The <see cref="Natural"/> that represents the base.</param>
	/// <returns>The result of the calculation.</returns>
	public static Natural Factorial(Natural value)
	{
		static Natural ProductRange(Natural a, Natural b)
		{
			if (a == b)
				return a;
			else if (b == a + 1)
				return a * b;

			Natural m = (a + b) / 2;

			Natural left = ProductRange(a, m);
			Natural right = ProductRange(m + 1, b);

			return left * right;
		}

		if (value.isZero || value == 1)
			return 1;

		return ProductRange(1, value);
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Natural"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Natural natural && this == natural;

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode()
	{
		throw new NotImplementedException("This method is not implemented because there are more possible values ​​than the int can handle.");
	}

	#endregion

	#region Operators

	public static implicit operator Natural(string value) => new(value);
	public static implicit operator Natural(byte value) => new(value);
	public static implicit operator Natural(byte[] value) => new(value);
	public static bool operator ==(Natural? left, Natural? right) => left is Natural l && right is Natural r && Equals(l, r);
	public static bool operator !=(Natural? left, Natural? right) => !(left == right);
	public static bool operator >(Natural left, Natural right) => GreaterThan(left, right);
	public static bool operator <(Natural left, Natural right) => GreaterThan(right, left);
	public static bool operator >=(Natural left, Natural right) => !GreaterThan(right, left);
	public static bool operator <=(Natural left, Natural right) => !GreaterThan(left, right);
	public static Natural operator ++(Natural value) => Add(value, 1);
	public static Natural operator --(Natural value) => Subtract(value, 1).Value;
	public static Natural operator +(Natural left, Natural right) => Add(left, right);
	public static Natural operator -(Natural left, Natural right) => Subtract(left, right).Value;
	public static Natural operator *(Natural left, Natural right) => Multiply(left, right);
	public static Natural operator /(Natural left, Natural right) => Divide(left, right).Whole;
	public static Natural operator %(Natural left, Natural right) => Divide(left, right).Remainder;
	public static Natural operator ^(Natural left, Natural right) => Power(left, right);
	public static Natural operator ~(Natural value) => SquareRoot(value).Whole;
	public static Natural operator |(Natural left, Natural right) => Root(right, left).Whole;

	#endregion
}