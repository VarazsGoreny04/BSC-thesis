using ProjectReal.NumberSet;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ProjectReal.Number;

/// <summary>
/// Represents a rational number.
/// </summary>
public class Rational :
	IComparisonOperators<Rational, Rational, bool>,
	IEqualityOperators<Rational, Rational, bool>,
	IIncrementOperators<Rational>,
	IDecrementOperators<Rational>,
	IUnaryPlusOperators<Rational, Rational>,
	IUnaryNegationOperators<Rational, Rational>,
	IAdditionOperators<Rational, Rational, Rational>,
	ISubtractionOperators<Rational, Rational, Rational>,
	IMultiplyOperators<Rational, Rational, Rational>,
	IDivisionOperators<Rational, Rational, Rational>,
	IModulusOperators<Rational, Rational, Rational>,
	IPowerOperations<Rational, Rational, Rational>,
	IRootOperations<Rational, Rational, Rational>,
	IAdditiveIdentity<Rational, Rational>,
	IMultiplicativeIdentity<Rational, Rational>,
	IParsable<Rational>
{
	#region Fields

	private static bool fractionalFormat = true;

	private readonly Writable numerator;
	private readonly Positive? denominator;

	#endregion

	#region Properties

	public static Rational AdditiveIdentity => 0;

	public static Rational MultiplicativeIdentity => 1;

	/// <summary>
	/// Gets or sets whether the <see cref="ToString"/> method should write the number in a fractional form or calculate the division.
	/// </summary>
	/// <remarks>
	/// If <see langword="true"/> than the <see cref="ToString"/> method writes the number as a fraction;
	/// if <see langword="false"/> it calculates the division.
	/// </remarks>
	public static bool FractionalFormat
	{
		get => fractionalFormat;
		set => fractionalFormat = value;
	}

	/// <summary>
	/// Gets or sets whether the <see cref="ToString"/> method should write the + sign to the front of the number.
	/// </summary>
	public static bool WriteSign
	{
		get => Writable.WriteSign;
		set => Writable.WriteSign = value;
	}

	/// <returns>The character the <see cref="ToString"/> method uses as separator</returns>
	public static char Separator => Writable.Separator;

	/// <summary>
	/// Gets or sets the length of calculating fractions.
	/// </summary>
	/// <exception cref="ArgumentException"><param name="value"/> cannot be less than 0.</exception>
	public static int FractionCalculationLength
	{
		get => Writable.FractionCalculationLength;
		set => Writable.FractionCalculationLength = value;
	}

	/// <summary>
	/// Returns whether <see langword="this"/> is equal to 0.
	/// </summary>
	/// <returns><see langword="true"/> if <see langword="this"/> is equal to 0; otherwise, <see langword="false"/>.</returns>
	public bool IsZero => numerator.IsZero;

	/// <summary>
	/// The sign of <see langword="this"/> <see cref="Rational"/> represented by a boolean.
	/// </summary>
	/// <returns><see langword="true"/> if the sign is +; <see langword="false"/> if the sign is -.</returns>
	public bool Sign => numerator.Sign;

	/// <returns>The <see cref="Positive"/> used to represent the numerator without indicating the sign.</returns>
	public Positive Numerator => numerator.Value;

	/// <returns>The <see cref="Positive"/> used to represent the denominator without indicating the sign.</returns>
	public Positive? Denominator => denominator;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Rational"/> with a value of 0.
	/// </summary>
	public Rational()
	{
		numerator = new Writable();
		denominator = null;
	}

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <see langword="string"/> parameter.
	/// </summary>
	/// <param name="number">
	/// Takes a <see langword="string"/> of <see cref="Writable"/> format 
	/// -or- 
	/// two <see cref="string"/>s of <see cref="Writable"/> format with a division character in between them. Just like a fraction.
	/// </param>
	/// <exception cref="ArgumentException"><paramref name="number"/> is not a valid number format.</exception>
	/// <exception cref="DivideByZeroException">The denominator cannot be 0, as it is not mathematically meaningful.</exception>
	public Rational(string number)
	{
		if (number.Length < 1)
			throw new ArgumentException("The given string parameter must not be empty!", nameof(number));

		string[] parts = number.Split('/');

		switch (parts.Length)
		{
			case 1:
				(numerator, denominator) = (parts[0], null);
				break;
			case 2:
				(Writable numerator, Writable denominator) temp = (parts[0], parts[1]);

				if (temp.denominator.IsZero)
					throw new DivideByZeroException("The denominator cannot be 0, as it is not mathematically meaningful!");

				(numerator, denominator) = Simplify(new Writable(temp.numerator.Sign == temp.denominator.Sign, temp.numerator.Value), temp.denominator.Value);
				break;
			default:
				throw new ArgumentException("The given string parameter must be a valid number format.");
		}
	}

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <paramref name="numerator"/> and <paramref name="denominator"/>.
	/// </summary>
	/// <param name="numerator">The value of the <paramref name="numerator"/>.</param>
	/// <param name="denominator">The value of the <paramref name="denominator"/>.</param>
	/// <exception cref="DivideByZeroException"><paramref name="denominator"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public Rational(Writable numerator, Writable? denominator)
	{
		if (denominator is Writable denominatorValue)
		{
			if (denominatorValue.IsZero)
				throw new DivideByZeroException("The denominator cannot be 0, as it is not mathematically meaningful!");

			(this.numerator, this.denominator) = Simplify(new Writable(numerator.Sign == denominatorValue.Sign, numerator.Value), denominatorValue.Value);
		}
		else
		{
			this.numerator = numerator;
			this.denominator = null;
		}
	}

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <paramref name="numerator"/> and <paramref name="denominator"/>.
	/// </summary>
	/// <param name="numerator">The value of the <paramref name="numerator"/>.</param>
	/// <param name="denominator">The value of the <paramref name="denominator"/>.</param>
	/// <exception cref="DivideByZeroException"><paramref name="denominator"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public Rational(Writable numerator, Positive? denominator = null) : this(numerator, denominator is Positive d ? new Writable(true, d) : null) { }

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <paramref name="sign"/>, <paramref name="numerator"/> and <paramref name="denominator"/>.
	/// </summary>
	/// <param name="sign">The <paramref name="sign"/> of the number.</param>
	/// <param name="numerator">The value of the <paramref name="numerator"/>.</param>
	/// <param name="denominator">The value of the <paramref name="denominator"/>.</param>
	/// <exception cref="DivideByZeroException"><paramref name="denominator"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public Rational(bool sign, Positive numerator, Positive? denominator = null) : this(new Writable(sign, numerator), denominator) { }

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <see cref="Digit"/>.
	/// </summary>
	public Rational(byte value) : this(true, value) { }

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <see cref="Natural"/>.
	/// </summary>
	public Rational(Natural value) : this(true, value) { }

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <see cref="Integer"/>.
	/// </summary>
	public Rational(Integer value) : this(value.Sign, value.Value) { }

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <see cref="Positive"/>.
	/// </summary>
	public Rational(Positive value) : this(true, value) { }

	#endregion

	#region Private methods

	/// <summary>
	/// For the sine and cosine functions. Approximates the minimum iterations needed for the given precision for the given value.
	/// </summary>
	/// <param name="x">The given value.</param>
	/// <param name="fractionCalculationLength">The precision needed in digits.</param>
	/// <returns>The number of iterations.</returns>
	private static int IterationsNeededSinCos(Rational x, int fractionCalculationLength)
	{
		int divisor = x switch
		{
			Rational when x <= "0.7" => 7,
			Rational when x <= "1.5" => 5,
			_ => 4
		};

		return (fractionCalculationLength * 2 / divisor) + 20;
	}

	/// <summary>
	/// Finds the lowest common denominator for the two <see cref="Rational"/> numbers.
	/// </summary>
	/// <param name="first">The first <see cref="Rational"/>.</param>
	/// <param name="second">The second <see cref="Rational"/>.</param>
	/// <returns>The two numerator values and the common denominator in a tuple.</returns>
	private static (Writable Numerator1, Writable Numerator2, Positive? Denominator) CommonDenominator(Rational first, Rational second)
	{
		Writable numerator1 = second.denominator is null ? first.numerator : new(first.numerator.Sign, first.numerator.Value * second.denominator);
		Writable numerator2 = first.denominator is null ? second.numerator : new(second.numerator.Sign, second.numerator.Value * first.denominator);
		Positive? denominator = first.denominator is null ? second.denominator :
			second.denominator is null ? first.denominator : first.denominator * second.denominator;

		return (numerator1, numerator2, denominator);
	}

	/// <summary>
	/// Gets the simplest form of the given <paramref name="numerator"/> and <paramref name="denominator"/>.
	/// </summary>
	/// <remarks>Note that every constructor of <see cref="Rational"/> already uses this method.</remarks>
	/// <param name="numerator">The numerator of the <see cref="Rational"/>.</param>
	/// <param name="denominator">The denominator of the <see cref="Rational"/>.</param>
	/// <returns>The simplified numerator and denominator value in a tuple.</returns>
	internal static (Writable Numerator, Positive? Denominator) Simplify(Writable numerator, Positive? denominator)
	{
		if (denominator is Positive denominatorValue)
		{
			Natural gCD = Natural.GreatestCommonDivisor(numerator.Value.Value, denominatorValue.Value);

			Positive resultNumerator = new(numerator.Value.Value / gCD, Math.Max(numerator.FractionLength - denominatorValue.FractionLength, 0));
			Positive resultDenominator = new(denominatorValue.Value / gCD, Math.Max(denominatorValue.FractionLength - numerator.FractionLength, 0));

			return (new Writable(numerator.Sign, resultNumerator), resultDenominator == 1 ? null : resultDenominator);
		}
		else
			return (numerator, null);
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a <see cref="string"/> that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Rational"/> number as a <see langword="string"/>.</returns>
	public override string ToString() => fractionalFormat ? ToFractionString(this) : ToWritableString(this);

	/// <summary>
	/// Returns a string that represents the value of the given <see cref="Rational"/> in fraction format.
	/// </summary>
	/// <returns>An <see cref="Rational"/> number as a <see langword="string"/>.</returns>
	public static string ToFractionString(Rational value)
	{
		return value.denominator is Positive denominator ? $"{value.numerator}/{denominator}" : value.numerator.ToString();
	}

	/// <summary>
	/// Returns a string that represents the value of the given <see cref="Rational"/> in writable format.
	/// </summary>
	/// <returns>An <see cref="Rational"/> number as a <see langword="string"/>.</returns>
	public static string ToWritableString(Rational value) => GetValue(value).Value.ToString();

	/// <summary>
	/// Parses a <see cref="string"/> into a <see cref="Rational"/> instance.
	/// </summary>
	/// <param name="s">The <see cref="string"/> to parse.</param>
	/// <param name="_">This parameter is unused.</param>
	/// <returns>The created instance.</returns>
	/// <exception cref="ArgumentException">The <see cref="string"/> must be accepted by the constructor.</exception>
	/// <exception cref="DivideByZeroException">The denominator cannot be 0, as it is not mathematically meaningful.</exception>
	public static Rational Parse(string s, IFormatProvider? _ = null) => new(s);

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Rational result)
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
	/// Gets the <see cref="Writable"/> value of the given <see cref="Rational"/>.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/>.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	public static (Writable Value, Writable Remainder) GetValue(Rational value, int? fractionCalculationLength = null)
	{
		return value.denominator is Positive denominator ? Writable.Divide(value.numerator, denominator, fractionCalculationLength) :
			(value.numerator, new Writable());
	}

	/// <summary>
	/// Rounds down the absolute value of the given <see cref="Rational"/> instance and keeps the sign.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> instance.</param>
	/// <returns>The whole part of the given <see cref="Rational"/> instance.</returns>
	public static Integer RoundDown(Rational value) => Writable.RoundDown(GetValue(value).Value);

	/// <summary>
	/// Rounds up the absolute value of the given <see cref="Rational"/> instance and keeps the sign.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> instance.</param>
	/// <returns>The whole part of the given <see cref="Rational"/> instance plus one if it was not whole.</returns>
	public static Integer RoundUp(Rational value) => Writable.RoundUp(GetValue(value).Value);

	/// <summary>
	/// Rounds the given <see cref="Rational"/> instance to the nearest number.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> instance.</param>
	/// <returns>The rounded value of the given <see cref="Rational"/> instance.</returns>
	public static Integer Round(Rational value) => Writable.Round(GetValue(value).Value);

	/// <summary>
	/// Gets the absolute value of the given <see cref="Rational"/>.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/>.</param>
	/// <returns>The absolute value of the given <see cref="Rational"/>.</returns>
	public static Rational Abs(Rational value) => new(true, value.Numerator, value.denominator);

	/// <summary>
	/// Compares two <see cref="Rational"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Rational"/> to compare.</param>
	/// <param name="right">The second <see cref="Rational"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is equal to the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(Rational left, Rational right)
	{
		(Writable numerator1, Writable numerator2, _) = CommonDenominator(left, right);

		return numerator1 == numerator2;
	}

	/// <summary>
	/// Compares two <see cref="Rational"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Rational"/> to compare.</param>
	/// <param name="right">The second <see cref="Rational"/> to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref name="right"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(Rational left, Rational right)
	{
		(Writable numerator1, Writable numerator2, _) = CommonDenominator(left, right);

		return numerator1 > numerator2;
	}

	/// <summary>
	/// Calculates the reciprocal of the given <see cref="Rational"/>.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> for which the reciprocal is to be calculated.</param>
	/// <returns>The reciprocal of the given <paramref name="value"/>.</returns>
	/// <exception cref="DivideByZeroException">
	/// <paramref name="value"/> cannot be 0, as it is not mathematically meaningful to calculate its reciprocal.
	/// </exception>
	public static Rational Reciprocal(Rational value)
	{
		return value.IsZero ?
			throw new DivideByZeroException("The denominator cannot be 0, as it is not mathematically meaningful to calculate its reciprocal!")
			: new Rational(value.Sign, value.denominator is Positive p ? p : 1, value.Numerator);
	}

	/// <summary>
	/// Adds two <see cref="Rational"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Rational"/> to add.</param>
	/// <param name="right">The second <see cref="Rational"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Rational Add(Rational left, Rational right)
	{
		(Writable numerator1, Writable numerator2, Positive? denominator) = CommonDenominator(left, right);

		return new Rational(numerator1 + numerator2, denominator);
	}

	/// <summary>
	/// Subtracts two <see cref="Rational"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the subtrahend.</param>
	/// <returns>The result of the calculation.</returns>
	public static Rational Subtract(Rational left, Rational right)
	{
		(Writable numerator1, Writable numerator2, Positive? denominator) = CommonDenominator(left, right);

		return new Rational(numerator1 - numerator2, denominator);
	}

	/// <summary>
	/// Multiplies two <see cref="Rational"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the multiplier.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the multiplicand.</param>
	/// <returns>The result of the calculation.</returns>
	public static Rational Multiply(Rational left, Rational right)
	{
		Positive? denominator = left.denominator is null ? right.denominator : right.denominator is null ? left.denominator : left.denominator * right.denominator;

		return new Rational(left.numerator * right.numerator, denominator);
	}

	/// <summary>
	/// Divides two <see cref="Rational"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the numerator.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the denominator.</param>
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static Rational Divide(Rational left, Rational right) => left * Reciprocal(right);

	/// <summary>
	/// Gets the modulo of two <see cref="Rational"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the left hand side of the operation.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the right hand side of the operation.</param>
	/// <returns>The modulus.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static Rational Modulo(Rational left, Rational right)
	{
		(Writable numerator1, Writable numerator2, Positive? denominator) = CommonDenominator(left, right);

		return new Rational(numerator1 % numerator2, denominator);
	}

	/// <summary>
	/// Raises the given <paramref name="value"/> to the second power.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> that represents the base.</param>
	/// <returns>The result of the calculation.</returns>
	public static Rational SecondPower(Rational value) => value * value;

	/// <summary>
	/// Raises the given base to the given power.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the base.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the exponent.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="NotSupportedException">
	/// Absolut value of <paramref name="right"/> cannot be higher than 999 as it would be too computationally expensive.
	/// -or-
	/// If <paramref name="left"/> is negative than <paramref name="right"/> must be a whole number.
	/// This type does not support complex number results.
	/// </exception>
	public static Rational Power(Rational left, Rational right, int? fractionCalculationLength = null)
	{
		Rational rightAbs = Abs(right);
		Rational result;

		if (rightAbs.denominator is not null || rightAbs.numerator.FractionLength > 0)
		{
			if (!left.Sign)
				throw new NotSupportedException("This type does not support complex number results!");

			int fCL = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 1);

			Rational ln = Ln(left, fCL + (right.numerator.WholeLength - right.denominator?.WholeLength) + 1);
			Rational mul = right * ln;

			return Exp(mul, fCL);
		}
		else
			result = new Rational(left.numerator ^ rightAbs.numerator, left.denominator is not null ? left.denominator ^ rightAbs.Numerator : left.denominator);

		return right.Sign ? result : Reciprocal(result);
	}

	/// <summary>
	/// Raises the given radicand to the second degree.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> that represents the radicand.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException"><paramref name="value"/> being negative is not mathematically meaningful.</exception>
	public static (Rational Value, Writable NumeratorRemainder, Positive DenominatorRemainder) SquareRoot(Rational value, int? fractionCalculationLength = null)
	{
		(Writable Value, Writable Remainder) numerator = Writable.SquareRoot(value.numerator, fractionCalculationLength);
		(Positive Value, Positive Remainder) denominator = value.denominator is Positive d ? Positive.SquareRoot(d, fractionCalculationLength) : (1, 0);

		return (new Rational(numerator.Value, denominator.Value), numerator.Remainder, denominator.Remainder);
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the degree.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="ArgumentException">
	/// <paramref name="left"/> being negative and <paramref name="right"/> being even is not mathematically meaningful.
	/// </exception>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0 as is not mathematically meaningful.</exception>
	/// <exception cref="NotSupportedException"> <paramref name="right"/> cannot be higher than 99 as it would be too computationally expensive.</exception>
	public static (Rational Value, Writable NumeratorRemainder, Positive DenominatorRemainder) Root(Rational left, Rational right, int? fractionCalculationLength = null)
	{
		if (right.denominator is not null || right.numerator.FractionLength > 0)
			return (Power(left, Reciprocal(right), fractionCalculationLength), 0, 0);

		(Writable Value, Writable Remainder) numerator = Writable.Root(left.numerator, right.numerator, fractionCalculationLength);
		(Positive Value, Positive Remainder) denominator = left.denominator is Positive d ?
			Positive.Root(d, right.Numerator, fractionCalculationLength) : (1, 0);

		return (new Rational(numerator.Value, denominator.Value), numerator.Remainder, denominator.Remainder);
	}

	/// <summary>
	/// Calculates π until the given <paramref name="fractionCalculationLength"/> using the Chudnovsky-formula with binary splitting.
	/// </summary>
	/// <remarks>
	/// <para>I wanted to make a tribute to Srinivasa Ramanujan who came up with this method and to the Chudnovsky brothers<br/>
	/// - David and Gregory Chudnovsky - for developing a generalisation to Ramanujan's formula.</para>
	/// <para><see href="https://en.wikipedia.org/wiki/Chudnovsky_algorithm"/><br/>
	/// <see href="https://www.craig-wood.com/nick/articles/pi-chudnovsky/"/></para>
	/// </remarks>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The number π.</returns>
	public static Rational Pi(int? fractionCalculationLength = null)
	{
		static (Natural P, Natural Q, Integer T) BinarySplitting(Natural a, Natural b)
		{
			if (a + 1 >= b)
			{
				(Natural P, Natural Q) = a.IsZero ? (1, 1) :
					((6 * a - 5) * (2 * a - 1) * (6 * a - 1), a * a * a * "10939058860032000");

				Integer T = P * ("13591409" + "545140134" * a);

				if (a[0] % 2 == 1)
					T = -T;

				return (P, Q, T);
			}
			else
			{
				Natural m = (a + b) / 2;

				(Natural Pl, Natural Ql, Integer Tl) = BinarySplitting(a, m);
				(Natural Pr, Natural Qr, Integer Tr) = BinarySplitting(m, b);

				return (
					P: Pl * Pr,
					Q: Ql * Qr,
					T: Qr * Tl + Pl * Tr
				);
			}
		}

		int fCL = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 1);
		int n = fCL / 13 + 1;

		(_, Natural Q, Integer T) = BinarySplitting(0, new Natural((uint)n));

		return new Rational(T.Sign, Q * Positive.SquareRoot("10005", fCL + 2).Value * "426880", T.Value);
	}

	/// <summary>
	/// Calculates the number e until the given <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/E_(mathematical_constant)#Computing_the_digits"/></remarks>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The number e.</returns>
	public static Rational E(int? fractionCalculationLength = null)
	{
		static (Natural Q, Natural T) BinarySplitting(int n1, int n2)
		{
			if (n1 + 1 >= n2)
			{
				return n1 == 0 ?
				(
					Q: 1,
					T: 1
				) :
				(
					Q: new Natural((uint)n1),
					T: (byte)1
				);
			}
			else
			{
				int nm = (n1 + n2) / 2;

				(Natural Ql, Natural Tl) = BinarySplitting(n1, nm);
				(Natural Qr, Natural Tr) = BinarySplitting(nm, n2);

				return (
					Q: Ql * Qr,
					T: Qr * Tl + Tr
				);
			}
		}

		int n = Math.Max((fractionCalculationLength ?? FractionCalculationLength) / 2, 1) + 23;

		(Natural Q, Natural T) = BinarySplitting(0, n);

		return new Rational(T, Q);
	}

	/// <summary>
	/// Calculates the exponential function for the given exponent
	/// until the given <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks>
	/// <see href="https://ginac.de/CLN/binsplit.pdf"/><br/>
	/// <see href="https://stackoverflow.com/questions/57510825/binary-splitting-in-pari-gp"/>
	/// </remarks>
	/// <param name="x">The exponent in e^<paramref name="x"/>.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The the exponential function for the given exponent.</returns>
	public static Rational Exp(Rational x, int? fractionCalculationLength = null)
	{
		static (Writable P, Positive Q, Writable T) BinarySplitting(int n1, int n2, Rational x)
		{
			if (n1 + 1 >= n2)
			{
				return n1 == 0 ?
				(
					P: 1,
					Q: 1,
					T: 1
				) :
				(
					P: x.numerator,
					Q: x.denominator is not null ? new Natural((uint)n1) * x.denominator : new Natural((uint)n1),
					T: x.numerator
				);
			}
			else
			{
				int nm = (n1 + n2) / 2;

				(Writable Pl, Positive Ql, Writable Tl) = BinarySplitting(n1, nm, x);
				(Writable Pr, Positive Qr, Writable Tr) = BinarySplitting(nm, n2, x);

				return (
					P: Pl * Pr,
					Q: Ql * Qr,
					T: Qr * Tl + Pl * Tr
				);
			}
		}

		static Rational Exp(int n, Rational x)
		{
			(_, Positive Q, Writable T) = BinarySplitting(0, n, x);

			return new Rational(T, Q);
		}

		Integer whole = RoundDown(x);
		Rational fraction = x - whole;

		int n = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 1);
		int plusFractionLength = Integer.ToInt32(RoundDown(x * "0.43457") + 1);

		Rational powE = whole.IsZero ? 1 : Power(E(n + plusFractionLength), whole, n);
		return fraction.IsZero ? powE : powE * Exp(n + 5, fraction);
	}

	/// <summary>
	/// Calculates the natural logarithm for the given anti-logarithm until
	/// the given <paramref name="fractionCalculationLength"/> using Halley's method.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/Natural_logarithm#High_precision"/></remarks>
	/// <param name="x">The anti-logarithm.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The natural logarithm of the given parameter.</returns>
	/// <exception cref="ArgumentOutOfRangeException">The anti-logarithm cannot be negative as it is not mathematically meaningful.</exception>
	public static Rational Ln(Rational x, int? fractionCalculationLength = null)
	{
		static (Integer Exponent, Writable ReducedX) DeconstructToMultiplication(Rational x, int fractionCalculationLength)
		{
			Integer n = 0;

			if (x >= 2)
			{
				Natural twoToTheNth = 1;

				while ((twoToTheNth *= 2) <= x)
					n += 1;

				return (n, GetValue(Divide(x, twoToTheNth / 2)).Value);
			}
			else
			{
				Rational twoToTheNth = 1;

				while ((twoToTheNth /= 2) >= x)
					n -= 1;

				return (n - 1, GetValue(Divide(x, twoToTheNth)).Value);
			}
		}

		static Rational Ln(Rational y, Writable x, int iterations, int fractionCalculationLength)
		{
			if (iterations <= 0)
				return y;

			Writable expY = GetValue(Exp(y, fractionCalculationLength), fractionCalculationLength).Value;
			return Ln(y + (2 * new Rational(x - expY) / new Rational(x + expY)), x, iterations - 1, fractionCalculationLength);
		}

		if (x <= 0)
			throw new ArgumentOutOfRangeException(nameof(x), x, "The anti-logarithm cannot be negative as it is not mathematically meaningful!");

		int n = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 10);

		(Integer exponent, Writable reducedX) = DeconstructToMultiplication(x, n);

		return Ln(("0.7" * (reducedX - 1)) - (Abs("1.5" - reducedX) / 9) + "0.06", reducedX, 3, n) + exponent * Ln("0.693", 2, 3, n + exponent.Length);
	}

	/// <summary>
	/// Calculates the sine function for the given <paramref name="x"/> value until the given
	/// <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks>
	/// <see href="https://ginac.de/CLN/binsplit.pdf"/>
	/// </remarks>
	/// <param name="x">The given <paramref name="x"/>.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The the exponential function for the given exponent.</returns>
	public static Rational Sin(Rational x, int? fractionCalculationLength = null)
	{
		static (Writable P, Positive Q, Writable T) BinarySplitting(int n1, int n2, Rational x)
		{
			if (n1 + 1 >= n2)
			{
				Writable tempP;

				return n1 == 0 ?
				(
					P: x.numerator,
					Q: x.denominator ?? 1,
					T: x.numerator
				) :
				(
					P: tempP = -Writable.SecondPower(x.numerator),
					Q: new Natural((uint)(2 * n1 * (2 * n1 + 1))) * Positive.SecondPower(x.denominator ?? 1),
					T: tempP
				);
			}
			else
			{
				int nm = (n1 + n2) / 2;

				(Writable Pl, Positive Ql, Writable Tl) = BinarySplitting(n1, nm, x);
				(Writable Pr, Positive Qr, Writable Tr) = BinarySplitting(nm, n2, x);

				return (
					P: Pl * Pr,
					Q: Ql * Qr,
					T: Qr * Tl + Pl * Tr
				);
			}
		}

		int fCL = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 1);

		if (Abs(x) > 6)
			x %= 2 * Pi(fCL);

		int n = IterationsNeededSinCos(x, fCL);

		(_, Positive Q, Writable T) = BinarySplitting(0, n, x);

		return new Rational(T, Q);
	}

	/// <summary>
	/// Calculates the cosine function for the given <paramref name="x"/> value until the given
	/// <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks>
	/// <see href="https://ginac.de/CLN/binsplit.pdf"/>
	/// </remarks>
	/// <param name="x">The given <paramref name="x"/>.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The the exponential function for the given exponent.</returns>
	public static Rational Cos(Rational x, int? fractionCalculationLength = null)
	{
		static (Writable P, Positive Q, Writable T) BinarySplitting(int n1, int n2, Rational x)
		{
			if (n1 + 1 >= n2)
			{
				Writable tempP;

				return n1 == 0 ?
				(
					P: 1,
					Q: 1,
					T: 1
				) :
				(
					P: tempP = -Writable.SecondPower(x.numerator),
					Q: new Natural((uint)(2 * n1 * (2 * n1 - 1))) * Positive.SecondPower(x.denominator ?? 1),
					T: tempP
				);
			}
			else
			{
				int nm = (n1 + n2) / 2;

				(Writable Pl, Positive Ql, Writable Tl) = BinarySplitting(n1, nm, x);
				(Writable Pr, Positive Qr, Writable Tr) = BinarySplitting(nm, n2, x);

				return (
					P: Pl * Pr,
					Q: Ql * Qr,
					T: Qr * Tl + Pl * Tr
				);
			}
		}

		int fCL = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 1);

		if (Abs(x) > 6)
			x %= 2 * Pi(fCL);

		int n = IterationsNeededSinCos(x, fCL);

		(_, Positive Q, Writable T) = BinarySplitting(0, n, x);

		return new Rational(T, Q);
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Rational"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Rational real && this == real;

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode()
	{
		throw new NotImplementedException("This method is not implemented because there are more possible values ​​than the int can handle.");
	}

	#endregion

	#region Operators

	public static implicit operator Rational(string value) => new(value);
	public static implicit operator Rational(byte value) => new(value);
	public static implicit operator Rational(Natural value) => new(value);
	public static implicit operator Rational(Integer value) => new(value);
	public static implicit operator Rational(Positive value) => new(value);
	public static implicit operator Rational(Writable value) => new(value);
	public static bool operator ==(Rational? left, Rational? right) => left is Rational l && right is Rational r && Equals(l, r);
	public static bool operator !=(Rational? left, Rational? right) => !(left == right);
	public static bool operator >(Rational left, Rational right) => GreaterThan(left, right);
	public static bool operator <(Rational left, Rational right) => GreaterThan(right, left);
	public static bool operator >=(Rational left, Rational right) => !GreaterThan(right, left);
	public static bool operator <=(Rational left, Rational right) => !GreaterThan(left, right);
	public static Rational operator +(Rational value) => value;
	public static Rational operator -(Rational value) => new(!value.numerator.Sign, value.numerator.Value, value.denominator);
	public static Rational operator ++(Rational value) => Add(value, 1);
	public static Rational operator --(Rational value) => Subtract(value, 1);
	public static Rational operator +(Rational left, Rational right) => Add(left, right);
	public static Rational operator -(Rational left, Rational right) => Subtract(left, right);
	public static Rational operator *(Rational left, Rational right) => Multiply(left, right);
	public static Rational operator /(Rational left, Rational right) => Divide(left, right);
	public static Rational operator %(Rational left, Rational right) => Modulo(left, right);
	public static Rational operator ^(Rational left, Rational right) => Power(left, right);
	public static Rational operator ~(Rational value) => SquareRoot(value).Value;
	public static Rational operator |(Rational left, Rational right) => Root(right, left).Value;

	#endregion
}