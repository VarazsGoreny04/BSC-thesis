using ProjectReal.NumberSet;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ProjectReal.Number;

/// <summary>
/// Represents a signed number.
/// </summary>
public class Writable :
	IComparisonOperators<Writable, Writable, bool>,
	IEqualityOperators<Writable, Writable, bool>,
	IIncrementOperators<Writable>,
	IDecrementOperators<Writable>,
	IUnaryPlusOperators<Writable, Writable>,
	IUnaryNegationOperators<Writable, Writable>,
	IAdditionOperators<Writable, Writable, Writable>,
	ISubtractionOperators<Writable, Writable, Writable>,
	IMultiplyOperators<Writable, Writable, Writable>,
	IDivisionOperators<Writable, Writable, Writable>,
	IModulusOperators<Writable, Writable, Writable>,
	IPowerOperations<Writable, Writable, Writable>,
	IRootOperations<Writable, Writable, Writable>,
	IAdditiveIdentity<Writable, Writable>,
	IMultiplicativeIdentity<Writable, Writable>,
	IParsable<Writable>
{
	#region Fields

	private readonly bool sign;
	private readonly Positive value;

	#endregion

	#region Properties

	public static Writable AdditiveIdentity => Digit.ZERO;

	public static Writable MultiplicativeIdentity => Digit.ONE;

	/// <summary>
	/// Gets or sets whether the <see cref="ToString"/> method should write the + sign to the front of the number.
	/// </summary>
	public static bool WriteSign 
	{
		get => Integer.WriteSign;
		set => Integer.WriteSign = value;
	}

	/// <returns>The character the <see cref="ToString"/> method uses as separator.</returns>
	public static char Separator => Positive.Separator;

	/// <summary>
	/// Gets or sets the length of calculating fractions.
	/// </summary>
	/// <exception cref="ArgumentException"><param name="value"/> cannot be less than 0.</exception>
	public static int FractionCalculationLength
	{
		get => Positive.FractionCalculationLength;
		set => Positive.FractionCalculationLength = value;
	}

	/// <returns>The number of <see cref="Digit"/>s used to represent <see langword="this"/> <see cref="Writable"/>.</returns>
	public int Length => value.Length;

	/// <returns>The number of <see cref="Digit"/>s used to represent the whole part of <see langword="this"/> <see cref="Writable"/>.</returns>
	public int WholeLength => value.WholeLength;

	/// <returns>The number of <see cref="Digit"/>s used to represent the fraction part of <see langword="this"/> <see cref="Writable"/>.</returns>
	public int FractionLength => value.FractionLength;

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

	/// <returns>The <see cref="Positive"/> used to represent <see langword="this"/> <see cref="Writable"/> without indicating the decimal separator.</returns>
	public Positive Value => value;

	/// <returns>
	/// The <see cref="ImmutableArray{Digit}"/> used to represent <see langword="this"/> <see cref="Writable"/> without indicating the decimal separator.
	/// </returns>
	public ImmutableArray<Digit> Digits => value.Digits;

	/// <returns>The <see cref="Digit"/> at the specified <see cref="Index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> must be within the bounds os the digits.</exception>
	public Digit this[Index index] => value.Digits[index];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Writable"/> with a value of 0.
	/// </summary>
	public Writable()
	{
		sign = true;
		value = new Positive();
	}

	/// <summary>
	/// Constructs a <see cref="Writable"/> by the given <see langword="string"/> parameter.
	/// </summary>
	/// <param name="number">
	/// A <see langword="string"/> of 0 to 9 characters maybe a <see cref="Separator"/> sign somewhere after the first digit 
	/// and maybe a + or - sign at the front.
	/// </param>
	/// <exception cref="ArgumentException"><paramref name="number"/> is not a valid number format.</exception>
	public Writable(string number)
	{
		if (number.Length < 1)
			throw new ArgumentException("The given string parameter must not be empty!", nameof(number));

		int start = 0;

		if (!(number[0] >= '0' && number[0] <= '9'))
		{
			sign = number[0] switch
			{
				'+' => true,
				'-' => false,
				_ => throw new ArgumentException("The given string parameter can only start with a sign (+/-) or number characters (0-9)!", nameof(number))
			};

			start = 1;
		}
		else
			sign = true;

		value = number[start..];

		sign |= IsZero;
	}

	/// <summary>
	/// Constructs a <see cref="Writable"/> by the given <paramref name="sign"/> and <paramref name="value"/>.
	/// </summary>
	/// <param name="sign">The sign of the number. <see langword="true"/> means +; <see langword="false"/> means -.</param>
	/// <param name="value">The absolute value of the number.</param>
	public Writable(bool sign, Positive value)
	{
		this.value = value;
		this.sign = sign || IsZero;
	}

	/// <summary>
	/// Constructs a <see cref="Writable"/> by the given <see cref="Digit"/>.
	/// </summary>
	public Writable(Digit value) : this(true, value) { }

	/// <summary>
	/// Constructs a <see cref="Writable"/> by the given <see cref="Natural"/>.
	/// </summary>
	public Writable(Natural value) : this(true, value) { }

	/// <summary>
	/// Constructs a <see cref="Writable"/> by the given <see cref="Integer"/>.
	/// </summary>
	public Writable(Integer value) : this(value.Sign, value.Value) { }

	/// <summary>
	/// Constructs a <see cref="Writable"/> by the given <see cref="Positive"/>.
	/// </summary>
	public Writable(Positive value) : this(true, value) { }

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a <see cref="string"/> that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Writable"/> number as a <see langword="string"/>.</returns>
	public override string ToString() => $"{(WriteSign || !sign ? sign ? '+' : '-' : "")}{value}";

	/// <summary>
	/// Parses a <see cref="string"/> into a <see cref="Writable"/> instance.
	/// </summary>
	/// <param name="s">The <see cref="string"/> to parse.</param>
	/// <param name="_">This parameter is unused.</param>
	/// <returns>The created instance.</returns>
	/// <exception cref="ArgumentException">The <see cref="string"/> must be accepted by the constructor.</exception>
	public static Writable Parse(string s, IFormatProvider? _ = null) => new(s);

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Writable result)
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
	/// Rounds down the absolute value of the given <see cref="Writable"/> instance and keeps the sign.
	/// </summary>
	/// <param name="value">The <see cref="Writable"/> instance.</param>
	/// <returns>The whole part of the given <see cref="Writable"/> instance.</returns>
	public static Integer RoundDown(Writable value) => new(value.Sign, Positive.RoundDown(value.Value));

	/// <summary>
	/// Rounds up the absolute value of the given <see cref="Writable"/> instance and keeps the sign.
	/// </summary>
	/// <param name="value">The <see cref="Writable"/> instance.</param>
	/// <returns>The whole part of the given <see cref="Writable"/> instance plus one if it was not whole.</returns>
	public static Integer RoundUp(Writable value) => new(value.Sign, Positive.RoundUp(value.Value));

	/// <summary>
	/// Rounds the given <see cref="Writable"/> instance to the nearest number.
	/// </summary>
	/// <param name="value">The <see cref="Writable"/> instance.</param>
	/// <returns>The rounded value of the given <see cref="Writable"/> instance.</returns>
	public static Integer Round(Writable value) => new(value.Sign, Positive.Round(value.Value));

	/// <summary>
	/// Gets the absolute value of the given <see cref="Writable"/>.
	/// </summary>
	/// <param name="value">The <see cref="Writable"/>.</param>
	/// <returns>The absolute value of the given <see cref="Writable"/>.</returns>
	public static Positive Abs(Writable value) => value.value;

	/// <summary>
	/// Compares two <see cref="Writable"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Writable"/> to compare.</param>
	/// <param name="right">The second <see cref="Writable"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is equal to the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(Writable left, Writable right) => left.sign == right.sign && left.value == right.value;

	/// <summary>
	/// Compares two <see cref="Writable"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Writable"/> to compare.</param>
	/// <param name="right">The second <see cref="Writable"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(Writable left, Writable right)
	{
		return left.Sign != right.Sign ? left.Sign : left.Sign ? left.Value > right.Value : left.Value < right.Value;
	}

	/// <summary>
	/// Adds two <see cref="Writable"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Writable"/> to add.</param>
	/// <param name="right">The second <see cref="Writable"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Writable Add(Writable left, Writable right)
	{
		if (left.sign == right.sign)
			return new Writable(left.sign, left.value + right.value);
		else
		{
			if (right.sign)
				(left, right) = (right, left);

			(bool swap, Positive value) = Positive.Subtract(left.value, right.value);

			return new Writable(!swap, value);
		}
	}

	/// <summary>
	/// Subtracts two <see cref="Writable"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Writable"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Writable"/> that represents the subtrahend.</param>
	/// <returns>The result of the calculation.</returns>
	public static Writable Subtract(Writable left, Writable right) => left + new Writable(!right.sign, right.value);

	/// <summary>
	/// Multiplies two <see cref="Writable"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Writable"/> that represents the multiplier.</param>
	/// <param name="right">The <see cref="Writable"/> that represents the multiplicand.</param>
	/// <returns>The result of the calculation.</returns>
	public static Writable Multiply(Writable left, Writable right) => new(left.sign == right.sign, left.value * right.value);

	/// <summary>
	/// Divides two <see cref="Writable"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Writable"/> that represents the numerator.</param>
	/// <param name="right">The <see cref="Writable"/> that represents the denominator.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The value and the remainder in a tuple.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static (Writable Value, Writable Remainder) Divide(Writable left, Writable right, int? fractionCalculationLength = null)
	{
		(Positive whole, Positive remainder) = Positive.Divide(left.value, right.value, fractionCalculationLength);

		return (new Writable(left.sign == right.sign, whole), new Writable(left.sign, remainder));
	}

	/// <summary>
	/// Raises the given <paramref name="value"/> to the second power.
	/// </summary>
	/// <param name="value">The <see cref="Writable"/> that represents the base.</param>
	/// <returns>The result of the calculation.</returns>
	public static Writable SecondPower(Writable value) => value * value;

	/// <summary>
	/// Raises the given base to the given power.
	/// </summary>
	/// <param name="left">The <see cref="Writable"/> that represents the base.</param>
	/// <param name="right">The <see cref="Writable"/> that represents the exponent.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException"><paramref name="right"/> cannot be negative.</exception>
	/// <exception cref="NotSupportedException">
	/// Absolut value of <paramref name="right"/> cannot be a fraction or higher than 999 as it would be too computationally expensive and
	/// the result of the 
	/// </exception>
	public static (Writable Value, Writable Remainder) Power(Writable left, Writable right, int? fractionCalculationLength = null)
	{
		if (right.FractionLength > 0)
			throw new NotSupportedException("This type does not support fractional exponents!");

		return right.sign ? (new Writable(left.sign || right[0] % Digit.TWO == Digit.ZERO, left.value ^ right.value), Digit.ZERO) :
			Divide(Digit.ONE, Power(left, right.Value).Value, fractionCalculationLength);
	}

	/// <summary>
	/// Raises the given radicand to the second degree.
	/// </summary>
	/// <param name="value">The <see cref="Writable"/> that represents the radicand.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> cannot be negative as it is not mathematically meaningful.</exception>
	public static (Writable Value, Writable Remainder) SquareRoot(Writable value, int? fractionCalculationLength = null)
	{
		return value.sign ? Positive.SquareRoot(value.value, fractionCalculationLength) :
			throw new ArgumentOutOfRangeException(nameof(value), value, "The radicand cannot be negative as it is not mathematically meaningful!");
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Writable"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Writable"/> that represents the degree.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="ArgumentException">
	/// <paramref name="left"/> being negative and <paramref name="right"/> being even is not mathematically meaningful.
	/// </exception>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0 as is not mathematically meaningful.</exception>
	/// <exception cref="NotSupportedException"><paramref name="right"/> cannot be higher than 99 as it would be too computationally expensive.</exception>
	public static (Writable Value, Writable Remainder) Root(Writable left, Writable right, int? fractionCalculationLength = null)
	{
		if (!left.sign && right[0] % Digit.TWO == Digit.ZERO)
			throw new ArgumentException("While the radicand is negative the degree cannot be even as it is not mathematically meaningful!");

		if (!right.sign)
		{
			(Writable rootValue, Writable rootRemainder) = Root(left, right.Value, (fractionCalculationLength ?? FractionCalculationLength) + 1);
			(Writable divisionValue, Writable divisionRemainder) = Divide(Digit.ONE, rootValue, fractionCalculationLength);

			return (divisionValue, Digit.ZERO /*rootRemainder + Power(divisionRemainder, right).Value*/); // TODO
		}

		(Positive value, Positive remainder) = Positive.Root(left.Value, right.Value, fractionCalculationLength);

		return (new Writable(left.Sign, value), new Writable(left.Sign, remainder));
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Writable"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Writable writable && this == writable;

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode()
	{
		throw new NotImplementedException("This method is not implemented because there are more possible values ​​than the int can handle.");
	}

	#endregion

	#region Operators

	public static implicit operator Writable(string value) => new(value);
	public static implicit operator Writable(Digit value) => new(value);
	public static implicit operator Writable(Natural value) => new(value);
	public static implicit operator Writable(Integer value) => new(value);
	public static implicit operator Writable(Positive value) => new(value);
	public static bool operator ==(Writable? left, Writable? right) => left is Writable l && right is Writable r && Equals(l, r);
	public static bool operator !=(Writable? left, Writable? right) => !(left == right);
	public static bool operator >(Writable left, Writable right) => GreaterThan(left, right);
	public static bool operator <(Writable left, Writable right) => GreaterThan(right, left);
	public static bool operator >=(Writable left, Writable right) => !GreaterThan(right, left);
	public static bool operator <=(Writable left, Writable right) => !GreaterThan(left, right);
	public static Writable operator +(Writable value) => value;
	public static Writable operator -(Writable value) => new(!value.Sign, value.Value);
	public static Writable operator ++(Writable value) => Add(value, Digit.ONE);
	public static Writable operator --(Writable value) => Subtract(value, Digit.ONE).Value;
	public static Writable operator +(Writable left, Writable right) => Add(left, right);
	public static Writable operator -(Writable left, Writable right) => Subtract(left, right);
	public static Writable operator *(Writable left, Writable right) => Multiply(left, right);
	public static Writable operator /(Writable left, Writable right) => Divide(left, right).Value;
	public static Writable operator %(Writable left, Writable right) => Divide(left, right, 0).Remainder;
	public static Writable operator ^(Writable left, Writable right) => Power(left, right).Value;
	public static Writable operator ~(Writable value) => SquareRoot(value).Value;
	public static Writable operator |(Writable left, Writable right) => Root(right, left).Value;

	#endregion
}