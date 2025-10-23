using System;

namespace Project_Real;

public readonly struct Rational
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
	public static int FractionCalculatonLength
	{
		get => Writable.FractionCalculatonLength;
		set => Writable.FractionCalculatonLength = value;
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
				(numerator, denominator) = (new Writable(parts[0]), null);
				break;
			case 2:
				(Writable numerator, Writable denominator) temp = (new(parts[0]), new(parts[1]));

				if (temp.denominator.IsZero)
					throw new ArgumentException();

				(numerator, denominator) = Simplify(new Writable(temp.numerator.Sign == temp.denominator.Sign, temp.numerator.Value), temp.denominator.Value);
				break;
			default:
				throw new ArgumentException();
		}
	}

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <paramref name="numerator"/> with a denominator value of one.
	/// </summary>
	/// <param name="numerator">The value of the <paramref name="numerator"/>.</param>
	public Rational(Writable numerator)
	{
		this.numerator = numerator;
		this.denominator = null;
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
	public Rational(Writable numerator, Positive? denominator)
	{
		this.numerator = numerator;

		if (denominator is Positive denominatorValue)
		{
			if (denominatorValue.IsZero)
				throw new ArgumentException();

			(this.numerator, this.denominator) = Simplify(numerator, denominatorValue);
		}
		else
		{
			this.numerator = numerator;
			this.denominator = null;
		}
	}

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <paramref name="sign"/> and <paramref name="numerator"/> with a denominator value of one.
	/// </summary>
	/// <param name="sign">The <paramref name="sign"/> of the number.</param>
	/// <param name="numerator">The value of the <paramref name="numerator"/>.</param>
	public Rational(bool sign, Positive numerator)
	{
		this.numerator = new Writable(sign, numerator);
		this.denominator = null;
	}

	/// <summary>
	/// Constructs a <see cref="Rational"/> by the given <paramref name="sign"/>, <paramref name="numerator"/> and <paramref name="denominator"/>.
	/// </summary>
	/// <param name="sign">The <paramref name="sign"/> of the number.</param>
	/// <param name="numerator">The value of the <paramref name="numerator"/>.</param>
	/// <param name="denominator">The value of the <paramref name="denominator"/>.</param>
	/// <exception cref="ArgumentException"><paramref name="denominator"/> cannot be 0, as it is not mathematically meaningful.</exception>
	public Rational(bool sign, Positive numerator, Positive? denominator)
	{
		if (denominator is Positive denominatorValue)
		{
			if (denominatorValue.IsZero)
				throw new ArgumentException();

			(this.numerator, this.denominator) = Simplify(new Writable(sign, numerator), denominatorValue);
		}
		else
		{
			this.numerator = new Writable(sign, numerator);
			this.denominator = null;
		}
	}

	#endregion

	#region Private methods

	/// <summary>
	/// Finds the lowest common denominator for the two <see cref="Rational"/> numbers.
	/// </summary>
	/// <param name="first">The first <see cref="Rational"/>.</param>
	/// <param name="second">The second <see cref="Rational"/>.</param>
	/// <returns>The two numerator values and the common denominator in a tuple.</returns>
	private static (Writable Numerator1, Writable Numerator2, Positive? Denominator) CommonDenominator(Rational first, Rational second)
	{
		Writable numerator1 = second.denominator is null ? first.numerator : new(first.numerator.Sign, first.numerator.Value * second.denominator.Value);
		Writable numerator2 = first.denominator is null ? second.numerator : new(second.numerator.Sign, second.numerator.Value * first.denominator.Value);
		Positive? denominator = first.Denominator is null ? second.Denominator :
			(second.Denominator is null ? first.Denominator : first.Denominator.Value * second.Denominator.Value);

		return (numerator1, numerator2, denominator);
	}

	/// <summary>
	/// Gets the simplest form of the given <paramref name="numerator"/> and <paramref name="denominator"/>.
	/// </summary>
	/// <remarks>Note that every constructor of <see cref="Rational"/> already uses this method.</remarks>
	/// <param name="numerator">The numerator of the <see cref="Rational"/>.</param>
	/// <param name="denominator">The denominator of the <see cref="Rational"/>.</param>
	/// <returns>The simplified numerator and denominator value in a tuple.</returns>
	private static (Writable Numerator, Positive? Denominator) Simplify(Writable numerator, Positive? denominator)
	{
		if (denominator is Positive denominatorValue)
		{
			Natural gCD = Natural.GreatestCommonDivisor(numerator.Value.Value, denominatorValue.Value);

			Positive resultNumerator = new(numerator.Value.Value / gCD, Math.Max(numerator.FractionLength - denominatorValue.FractionLength, 0));
			Positive resultDenominator = new(denominatorValue.Value / gCD, Math.Max(denominatorValue.FractionLength - numerator.FractionLength, 0));

			return (new Writable(numerator.Sign, resultNumerator), (resultDenominator == "1" ? null as Positive? : resultDenominator));
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
	/// <param name="fractionCalculatonLength">A local variable to override <see cref="FractionCalculatonLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	public static (Writable Value, Writable Remainder) GetValue(Rational value, int? fractionCalculatonLength = null)
	{
		return (value.denominator is Positive denominator) ? Writable.Divide(value.numerator, new(true, denominator), fractionCalculatonLength) :
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
	/// <param name="fractionCalculatonLength">A local variable to override <see cref="FractionCalculatonLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException"><paramref name="value"/> being negative is not mathematically meaningful.</exception>
	public static Rational SquareRoot(Rational value, int? fractionCalculatonLength = null)
	{
		return new Rational(Writable.SquareRoot(value.numerator, fractionCalculatonLength).Value, value.denominator);
	}

	/// <summary>
	/// Raises the given radicand to the given degree.
	/// </summary>
	/// <param name="left">The <see cref="Rational"/> that represents the radicand.</param>
	/// <param name="right">The <see cref="Rational"/> that represents the degree.</param>
	/// <param name="fractionCalculatonLength">A local variable to override <see cref="FractionCalculatonLength"/> just for this method.</param>
	/// <returns>The whole value and the remainder in a tuple.</returns>
	/// <exception cref="NotImplementedException">
	/// <paramref name="right"/> being a fraction, negative or 0
	/// -or-
	/// <paramref name="left"/> being negative and <paramref name="right"/> being even is not mathematically meaningful.
	/// </exception>
	public static (Rational Value, Rational Remainder) Root(Rational left, Rational right, int? fractionCalculatonLength = null)
	{
		if (right.denominator is not null)
			throw new NotImplementedException();

		(Writable Value, Writable Remainder) numerator = Writable.Root(left.numerator, right.numerator, fractionCalculatonLength);
		(Positive Value, Positive Remainder)? denominator = left.denominator is Positive d ? Positive.Root(d, right.Numerator) : null;

		if (right.Sign)
			return (new Rational(numerator.Value, denominator?.Value), new Rational(numerator.Remainder, denominator?.Remainder));
		else
		{
			return (new Rational(true, denominator?.Value ?? "1", numerator.Value.Value),
				new Rational(true, denominator?.Remainder ?? "1", numerator.Remainder.Value));
		}
	}

	/*public static Rational PI_ChudnovskyBinary(int? fractionCalculatonLength = null) // Chudnovsky formula
	{
		int fCL = fractionCalculatonLength ?? FractionCalculatonLength;

		if (fCL < 0)
			throw new ArgumentOutOfRangeException();

		Natural COver24 = "10939058860032000";

		Natural one = new([Digit.ONE]);
		Natural two = new([Digit.TWO]);
		Natural five = new([Digit.FIVE]);
		Natural six = new([Digit.SIX]);

		Natural k = "1";
		Natural nFCL = Convert.ToString(fCL);
		Rational a_k = new(new Writable(true, new(nFCL, 0)));
		Rational a_sum = a_k;
		Rational b_sum = new();

		void bs(Natural a, Natural b)
		{
			if (b == a + "1")
			{
				// Directly compute P(a,a+1), Q(a,a+1) and T(a,a+1)
				if (a == 0)
					Pab = Qab = 1;
				else
				{
					Pab = (6 * a - 5) * (2 * a - 1) * (6 * a - 1);

					Qab = a * a * a * COver24;
				}
				Tab = Pab * (13591409 + 545140134 * a); // a(a) * p(a)
				if (a & 1)
					Tab = -Tab;
			}
			else:
			# Recursively compute P(a,b), Q(a,b) and T(a,b)
			# m is the midpoint of a and b
			m = (a + b) // 2
			# Recursively calculate P(a,m), Q(a,m) and T(a,m)
			Pam, Qam, Tam = bs(a, m)
			# Recursively calculate P(m,b), Q(m,b) and T(m,b)
			Pmb, Qmb, Tmb = bs(m, b)
			# Now combine
			Pab = Pam * Pmb;

			Qab = Qam * Qmb;

			Tab = Qmb * Tam + Pam * Tmb;

			return Pab, Qab, Tab;
		}

		for (int i = fCL / 100; i >= 0; --i)
		{
			a_k *= new Rational(new(false, new((six * k - five) * (two * k - one) * (six * k - one), 0)), new(true, new(k * k * k * COver24, 0)));
			a_sum += a_k;
			b_sum += new Rational(new Writable(true, new(k, 0))) * a_k;

			k += "1";
		}

		return new Rational(new Writable(true, new Positive(new Natural("426880"), 0) * Positive.SquareRoot("10005", fCL).Value)) /
			("13591409" * a_sum + "545140134" * b_sum);
	}*/

	/// <summary>
	/// Calculates π until the given <see cref="FractionCalculatonLength"/> using the Chudnovsky-formula.
	/// </summary>
	/// <remarks>
	/// I wanted to make a tribute to Srinivasa Ramanujan who came up with this method and to the Chudnovsky brothers
	/// - David and Gregory Chudnovsky - for developing a generalisation to Ramanujan's formula.
	/// <para><see href="https://en.wikipedia.org/wiki/Chudnovsky_algorithm"/></para>
	/// </remarks>
	/// <param name="fractionCalculatonLength">A local variable to override <see cref="FractionCalculatonLength"/> just for this method.</param>
	/// <returns></returns>
	public static Rational PI_Chudnovsky(int? fractionCalculatonLength = null) // Chudnovsky formula
	{
		int fCL = fractionCalculatonLength ?? FractionCalculatonLength;

		Natural first = "13591409";
		Natural second = "545140134";
		Integer third = "-640320";

		Natural one = new([Digit.ONE]);
		Natural three = new([Digit.THREE]);
		Natural six = new([Digit.SIX]);

		Natural k = one;
		Natural numerator;
		Integer denominator;
		Rational result = new(new Writable(true, new(first, 0)));

		for (int i = fCL / 13; i >= 0; --i)
		{
			numerator = Natural.Factorial(k * six) * (first + (second * k));
			Natural temp = k * three;
			denominator = new Integer(true, Natural.Factorial(temp) * (Natural.Factorial(k) ^ three)) * (third ^ new Integer(true, temp));

			result += new Rational(new Writable(true, new(numerator, 0)), new Positive(denominator.Value, 0));

			k += one;
		}

		result *= new Rational(true, Positive.SquareRoot("10005", fCL).Value, "4270934400");

		return new Rational(true, result.denominator ?? new(one, 0), result.Numerator);
	}

	public static Rational PI(int? fractionCalculatonLength = null)
	{
		bool turn = true;
		Natural one = new([Digit.ONE]);
		Natural n = new([Digit.TWO]);
		Natural denominator;
		Rational result = new(new Writable(true, new(new([Digit.THREE]), 0)));

		for (int i = fractionCalculatonLength ?? FractionCalculatonLength; i > 0; --i)
		{
			denominator = n * (n += one) * (n += one);

			result += new Rational(new Writable(turn, new(new([Digit.FOUR]), 0)), new Positive(denominator, 0));

			turn = !turn;
		}

		return result;
	}

	public static Rational PI_Wallis(int? fractionCalculatonLength = null)
	{
		int fCL = fractionCalculatonLength ?? FractionCalculatonLength;

		if (fCL < 0)
			throw new ArgumentOutOfRangeException();

		Natural n = new(fCL.ToString());

		Natural two = new([Digit.TWO]);
		Natural numerator = Natural.Factorial(n) * Natural.Power(new Natural([Digit.TWO]), n);
		Natural denominator = new([Digit.ONE]);
		Natural counter = new([Digit.THREE]);

		for (int i = fCL - 1; i > 0; --i)
		{
			denominator *= counter;

			counter += two;
		}

		return new Rational(true, new(numerator * numerator, 0), new(denominator * denominator * n, 0));
	}

	/// <summary>
	/// Calculates the number e until the given <see cref="FractionCalculatonLength"/>.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/E_(mathematical_constant)#Computing_the_digits"/></remarks>
	/// <param name="fractionCalculatonLength">A local variable to override <see cref="FractionCalculatonLength"/> just for this method.</param>
	/// <returns></returns>
	public static Rational EBinary(int? fractionCalculatonLength = null)
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

		int fCL = fractionCalculatonLength ?? FractionCalculatonLength;

		Natural n = Convert.ToString(fCL);

		return "1" + new Rational(true, new(P("0", n), 0), new(Q("0", n), 0));
	}

	public static Rational E(int? fractionCalculatonLength = null)
	{
		int fCL = fractionCalculatonLength ?? FractionCalculatonLength;

		Positive temp = "1";
		Positive one = "1";
		Rational result = "2";

		for (int i = fCL; i > 0; --i)
		{
			temp *= temp + one;

			result += new Rational(true, one, temp);
		}

		return result;
	}
	
	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Rational"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override readonly bool Equals(object? obj) => obj is Rational real && this == real;
	
	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Rational(string value) => new(value);
	public static bool operator ==(Rational left, Rational right) => Equals(left, right);
	public static bool operator !=(Rational left, Rational right) => !Equals(left, right);
	public static bool operator >(Rational left, Rational right) => GreaterThan(left, right);
	public static bool operator <(Rational left, Rational right) => GreaterThan(right, left);
	public static bool operator >=(Rational left, Rational right) => !GreaterThan(right, left);
	public static bool operator <=(Rational left, Rational right) => !GreaterThan(left, right);
	public static Rational operator -(Rational value) => new(new Writable(!value.numerator.Sign, value.numerator.Value), value.denominator);
	public static Rational operator +(Rational left, Rational right) => Add(left, right);
	public static Rational operator -(Rational left, Rational right) => Subtract(left, right);
	public static Rational operator *(Rational left, Rational right) => Multiply(left, right);
	public static Rational operator /(Rational left, Rational right) => Divide(left, right);
	public static Rational operator %(Rational left, Rational right) => new(GetValue(left / right, 0).Remainder);
	public static Rational operator ^(Rational left, Rational right) => Power(left, right);
	public static Rational operator ~(Rational value) => SquareRoot(value);
	public static Rational operator |(Rational left, Rational right) => Root(right, left).Value;

	#endregion
}