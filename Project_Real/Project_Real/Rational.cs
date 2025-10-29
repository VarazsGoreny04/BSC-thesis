using System;

namespace Project_Real;

/// <summary>
/// Represents a rational number.
/// </summary>
public class Rational
{
	#region Fields

	private static bool fractionalFormat = true;

	private readonly Writable numerator;
	private readonly Positive? denominator;

	#endregion

	#region Properties

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

	/// <summary>
	/// Gets or sets the character the <see cref="ToString"/> method should use as separator.
	/// </summary>
	/// <exception cref="ArgumentException">
	/// <param name="value"/> cannot be a number character.
	/// </exception>
	public static char Separator
	{
		get => Writable.Separator;
		set => Writable.Separator = value;
	}

	/// <summary>
	/// Gets or sets the length of calculating fractions.
	/// </summary>
	/// <exception cref="ArgumentException">
	/// <param name="value"/> cannot be less than 0.
	/// </exception>
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
	public Rational(string number)
	{
		if (number is null)
			throw new ArgumentException();

		string[] parts = number.Split('/');

		switch (parts.Length)
		{
			case 1:
				(numerator, denominator) = (parts[0], null);
				break;
			case 2:
				(Writable numerator, Writable denominator) temp = (parts[0], parts[1]);

				if (temp.denominator.IsZero)
					throw new ArgumentException();

				(numerator, denominator) = Simplify(new Writable(temp.numerator.Sign == temp.denominator.Sign, temp.numerator.Value), temp.denominator.Value);
				break;
			default:
				throw new ArgumentException();
		}
	}

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <paramref name="numerator"/> and <paramref name="denominator"/>.
	/// </summary>
	/// <param name="numerator">The value of the <paramref name="numerator"/>.</param>
	/// <param name="denominator">The value of the <paramref name="denominator"/>.</param>
	/// <exception cref="ArgumentException"><paramref name="denominator"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public Rational(Writable numerator, Writable? denominator)
	{
		if (denominator is Writable denominatorValue)
		{
			if (denominatorValue.IsZero)
				throw new ArgumentException();

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
	/// <exception cref="ArgumentException"><paramref name="denominator"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public Rational(Writable numerator, Positive? denominator = null) : this(numerator, (denominator is Positive d ? new Writable(true, d) : null)) { }

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <paramref name="sign"/>, <paramref name="numerator"/> and <paramref name="denominator"/>.
	/// </summary>
	/// <param name="sign">The <paramref name="sign"/> of the number.</param>
	/// <param name="numerator">The value of the <paramref name="numerator"/>.</param>
	/// <param name="denominator">The value of the <paramref name="denominator"/>.</param>
	/// <exception cref="ArgumentException"><paramref name="denominator"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public Rational(bool sign, Positive numerator, Positive? denominator = null) : this(new Writable(sign, numerator), denominator) { }

	#endregion

	#region Internal methods

	/// <summary>
	/// Finds the lowest common denominator for the two <see cref="Rational"/> numbers.
	/// </summary>
	/// <param name="first">The first <see cref="Rational"/>.</param>
	/// <param name="second">The second <see cref="Rational"/>.</param>
	/// <returns>The two numerator values and the common denominator in a tuple.</returns>
	internal static (Writable Numerator1, Writable Numerator2, Positive? Denominator) CommonDenominator(Rational first, Rational second)
	{
		Writable numerator1 = second.denominator is null ? first.numerator : new(first.numerator.Sign, first.numerator.Value * second.denominator);
		Writable numerator2 = first.denominator is null ? second.numerator : new(second.numerator.Sign, second.numerator.Value * first.denominator);
		Positive? denominator = first.denominator is null ? second.denominator :
			(second.denominator is null ? first.denominator : first.denominator * second.denominator);

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

			return (new Writable(numerator.Sign, resultNumerator), (resultDenominator == "1" ? null : resultDenominator));
		}
		else
			return (numerator, null);
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a string that represents the value of <see langword="this"/> instance.
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
	/// Gets the <see cref="Writable"/> value of the given <see cref="Rational"/>.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/>.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	public static (Writable Value, Writable Remainder) GetValue(Rational value, int? fractionCalculationLength = null)
	{
		return (value.denominator is Positive denominator) ? Writable.Divide(value.numerator, new(true, denominator), fractionCalculationLength) :
			(value.numerator, new Writable());
	}

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
		return value.IsZero ? throw new DivideByZeroException() : new Rational(value.Sign, (value.denominator is Positive p ? p : "1"), value.Numerator);
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
		Positive? denominator = left.denominator is null ? right.denominator : (right.denominator is null ? left.denominator : left.denominator * right.denominator);

		return new Rational(left.numerator * right.numerator, denominator);
	}

	/// <summary>
	/// Divides two <see cref="Rational"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the numerator.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the denominator.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="DivideByZeroException"><paramref name="right"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public static Rational Divide(Rational left, Rational right) => left * Reciprocal(right);

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
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="NotImplementedException"><paramref name="right"/> cannot be a fraction.</exception>
	public static Rational Power(Rational left, Rational right)
	{
		if (right.denominator is not null)
			throw new NotImplementedException();

		if (right.Sign)
			return new Rational(left.numerator ^ right.numerator, (left.denominator is Positive denominator ? denominator ^ right.Numerator : left.denominator));
		else
			return new Rational(new Writable(left.Sign, (left.denominator is Positive denominator ? denominator ^ right.Numerator : "1")),
				left.Numerator ^ right.Numerator);
	}

	/// <summary>
	/// Raises the given radicand to the second degree.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> that represents the radicand.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException"><paramref name="value"/> being negative is not mathematically meaningful.</exception>
	public static (Rational Value, Rational Remainder) SquareRoot(Rational value, int? fractionCalculationLength = null)
	{
		(Writable Value, Writable Remainder) numerator = Writable.SquareRoot(value.numerator, fractionCalculationLength);
		(Positive Value, Positive Remainder)? denominator = value.denominator is Positive d ? Positive.SquareRoot(d, fractionCalculationLength) : null;

		return (new Rational(numerator.Value, denominator?.Value), new Rational(numerator.Remainder, denominator?.Remainder));
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the degree.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException">
	/// <paramref name="right"/> being a fraction, negative or 0
	/// -or-
	/// <paramref name="left"/> being negative and <paramref name="right"/> being even is not mathematically meaningful.
	/// </exception>
	public static (Rational Value, Rational Remainder) Root(Rational left, Rational right, int? fractionCalculationLength = null)
	{
		if (right.denominator is not null)
			throw new NotImplementedException();

		(Writable Value, Writable Remainder) numerator = Writable.Root(left.numerator, right.numerator, fractionCalculationLength);
		(Positive Value, Positive Remainder)? denominator = left.denominator is Positive d ? Positive.Root(d, right.Numerator, fractionCalculationLength) : null;

		if (right.Sign)
			return (new Rational(numerator.Value, denominator?.Value), new Rational(numerator.Remainder, denominator?.Remainder));
		else
		{
			return (new Rational(true, denominator?.Value ?? "1", numerator.Value.Value),
				new Rational(true, denominator?.Remainder ?? "1", numerator.Remainder.Value));
		}
	}

	/// <summary>
	/// Calculates π until the given <paramref name="fractionCalculationLength"/> using the Chudnovsky-formula with binary splitting.
	/// </summary>
	/// <remarks>
	/// I wanted to make a tribute to Srinivasa Ramanujan who came up with this method and to the Chudnovsky brothers
	/// - David and Gregory Chudnovsky - for developing a generalisation to Ramanujan's formula.
	/// <para><see href="https://en.wikipedia.org/wiki/Chudnovsky_algorithm"/></para>
	/// <para><see href="https://www.craig-wood.com/nick/articles/pi-chudnovsky/"/></para>
	/// </remarks>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns></returns>
	public static Rational PI(int? fractionCalculationLength = null)
	{
		static (Natural P, Natural Q, Integer T) BinarySplitting(Natural a, Natural b)
		{
			Natural Pab, Qab;
			Integer Tab;

			if (b == a + "1")
			{
				// Directly compute P(a,a+1), Q(a,a+1) and T(a,a+1)
				if (a == "0")
				{
					Pab = "1";
					Qab = "1";
				}
				else
				{
					Pab = ("6" * a - "5") * ("2" * a - "1") * ("6" * a - "1");
					Qab = a * a * a * "10939058860032000";
				}

				Tab = Pab * ("13591409" + "545140134" * a); // a(a) * p(a)

				if (a[0] % '2' == '1')
					Tab = -Tab;
			}
			else
			{
				// Recursively compute P(a,b), Q(a,b) and T(a,b)
				// m is the midpoint of a and b
				Natural m = (a + b) / "2";

				// Recursively calculate P(a,m), Q(a,m) T(a,m) and P(m,b), Q(m,b), T(m,b)
				(Natural Pam, Natural Qam, Integer Tam) = BinarySplitting(a, m);
				(Natural Pmb, Natural Qmb, Integer Tmb) = BinarySplitting(m, b);

				// Now combine
				Pab = Pam * Pmb;
				Qab = Qam * Qmb;
				Tab = Qmb * Tam + Pam * Tmb;
			}
			return (Pab, Qab, Tab);
		}

		int fCL = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 0);

		// how many terms to compute
		Natural DIGITS_PER_TERM = "13";
		Natural n = (new Natural((uint)fCL)) / DIGITS_PER_TERM + "1";

		// Calculate P(0,N) and Q(0,N)
		(Natural _, Natural Q, Integer T) = BinarySplitting("0", n);

		return new Rational(T.Sign, Q * Positive.SquareRoot("10005", fCL).Value * "426880", T.Value);
	}

	/// <summary>
	/// Calculates the number e until the given <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/E_(mathematical_constant)#Computing_the_digits"/></remarks>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns></returns>
	public static Rational E(int? fractionCalculationLength = null)
	{
		static Natural P(Natural a, Natural b)
		{
			if (b == a + "1")
				return "1";
			else
			{
				Natural m = (a + b) / "2";
				return P(a, m) * Q(m, b) + P(m, b);
			}
		}

		static Natural Q(Natural a, Natural b)
		{
			if (b == a + "1")
				return b;
			else
			{
				Natural m = (a + b) / "2";
				return Q(a, m) * Q(m, b);
			}
		}

		Natural n = new((uint)Math.Max(fractionCalculationLength ?? FractionCalculationLength, 0));
		
		return "1" + new Rational(true, P("0", n), Q("0", n));
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
	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Rational(string value) => new(value);
	public static implicit operator Rational(Writable value) => new(value);
	public static bool operator ==(Rational left, Rational right) => Equals(left, right);
	public static bool operator !=(Rational left, Rational right) => !Equals(left, right);
	public static bool operator >(Rational left, Rational right) => GreaterThan(left, right);
	public static bool operator <(Rational left, Rational right) => GreaterThan(right, left);
	public static bool operator >=(Rational left, Rational right) => !GreaterThan(right, left);
	public static bool operator <=(Rational left, Rational right) => !GreaterThan(left, right);
	public static Rational operator -(Rational value) => new(!value.numerator.Sign, value.numerator.Value, value.denominator);
	public static Rational operator +(Rational left, Rational right) => Add(left, right);
	public static Rational operator -(Rational left, Rational right) => Subtract(left, right);
	public static Rational operator *(Rational left, Rational right) => Multiply(left, right);
	public static Rational operator /(Rational left, Rational right) => Divide(left, right);
	public static Rational operator %(Rational left, Rational right) => new(GetValue(left / right, 0).Remainder);
	public static Rational operator ^(Rational left, Rational right) => Power(left, right);
	public static Rational operator ~(Rational value) => SquareRoot(value).Value;
	public static Rational operator |(Rational left, Rational right) => Root(right, left).Value;

	#endregion
}