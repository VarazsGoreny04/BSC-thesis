using System;
using System.Collections.Immutable;

namespace Project_Real;

/// <summary>
/// Represents a signed number.
/// </summary>
public class Writable
{
	#region Fields

	private readonly bool sign;
	private readonly Positive value;

	#endregion

	#region Properties

	/// <summary>
	/// Gets or sets whether the <see cref="ToString"/> method should write the + sign to the front of the number.
	/// </summary>
	public static bool WriteSign 
	{
		get => Integer.WriteSign;
		set => Integer.WriteSign = value;
	}

	/// <summary>
	/// Gets or sets the character the <see cref="ToString"/> method should use as separator.
	/// </summary>
	/// <exception cref="ArgumentException"><param name="value"/> cannot be a number character.</exception>
	public static char Separator 
	{
		get => Positive.Separator;
		set => Positive.Separator = value;
	}

	/// <summary>
	/// Gets or sets the length of calculating fractions.
	/// </summary>
	/// <exception cref="ArgumentException">
	/// <param name="value"/> cannot be less than 0.
	/// </exception>
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
	/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> cannot be less than 0.</exception>
	public Digit this[Index i] => value.Digits[i];

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
	/// Returns a string that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Writable"/> number as a <see langword="string"/>.</returns>
	public override string ToString() => $"{(WriteSign || !sign ? (sign ? '+' : '-') : "")}{value}";

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
		return left.Sign != right.Sign ? left.Sign : (left.Sign ? left.Value > right.Value : left.Value < right.Value);
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
	/// <returns>The whole value and the remainder in a tuple.</returns>
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
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="NotImplementedException"><paramref name="right"/> cannot be negative.</exception>
	public static Writable Power(Writable left, Writable right)
	{
		if (!right.sign)
			throw new NotImplementedException();

		return new Writable(left.sign || right[0] % Digit.TWO == Digit.ZERO, left.value ^ right.value);
	}

	/// <summary>
	/// Raises the given radicand to the second degree.
	/// </summary>
	/// <param name="value">The <see cref="Writable"/> that represents the radicand.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException"><paramref name="value"/> cannot be negative as it is not mathematically meaningful.</exception>
	public static (Writable Value, Writable Remainder) SquareRoot(Writable value, int? fractionCalculationLength = null)
	{
		return value.sign ? Positive.SquareRoot(value.value, fractionCalculationLength) : throw new NotImplementedException();
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Writable"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Writable"/> that represents the degree.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException">
	/// <paramref name="right"/> being negative or 0
	/// -or-
	/// <paramref name="left"/> being negative and <paramref name="right"/> being even is not mathematically meaningful.
	/// </exception>
	public static (Writable Value, Writable Remainder) Root(Writable left, Writable right, int? fractionCalculationLength = null)
	{
		if (!right.sign || (!left.sign && right[0] % Digit.TWO == Digit.ZERO))
			throw new NotImplementedException();

		(Positive whole, Positive remainder) = Positive.Root(left.Value, right.Value, fractionCalculationLength);

		return (new Writable(left.Sign, whole), new Writable(left.Sign, remainder));
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
	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Writable(char value) => new(value.ToString());
	public static implicit operator Writable(string value) => new(value);
	public static implicit operator Writable(Digit value) => new(value);
	public static implicit operator Writable(Natural value) => new(value);
	public static implicit operator Writable(Integer value) => new(value);
	public static implicit operator Writable(Positive value) => new(value);
	public static bool operator ==(Writable left, Writable right) => Equals(left, right);
	public static bool operator !=(Writable left, Writable right) => !Equals(left, right);
	public static bool operator >(Writable left, Writable right) => GreaterThan(left, right);
	public static bool operator <(Writable left, Writable right) => GreaterThan(right, left);
	public static bool operator >=(Writable left, Writable right) => !GreaterThan(right, left);
	public static bool operator <=(Writable left, Writable right) => !GreaterThan(left, right);
	public static Writable operator -(Writable value) => new(!value.Sign, value.Value);
	public static Writable operator +(Writable left, Writable right) => Add(left, right);
	public static Writable operator -(Writable left, Writable right) => Subtract(left, right);
	public static Writable operator *(Writable left, Writable right) => Multiply(left, right);
	public static Writable operator /(Writable left, Writable right) => Divide(left, right).Value;
	public static Writable operator %(Writable left, Writable right) => Divide(left, right, 0).Remainder;
	public static Writable operator ^(Writable left, Writable right) => Power(left, right);
	public static Writable operator ~(Writable value) => SquareRoot(value).Value;
	public static Writable operator |(Writable left, Writable right) => Root(right, left).Value;

	#endregion
}