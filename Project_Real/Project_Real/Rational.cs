using System;

namespace Project_Real;

public readonly struct Rational
{
	public class Container(Writable value)
	{
		public Writable Value = value;
	}

	#region Exceptions

	public class ZeroReciprocalException() : Exception();

	#endregion

	#region Fields

	public readonly Writable Numerator;
	public readonly Container? Denominator;

	#endregion

	#region Constructors

	public Rational()
	{
		Numerator = new Writable();
		Denominator = null;
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

		(Numerator, Denominator) = parts.Length switch
		{
			1 => (new Writable(parts[0]), null),
			2 => (new Writable(parts[0]), new Container(new Writable(parts[1]))),
			_ => throw new ArgumentException()
		};
	}

	public Rational(Writable numerator, Writable denominator)
	{
		Numerator = numerator;
		Denominator = new Container(denominator);
	}

	private Rational(Writable numerator, Container? denominator)
	{
		Numerator = numerator;
		Denominator = denominator;
	}

	#endregion

	#region Private methods

	private static (Writable Numerator1, Writable Numerator2, Container? Denominator) CommonDenominator(Rational r1, Rational r2)
	{
		Writable r1Numerator = r2.Denominator is null ? r1.Numerator : r1.Numerator * r2.Denominator.Value;
		Writable r2Numerator = r1.Denominator is null ? r2.Numerator : r2.Numerator * r1.Denominator.Value;
		Container? denominator = r1.Denominator is null ? r2.Denominator :
			(r2.Denominator is null ? r1.Denominator : new Container(r2.Denominator.Value * r2.Denominator.Value));

		return (r1Numerator, r2Numerator, denominator);
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return ToFractionString(this);
	}

	public static string ToFractionString(Rational r)
	{
		return (r.Denominator is null) ? r.Numerator.ToString() :
				$"{(r.Numerator.Sign == r.Denominator.Value.Sign ? '+' : '-')}{r.Numerator.Value}/{r.Denominator.Value.Value}";
	}

	public static string ToWritableString(Rational r)
	{
		return GetValue(r).ToString();
	}

	public static Writable GetValue(Rational r)
	{
		return (r.Denominator is null) ? r.Numerator : r.Numerator / r.Denominator.Value;
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
		return r.Numerator.IsZero ? throw new ZeroReciprocalException() :
			new Rational((r.Denominator is null ? new Writable(true, new Positive(new Natural([Digit.ONE]), 0)) : r.Denominator.Value), r.Numerator);
	}

	public static Rational Add(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Rational(numerator1 + numerator2, denominator);
	}

	public static Rational Substract(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Rational(numerator1 - numerator2, denominator);
	}

	public static Rational Multiply(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Rational(numerator1 * numerator2, denominator);
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
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Rational(numerator1 ^ numerator2, denominator);
	}

	public static Rational SquareRoot(Rational r)
	{
		return new Rational(~r.Numerator, r.Denominator);
	}

	public static Rational Root(Rational r1, Rational r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Rational(numerator2 | numerator1, denominator);
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
	public static Rational operator +(Rational f1, Rational f2) => Add(f1, f2);
	public static Rational operator -(Rational f1, Rational f2) => Substract(f1, f2);
	public static Rational operator *(Rational f1, Rational f2) => Multiply(f1, f2);
	public static Rational operator /(Rational f1, Rational f2) => Divide(f1, f2);
	//public static Rational operator %(Rational f1, Rational f2) => Divide(f1, f2).Remainder;
	public static Rational operator ^(Rational f1, Rational f2) => Power(f1, f2);
	public static Rational operator ~(Rational f) => SquareRoot(f);
	public static Rational operator |(Rational f1, Rational f2) => Root(f2, f1);

	#endregion
}