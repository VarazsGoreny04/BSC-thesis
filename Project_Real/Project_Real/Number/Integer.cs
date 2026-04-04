using Project_Real.NumberSet;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Project_Real.Number;

/// <summary>
/// Represents an integer number.
/// </summary>
public class Integer : 
	IComparisonOperators<Integer, Integer, bool>, 
	IEqualityOperators<Integer, Integer, bool>, 
	IIncrementOperators<Integer>, 
	IDecrementOperators<Integer>,
	IUnaryPlusOperators<Integer, Integer>,
	IUnaryNegationOperators<Integer, Integer>,
	IAdditionOperators<Integer, Integer, Integer>, 
	ISubtractionOperators<Integer, Integer, Integer>, 
	IMultiplyOperators<Integer, Integer, Integer>, 
	IDivisionOperators<Integer, Integer, Integer>, 
	IModulusOperators<Integer, Integer, Integer>,
	IPowerOperations<Integer, Integer, Integer>,
	IRootOperations<Integer, Integer, Integer>,
	IAdditiveIdentity<Integer, Integer>, 
	IMultiplicativeIdentity<Integer, Integer>,
	IParsable<Integer>
{
	#region Fields

	private static bool writeSign = true;

	private readonly bool sign;
	private readonly Natural value;

	#endregion

	#region Properties

	public static Integer AdditiveIdentity => Digit.ZERO;

	public static Integer MultiplicativeIdentity => Digit.ONE;

	/// <summary>
	/// Gets or sets whether the <see cref="ToString"/> method should write the + sign to the front of the number.
	/// </summary>
	public static bool WriteSign
	{
		get => writeSign;
		set => writeSign = value;
	}

	/// <returns>The number of <see cref="Digit"/>s used to represent <see langword="this"/> <see cref="Integer"/>.</returns>
	public int Length => value.Length;

	/// <summary>
	/// Returns whether <see langword="this"/> is equal to 0.
	/// </summary>
	/// <returns><see langword="true"/> if <see langword="this"/> is equal to 0; otherwise, <see langword="false"/>.</returns>
	public bool IsZero => value.IsZero;

	/// <summary>
	/// The sign of <see langword="this"/> <see cref="Positive"/> represented by a boolean.
	/// </summary>
	/// <returns><see langword="true"/> if the sign is +; <see langword="false"/> if the sign is -.</returns>
	public bool Sign => sign;

	/// <returns>The <see cref="Natural"/> used to represent <see langword="this"/> <see cref="Integer"/> without indicating sign.</returns>
	public Natural Value => value;

	/// <returns>The <see cref="ImmutableArray{Digit}"/> used to represent <see langword="this"/> <see cref="Integer"/>.</returns>
	public ImmutableArray<Digit> Digits => value.Digits;

	/// <returns>The <see cref="Digit"/> at the specified <see cref="Index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> cannot be less than 0.</exception>
	public Digit this[Index index] => value.Digits[index];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs an <see cref="Integer"/> with a value of 0.
	/// </summary>
	public Integer()
	{
		sign = true;
		value = new Natural();
	}

	/// <summary>
	/// Constructs an <see cref="Integer"/> by the given <see langword="string"/> parameter.
	/// </summary>
	/// <param name="number">A <see langword="string"/> of 0 to 9 characters and maybe a + or - sign at the front.</param>
	/// <exception cref="ArgumentException"><paramref name="number"/> is not a valid number format.</exception>
	public Integer(string number)
	{
		if (number is null || number.Length < 1)
			throw new ArgumentException();

		int start = 0;

		if (!(number[0] >= '0' && number[0] <= '9'))
		{
			sign = number[0] switch
			{
				'+' => true,
				'-' => false,
				_ => throw new ArgumentException()
			};

			start = 1;
		}
		else
			sign = true;

		value = number[start..];

		sign |= IsZero;
	}

	/// <summary>
	/// Constructs an <see cref="Integer"/> by the given <paramref name="sign"/> and <paramref name="value"/>.
	/// </summary>
	/// <param name="sign">The sign of the number. <see langword="true"/> means +; <see langword="false"/> means -.</param>
	/// <param name="value">The absolute value of the number.</param>
	public Integer(bool sign, Natural value)
	{
		this.value = value;
		this.sign = sign || IsZero;
	}

	/// <summary>
	/// Constructs a <see cref="Positive"/> by the given <see cref="Digit"/>.
	/// </summary>
	public Integer(Digit value) : this(true, value) { }

	/// <summary>
	/// Constructs a <see cref="Positive"/> by the given <see cref="Natural"/>.
	/// </summary>
	public Integer(Natural value) : this(true, value) { }

	/// <summary>
	/// Constructs a <see cref="Integer"/> by the given <paramref name="number"/>.
	/// </summary>
	/// <param name="number">An unsigned integer value.</param>
	public Integer(uint number) : this(true, new Natural(number)) { }

	/// <summary>
	/// Constructs a <see cref="Integer"/> by the given <paramref name="number"/>.
	/// </summary>
	/// <param name="number">An integer value.</param>
	public Integer(int number) : this(number.ToString()) { }

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a <see cref="string"/> that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>An <see cref="Integer"/> number as a <see langword="string"/>.</returns>
	public override string ToString() => writeSign ? $"{(sign ? '+' : '-')}{value}" : value.ToString();

	public static Integer Parse(string s, IFormatProvider? _ = null) => new(s);

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Integer result)
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
	/// Returns an integer that represents the given <paramref name="value"/>.
	/// </summary>
	/// <param name="value">The <see cref="Integer"/> to convert.</param>
	/// <returns>A <see cref="Integer"/> number as an <see langword="int"/>.</returns>
	/// <exception cref="OverflowException"><paramref name="value"/> cannot be greater than than <see cref="int.MaxValue"/>.</exception>
	public static int ToInt32(Integer value) => Convert.ToInt32(value.ToString());

	/// <summary>
	/// Gets the absolute value of the given <see cref="Integer"/>.
	/// </summary>
	/// <param name="value">The <see cref="Integer"/>.</param>
	/// <returns>The absolute value of the given <see cref="Integer"/>.</returns>
	public static Natural Abs(Integer value) => value.value;

	/// <summary>
	/// Compares two <see cref="Integer"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Integer"/> to compare.</param>
	/// <param name="right">The second <see cref="Integer"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is equal to the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(Integer left, Integer right) => left.sign == right.sign && left.value == right.value;

	/// <summary>
	/// Compares two <see cref="Integer"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Integer"/> to compare.</param>
	/// <param name="right">The second <see cref="Integer"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(Integer left, Integer right) => left.sign != right.sign ? left.sign : left.sign ? left.value > right.value : left.value < right.value;

	/// <summary>
	/// Adds two <see cref="Integer"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Integer"/> to add.</param>
	/// <param name="right">The second <see cref="Integer"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Integer Add(Integer left, Integer right)
	{
		if (left.sign == right.sign)
			return new Integer(left.sign, left.value + right.value);
		else
		{
			if (right.sign)
				(left, right) = (right, left);

			(bool swap, Natural value) = Natural.Subtract(left.value, right.value);

			return new Integer(!swap, value);
		}
	}

	/// <summary>
	/// Subtracts two <see cref="Integer"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Integer"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Integer"/> that represents the subtrahend.</param>
	/// <returns>The result of the calculation.</returns>
	public static Integer Subtract(Integer left, Integer right) => left + -right;

	/// <summary>
	/// Multiplies two <see cref="Integer"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Integer"/> that represents the multiplier.</param>
	/// <param name="right">The <see cref="Integer"/> that represents the multiplicand.</param>
	/// <returns>The result of the calculation.</returns>
	public static Integer Multiply(Integer left, Integer right) => new(left.sign == right.sign, left.value * right.value);

	/// <summary>
	/// Divides two <see cref="Integer"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Integer"/> that represents the numerator.</param>
	/// <param name="right">The <see cref="Integer"/> that represents the denominator.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static (Integer Whole, Integer Remainder) Divide(Integer left, Integer right)
	{
		(Natural whole, Natural remainder) = Natural.Divide(left.value, right.value);
		return (new Integer(left.sign == right.sign, whole), new Integer(left.sign, remainder));
	}

	/// <summary>
	/// Raises the given <paramref name="value"/> to the second power.
	/// </summary>
	/// <param name="value">The <see cref="Integer"/> that represents the base.</param>
	/// <returns>The result of the calculation.</returns>
	public static Integer SecondPower(Integer value) => value * value;

	/// <summary>
	/// Raises the given base to the given power.
	/// </summary>
	/// <param name="left">The <see cref="Integer"/> that represents the base.</param>
	/// <param name="right">The <see cref="Integer"/> that represents the exponent.</param>
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="NotImplementedException"><paramref name="right"/> cannot be negative.</exception>
	public static Integer Power(Integer left, Integer right)
	{
		if (!right.sign)
			throw new NotImplementedException();

		return new Integer(left.sign || right[0] % Digit.TWO == Digit.ZERO, left.value ^ right.value);
	}

	/// <summary>
	/// Raises the given radicand to the second degree.
	/// </summary>
	/// <param name="value">The <see cref="Integer"/> that represents the radicand.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException"><paramref name="value"/> cannot be negative as it is not mathematically meaningful.</exception>
	public static (Integer Whole, Integer Remainder) SquareRoot(Integer value)
	{
		if (!value.sign)
			throw new NotImplementedException();

		return Natural.SquareRoot(value.value);
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Integer"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Integer"/> that represents the degree.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException">
	/// <paramref name="right"/> being negative or 0
	/// -or-
	/// <paramref name="left"/> being negative and <paramref name="right"/> being even is not mathematically meaningful.
	/// </exception>
	public static (Integer Whole, Integer Remainder) Root(Integer left, Integer right)
	{
		if (!right.sign || !left.sign && right[0] % Digit.TWO == Digit.ZERO)
			throw new NotImplementedException();

		(Natural whole, Natural remainder) = Natural.Root(left.value, right.value);

		return (new Integer(left.sign, whole), new Integer(left.sign, remainder));
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Integer"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Integer integer && this == integer;

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Integer(char value) => new(value.ToString());
	public static implicit operator Integer(string value) => new(value);
	public static implicit operator Integer(Digit value) => new(value);
	public static implicit operator Integer(Natural value) => new(value);
	public static bool operator ==(Integer? left, Integer? right) => left is Integer l && right is Integer r && Equals(l, r);
	public static bool operator !=(Integer? left, Integer? right) => !(left == right);
	public static bool operator >(Integer left, Integer right) => GreaterThan(left, right);
	public static bool operator <(Integer left, Integer right) => GreaterThan(right, left);
	public static bool operator >=(Integer left, Integer right) => !GreaterThan(right, left);
	public static bool operator <=(Integer left, Integer right) => !GreaterThan(left, right);
	public static Integer operator +(Integer value) => value;
	public static Integer operator -(Integer value) => new(!value.sign, value.value);
	public static Integer operator ++(Integer value) => Add(value, Digit.ONE);
	public static Integer operator --(Integer value) => Subtract(value, Digit.ONE);
	public static Integer operator +(Integer left, Integer right) => Add(left, right);
	public static Integer operator -(Integer left, Integer right) => Subtract(left, right);
	public static Integer operator *(Integer left, Integer right) => Multiply(left, right);
	public static Integer operator /(Integer left, Integer right) => Divide(left, right).Whole;
	public static Integer operator %(Integer left, Integer right) => Divide(left, right).Remainder;
	public static Integer operator ^(Integer left, Integer right) => Power(left, right);
	public static Integer operator ~(Integer value) => SquareRoot(value).Whole;
	public static Integer operator |(Integer left, Integer right) => Root(right, left).Whole;

	#endregion
}