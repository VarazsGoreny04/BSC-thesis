namespace Project_Real;

public readonly struct Writable
{
	#region Fields

	public readonly bool Sign;
	public readonly Positive Value;

	#endregion

	#region Properties

	public readonly bool IsZero => Value.IsZero;
	public readonly int Length => Value.Length;
	public readonly int WholeLength => Value.WholeLength;
	public readonly int FractionLength => Value.FractionLength;
	public readonly Digit this[Index i] => Value.Digits[i];
	public readonly Digit[] Digits => Value.Digits;

	#endregion

	#region Constructors

	public Writable()
	{
		Sign = true;
		Value = new Positive();
	}

	public Writable(string number)
	{
		int start = 0;

		if (number.Length > 1 && !(number[0] >= '0' && number[0] <= '9'))
		{
			Sign = number[0] switch
			{
				'+' => true,
				'-' => false,
				_ => throw new ArgumentException()
			};

			start = 1;
		}
		else
			Sign = true;

		Value = new Positive(number[start..]);

		Sign |= IsZero;
	}

	public Writable(bool sign, Positive value)
	{
		Value = value;
		Sign = sign || IsZero;
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return Integer.WriteSign ? $"{(Sign ? '+' : '-')}{Value}" : Value.ToString();
	}

	public static bool Equals(Writable w1, Writable w2)
	{
		return w1.Sign == w2.Sign && Positive.Equals(w1.Value, w2.Value);
	}

	public static bool GreaterThan(Writable w1, Writable w2)
	{
		if (w1.Sign != w2.Sign)
			return w1.Sign;
		else
			return Positive.GreaterThan(w1.Value, w2.Value);
	}

	public static Writable Add(Writable w1, Writable w2)
	{
		if (w1.Sign && w2.Sign)
			return new Writable(w1.Sign, Positive.Add(w1.Value, w2.Value));
		else
		{
			if (w2.Sign)
				(w1, w2) = (w2, w1);

			(bool swap, Positive value) = Positive.Substract(w1.Value, w2.Value);

			return new Writable(!swap, value);
		}
	}

	public static Writable Substract(Writable w1, Writable w2)
	{
		return Add(w1, new Writable(!w2.Sign, w2.Value));
	}

	public static Writable Multiply(Writable w1, Writable w2)
	{
		return new Writable(w1.Sign == w2.Sign, Positive.Multiply(w1.Value, w2.Value));
	}

	public static (Writable Value, Writable Remainder) Divide(Writable w1, Writable w2)
	{
		(Positive whole, Positive remainder) = Positive.Divide(w1.Value, w2.Value);
		return (new Writable(w1.Sign == w2.Sign, whole), new Writable(w1.Sign, remainder));
	}

	public static Writable SecondPower(Writable w)
	{
		return w.IsZero ? throw new NotImplementedException() : Multiply(w, w);
	}

	public static Writable Power(Writable w1, Writable w2)
	{
		if (!w2.Sign)
			throw new NotImplementedException();

		return new(w1.Sign || Digit.Equals(Digit.Divide(w1[0], '2').Remainder, Digit.ZERO), Positive.Power(w1.Value, w2.Value));
	}

	public static Writable SquareRoot(Writable w)
	{
		if (!w.Sign)
			throw new NotImplementedException();

		return new Writable(true, Positive.SquareRoot(w.Value));
	}

	public static Writable Root(Writable w1, Writable w2)
	{
		if (w2.FractionLength != 0 || !w2.Sign || w1.Sign != Equals(Digit.Divide(w2[0], '2').Remainder, Digit.ZERO))
			throw new NotImplementedException();

		return new Writable(w1.Sign, Positive.Root(w1.Value, w2.Value));
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Writable writable && Equals(this, writable);
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Writable(string num) => new(num);
	public static bool operator ==(Writable f1, Writable f2) => Equals(f1, f2);
	public static bool operator !=(Writable f1, Writable f2) => !Equals(f1, f2);
	public static bool operator >(Writable f1, Writable f2) => GreaterThan(f1, f2);
	public static bool operator <(Writable f1, Writable f2) => GreaterThan(f2, f1);
	public static bool operator >=(Writable f1, Writable f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Writable f1, Writable f2) => !GreaterThan(f1, f2);
	public static Writable operator +(Writable f1, Writable f2) => Add(f1, f2);
	public static Writable operator -(Writable f1, Writable f2) => Substract(f1, f2);
	public static Writable operator *(Writable f1, Writable f2) => Multiply(f1, f2);
	public static Writable operator /(Writable f1, Writable f2) => Divide(f1, f2).Value;
	public static Writable operator %(Writable f1, Writable f2) => Divide(f1, f2).Remainder;
	public static Writable operator ^(Writable f1, Writable f2) => Power(f1, f2);
	public static Writable operator ~(Writable f) => SquareRoot(f);
	public static Writable operator |(Writable f1, Writable f2) => Root(f2, f1);

	#endregion
}