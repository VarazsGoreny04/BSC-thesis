namespace Project_Real;

public readonly struct Real
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

	public Real()
	{
		Numerator = new Writable();
		Denominator = null;
	}

	public Real(string number)
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

	public Real(Writable numerator, Writable denominator)
	{
		Numerator = numerator;
		Denominator = new Container(denominator);
	}

	private Real(Writable numerator, Container? denominator)
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

	public static string ToFractionString(Real r)
	{
		return (r.Denominator is null) ? r.Numerator.ToString() :
				$"{(r.Numerator.Sign == r.Denominator.Value.Sign ? '+' : '-')}{r.Numerator.Value}/{r.Denominator.Value.Value}";
	}

	public static string ToWritableString(Real r)
	{
		return GetValue(r).ToString();
	}

	public static Writable GetValue(Real r)
	{
		return (r.Denominator is null) ? r.Numerator : Writable.Divide(r.Numerator, r.Denominator.Value).Value;
	}

	public static bool Equals(Real r1, Real r2)
	{
		(Writable numerator1, Writable numerator2, _) = CommonDenominator(r1, r2);

		return Writable.Equals(numerator1, numerator2);
	}

	public static bool GreaterThan(Real r1, Real r2)
	{
		(Writable numerator1, Writable numerator2, _) = CommonDenominator(r1, r2);

		return Writable.GreaterThan(numerator1, numerator2);
	}

	private static (Writable Numerator1, Writable Numerator2, Container? Denominator) CommonDenominator(Real r1, Real r2)
	{
		Writable r1Numerator = r2.Denominator is null ? r1.Numerator : Writable.Multiply(r1.Numerator, r2.Denominator.Value);
		Writable r2Numerator = r1.Denominator is null ? r2.Numerator : Writable.Multiply(r2.Numerator, r1.Denominator.Value);
		Container? denominator = r1.Denominator is null ?
			(r2.Denominator is null ? null : r2.Denominator) :
			(r2.Denominator is null ? r1.Denominator : new Container(Writable.Multiply(r2.Denominator.Value, r2.Denominator.Value)));

		return (r1Numerator, r2Numerator, denominator);
	}

	public static Real Reciprocal(Real r)
	{
		return r.Numerator.IsZero ? throw new ZeroReciprocalException() :
			new Real((r.Denominator is null ? new Writable("1") : r.Denominator.Value), r.Numerator);
	}

	public static Real Add(Real r1, Real r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Real(Writable.Add(numerator1, numerator2), denominator);
	}

	public static Real Substract(Real r1, Real r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Real(Writable.Substract(numerator1, numerator2), denominator);
	}

	public static Real Multiply(Real r1, Real r2)
	{
		(Writable numerator1, Writable numerator2, Container? denominator) = CommonDenominator(r1, r2);

		return new Real(Writable.Multiply(numerator1, numerator2), denominator);
	}

	public static Real Divide(Real r1, Real r2)
	{
		return Multiply(r1, Reciprocal(r2));
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Real real && Equals(this, real);
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Real(string num) => new(num);
	public static bool operator ==(Real f1, Real f2) => Equals(f1, f2);
	public static bool operator !=(Real f1, Real f2) => !Equals(f1, f2);
	public static bool operator >(Real f1, Real f2) => GreaterThan(f1, f2);
	public static bool operator <(Real f1, Real f2) => GreaterThan(f2, f1);
	public static bool operator >=(Real f1, Real f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Real f1, Real f2) => !GreaterThan(f1, f2);
	public static Real operator +(Real f1, Real f2) => Add(f1, f2);
	public static Real operator -(Real f1, Real f2) => Substract(f1, f2);
	public static Real operator *(Real f1, Real f2) => Multiply(f1, f2);
	public static Real operator /(Real f1, Real f2) => Divide(f1, f2);
	//public static Real operator %(Real f1, Real f2) => Divide(f1, f2);

	#endregion
}