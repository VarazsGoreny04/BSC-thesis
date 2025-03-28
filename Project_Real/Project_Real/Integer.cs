using System;
using System.Collections.Immutable;

namespace Project_Real;

public readonly struct Integer
{
	#region Fields

	private static bool writeSign = true;

	public readonly bool Sign;
	public readonly Natural Value;

	#endregion

	#region Properties

	public static bool WriteSign { get => writeSign; set => writeSign = value; }

	public readonly bool IsZero => Value.IsZero;
	public readonly int Length => Value.Length;
	public readonly Digit this[Index i] => Value.Digits[i];
	public readonly ImmutableArray<Digit> Digits => Value.Digits;

	#endregion

	#region Constructors

	public Integer()
	{
		Sign = true;
		Value = new Natural();
	}

	public Integer(string number)
	{
		if (number is null || number.Length < 1)
			throw new ArgumentException();

		int start = 0;

		if (!(number[0] >= '0' && number[0] <= '9'))
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

		Value = new Natural(number[start..]);

		Sign |= IsZero;
	}

	public Integer(bool sign, Natural value)
	{
		Value = value;
		Sign = sign || IsZero;
	}
	
	#endregion

	#region Public methods

	public override string ToString()
	{
		return writeSign ? $"{(Sign ? '+' : '-')}{Value}" : Value.ToString(); 
	}

	public static bool Equals(Integer i1, Integer i2)
	{
		return i1.Sign == i2.Sign && i1.Value == i2.Value;
	}

	public static bool GreaterThan(Integer i1, Integer i2)
	{
		return i1.Sign != i2.Sign ? i1.Sign : i1.Sign == (i1.Value > i2.Value);
	}

	public static Integer Add(Integer i1, Integer i2)
	{
		if (i1.Sign == i2.Sign)
			return new Integer(i1.Sign, i1.Value + i2.Value);
		else
		{
			if (i2.Sign)
				(i1, i2) = (i2, i1);

			(bool swap, Natural value) = Natural.Substract(i1.Value, i2.Value);

			return new Integer(!swap, value);
		}
	}

	public static Integer Substract(Integer i1, Integer i2)
	{
		return i1 + new Integer(!i2.Sign, i2.Value);
	}

	public static Integer Multiply(Integer i1, Integer i2)
	{
		return new Integer(i1.Sign == i2.Sign, i1.Value * i2.Value);
	}

	public static (Integer Whole, Integer Remainder) Divide(Integer i1, Integer i2)
	{
		(Natural whole, Natural remainder) = Natural.Divide(i1.Value, i2.Value);
		return (new Integer(i1.Sign == i2.Sign, whole), new Integer(i1.Sign, remainder));
	}

	public static Integer SecondPower(Integer i)
	{
		return i * i;
	}

	public static Integer Power(Integer i1, Integer i2)
	{
		if (!i2.Sign)
			throw new NotImplementedException();

		return new Integer(i1.Sign || i1[0] % Digit.TWO == Digit.ZERO, i1.Value ^ i2.Value);
	}

	public static (Integer Whole, Integer Remainder) SquareRoot(Integer i)
	{
		if (!i.Sign)
			throw new NotImplementedException();

		(Natural whole, Natural remainder) = Natural.SquareRoot(i.Value);
		return (new Integer(true, whole), new Integer(true, remainder));
	}

	public static (Integer Whole, Integer Remainder) Root(Integer i1, Integer i2)
	{
		if (!i2.Sign || i1.Sign != (i2[0] % Digit.TWO == Digit.ZERO))
			throw new NotImplementedException();

		(Natural whole, Natural remainder) = Natural.Root(i1.Value, i2.Value);
		return (new Integer(i1.Sign, whole), new Integer(i1.Sign, remainder));
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Integer integer && this == integer;
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Integer(string num) => new(num);
	public static bool operator ==(Integer f1, Integer f2) => Equals(f1, f2);
	public static bool operator !=(Integer f1, Integer f2) => !Equals(f1, f2);
	public static bool operator >(Integer f1, Integer f2) => GreaterThan(f1, f2);
	public static bool operator <(Integer f1, Integer f2) => GreaterThan(f2, f1);
	public static bool operator >=(Integer f1, Integer f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Integer f1, Integer f2) => !GreaterThan(f1, f2);
	public static Integer operator +(Integer f1, Integer f2) => Add(f1, f2);
	public static Integer operator -(Integer f1, Integer f2) => Substract(f1, f2);
	public static Integer operator *(Integer f1, Integer f2) => Multiply(f1, f2);
	public static Integer operator /(Integer f1, Integer f2) => Divide(f1, f2).Whole;
	public static Integer operator %(Integer f1, Integer f2) => Divide(f1, f2).Remainder;
	public static Integer operator ^(Integer f1, Integer f2) => Power(f1, f2);
	public static Integer operator ~(Integer f) => SquareRoot(f).Whole;
	public static Integer operator |(Integer f1, Integer f2) => Root(f2, f1).Whole;

	#endregion
}