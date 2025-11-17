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

	/// <returns>The character the <see cref="ToString"/> method uses as separator</returns>
	public static char Separator => Writable.Separator;

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

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <see cref="Digit"/>.
	/// </summary>
	public Rational(Digit value) : this(true, value) { }

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

	private class ABPQSeriesResult(Writable P, Writable Q, Writable B, Writable T)
	{
		public Writable P = P, Q = Q, B = B, T = T;
	}

	private static int IterationsNeededSinCos(Rational x, int fractionCalculationLength)
	{
		int divisor;

		if (x < "0.7")
			divisor = 6;
		else if (x < "1.5")
			divisor = 4;
		else if (x < "3.1")
			divisor = 3;
		else
			divisor = 2;

		return ((fractionCalculationLength - 15) / divisor) + 22;
	}

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

			return (new Writable(numerator.Sign, resultNumerator), (resultDenominator == Digit.ONE ? null : resultDenominator));
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
		return (value.denominator is Positive denominator) ? Writable.Divide(value.numerator, denominator, fractionCalculationLength) :
			(value.numerator, new Writable());
	}

	/// <summary>
	/// Rounds down the given <see cref="Rational"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> instance.</param>
	/// <returns>The whole part of the given <see cref="Rational"/> instance.</returns>
	public static Integer RoundDown(Rational value) => new(value.Sign, Positive.RoundDown(GetValue(value).Value.Value));

	/// <summary>
	/// Rounds up the given <see cref="Rational"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> instance.</param>
	/// <returns>The whole part of the given <see cref="Rational"/> instance plus one if it was not whole.</returns>
	public static Integer RoundUp(Rational value) => new(value.Sign, Positive.RoundUp(GetValue(value).Value.Value));

	/// <summary>
	/// Rounds the given <see cref="Rational"/> instance to the nearest number.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> instance.</param>
	/// <returns>The rounded value of the given <see cref="Rational"/> instance.</returns>
	public static Integer Round(Rational value) => new(value.Sign, Positive.Round(GetValue(value).Value.Value));

	/// <summary>
	/// Gets the absolut value of the given <see cref="Rational"/>.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/>.</param>
	/// <returns>The absolut value of the given <see cref="Rational"/>.</returns>
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
		return value.IsZero ? throw new DivideByZeroException() : new Rational(value.Sign, (value.denominator is Positive p ? p : Digit.ONE), value.Numerator);
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
	/// <returns>The result of the calculation.</returns>
	/// <exception cref="NotImplementedException"><paramref name="right"/> cannot be a fraction.</exception>
	public static Rational Power(Rational left, Rational right)
	{
		if (right.denominator is not null)
			throw new NotImplementedException();

		if (right.Sign)
			return new Rational(left.numerator ^ right.numerator, (left.denominator is Positive denominator ? denominator ^ right.Numerator : left.denominator));
		else
			return new Rational(new Writable(left.Sign, (left.denominator is Positive denominator ? denominator ^ right.Numerator : Digit.ONE)),
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
			return (new Rational(true, denominator?.Value ?? Digit.ONE, numerator.Value.Value),
				new Rational(true, denominator?.Remainder ?? Digit.ONE, numerator.Remainder.Value));
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
	/// <returns>The number π.</returns>
	public static Rational Pi(int? fractionCalculationLength = null)
	{
		static (Natural P, Natural Q, Integer T) BinarySplitting(Natural a, Natural b)
		{
			Natural Pab, Qab;
			Integer Tab;

			if (b == a + Digit.ONE)
			{
				// Directly compute P(a,a+1), Q(a,a+1) and T(a,a+1)
				if (a.IsZero)
				{
					Pab = Digit.ONE;
					Qab = Digit.ONE;
				}
				else
				{
					Pab = (Digit.SIX * a - Digit.FIVE) * (Digit.TWO * a - Digit.ONE) * (Digit.SIX * a - Digit.ONE);
					Qab = a * a * a * "10939058860032000";
				}

				Tab = Pab * ("13591409" + "545140134" * a); // a(a) * p(a)

				if (a[0] % Digit.TWO == Digit.ONE)
					Tab = -Tab;
			}
			else
			{
				// Recursively compute P(a,b), Q(a,b) and T(a,b)
				// m is the midpoint of a and b
				Natural m = (a + b) / Digit.TWO;

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
		Natural n = (new Natural((uint)fCL)) / DIGITS_PER_TERM + Digit.ONE;

		// Calculate P(0,N) and Q(0,N)
		(Natural _, Natural Q, Integer T) = BinarySplitting(Digit.ZERO, n);

		return new Rational(T.Sign, Q * Positive.SquareRoot("10005", fCL).Value * "426880", T.Value);
	}

	/// <summary>
	/// Calculates the number e until the given <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/E_(mathematical_constant)#Computing_the_digits"/></remarks>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The number e.</returns>
	public static Rational E(int? fractionCalculationLength = null)
	{
		static Natural P(Natural a, Natural b)
		{
			if (b == a + Digit.ONE)
				return Digit.ONE;
			else
			{
				Natural m = (a + b) / Digit.TWO;
				return P(a, m) * Q(m, b) + P(m, b);
			}
		}

		static Natural Q(Natural a, Natural b)
		{
			if (b == a + Digit.ONE)
				return b;
			else
			{
				Natural m = (a + b) / Digit.TWO;
				return Q(a, m) * Q(m, b);
			}
		}

		Natural n = new((uint)Math.Max(fractionCalculationLength ?? FractionCalculationLength, 0));

		return Digit.ONE + new Rational(true, P(Digit.ZERO, n), Q(Digit.ZERO, n));
	}

	/// <summary>
	/// Calculates the exponential function for the given exponent until the given <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks>
	/// <see href="https://ginac.de/CLN/binsplit.pdf"/>
	/// <br />
	/// <see href="https://stackoverflow.com/questions/57510825/binary-splitting-in-pari-gp"/>
	/// </remarks>
	/// <param name="x">The exponent in e^<paramref name="x"/>.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The the exponential function for the given exponent.</returns>
	public static Rational Exp(Rational x, int? fractionCalculationLength = null)
	{
		static ABPQSeriesResult SumABPQ(int n1, int n2, Rational x)
		{
			ABPQSeriesResult r;

			switch (n2 - n1)
			{
				case 0:
					throw new Exception();
				case 1:
					r = n1 == 0 ?
					new(
						P: Digit.ONE,
						Q: Digit.ONE,
						B: null!,
						T: Digit.ONE
					) :
					new(
						P: x.numerator,
						Q: new Natural((uint)n1) * (x.denominator ?? Digit.ONE),
						B: null!,
						T: x.numerator
					);
					break;
				default:
					int nm = (n1 + n2) / 2;

					ABPQSeriesResult L = SumABPQ(n1, nm, x);
					ABPQSeriesResult R = SumABPQ(nm, n2, x);

					r = new(
						P: L.P * R.P,
						Q: L.Q * R.Q,
						B: null!,
						T: R.Q * L.T + L.P * R.T
					);
					break;
			}

			return r;
		}

		int n = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 0);

		ABPQSeriesResult r = SumABPQ(0, n + 1, x);

		return new Rational(r.T, r.Q);
	}

	/// <summary>
	/// Calculates the natural logarithm for the given anti-logarithm until the given <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks><see href="https://ginac.de/CLN/binsplit.pdf"/></remarks>
	/// <param name="x">The anti-logarithm in ln(<paramref name="x"/>).</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The natural logarithm of the given parameter.</returns>
	public static Rational Ln(Rational x, int? fractionCalculationLength = null)
	{
		static ABPQSeriesResult SumABPQ(int n1, int n2, Rational x)
		{
			ABPQSeriesResult r;

			switch (n2 - n1)
			{
				case 0:
					throw new Exception();
				case 1:
					Natural denominator = new(2 * (uint)n1 + 1);
					Rational a = Power(x, denominator);

					r = new(
						P: null!,
						Q: a.denominator ?? Digit.ONE,
						B: denominator,
						T: a.numerator
					);
					break;
				default:
					int nm = (n1 + n2) / 2;

					ABPQSeriesResult L = SumABPQ(n1, nm, x);
					ABPQSeriesResult R = SumABPQ(nm, n2, x);

					r = new(
						P: null!,
						Q: L.Q * R.Q,
						B: L.B * R.B,
						T: R.B * R.Q * L.T + L.B * L.Q * R.T
					);
					break;
			}

			return r;
		}

		static int IterationsNeeded(Rational x, int fractionCalculationLength)
		{
			int result;

			if (x < "1.1")
				result = fractionCalculationLength * 3 / 8;
			else if (x < "1.5")
				result = fractionCalculationLength * 13 / 18;
			else if (x >= "1.5")
				result = fractionCalculationLength * 11 / 10;
			else
				throw new Exception(); // TODO

			return result + 1;
		}

		int fCL = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 0);

		int n = IterationsNeeded(x, fCL);

		Rational y = (x - Digit.ONE) / (x + Digit.ONE);

		ABPQSeriesResult r = SumABPQ(0, n, y);

		return new Rational(r.T * Digit.TWO, r.B * r.Q);
	}

	public static Rational LnFast(Rational x, int? fractionCalculationLength = null)
	{
		Natural n = Digit.ZERO;
		Natural twoToTheNth = Digit.ONE;

		while ((twoToTheNth *= Digit.TWO) <= x)
			n += Digit.ONE;

		twoToTheNth /= Digit.TWO;

		int fCL = Math.Max(fractionCalculationLength ?? FractionCalculationLength, 0);

		Natural r2 = RoundUp(SecondPower((x - "1") / (x + "1"))).Value; // r^2 where r is (x−1)/(x+1)
		Natural log10r2 = Natural.Log(r2, "10"); // log10(r^2)
		int ln2n = fCL / (int)Natural.ToUInt32(log10r2.IsZero ? Digit.ONE : log10r2); // p / log10(r^2) where p is the precision (10^-p)

		/*{
			Rational ln2 = Ln(Digit.TWO, ln2n);
			Rational lnLt2 = Ln(x / twoToTheNth, IterationsNeededSinCos("1", fCL));

			Console.WriteLine($"{fractionCalculationLength} -");
			Console.WriteLine(GetValue(ln2).Value);
			Console.WriteLine(GetValue(lnLt2).Value);

			return lnLt2 + n * ln2;
		}*/

		return Ln(x / twoToTheNth, ln2n) + n * Ln(Digit.TWO, ln2n);
	}

	/// <summary>
	/// Calculates the sinus function for the given <paramref name="x"/> value until the given <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks>
	/// <see href="https://ginac.de/CLN/binsplit.pdf"/>
	/// </remarks>
	/// <param name="x">The exponent in e^<paramref name="x"/>.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The the exponential function for the given exponent.</returns>
	public static Rational Sin(Rational x, int? fractionCalculationLength = null)
	{
		static ABPQSeriesResult SumABPQ(int n1, int n2, Rational x)
		{
			ABPQSeriesResult r;

			switch (n2 - n1)
			{
				case 0:
					throw new Exception();
				case 1:
					Writable tempP;

					r = n1 == 0 ?
					new(
						P: x.numerator,
						Q: x.denominator ?? Digit.ONE,
						B: null!,
						T: x.numerator
					) :
					new(
						P: tempP = -Writable.SecondPower(x.numerator),
						Q: new Natural((uint)((2 * n1) * (2 * n1 + 1))) * Writable.SecondPower(x.denominator ?? Digit.ONE),
						B: null!,
						T: tempP
					);
					break;
				default:
					int nm = (n1 + n2) / 2;

					ABPQSeriesResult L = SumABPQ(n1, nm, x);
					ABPQSeriesResult R = SumABPQ(nm, n2, x);

					r = new(
						P: L.P * R.P,
						Q: L.Q * R.Q,
						B: null!,
						T: R.Q * L.T + L.P * R.T
					);
					break;
			}

			return r;
		}

		int fCL = fractionCalculationLength ?? FractionCalculationLength;

		if (Abs(x) > Digit.SIX)
			x %= Digit.TWO * Pi(fCL);

		int n = IterationsNeededSinCos(x, fCL);

		ABPQSeriesResult r = SumABPQ(0, n, x);

		return new Rational(r.T, r.Q);
	}

	/// <summary>
	/// Calculates the sinus function for the given <paramref name="x"/> value until the given <paramref name="fractionCalculationLength"/> using binary splitting.
	/// </summary>
	/// <remarks>
	/// <see href="https://ginac.de/CLN/binsplit.pdf"/>
	/// </remarks>
	/// <param name="x">The exponent in e^<paramref name="x"/>.</param>
	/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
	/// <returns>The the exponential function for the given exponent.</returns>
	public static Rational Cos(Rational x, int? fractionCalculationLength = null)
	{
		static ABPQSeriesResult SumABPQ(int n1, int n2, Rational x)
		{
			ABPQSeriesResult r;

			switch (n2 - n1)
			{
				case 0:
					throw new Exception();
				case 1:
					Writable tempP;

					r = n1 == 0 ?
					new(
						P: Digit.ONE,
						Q: Digit.ONE,
						B: null!,
						T: Digit.ONE
					) :
					new(
						P: tempP = -Writable.SecondPower(x.numerator),
						Q: new Natural((uint)((2 * n1) * (2 * n1 - 1))) * Writable.SecondPower(x.denominator ?? Digit.ONE),
						B: null!,
						T: tempP
					);
					break;
				default:
					int nm = (n1 + n2) / 2;

					ABPQSeriesResult L = SumABPQ(n1, nm, x);
					ABPQSeriesResult R = SumABPQ(nm, n2, x);

					r = new(
						P: L.P * R.P,
						Q: L.Q * R.Q,
						B: null!,
						T: R.Q * L.T + L.P * R.T
					);
					break;
			}

			return r;
		}

		int fCL = fractionCalculationLength ?? FractionCalculationLength;

		if (Abs(x) > Digit.SIX)
			x %= Digit.TWO * Pi(fCL);

		int n = IterationsNeededSinCos(x, fCL);

		ABPQSeriesResult r = SumABPQ(0, n, x);

		return new Rational(r.T, r.Q);
	}

	/*
		/// <summary>
		/// Calculates the sinus function for the given <paramref name="x"/> value until the given <paramref name="fractionCalculationLength"/> using binary splitting.
		/// </summary>
		/// <remarks>
		/// <see href="https://ginac.de/CLN/binsplit.pdf"/>
		/// </remarks>
		/// <param name="x">The exponent in e^<paramref name="x"/>.</param>
		/// <param name="fractionCalculationLength">A local variable to override <see cref="FractionCalculationLength"/> just for this method.</param>
		/// <returns>The the exponential function for the given exponent.</returns>
		public static Rational Fact(Rational x, int? fractionCalculationLength = null)
		{

		}
	*/

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

	public static implicit operator Rational(char value) => new(value.ToString());
	public static implicit operator Rational(string value) => new(value);
	public static implicit operator Rational(Digit value) => new(value);
	public static implicit operator Rational(Natural value) => new(value);
	public static implicit operator Rational(Integer value) => new(value);
	public static implicit operator Rational(Positive value) => new(value);
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
	public static Rational operator %(Rational left, Rational right) => Modulo(left, right);
	public static Rational operator ^(Rational left, Rational right) => Power(left, right);
	public static Rational operator ~(Rational value) => SquareRoot(value).Value;
	public static Rational operator |(Rational left, Rational right) => Root(right, left).Value;

	#endregion
}