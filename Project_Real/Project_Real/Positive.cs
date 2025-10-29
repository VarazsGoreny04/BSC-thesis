using System;
using System.Collections.Immutable;

namespace Project_Real;

/// <summary>
/// Represents an unsigned number.
/// </summary>
public class Positive
{
	#region Fields

	private static char separator = '.';
	private static int fractionCalculationLength = 10;

	private readonly int length;
	private readonly int fractionLength;
	private readonly Natural value;

	#endregion

	#region Properties

	/// <summary>
	/// Gets or sets the character the <see cref="ToString"/> method should use as separator.
	/// </summary>
	/// <exception cref="ArgumentException"><param name="value"/> cannot be a number character.</exception>
	public static char Separator
	{
		get => separator;
		set => separator = value < '0' || value > '9' ? value : throw new ArgumentException();
	}

	/// <summary>
	/// Gets or sets the length of calculating fractions.
	/// </summary>
	/// <exception cref="ArgumentException"><param name="value"/> cannot be less than 0.</exception>
	public static int FractionCalculationLength
	{
		get => fractionCalculationLength;
		set => fractionCalculationLength = (value >= 0) ? value : throw new ArgumentException();
	}

	/// <returns>The number of <see cref="Digit"/>s used to represent <see langword="this"/> <see cref="Positive"/>.</returns>
	public int Length => length;

	/// <returns>The number of <see cref="Digit"/>s used to represent the whole part of <see langword="this"/> <see cref="Positive"/>.</returns>
	public int WholeLength => length - fractionLength;

	/// <returns>The number of <see cref="Digit"/>s used to represent the fraction part of <see langword="this"/> <see cref="Positive"/>.</returns>
	public int FractionLength => fractionLength;

	/// <summary>
	/// Returns whether <see langword="this"/> is equal to 0.
	/// </summary>
	/// <returns><see langword="true"/> if <see langword="this"/> is equal to 0; otherwise, <see langword="false"/>.</returns>
	public bool IsZero => value.IsZero;

	/// <returns>The <see cref="Natural"/> used to represent <see langword="this"/> <see cref="Positive"/> without indicating the decimal separator.</returns>
	public Natural Value => value;

	/// <returns>
	/// The <see cref="ImmutableArray{Digit}"/> used to represent <see langword="this"/> <see cref="Positive"/> without indicating the decimal separator.
	/// </returns>
	public ImmutableArray<Digit> Digits => value.Digits;

	/// <returns>The <see cref="Digit"/> at the specified <see cref="Index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> cannot be less than 0.</exception>
	public Digit this[Index i] => value.Digits[i];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Positive"/> with a value of 0.
	/// </summary>
	public Positive()
	{
		value = new Natural();
		length = 1;
		fractionLength = 0;
	}

	/// <summary>
	/// Constructs a <see cref="Positive"/> by the given <see langword="string"/> parameter.
	/// </summary>
	/// <param name="number">
	/// A <see langword="string"/> of 0 to 9 characters and maybe a <see cref="Separator"/> sign somewhere after the first digit.
	/// </param>
	/// <exception cref="ArgumentException"><paramref name="number"/> is not a valid number format.</exception>
	public Positive(string number)
	{
		if (number is null || number.Length < 1 || number[0] == separator)
			throw new ArgumentException();

		number = number.TrimStart('0');

		if (number.Length < 1 || number[0] == separator)
			number = '0' + number;

		fractionLength = number.IndexOf(separator);

		if (fractionLength != -1)
		{
			number = number.TrimEnd('0');
			number = number.Remove(fractionLength, 1);
		}

		value = number;
		length = number.Length;
		fractionLength = fractionLength == -1 ? 0 : number.Length - fractionLength;
	}

	/// <summary>
	/// Constructs an <see cref="Positive"/> by the given <paramref name="value"/> and <paramref name="fractionLength"/>.
	/// </summary>
	/// <param name="value">The value of the number without a decimal separator.</param>
	/// <param name="fractionLength">Indicates the number of fraction characters in <see langword="this"/> <see cref="Positive"/>.</param>
	public Positive(Natural value, int fractionLength)
	{
		if (fractionLength < 0)
			throw new ArgumentException();

		if (value.IsZero)
		{
			this.value = value;
			length = 1;
			this.fractionLength = 0;
		}
		else
		{
			int i = 0;

			while (i < Math.Min(fractionLength, value.Length) && value[i] == Digit.ZERO) { ++i; }

			this.value = new Natural([.. value.Digits[i..]]);
			this.fractionLength = fractionLength - i;
			length = Math.Max(this.value.Length, this.fractionLength + 1);
		}
	}

	#endregion

	#region Private methods

	/// <summary>
	/// Removes the heading 0 values from the given <see cref="Positive"/>.
	/// </summary>
	/// <param name="value">The <see cref="Positive"/> to trim.</param>
	/// <returns>The trimmed <see cref="Positive"/>.</returns>
	private static Positive TrimStart(Positive value)
	{
		int i = 0;

		while (i < Math.Min(value.fractionLength, value.Digits.Length) && value[i] == Digit.ZERO) { ++i; }

		return i > 0 ? new Positive(new Natural([.. value.Digits[i..]]), value.fractionLength - i) : value;
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a string that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Positive"/> number as a <see langword="string"/>.</returns>
	public override string ToString()
	{
		return (fractionLength == 0) ? value.ToString() :
			(
				(fractionLength < Digits.Length) ? value.ToString() : 
				new string('0', length - Digits.Length) + value.ToString()
			).Insert(WholeLength, separator.ToString());
	}

	/// <summary>
	/// Returns the whole part of the given <see cref="Positive"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="Positive"/> instance.</param>
	/// <returns>The whole value.</returns>
	public static Natural GetWhole(Positive value)
	{
		return value.fractionLength >= value.Digits.Length ? new Natural([.. value.Digits[(^value.WholeLength)..]]) : new Natural();
	}

	/// <summary>
	/// Compares two <see cref="Positive"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Positive"/> to compare.</param>
	/// <param name="right">The second <see cref="Positive"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is equal to the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(Positive left, Positive right) => left.length == right.length && left.fractionLength == right.fractionLength && left.value == right.value;

	/// <summary>
	/// Compares two <see cref="Positive"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Positive"/> to compare.</param>
	/// <param name="right">The second <see cref="Positive"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(Positive left, Positive right)
	{
		if (left.WholeLength != right.WholeLength)
			return left.WholeLength > right.WholeLength;
		else if (left.fractionLength == right.fractionLength)
			return left.value > right.value;
		else
		{
			Digit[] splicing = Digit.CreateArray(Math.Abs(left.fractionLength - right.fractionLength));

			return left.fractionLength < right.fractionLength ? new Natural([.. splicing, .. left.Digits]) > right.Value : 
				left.Value > new Natural([.. splicing, .. right.Digits]);
		}
	}

	/// <summary>
	/// Adds two <see cref="Positive"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Positive"/> to add.</param>
	/// <param name="right">The second <see cref="Positive"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Positive Add(Positive left, Positive right)
	{
		int difference = left.fractionLength - right.fractionLength;

		if (difference < 0)
			(left, right) = (right, left);

		return TrimStart(new Positive(left.value + new Natural([.. Digit.CreateArray(Math.Abs(difference)), .. right.Digits]), left.fractionLength));
	}

	/// <summary>
	/// Subtracts two <see cref="Positive"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Positive"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Positive"/> that represents the subtrahend.</param>
	/// <returns>The result value and if there was a swap in a tuple.</returns>
	public static (bool Swap, Positive Value) Subtract(Positive left, Positive right)
	{
		int maxFractionLength = Math.Max(left.fractionLength, right.fractionLength);
		Digit[] splicing = Digit.CreateArray(Math.Abs(left.fractionLength - right.fractionLength));

		(bool swap, Natural result) = left.fractionLength < right.fractionLength ? Natural.Subtract(new Natural([.. splicing, .. left.Digits]), right.value) :
			Natural.Subtract(left.value, new Natural([.. splicing, .. right.Digits]));

		return (swap, TrimStart(new Positive(result, maxFractionLength)));
	}

	/// <summary>
	/// Multiplies two <see cref="Positive"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Positive"/> that represents the multiplier.</param>
	/// <param name="right">The <see cref="Positive"/> that represents the multiplicand.</param>
	/// <returns>The result of the calculation.</returns>
	public static Positive Multiply(Positive left, Positive right) => new(Natural.Multiply(left.value, right.value), left.fractionLength + right.fractionLength);

	/// <summary>
	/// Divides two <see cref="Positive"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Positive"/> that represents the numerator.</param>
	/// <param name="right">The <see cref="Positive"/> that represents the denominator.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static (Positive Value, Positive Remainder) Divide(Positive left, Positive right, int? fractionCalculationLength = null)
	{
		int fCL = Math.Max(fractionCalculationLength ?? Positive.fractionCalculationLength, 0);

		int denominatorSlicingLength = Math.Max(left.fractionLength - right.fractionLength, 0);
		int numeratorSlicingLength = Math.Max(right.fractionLength - left.fractionLength, 0);

		Natural denominator = new([.. Digit.CreateArray(denominatorSlicingLength), .. right.Digits]);
		Natural numerator = new([.. Digit.CreateArray(numeratorSlicingLength + fCL), .. left.Digits]);

		(Natural whole, Natural remainder) = Natural.Divide(numerator, denominator);

		return (new Positive(whole, fCL), new Positive(remainder, fCL + numeratorSlicingLength + left.FractionLength));
	}

	/// <summary>
	/// Raises the given <paramref name="value"/> to the second power.
	/// </summary>
	/// <param name="value">The <see cref="Positive"/> that represents the base.</param>
	/// <returns>The result of the calculation.</returns>
	public static Positive SecondPower(Positive value) => value * value;

	/// <summary>
	/// Raises the given base to the given power.
	/// </summary>
	/// <param name="left">The <see cref="Positive"/> that represents the base.</param>
	/// <param name="right">The <see cref="Positive"/> that represents the exponent.</param>
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="NotSupportedException">
	/// <paramref name="right"/> cannot be a fraction or higher than 999 as it would be too computationally expensive.
	/// </exception>
	public static Positive Power(Positive left, Positive right)
	{
		return right.fractionLength == 0 ? 
			new Positive(left.value ^ right.value, left.fractionLength * (int)Natural.ToUInt32(right.Value)) : throw new NotSupportedException();
	}

	/// <summary>
	/// Raises the given radicand to the second degree.
	/// </summary>
	/// <param name="value">The <see cref="Positive"/> that represents the radicand.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	public static (Positive Value, Positive Remainder) SquareRoot(Positive value, int? fractionCalculationLength = null)
	{
		if (value.IsZero || value == "1")
			return (value, new Positive());

		int fCL = Math.Max(fractionCalculationLength ?? Positive.fractionCalculationLength, 0);
		int splicingLength = value.fractionLength & 1;
		int zeroCalculationLength = (fCL * 2 - (value.fractionLength + splicingLength)) / 2;

		(Natural root, Natural remainder) = Natural.SquareRoot(new Natural([.. Digit.CreateArray(splicingLength), .. value.Digits]));

		for (int i = zeroCalculationLength; i > 0; --i)
		{
			remainder = new Natural([Digit.ZERO, Digit.ZERO, .. remainder.Digits]);

			root = Natural.CalculateTwoRootDigits(root, ref remainder);
		}

		return (new Positive(root, fCL), new Positive(remainder, fCL * 2));
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Positive"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Positive"/> that represents the degree.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException"><paramref name="right"/> cannot be 0 as it is not mathematically meaningful.</exception>
	/// <exception cref="NotSupportedException">
	/// <paramref name="right"/> cannot be a fraction or higher than 99 as it would be too computationally expensive.
	/// </exception>
	public static (Positive Value, Positive Remainder) Root(Positive left, Positive right, int? fractionCalculationLength = null)
	{
		if (right.IsZero)
			throw new NotImplementedException();
		if (right.fractionLength != 0)
			throw new NotSupportedException();
		else if (right.value < Digit.THREE)
		{
			return Digit.ToChar(right[0]) switch
			{
				'0' => throw new NotImplementedException(),
				'1' => (left, "0"),
				_ => SquareRoot(left)
			};
		}

		if (left.IsZero || left == "1")
			return (left, "0");

		Natural degree = right.Value;
		int degreeInt = (int)Natural.ToUInt32(degree);
		int splicingLength = degreeInt - (left.fractionLength % degreeInt);

		(Natural root, Natural remainder) = Natural.Root(new([.. Digit.CreateArray(splicingLength), .. left.Digits]), degree);

		int fCL = Math.Max(fractionCalculationLength ?? Positive.fractionCalculationLength, 0);
		Natural degreeFactorial = Natural.Factorial(degree);

		for (int i = (fCL * degreeInt - (left.fractionLength + splicingLength)) / degreeInt; i > 0; --i)
		{
			remainder = new Natural([.. Digit.CreateArray(degreeInt), .. remainder.Digits]);

			root = Natural.CalculateNRootDigits(root, ref remainder, degree, degreeFactorial);
		}

		return (new Positive(root, fCL), new Positive(remainder, fCL * degreeInt));
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Positive"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Positive positive && this == positive;

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Positive(string value) => new(value);
	public static implicit operator Positive(Natural value) => new(value, 0);
	public static bool operator ==(Positive left, Positive right) => Equals(left, right);
	public static bool operator !=(Positive left, Positive right) => !Equals(left, right);
	public static bool operator >(Positive left, Positive right) => GreaterThan(left, right);
	public static bool operator <(Positive left, Positive right) => GreaterThan(right, left);
	public static bool operator >=(Positive left, Positive right) => !GreaterThan(right, left);
	public static bool operator <=(Positive left, Positive right) => !GreaterThan(left, right);
	public static Positive operator +(Positive left, Positive right) => Add(left, right);
	public static Positive operator -(Positive left, Positive right) => Subtract(left, right).Value;
	public static Positive operator *(Positive left, Positive right) => Multiply(left, right);
	public static Positive operator /(Positive left, Positive right) => Divide(left, right).Value;
	public static Positive operator %(Positive left, Positive right) => Divide(left, right, 0).Remainder;
	public static Positive operator ^(Positive left, Positive right) => Power(left, right);
	public static Positive operator ~(Positive value) => SquareRoot(value).Value;
	public static Positive operator |(Positive left, Positive right) => Root(right, left).Value;

	#endregion
}