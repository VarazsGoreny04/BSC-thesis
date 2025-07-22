using System;

namespace Project_Real;

public readonly struct Rational
{
	#region Fields

	private readonly Writable numerator;
	private readonly Writable? denominator;

	#endregion

	#region Properties

	public static bool WriteSign { get => Writable.WriteSign; set => Writable.WriteSign = value; }
	public static char Separator { get => Writable.Separator; set => Writable.Separator = value; }
	public static int FractionCalculatonLength { get => Writable.FractionCalculatonLength; set => Writable.FractionCalculatonLength = value; }

	public Writable Numerator => numerator;
	public Writable? Denominator => denominator;

	#endregion

	#region Constructors

	public Rational()
	{
		numerator = new Writable();
		denominator = null;
	}

	public Rational(string number)
	{
		string[] parts;

		try
		{
			parts = number.Split('/');
		}
		catch (Exception)
		{
			throw new ArgumentException();
		}

		(numerator, denominator) = parts.Length switch
		{
			1 => (new Writable(parts[0]), null as Writable?),
			2 => (new Writable(parts[0]), new Writable(parts[1])),
			_ => throw new ArgumentException()
		};
	}

	public Rational(Writable numerator, Writable denominator)
	{
		this.numerator = numerator;
		this.denominator = denominator;
	}

	private Rational(Writable numerator, Writable? denominator = null)
	{
		this.numerator = numerator;
		this.denominator = denominator;
	}

	#endregion

	#region Private methods

	private static (Writable Numerator1, Writable Numerator2, Writable? Denominator) CommonDenominator(Rational r1, Rational r2)
	{
		Writable r1Numerator = r2.denominator is null ? r1.numerator : r1.numerator * r2.denominator.Value;
		Writable r2Numerator = r1.denominator is null ? r2.numerator : r2.numerator * r1.denominator.Value;
		Writable? denominator = r1.Denominator is null ? r2.Denominator :
			(r2.Denominator is null ? r1.Denominator : r1.Denominator.Value * r2.Denominator.Value);

		return (r1Numerator, r2Numerator, denominator);
	}

	private static Rational GreatestCommonDivisor(Writable numerator, Writable? denominator)
	{
		if (denominator is null)
			return new Rational(numerator, denominator);

		Natural a = numerator.Value.Value;
		Natural b = denominator.Value.Value.Value;
		Natural temp;

		while (!b.IsZero)
		{
			temp = b;
			b = a % b;
			a = temp;
		}

		return new Rational(new Writable(numerator.Sign, new(numerator.Value.Value / a, numerator.FractionLength)),
			new Writable(denominator.Value.Sign, new(denominator.Value.Value.Value / a, denominator.Value.FractionLength)));
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return ToFractionString(this);
	}

	public static string ToFractionString(Rational r)
	{
		if (r.denominator is Writable denominator)
		{
			bool sign = r.numerator.Sign == denominator.Sign;
			char character = sign ? '+' : '-';
			return $"{(!sign || WriteSign ? character : "")}{r.numerator.Value}/{denominator.Value}";
		}
		else
			return r.numerator.ToString();
	}

	public static string ToWritableString(Rational r)
	{
		return GetValue(r).Value.ToString();
	}

	public static (Writable Value, Writable Remainder) GetValue(Rational r, int? fractionCalculatonLength = null)
	{
		return (r.denominator is null) ? (r.numerator, new Writable()) : Writable.Divide(r.numerator, r.denominator.Value, fractionCalculatonLength);
	}

	public static bool Equals(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, _) = CommonDenominator(r1, r2);

		return numerator1 == numerator2;
	}

	public static bool GreaterThan(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, _) = CommonDenominator(r1, r2);

		return numerator1 > numerator2;
	}

	public static Rational Reciprocal(Rational r)
	{
		return r.numerator.IsZero ? throw new DivideByZeroException() :
			new Rational((r.denominator is null ? new Writable(true, new(new([Digit.ONE]), 0)) : r.denominator.Value), r.numerator);
	}

	public static Rational Add(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, Writable? denominator) = CommonDenominator(r1, r2);

		return GreatestCommonDivisor(numerator1 + numerator2, denominator);
	}

	public static Rational Substract(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, Writable? denominator) = CommonDenominator(r1, r2);

		return GreatestCommonDivisor(numerator1 - numerator2, denominator);
	}

	public static Rational Multiply(Rational r1, Rational r2)
	{
		Writable? denominator = r1.denominator is null ? r2.denominator :
			(r2.denominator is null ? r1.denominator : r1.denominator * r2.denominator);

		return GreatestCommonDivisor(r1.numerator * r2.numerator, denominator);
	}

	public static Rational Divide(Rational r1, Rational r2)
	{
		return r1 * Reciprocal(r2);
	}

	public static Rational SecondPower(Rational r)
	{
		return r * r;
	}

	public static Rational Power(Rational r1, Rational r2)
	{
		if (r2.denominator?.Value is not null)
			throw new Exception();

		if (r2.numerator.Sign)
			return GreatestCommonDivisor(r1.numerator ^ r2.numerator,
				(r1.denominator is null ? null as Writable? : r1.denominator.Value ^ r2.numerator));
		else
			return GreatestCommonDivisor((r1.denominator is null ? new Writable() : r1.denominator.Value ^ r2.numerator),
				r1.numerator ^ r2.numerator);
	}

	public static Rational SquareRoot(Rational r, int? fractionCalculatonLength = null)
	{
		return new Rational(Writable.SquareRoot(r.numerator, fractionCalculatonLength).Value, r.denominator);
	}

	public static (Rational Value, Rational Remainder) Root(Rational r1, Rational r2, int? fractionCalculatonLength = null)
	{
		if (r2.denominator is not null)
			throw new NotImplementedException();

		(Writable Value, Writable Remainder) numerator = Writable.Root(r1.numerator, new Writable(true, r2.numerator.Value), fractionCalculatonLength);
		(Writable Value, Writable Remainder)? denominator = r1.denominator is null ? null :
			Writable.Root(r1.denominator.Value, new Writable(true, r2.numerator.Value));

		if (r2.numerator.Sign)
			return (new Rational(numerator.Value, denominator?.Value), new Rational(numerator.Remainder, denominator?.Remainder));
		else
		{
			Writable one = new(true, new(new([Digit.ONE]), 0));

			return (new Rational((denominator?.Value ?? one), numerator.Value), new Rational((denominator?.Remainder ?? one), numerator.Remainder));
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

	public static Rational PI_Chudnovsky(int? fractionCalculatonLength = null) // Chudnovsky formula
	{
		int fCL = fractionCalculatonLength ?? FractionCalculatonLength;

		if (fCL < 0)
			throw new ArgumentOutOfRangeException();

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

			result += new Rational(new Writable(true, new(numerator, 0)), new Writable(denominator.Sign, new(denominator.Value, 0)));

			k += one;
		}

		result *= new Rational(new Writable(true, Positive.SquareRoot("10005", fCL).Value), "4270934400");

		return new Rational(result.denominator ?? new Writable(true, new(one, 0)), result.numerator);
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

			result += new Rational(new Writable(turn, new(new([Digit.FOUR]), 0)), new Writable(true, new(denominator, 0)));

			turn = !turn;

			//Console.WriteLine(Rational.ToWritableString(result));
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

		return new Rational(new(true, new(numerator * numerator, 0)), new(true, new((denominator * denominator) * n, 0)));
	}

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

		if (fCL < 0)
			throw new ArgumentOutOfRangeException();

		Natural n = Convert.ToString(fCL);

		return "1" + new Rational(new(true, new(P("0", n), 0)), new(true, new(Q("0", n), 0)));
	}

	public static Rational E(int? fractionCalculatonLength = null)
	{
		int fCL = fractionCalculatonLength ?? FractionCalculatonLength;

		if (fCL < 0)
			throw new ArgumentOutOfRangeException();

		Writable temp = "1";
		Writable one = "1";
		Rational result = "2";

		for (int i = fCL; i > 0; --i)
		{
			temp *= temp + one;

			result += new Rational(one, temp);
		}

		return result;
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Rational real && this == real;
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Rational(string num) => new(num);
	public static bool operator ==(Rational f1, Rational f2) => Equals(f1, f2);
	public static bool operator !=(Rational f1, Rational f2) => !Equals(f1, f2);
	public static bool operator >(Rational f1, Rational f2) => GreaterThan(f1, f2);
	public static bool operator <(Rational f1, Rational f2) => GreaterThan(f2, f1);
	public static bool operator >=(Rational f1, Rational f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Rational f1, Rational f2) => !GreaterThan(f1, f2);
	public static Rational operator -(Rational f) => new(new(!f.numerator.Sign, f.numerator.Value), f.denominator);
	public static Rational operator +(Rational f1, Rational f2) => Add(f1, f2);
	public static Rational operator -(Rational f1, Rational f2) => Substract(f1, f2);
	public static Rational operator *(Rational f1, Rational f2) => Multiply(f1, f2);
	public static Rational operator /(Rational f1, Rational f2) => Divide(f1, f2);
	public static Rational operator %(Rational f1, Rational f2) => new(GetValue(f1 / f2, 0).Remainder);
	public static Rational operator ^(Rational f1, Rational f2) => Power(f1, f2);
	public static Rational operator ~(Rational f) => SquareRoot(f);
	public static Rational operator |(Rational f1, Rational f2) => Root(f2, f1).Value;

	#endregion
}