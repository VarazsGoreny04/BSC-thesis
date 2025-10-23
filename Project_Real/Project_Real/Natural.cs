using System;
using System.Collections.Immutable;
using System.Linq;

namespace Project_Real;

/// <summary>
/// Represents a natural number using a <see cref="Digit"/> array.
/// </summary>
public readonly struct Natural
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
	public readonly Digit this[Index index] => digits[index];

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
	/// Constructs a <see cref="Natural"/> by the given <paramref name="digits"/>.
	/// </summary>
	/// <param name="digits">An array of <see cref="Digit"/>s, where the first index represents the lowest value.</param>
	/// <exception cref="ArgumentException"><paramref name="digits"/> cannot be null or empty.</exception>
	public Natural(Digit[] digits)
	{
		if (digits is null || digits.Length < 1)
			throw new ArgumentException();

		this.digits = ImmutableArray.Create(Digit.TrimEnd(digits));
		length = this.digits.Length;
		isZero = length == 1 && this.digits[0] == Digit.ZERO;
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

		return carry ? new Natural([.. result, Digit.ONE]) : new Natural(result);
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

		return (swap, new Natural(result));
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
		else if (right == "1")
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

			result += new Natural(temp);
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

		return (new Natural(result), temp);
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
	public static Natural Power(Natural left, Natural right)
	{
		Natural result = new([new Digit('1')]);
		Natural two = new([new Digit('2')]);

		if (right.isZero)
			return /*left.IsZero ? throw new NotImplementedException() :*/ result;
		else if (right == result)
			return left;
		else if (right == two)
			return left * left;

		Natural lastPowerCalculated = left;

		(Natural whole, Natural remainder) = Divide(right, two);

		if (!remainder.isZero)
			result = lastPowerCalculated;

		while (!whole.isZero)
		{
			lastPowerCalculated *= lastPowerCalculated;
			(whole, remainder) = Divide(whole, two);

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
		if (value.isZero || value.Length == 1 && value[0] == Digit.ONE)
			return (value, new Natural());

		Natural two = new([Digit.TWO]);
		Natural rootTimesTwo, test;
		Natural remainder = new();
		Natural root = new();
		Digit xTry;

		for (int i = ((value.length + 1) / 2 - 1) * 2; i >= 0; i -= 2)
		{
			remainder = new([value[i], (i + 1 < value.digits.Length ? value[i + 1] : Digit.ZERO), .. remainder.digits]);

			xTry = Digit.ZERO;

			if (!remainder.isZero)
			{
				rootTimesTwo = root * two;

				byte j = 10;
				do
				{
					xTry -= Digit.ONE;
					test = new Natural([xTry, .. rootTimesTwo.digits]) * new Natural([xTry]);
				} while (--j > 0 && test > remainder);

				remainder -= test;
			}

			root = new Natural([xTry, .. root.digits]);
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
	public static (Natural Whole, Natural Remainder) Root(Natural left, Natural right)
	{
		Natural remainder = new();

		if (right < new Natural([Digit.THREE]))
		{
			return Digit.ToChar(right[0]) switch
			{
				'0' => throw new NotImplementedException(),
				'1' => (left, remainder),
				_ => SquareRoot(left)
			};
		}

		if (left.isZero || (left.Length == 1 && left[0] == Digit.ONE))
			return (left, remainder);

		ushort nInt = Convert.ToUInt16(right.ToString());
		Digit[] digits = [.. left.digits, .. Digit.CreateArray((nInt - (left.digits.Length % nInt)) % nInt)];

		Digit xTry;
		Natural test, kNatural, nMinusKN, binomial;
		Natural nFactorial = Factorial(right);
		Natural root = new();

		for (int i = digits.Length - nInt; i >= 0; i -= nInt)
		{
			remainder = new Natural([.. digits[i..(i + nInt)], .. remainder.digits]);

			xTry = Digit.ZERO;

			if (!remainder.isZero)
			{
				byte j = 0;
				do
				{
					xTry -= Digit.ONE;
					test = new();

					for (int k = nInt - 1; k >= 0; --k)
					{

						kNatural = k.ToString();
						nMinusKN = right - kNatural;
						binomial = nFactorial / (Factorial(kNatural) * Factorial(nMinusKN));

						test += new Natural([.. Digit.CreateArray(k), .. (binomial * (root ^ kNatural) * ((new Natural([xTry])) ^ nMinusKN)).digits]);
					}
				} while (++j < 10 && test > remainder);

				remainder -= test;
			}

			root = new Natural([xTry, .. root.digits]);
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
		Natural one = new([Digit.ONE]);

		if (value.isZero || value == one)
			return one;

		Natural result = value;

		while (value != one)
		{
			value -= one;
			result *= value;
		}

		return result;
	}

	/*public static Natural Log(Natural left, Natural right)
	{
		if (left.isZero || right.isZero)
			throw new NotImplementedException();

		Natural one = new([Digit.ONE]);
		Natural result = new();

		while (right <= left)
		{
			left /= right;
			result += one;
		}

		return result;
	}*/

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
	public override readonly bool Equals(object? obj) => obj is Natural natural && this == natural;

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Natural(string value) => new(value);
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