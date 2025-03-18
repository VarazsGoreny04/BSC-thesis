namespace Project_Real;

public readonly struct Racional
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

	public Racional()
	{
		Numerator = new Writable();
		Denominator = null;
	}

	public Racional(string number)
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
			_ => throw new FormatException()
		};
	}

	public Racional(Writable numerator, Writable denominator)
	{
		Numerator = numerator;
		Denominator = new Container(denominator);
	}

	private Racional(Writable numerator, Container? denominator)
	{
		Numerator = numerator;
		Denominator = denominator;
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return ToFractionString(this);
	}

	public static string ToFractionString(Racional r)
	{
		return (r.Denominator is null) ? r.Numerator.ToString() :
				$"{(r.Numerator.Sign == r.Denominator.Value.Sign ? '+' : '-')}{r.Numerator.Value}/{r.Denominator.Value.Value}";
	}

	public static string ToWritableString(Racional r)
	{
		return GetValue(r).ToString();
	}

	public static Writable GetValue(Racional r)
	{
		return (r.Denominator is null) ? r.Numerator : Writable.Divide(r.Numerator, r.Denominator.Value).Value;
	}

	public static bool Equals(Racional r1, Racional r2)
	{
		(Writable numerator1, Writable numerator2, _) = CommonDenominator(r1, r2);

		return Writable.Equals(numerator1, numerator2);
	}

	public static bool GreaterThan(Racional r1, Racional r2)
	{
		(Writable numerator1, Writable numerator2, _) = CommonDenominator(r1, r2);

		return Writable.GreaterThan(numerator1, numerator2);
	}

	private static (Writable Numerator1, Writable Numerator2, Container? Denominator) CommonDenominator(Racional r1, Racional r2)
	{
		Writable r1Numerator = r2.Denominator is null ? r1.Numerator : Writable.Multiply(r1.Numerator, r2.Denominator.Value);
		Writable r2Numerator = r1.Denominator is null ? r2.Numerator : Writable.Multiply(r2.Numerator, r1.Denominator.Value);
		Container? denominator = r1.Denominator is null ? r2.Denominator :
			(r2.Denominator is null ? r1.Denominator : new Container(Writable.Multiply(r2.Denominator.Value, r2.Denominator.Value)));

		return (r1Numerator, r2Numerator, denominator);
	}

	public static Racional Reciprocal(Racional r)
	{
		return r.Numerator.IsZero ? throw new ZeroReciprocalException() :
			new Racional((r.Denominator is null ? new Writable("1") : r.Denominator.Value), r.Numerator);
	}

	public static Racional Add(Racional r1, Racional r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Racional(Writable.Add(numerator1, numerator2), denominator);
	}

	public static Racional Substract(Racional r1, Racional r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Racional(Writable.Substract(numerator1, numerator2), denominator);
	}

	public static Racional Multiply(Racional r1, Racional r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Racional(Writable.Multiply(numerator1, numerator2), denominator);
	}

	public static Racional Divide(Racional r1, Racional r2)
	{
		return Multiply(r1, Reciprocal(r2));
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Racional real && Equals(this, real);
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Racional(string num) => new(num);
	public static bool operator ==(Racional f1, Racional f2) => Equals(f1, f2);
	public static bool operator !=(Racional f1, Racional f2) => !Equals(f1, f2);
	public static bool operator >(Racional f1, Racional f2) => GreaterThan(f1, f2);
	public static bool operator <(Racional f1, Racional f2) => GreaterThan(f2, f1);
	public static bool operator >=(Racional f1, Racional f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Racional f1, Racional f2) => !GreaterThan(f1, f2);
	public static Racional operator +(Racional f1, Racional f2) => Add(f1, f2);
	public static Racional operator -(Racional f1, Racional f2) => Substract(f1, f2);
	public static Racional operator *(Racional f1, Racional f2) => Multiply(f1, f2);
	public static Racional operator /(Racional f1, Racional f2) => Divide(f1, f2);
	//public static Real operator %(Real f1, Real f2) => Divide(f1, f2);
	//public static Real operator ^(Real f1, Real f2) => Power(f1, f2);

	#endregion
}