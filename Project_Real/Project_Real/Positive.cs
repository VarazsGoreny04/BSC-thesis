using System;
using System.Collections.Immutable;

namespace Project_Real;

public readonly struct Positive
{
	#region Fields

	private static char separator = '.';
	private static int fractionCalculatonLength = 10;

	private readonly int length;
	private readonly int wholeLength;
	private readonly int fractionLength;
	private readonly Natural value;

	#endregion

	#region Properties

	public static char Separator
	{
		get => separator;
		set
		{
			separator = value < '0' || value > '9' ? value : throw new Exception();
		}
	}
	public static int FractionCalculatonLength
	{
		get => fractionCalculatonLength;
		set => fractionCalculatonLength = (value >= 0) ? value : throw new ArgumentException();
	}

	public readonly int Length => length;
	public readonly int WholeLength => wholeLength;
	public readonly int FractionLength => fractionLength;
	public readonly bool IsZero => value.IsZero;
	public readonly Digit this[Index i] => value.Digits[i];
	public readonly ImmutableArray<Digit> Digits => value.Digits;

	#endregion

	#region Constructors

	public Positive()
	{
		value = new Natural();
		length = 1;
		wholeLength = 1;
		fractionLength = 0;
	}

	public Positive(string number)
	{
		if (number is null || number.Length < 1 || number[0] == separator)
			throw new ArgumentException();

		number = number.TrimStart('0');

		if (number.Length < 1 || number[0] == separator)
			number = '0' + number;

		fractionLength = number.IndexOf(separator);

		if (fractionLength != -1)
		{
			number = number.TrimEnd('0');
			number = number.Remove(fractionLength, 1);
		}

		value = new Natural(number);
		length = number.Length;
		fractionLength = fractionLength == -1 ? 0 : number.Length - fractionLength;
		wholeLength = length - fractionLength;
	}

	public Positive(Natural value, int fractionLength)
	{
		if (fractionLength < 0)
			throw new ArgumentException();

		if (value.IsZero)
		{
			this.value = value;
			length = 1;
			wholeLength = 1;
			this.fractionLength = 0;
		}
		else
		{
			int i = 0;

			while (i < Math.Min(fractionLength, value.Length) && value[i] == Digit.ZERO) { ++i; }

			this.value = new Natural([.. value.Digits[i..]]);
			this.fractionLength = fractionLength - i;
			length = Math.Max(this.value.Length, this.fractionLength + 1);
			wholeLength = length - this.fractionLength;
		}
	}

	#endregion

	#region Private methods

	public static Positive TrimStart(Positive p)
	{
		int i = 0;

		while (i < Math.Min(p.fractionLength, p.Digits.Length) && p[i] == Digit.ZERO) { ++i; }

		return i > 0 ? new Positive(new Natural([.. p.Digits[i..]]), p.fractionLength - i) : p;
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		return (fractionLength == 0) ? value.ToString() :
				(
					(fractionLength < Digits.Length) ? value.ToString() :
					new string('0', length - Digits.Length) + value.ToString()
				).Insert(wholeLength, separator.ToString());
	}

	public static Positive GetWhole(Positive p)
	{
		return new Positive(new Natural([.. p.Digits[(^p.wholeLength)..]]), 0);
	}

	public static Positive GetFraction(Positive p)
	{
		return new Positive(new([.. p.Digits[..Math.Min(p.fractionLength, p.Digits.Length)]]), p.fractionLength);
	}

	public static bool Equals(Positive p1, Positive p2)
	{
		return p1.length == p2.length && p1.fractionLength == p2.fractionLength && p1.value == p2.value;
	}

	public static bool GreaterThan(Positive p1, Positive p2)
	{
		if (p1.wholeLength != p2.wholeLength)
			return p1.wholeLength > p2.wholeLength;

		if (p1.fractionLength == p2.fractionLength)
			return p1.value > p2.value;
		else
		{
			Digit[] splicing = Digit.CreateArray(Math.Max(p1.fractionLength, p2.fractionLength));

			if (p1.fractionLength < p2.fractionLength)
				return new Natural([.. splicing, .. p1.Digits]) > p2.value;
			else
				return p1.value > new Natural([.. splicing, .. p2.Digits]);
		}
	}

	public static Positive Add(Positive p1, Positive p2)
	{
		int difference = p1.fractionLength - p2.fractionLength;

		if (difference < 0)
			(p1, p2) = (p2, p1);

		Positive result = new(p1.value + new Natural([.. Digit.CreateArray(Math.Abs(difference)), .. p2.Digits]), p1.fractionLength);

		return TrimStart(result);
	}

	public static (bool Swap, Positive Value) Substract(Positive p1, Positive p2)
	{
		int maxFractionLength = Math.Max(p1.fractionLength, p2.fractionLength);
		Digit[] splicing = Digit.CreateArray(Math.Abs(p1.fractionLength - p2.fractionLength));

		bool swap;
		Natural result;

		if (p1.fractionLength < p2.fractionLength)
			(swap, result) = Natural.Substract(new Natural([.. splicing, .. p1.Digits]), p2.value);
		else
			(swap, result) = Natural.Substract(p1.value, new Natural([.. splicing, .. p2.Digits]));

		return (swap, TrimStart(new Positive(result, maxFractionLength)));
	}

	public static Positive Multiply(Positive p1, Positive p2)
	{
		return new Positive(Natural.Multiply(p1.value, p2.value), p1.fractionLength + p2.fractionLength);
	}

	public static (Positive Value, Positive Remainder) Divide(Positive p1, Positive p2)
	{
		int denominatorSlicingLength = (p1.fractionLength > p2.fractionLength) ? p1.fractionLength - p2.fractionLength : 0;
		int numeratorSlicingLength = (p2.fractionLength > p1.fractionLength) ? p2.fractionLength - p1.fractionLength : 0;

		Natural denominator = new([.. Digit.CreateArray(denominatorSlicingLength), .. p2.Digits]);
		Natural numerator = new([.. Digit.CreateArray(numeratorSlicingLength + fractionCalculatonLength), .. p1.Digits]);

		(Natural whole, Natural remainder) = Natural.Divide(numerator, denominator);

		return (new Positive(whole, fractionCalculatonLength), new Positive(remainder, fractionCalculatonLength + numeratorSlicingLength + p1.FractionLength));
	}

	public static Positive SecondPower(Positive p)
	{
		return p * p;
	}

	public static Positive Power(Positive i1, Positive i2)
	{
		if (i2.fractionLength != 0)
			throw new NotImplementedException();

		return new(i1.value ^ i2.value, i1.fractionLength * Convert.ToInt32(i2.ToString()));
	}

	public static (Positive Whole, Positive Remainder) SquareRoot(Positive value)
	{
		Digit[] splicing = Digit.CreateArray(((fractionCalculatonLength * 2 - value.fractionLength) * 2 + 1) / 2);
		(Natural whole, Natural remainder) = Natural.SquareRoot(new Natural([.. splicing, .. value.Digits]));
		int fractionLength = (value.fractionLength + splicing.Length) / 2;
		
		return (new Positive(whole, fractionLength), new Positive(remainder, fractionLength * 2));
	}

	public static (Positive Whole, Positive Remainder) Root(Positive value, Positive n)
	{
		if (n.fractionLength != 0 || n.IsZero)
			throw new NotImplementedException();

		int nInt = Convert.ToUInt16(n.ToString());
		Digit[] splicing = Digit.CreateArray(((fractionCalculatonLength * nInt - value.fractionLength) * nInt + (nInt - 1)) / nInt);
		(Natural whole, Natural remainder) = Natural.Root(new Natural([.. splicing, .. value.Digits]), new Natural([.. n.Digits]));
		int fractionLength = (value.fractionLength + splicing.Length) / nInt;

		return (new Positive(whole, fractionLength), new Positive(remainder, fractionLength * nInt));
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Positive positive && this == positive;
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
	public static Positive operator %(Positive f1, Positive f2) => Divide(f1, f2).Remainder;
	public static Positive operator ^(Positive f1, Positive f2) => Power(f1, f2);
	public static Positive operator ~(Positive f) => SquareRoot(f).Whole;
	public static Positive operator |(Positive f1, Positive f2) => Root(f2, f1).Whole;

	#endregion
}