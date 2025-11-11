using System;
using System.Collections.Immutable;
using System.Linq;

namespace Project_Real;

/// <summary>
/// Represents a natural number using a <see cref="Digit"/> array.
/// </summary>
public class Natural
{
	#region Fields

	private readonly int length;
	private readonly bool isZero;
	private readonly ImmutableArray<Digit> digits;

	#endregion

	#region Properties

	/// <returns>The number of <see cref="Digit"/>s used to represent <see langword="this"/> <see cref="Natural"/>.</returns>
	public int Length => length;

	/// <summary>
	/// Returns whether <see langword="this"/> is equal to 0.
	/// </summary>
	/// <returns><see langword="true"/> if <see langword="this"/> is equal to 0; otherwise, <see langword="false"/>.</returns>
	public bool IsZero => isZero;

	/// <returns>The <see cref="ImmutableArray{Digit}"/> used to represent <see langword="this"/> <see cref="Natural"/>.</returns>
	public ImmutableArray<Digit> Digits => digits;

	/// <returns>The <see cref="Digit"/> at the specified <see cref="Index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> cannot be less than 0.</exception>
	public Digit this[Index index] => digits[index];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Natural"/> with a value of 0.
	/// </summary>
	public Natural()
	{
		length = 1;
		digits = [Digit.ZERO];
		isZero = true;
	}

	/// <summary>
	/// Constructs a <see cref="Natural"/> by the given <see langword="string"/> parameter.
	/// </summary>
	/// <param name="number">A <see langword="string"/> of 0 to 9 characters.</param>
	/// <exception cref="ArgumentException"><paramref name="number"/> is not a valid number format.</exception>
	public Natural(string number)
	{
		if (number is null || number.Length < 1)
			throw new ArgumentException();

		number = number.TrimStart('0');

		isZero = number.Length < 1;

		if (isZero)
			number = "0";

		length = number.Length;
		Digit[] digits = new Digit[length];

		try
		{
			for (int i = 0; i < length; ++i)
				digits[i] = new Digit(number[^(i + 1)]);
		}
		catch (Digit.ValueOutOfRangeException)
		{
			throw new ArgumentException();
		}

		this.digits = ImmutableArray.Create(digits);
	}

	/// <summary>
	/// Constructs a <see cref="Natural"/> by the given <paramref name="number"/>.
	/// </summary>
	/// <param name="number">An array of <see cref="Digit"/>s, where the first index represents the lowest value.</param>
	/// <exception cref="ArgumentException"><paramref name="number"/> cannot be null or empty.</exception>
	public Natural(Digit[] number)
	{
		if (number is null || number.Length < 1)
			throw new ArgumentException();

		digits = ImmutableArray.Create(Digit.TrimEnd(number));
		length = digits.Length;
		isZero = length == 1 && digits[0] == Digit.ZERO;
	}

	/// <summary>
	/// Constructs a <see cref="Natural"/> by the given <see cref="Digit"/>.
	/// </summary>
	public Natural(Digit value) : this([value]) { }

	/// <summary>
	/// Constructs a <see cref="Natural"/> by the given <paramref name="number"/>.
	/// </summary>
	/// <param name="number">An unsigned integer value.</param>
	public Natural(uint number) : this(number.ToString()) { }

	#endregion

	#region Internal methods

	internal static Natural CalculateTwoRootDigits(Natural root, ref Natural remainder)
	{
		Digit xTry;
		Natural test;

		if (!remainder.IsZero)
		{
			Natural rootTimesTwo = root * Digit.TWO;
			xTry = Digit.FIVE;

			test = new Natural([xTry, .. rootTimesTwo.Digits]) * xTry;
			if (test <= remainder)
				xTry = Digit.ZERO;

			byte j = 0;
			do
			{
				xTry -= Digit.ONE;
				test = new Natural([xTry, .. rootTimesTwo.Digits]) * xTry;
			} while (++j < 5 && test > remainder);

			remainder -= test;
		}
		else
			xTry = Digit.ZERO;

			return new Natural([xTry, .. root.Digits]);
	}

	internal static Natural CalculateNRootDigits(Natural root, ref Natural remainder, Natural degree, Natural degreeFactorial)
	{
		Digit xTry;
		Natural test, degUp, binomial;

		if (!remainder.IsZero)
		{
			xTry = Digit.FIVE;
			test = new();

			Natural degDown = degree;
			while (!degDown.IsZero)
			{
				degDown -= Digit.ONE;

				degUp = degree - degDown;
				binomial = degreeFactorial / (Factorial(degDown) * Factorial(degUp));

				test += new Natural([.. Digit.CreateArray((int)ToUInt32(degDown)), .. (binomial * (root ^ degDown) * (xTry ^ degUp)).Digits]);
			}

			if (test <= remainder)
				xTry = Digit.ZERO;

			byte j = 0;
			do
			{
				xTry -= Digit.ONE;
				test = new();

				degDown = degree;
				while (!degDown.IsZero)
				{
					degDown -= Digit.ONE;

					degUp = degree - degDown;
					binomial = degreeFactorial / (Factorial(degDown) * Factorial(degUp));

					test += new Natural([.. Digit.CreateArray((int)ToUInt32(degDown)), .. (binomial * (root ^ degDown) * (xTry ^ degUp)).Digits]);
				}
			} while (++j < 10 && test > remainder);

			remainder -= test;
		}
		else
			xTry = Digit.ZERO;

		return new Natural([xTry, .. root.Digits]);
	}

	internal static Natural Log(Natural left, Natural right)
	{
		if (left.isZero || right.isZero)
			throw new NotImplementedException();

		Natural one = Digit.ONE;
		Natural result = new();

		while (right <= left)
		{
			left /= right;
			result += one;
		}

		return result;
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a string that represents the value of <see langword="this"/> instance.
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
		Digit[] result = new Digit[left.Length];

		for (int i = 0; i < left.Length; ++i)
			(carry, result[i]) = Digit.Add(left[i], (i < right.Length ? right[i] : Digit.ZERO), carry);

		return carry ? new Natural([.. result, Digit.ONE]) : result;
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
		Digit[] result = new Digit[left.Length];

		for (int i = 0; i < left.Length; ++i)
			(carry, result[i]) = Digit.Subtract(left[i], (i < right.Length ? right[i] : Digit.ZERO), carry);

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
		else if (right == Digit.ONE)
			return left;

		Natural result = new();
		Digit[] temp;
		Digit overflowD, digit;
		bool overflowB;
		int addedIndex;

		for (int n2i = 0; n2i < right.Length; ++n2i)
		{
			if (right[n2i] == Digit.ZERO)
				continue;

			temp = new Digit[left.Length + n2i + 1];
			Array.Fill(temp, Digit.ZERO, 0, n2i + 1);

			addedIndex = n2i;

			for (int n1i = 0; n1i < left.Length; ++n1i)
			{
				(overflowD, digit) = Digit.Multiply(left[n1i], right[n2i]);
				(overflowB, temp[addedIndex]) = Digit.Add(temp[addedIndex], digit);
				++addedIndex;
				temp[addedIndex] = Digit.Add(overflowD, Digit.ZERO, overflowB).Digit;
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
			throw new DivideByZeroException();
		else if (left.Length < right.Length)
			return (new Natural(), left);
		else if (right.Length == 1 && right[0] == Digit.ONE)
			return (left, new Natural());

		int tempLength;
		Natural temp = new();
		Digit[] remainder = [.. left.digits];
		Digit[] result = Digit.CreateArray(left.Length - right.Length + 1);

		int i = left.Length - right.Length;
		while (i >= 0)
		{
			temp = new Natural([.. remainder.Skip(i)]);
			tempLength = temp.Length;

			while (right <= temp)
			{
				temp -= right;
				result[i] += Digit.ONE;
			}

			Array.Copy(temp.digits.ToArray(), 0, remainder, i, temp.Length);
			Array.Fill(remainder, Digit.ZERO, i + temp.Length, tempLength - temp.Length);

			if (temp.isZero)
				while (--i >= 0 && remainder[i] == Digit.ZERO) { }
			else
				--i;
		}

		return (result, temp);
	}

	/// <summary>
	/// Raises the given <paramref name="value"/> to the second power.
	/// </summary>
	/// <param name="value">The <see cref="Natural"/> that represents the base.</param>
	/// <returns>The result of the calculation.</returns>
	public static Natural SecondPower(Natural value) => value * value;

	/// <summary>
	/// Raises the given base to the given power.
	/// </summary>
	/// <param name="left">The <see cref="Natural"/> that represents the base.</param>
	/// <param name="right">The <see cref="Natural"/> that represents the exponent.</param>
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="NotSupportedException">
	/// <paramref name="right"/> cannot be higher than 999 as it would be too computationally expensive.
	/// </exception>
	public static Natural Power(Natural left, Natural right)
	{
		Natural result = Digit.ONE;

		if (right < Digit.THREE)
		{
			return right.ToString() switch
			{
				"0" => /*left.IsZero ? throw new NotImplementedException() :*/ result,
				"1" => left,
				_ => left * left
			};
		}

		if (left < Digit.TWO)
			return left;

		if (right.length > 3)
			throw new NotSupportedException();

		Natural lastPowerCalculated = left;

		(Natural whole, Natural remainder) = Divide(right, Digit.TWO);

		if (!remainder.isZero)
			result = lastPowerCalculated;

		while (!whole.isZero)
		{
			lastPowerCalculated *= lastPowerCalculated;
			(whole, remainder) = Divide(whole, Digit.TWO);

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
		if (value.isZero || value == Digit.ONE)
			return (value, new Natural());

		Natural remainder = new();
		Natural root = new();

		for (int i = value.length - (((value.Length - 1) & 1) + 1); i >= 0; i -= 2)
		{
			remainder = new Natural([value[i], (i + 1 < value.Length ? value[i + 1] : Digit.ZERO), .. remainder.digits]);

			root = CalculateTwoRootDigits(root, ref remainder);
		}

		return (root, remainder);
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Natural"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Natural"/> that represents the degree.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException"><paramref name="right"/> cannot be 0 as is not mathematically meaningful.</exception>
	/// <exception cref="NotSupportedException">
	/// <paramref name="right"/> cannot be higher than 99 as it would be too computationally expensive.
	/// </exception>
	public static (Natural Whole, Natural Remainder) Root(Natural left, Natural right)
	{
		Natural remainder = new();

		if (right < Digit.THREE)
		{
			return Digit.ToChar(right[0]) switch
			{
				'0' => throw new NotImplementedException(),
				'1' => (left, remainder),
				_ => SquareRoot(left)
			};
		}

		if (left.isZero || left == Digit.ONE)
			return (left, remainder);

		if (right.length > 2)
			throw new NotSupportedException();

		int degreeInt = (int)ToUInt32(right);
		Digit[] digits = [.. left.digits, .. Digit.CreateArray(degreeInt - (left.Length % degreeInt))];

		Natural degreeFactorial = Factorial(right);
		Natural root = new();

		for (int i = digits.Length - degreeInt; i >= 0; i -= degreeInt)
		{
			remainder = new Natural([.. digits[i..(i + degreeInt)], .. remainder.digits]);

			root = CalculateNRootDigits(root, ref remainder, right, degreeFactorial);
		}

		return (root, remainder);
	}

	/// <summary>
	/// Calculates the factorial of the given base.
	/// </summary>
	/// <param name="value">The <see cref="Natural"/> that represents the base.</param>
	/// <returns>The result of the calculation.</returns>
	public static Natural Factorial(Natural value)
	{
		if (value.isZero || value == Digit.ONE)
			return Digit.ONE;

		Natural result = value;

		while (value != Digit.ONE)
		{
			value -= Digit.ONE;
			result *= value;
		}

		return result;
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
	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Natural(string value) => new(value);
	public static implicit operator Natural(Digit value) => new(value);
	public static implicit operator Natural(Digit[] value) => new(value);
	public static bool operator ==(Natural left, Natural right) => Equals(left, right);
	public static bool operator !=(Natural left, Natural right) => !Equals(left, right);
	public static bool operator >(Natural left, Natural right) => GreaterThan(left, right);
	public static bool operator <(Natural left, Natural right) => GreaterThan(right, left);
	public static bool operator >=(Natural left, Natural right) => !GreaterThan(right, left);
	public static bool operator <=(Natural left, Natural right) => !GreaterThan(left, right);
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