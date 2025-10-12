using System;

namespace Project_Real;

public readonly struct Rational
{
	#region Fields

	private static bool fractionFormat = true;

	private readonly Writable numerator;
	private readonly Positive? denominator;

	#endregion

	#region Properties

	public static bool FractionFormat { get => fractionFormat; set => fractionFormat = value; }
	public static bool WriteSign { get => Writable.WriteSign; set => Writable.WriteSign = value; }
	public static char Separator { get => Writable.Separator; set => Writable.Separator = value; }
	public static int FractionCalculatonLength { get => Writable.FractionCalculatonLength; set => Writable.FractionCalculatonLength = value; }

	public bool IsZero => numerator.IsZero;
	public bool Sign => numerator.Sign;
	public Positive Numerator => numerator.Value;

	public Positive? Denominator => denominator;

	#endregion

	#region Constructors

	public Rational()
	{
		numerator = new Writable();
		denominator = null;
	}

	public Rational(string number)
	{
		if (number is null)
			throw new ArgumentException();

		string[] parts = number.Split('/');

		if (parts.Length == 1)
			(numerator, denominator) = (new Writable(parts[0]), null);
		else if (parts.Length == 2)
		{
			(Writable numerator, Writable denominator) temp = (new(parts[0]), new(parts[1]));

			if (temp.denominator.IsZero)
				throw new ArgumentException();

			(numerator, denominator) = EuclideanAlgorithm(new Writable(temp.numerator.Sign == temp.denominator.Sign, 
				temp.numerator.Value), temp.denominator.Value);
		}
		else
			throw new ArgumentException();
	}

	public Rational(Writable numerator)
	{
		this.numerator = numerator;
		this.denominator = null;
	}

	public Rational(Writable numerator, Writable? denominator)
	{
		if (denominator is Writable w)
		{
			if (w.IsZero)
				throw new ArgumentException();

			(this.numerator, this.denominator) = EuclideanAlgorithm(new Writable(numerator.Sign == w.Sign, numerator.Value), w.Value);
		}
		else
		{
			this.numerator = numerator;
			this.denominator = null;
		}
	}

	public Rational(Writable numerator, Positive? denominator)
	{
		this.numerator = numerator;

		if (denominator is Positive p)
		{
			if (p.IsZero)
				throw new ArgumentException();

			(this.numerator, this.denominator) = EuclideanAlgorithm(numerator, p);
		}
		else
		{
			this.numerator = numerator;
			this.denominator = null;
		}
	}

	public Rational(bool sign, Positive numerator)
	{
		this.numerator = new Writable(sign, numerator);
		this.denominator = null;
	}

	public Rational(bool sign, Positive numerator, Positive? denominator)
	{
		if (denominator is Positive p)
		{
			if (p.IsZero)
				throw new ArgumentException();

			(this.numerator, this.denominator) = EuclideanAlgorithm(new Writable(sign, numerator), p);
		}
		else
		{
			this.numerator = new Writable(sign, numerator);
			this.denominator = null;
		}
	}

	#endregion

	#region Private methods

	private static (Writable Numerator1, Writable Numerator2, Positive? Denominator) CommonDenominator(Rational r1, Rational r2)
	{
		Writable r1Numerator = r2.denominator is null ? r1.numerator : new(r1.numerator.Sign, r1.numerator.Value * r2.denominator.Value);
		Writable r2Numerator = r1.denominator is null ? r2.numerator : new(r2.numerator.Sign, r2.numerator.Value * r1.denominator.Value);
		Positive? denominator = r1.Denominator is null ? r2.Denominator :
			(r2.Denominator is null ? r1.Denominator : r1.Denominator.Value * r2.Denominator.Value);

		return (r1Numerator, r2Numerator, denominator);
	}

	private static Rational GreatestCommonDivisor(Writable numerator, Positive? denominator)
	{
		(Writable n, Positive? d) = EuclideanAlgorithm(numerator, denominator);

		return new Rational(n, d);
	}

	private static (Writable Numerator, Positive? Denominator) EuclideanAlgorithm(Writable numerator, Positive? denominator)
	{
		if (denominator is Positive d)
		{
			Natural a = numerator.Value.Value;
			Natural b = d.Value;
			Natural temp;

			while (!b.IsZero)
			{
				temp = b;
				b = a % b;
				a = temp;
			}

			Positive resultDenominator = new(d.Value / a, Math.Max(d.FractionLength - numerator.FractionLength, 0));

			return (new Writable(numerator.Sign, new(numerator.Value.Value / a, Math.Max(numerator.FractionLength - d.FractionLength, 0))), 
				resultDenominator == "1" ? null as Positive? : resultDenominator);
		}
		else
			return (numerator, null);
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return fractionFormat ? ToFractionString(this) : ToWritableString(this);
	}

	public static string ToFractionString(Rational r)
	{
		return r.denominator is Positive denominator ? $"{r.numerator}/{denominator}" : r.numerator.ToString();
	}

	public static string ToWritableString(Rational r)
	{
		return GetValue(r).Value.ToString();
	}

	public static (Writable Value, Writable Remainder) GetValue(Rational r, int? fractionCalculatonLength = null)
	{
		if (r.denominator is Positive denominator)
			return Writable.Divide(r.numerator, new(true, denominator), fractionCalculatonLength);
		else
			return (r.numerator, new Writable());
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
		return r.IsZero ? throw new DivideByZeroException() : new Rational(r.Sign, (r.denominator is Positive p ? p : "1"), r.Numerator);
	}

	public static Rational Add(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, Positive? denominator) = CommonDenominator(r1, r2);

		return GreatestCommonDivisor(numerator1 + numerator2, denominator);
	}

	public static Rational Subtract(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, Positive? denominator) = CommonDenominator(r1, r2);

		return GreatestCommonDivisor(numerator1 - numerator2, denominator);
	}

	public static Rational Multiply(Rational r1, Rational r2)
	{
		Positive? denominator = r1.denominator is null ? r2.denominator :
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
		if (r2.denominator is not null)
			throw new NotImplementedException();

		if (r2.Sign)
			return GreatestCommonDivisor(r1.numerator ^ r2.numerator,
				(r1.denominator is Positive denominator ? denominator ^ r2.Numerator : r1.denominator));
		else
			return GreatestCommonDivisor(new(r1.Sign, r1.denominator is Positive denominator ? denominator ^ r2.Numerator : "1"),
				r1.Numerator ^ r2.Numerator);
	}

	public static Rational SquareRoot(Rational r, int? fractionCalculatonLength = null)
	{
		return new Rational(Writable.SquareRoot(r.numerator, fractionCalculatonLength).Value, r.denominator);
	}

	public static (Rational Value, Rational Remainder) Root(Rational r1, Rational r2, int? fractionCalculatonLength = null)
	{
		if (r2.denominator is not null)
			throw new NotImplementedException();

		(Writable Value, Writable Remainder) numerator = Writable.Root(r1.numerator, r2.numerator, fractionCalculatonLength);
		(Positive Value, Positive Remainder)? denominator = r1.denominator is Positive d ? Positive.Root(d, r2.Numerator) : null;

		if (r2.Sign)
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

		return new Rational(true, new(numerator * numerator, 0), new(denominator * denominator * n, 0));
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

		return "1" + new Rational(true, new(P("0", n), 0), new(Q("0", n), 0));
	}

	public static Rational E(int? fractionCalculatonLength = null)
	{
		int fCL = fractionCalculatonLength ?? FractionCalculatonLength;

		if (fCL < 0)
			throw new ArgumentOutOfRangeException();

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
	public static Rational operator -(Rational f1, Rational f2) => Subtract(f1, f2);
	public static Rational operator *(Rational f1, Rational f2) => Multiply(f1, f2);
	public static Rational operator /(Rational f1, Rational f2) => Divide(f1, f2);
	public static Rational operator %(Rational f1, Rational f2) => new(GetValue(f1 / f2, 0).Remainder);
	public static Rational operator ^(Rational f1, Rational f2) => Power(f1, f2);
	public static Rational operator ~(Rational f) => SquareRoot(f);
	public static Rational operator |(Rational f1, Rational f2) => Root(f2, f1).Value;

	#endregion
}