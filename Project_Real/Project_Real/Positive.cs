namespace Project_Real;

public readonly struct Positive
{
	#region Fields

	private static char separator = '.';
	private static int fractionCalculatonLength = 10;

	public readonly int Length;
	public readonly int WholeLength;
	public readonly int FractionLength;
	public readonly Natural Value;

	#endregion

	#region Properties

	public static char Separator { get => separator; set => separator = value; }
	public static int FractionCalculatonLength
	{
		get { return fractionCalculatonLength; }
		set { fractionCalculatonLength = (value >= 0) ? value : throw new ArgumentException(); }
	}

	public readonly bool IsZero => Value.IsZero;
	public readonly Digit this[Index i] => Value.Digits[i];
	public readonly Digit[] Digits => Value.Digits;

	#endregion

	#region Constructors

	public Positive()
	{
		Value = new Natural();
		Length = 1;
		WholeLength = 1;
		FractionLength = 0;
	}

	public Positive(string number)
	{
		if (number is null || number.Length < 1 || number[0] == separator)
			throw new ArgumentException();

		number = number.TrimStart('0');

		if (number.Length < 1 || number[0] == separator)
			number = '0' + number;

		FractionLength = number.IndexOf(separator);

		if (FractionLength != -1)
		{
			number = number.TrimEnd('0');
			number = number.Remove(FractionLength, 1);
		}

		Value = new Natural(number);
		Length = number.Length;
		FractionLength = FractionLength == -1 ? 0 : number.Length - FractionLength;
		WholeLength = Length - FractionLength;
	}

	public Positive(Natural value, int fractionLength) // Hibás
	{
		if (fractionLength < 0)
			throw new ArgumentException();
		else if (value.IsZero)
		{
			Value = value;
			Length = 1;
			WholeLength = 1;
			FractionLength = 0;
		}
		else
		{
			int i = 0;

			while (i < Math.Min(fractionLength, value.Length) && Digit.Equals(value[i], Digit.ZERO)) { ++i; }

			Value = new Natural(value.Digits[i..]);
			FractionLength = fractionLength - i;
			Length = Math.Max(Value.Length, FractionLength + 1);
			WholeLength = Length - FractionLength;
		}
	}

	#endregion

	#region Private methods

	public static Positive TrimStart(Positive p)
	{
		int i = 0;

		while (i < Math.Min(p.FractionLength, p.Digits.Length) && Digit.Equals(p[i], Digit.ZERO)) { ++i; }

		return i > 0 ? new Positive(new Natural(p.Digits[i..]), (p.FractionLength - i)) : p;
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return (FractionLength == 0) ? Value.ToString() :
				(
					(FractionLength < Digits.Length) ? Value.ToString() :
					new string('0', Length - Digits.Length) + Value.ToString()
				).Insert(WholeLength, separator.ToString());
	}

	public static Positive GetWhole(Positive p)
	{
		return new Positive(new Natural([.. p.Digits[(^p.WholeLength)..]]), 0);
	}

	public static Positive GetFraction(Positive p)
	{
		return new Positive(new([.. p.Digits[..Math.Min(p.FractionLength, p.Digits.Length)]]), p.FractionLength);
	}

	public static bool Equals(Positive p1, Positive p2)
	{
		return p1.Length == p2.Length && p1.FractionLength == p2.FractionLength && Natural.Equals(p1.Value, p2.Value);
	}

	public static bool GreaterThan(Positive p1, Positive p2)
	{
		if (p1.WholeLength != p2.WholeLength)
			return p1.Length > p2.Length;

		if (p1.FractionLength == p2.FractionLength)
			return Natural.GreaterThan(p1.Value, p2.Value);
		else
		{
			Digit[] splicing = Digit.CreateArray(Math.Max(p1.FractionLength, p2.FractionLength));

			if (p1.FractionLength < p2.FractionLength)
				return Natural.GreaterThan(new Natural([.. splicing, .. p1.Digits]), p2.Value);
			else
				return Natural.GreaterThan(p1.Value, new Natural([.. splicing, .. p2.Digits]));
		}
	}

	public static Positive Add(Positive p1, Positive p2)
	{
		int difference = p1.FractionLength - p2.FractionLength;

		if (difference < 0)
			(p1, p2) = (p2, p1);

		Digit[] splicing = Digit.CreateArray(Math.Abs(difference));

		Positive result = new(Natural.Add(p1.Value, new Natural([.. splicing, .. p2.Digits])), p1.FractionLength);

		return TrimStart(result);
	}

	public static (bool Swap, Positive Value) Substract(Positive p1, Positive p2)
	{
		int maxFractionLength = Math.Max(p1.FractionLength, p2.FractionLength);
		Digit[] splicing = Digit.CreateArray(Math.Abs(p1.FractionLength - p2.FractionLength));

		bool swap;
		Natural result;

		if (p1.FractionLength < p2.FractionLength)
			(swap, result) = Natural.Substract(new Natural([.. splicing, .. p1.Digits]), p2.Value);
		else
			(swap, result) = Natural.Substract(p1.Value, new Natural([.. splicing, .. p2.Digits]));

		return (swap, TrimStart(new Positive(result, maxFractionLength)));
	}

	public static Positive Multiply(Positive p1, Positive p2)
	{
		return new Positive(Natural.Multiply(p1.Value, p2.Value), p1.FractionLength + p2.FractionLength);
	}

	public static (Positive Value, Positive Remainder) Divide(Positive p1, Positive p2)
	{
		int denominatorSlicingLength = (p1.FractionLength > p2.FractionLength) ? p1.FractionLength - p2.FractionLength : 0;
		int numeratorSlicingLength = (p2.FractionLength > p1.FractionLength) ? p2.FractionLength - p1.FractionLength : 0;

		Natural denominator = new([.. Digit.CreateArray(denominatorSlicingLength), .. p2.Digits]);
		Natural numerator = new([.. Digit.CreateArray(numeratorSlicingLength + fractionCalculatonLength), .. p1.Digits]);

		(Natural whole, Natural remainder) = Natural.Divide(numerator, denominator);

		return (new Positive(whole, (whole.IsZero ? 0 : fractionCalculatonLength)),
				new Positive(remainder, (remainder.IsZero ? 0 : p2.FractionLength + denominatorSlicingLength)));
	}

	public static Positive SecondPower(Positive p)
	{
		return p.IsZero ? throw new NotImplementedException() : Multiply(p, p);
	}

	public static Positive Power(Positive i1, Positive i2)
	{
		if (i2.FractionLength != 0)
			throw new NotImplementedException();

		return new(Natural.Power(i1.Value, i2.Value), i1.FractionLength * Convert.ToInt32(i2.ToString()));
	}

	public static Positive SquareRoot(Positive value)
	{
		Digit[] padding = Digit.CreateArray(((fractionCalculatonLength * 2 - value.FractionLength) * 2 + 1) / 2);
		Natural valueWithPadding = new([.. padding, .. value.Digits]);
		return new Positive(Natural.SquareRoot(valueWithPadding).Whole, (value.FractionLength + padding.Length) / 2);
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Positive positive && Equals(this, positive);
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Positive(string num) => new(num);
	public static bool operator ==(Positive f1, Positive f2) => Equals(f1, f2);
	public static bool operator !=(Positive f1, Positive f2) => !Equals(f1, f2);
	public static bool operator >(Positive f1, Positive f2) => GreaterThan(f1, f2);
	public static bool operator <(Positive f1, Positive f2) => GreaterThan(f2, f1);
	public static bool operator >=(Positive f1, Positive f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Positive f1, Positive f2) => !GreaterThan(f1, f2);
	public static Positive operator +(Positive f1, Positive f2) => Add(f1, f2);
	public static Positive operator -(Positive f1, Positive f2) => Substract(f1, f2).Value;
	public static Positive operator *(Positive f1, Positive f2) => Multiply(f1, f2);
	public static Positive operator /(Positive f1, Positive f2) => Divide(f1, f2).Value;
	public static Positive operator %(Positive f1, Positive f2) => new(Natural.Divide(f1.Value, f2.Value).Remainder, 0);
	public static Positive operator ^(Positive f1, Positive f2) => Power(f1, f2);
	public static Positive operator ~(Positive f) => SquareRoot(f);

	#endregion
}