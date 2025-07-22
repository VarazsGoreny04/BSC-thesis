using System;
using System.Collections.Immutable;

namespace Project_Real;

public readonly struct Writable
{
	#region Fields

	private readonly bool sign;
	private readonly Positive value;

	#endregion

	#region Properties

	public static bool WriteSign { get => Integer.WriteSign; set => Integer.WriteSign = value; }
	public static char Separator { get => Positive.Separator; set => Positive.Separator = value; }
	public static int FractionCalculatonLength { get => Positive.FractionCalculatonLength; set => Positive.FractionCalculatonLength = value; }

	public readonly bool IsZero => value.IsZero;
	public readonly int Length => value.Length;
	public readonly int WholeLength => value.WholeLength;
	public readonly int FractionLength => value.FractionLength;
	public readonly Digit this[Index i] => value.Digits[i];
	public readonly ImmutableArray<Digit> Digits => value.Digits;

	public readonly bool Sign => sign;
	public readonly Positive Value => value;

	#endregion

	#region Constructors

	public Writable()
	{
		sign = true;
		value = new Positive();
	}

	public Writable(string number)
	{
		if (number is null || number.Length < 1)
			throw new ArgumentException();

		int start = 0;

		if (!(number[0] >= '0' && number[0] <= '9'))
		{
			sign = number[0] switch
			{
				'+' => true,
				'-' => false,
				_ => throw new ArgumentException()
			};

			start = 1;
		}
		else
			sign = true;

		value = new Positive(number[start..]);

		sign |= IsZero;
	}

	public Writable(bool sign, Positive value)
	{
		this.value = value;
		this.sign = sign || IsZero;
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return $"{(WriteSign || !sign ? (sign ? '+' : '-') : "")}{value}";
	}

	public static bool Equals(Writable w1, Writable w2)
	{
		return w1.sign == w2.sign && w1.value == w2.value;
	}

	public static bool GreaterThan(Writable w1, Writable w2)
	{
		return w1.Sign != w2.Sign ? w1.Sign : (w1.Sign ? w1.Value > w2.Value : w1.Value < w2.Value);
	}

	public static Writable Add(Writable w1, Writable w2)
	{
		if (w1.sign == w2.sign)
			return new Writable(w1.sign, w1.value + w2.value);
		else
		{
			if (w2.sign)
				(w1, w2) = (w2, w1);

			(bool swap, Positive value) = Positive.Substract(w1.value, w2.value);

			return new Writable(!swap, value);
		}
	}

	public static Writable Substract(Writable w1, Writable w2)
	{
		return w1 + new Writable(!w2.sign, w2.value);
	}

	public static Writable Multiply(Writable w1, Writable w2)
	{
		return new Writable(w1.sign == w2.sign, w1.value * w2.value);
	}

	public static (Writable Value, Writable Remainder) Divide(Writable w1, Writable w2, int? fractionCalculatonLength = null)
	{
		(Positive whole, Positive remainder) = Positive.Divide(w1.value, w2.value, fractionCalculatonLength);

		return (new Writable(w1.sign == w2.sign, whole), new Writable(w1.sign, remainder));
	}

	public static Writable SecondPower(Writable w)
	{
		return w * w;
	}

	public static Writable Power(Writable w1, Writable w2)
	{
		if (!w2.sign)
			throw new NotImplementedException();

		return new(w1.sign || w2[0] % Digit.TWO == Digit.ZERO, w1.value ^ w2.value);
	}

	public static (Writable Value, Writable Remainder) SquareRoot(Writable w, int? fractionCalculatonLength = null)
	{
		if (!w.sign)
			throw new NotImplementedException();

		(Positive whole, Positive remainder) = Positive.SquareRoot(w.value, fractionCalculatonLength);

		return (new Writable(true, whole), new Writable(true, remainder));
	}

	public static (Writable Value, Writable Remainder) Root(Writable w1, Writable w2, int? fractionCalculatonLength = null)
	{
		if (!w2.sign || (!w1.sign && w2[0] % Digit.TWO == Digit.ZERO))
			throw new NotImplementedException();

		(Positive whole, Positive remainder) = Positive.Root(w1.Value, w2.Value, fractionCalculatonLength);

		return (new Writable(w1.Sign, whole), new Writable(w1.Sign, remainder));
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Writable writable && this == writable;
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
	public static Writable operator -(Writable f) => new(!f.Sign, f.Value);
	public static Writable operator +(Writable f1, Writable f2) => Add(f1, f2);
	public static Writable operator -(Writable f1, Writable f2) => Substract(f1, f2);
	public static Writable operator *(Writable f1, Writable f2) => Multiply(f1, f2);
	public static Writable operator /(Writable f1, Writable f2) => Divide(f1, f2).Value;
	public static Writable operator %(Writable f1, Writable f2) => Divide(f1, f2, 0).Remainder;
	public static Writable operator ^(Writable f1, Writable f2) => Power(f1, f2);
	public static Writable operator ~(Writable f) => SquareRoot(f).Value;
	public static Writable operator |(Writable f1, Writable f2) => Root(f2, f1).Value;

	#endregion
}