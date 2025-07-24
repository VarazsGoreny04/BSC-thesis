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
	public Natural Value => value;
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

	public static (Positive Value, Positive Remainder) Divide(Positive p1, Positive p2, int? fractionCalculatonLength = null)
	{
		int fCL = fractionCalculatonLength ?? Positive.fractionCalculatonLength;

		int denominatorSlicingLength = (p1.fractionLength > p2.fractionLength) ? p1.fractionLength - p2.fractionLength : 0;
		int numeratorSlicingLength = (p2.fractionLength > p1.fractionLength) ? p2.fractionLength - p1.fractionLength : 0;

		Natural denominator = new([.. Digit.CreateArray(denominatorSlicingLength), .. p2.Digits]);
		Natural numerator = new([.. Digit.CreateArray(numeratorSlicingLength + fCL), .. p1.Digits]);

		(Natural whole, Natural remainder) = Natural.Divide(numerator, denominator);

		return (new Positive(whole, fCL), new Positive(remainder, fCL + numeratorSlicingLength + p1.FractionLength));
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

	public static (Positive Value, Positive Remainder) SquareRoot(Positive value, int? fractionCalculatonLength = null)
	{
		int fCL = fractionCalculatonLength ?? Positive.fractionCalculatonLength;

		if (fCL < 0)
			throw new ArgumentOutOfRangeException();

		if (value.value.IsZero || value == new Positive(new([Digit.ONE]), 0))
			return (value, new Positive());

		int splicingLength = (fCL * 2 - value.fractionLength + 1) / 2;

		Natural two = new([Digit.TWO]);
		Natural rootTimesTwo, test;
		Natural remainder = new();
		Natural root = new();
		Digit xTry;

		for (int i = ((value.Length + 1) / 2 - 1) * 2; i >= 0; i -= 2)
		{
			remainder = new([value.value[i], (i + 1 < value.Digits.Length ? value.value[i + 1] : Digit.ZERO), .. remainder.Digits]);

			xTry = Digit.ZERO;

			if (!remainder.IsZero)
			{
				rootTimesTwo = root * two;

				byte j = 10;
				do
				{
					xTry -= Digit.ONE;
					test = new Natural([xTry, .. rootTimesTwo.Digits]) * new Natural([xTry]);
				} while (--j > 0 && test > remainder);

				remainder -= test;
			}

			root = new Natural([xTry, .. root.Digits]);
		}

		for (int i = splicingLength; i > 0; --i)
		{
			remainder = new([Digit.ZERO, Digit.ZERO, .. remainder.Digits]);

			xTry = Digit.ZERO;

			if (!remainder.IsZero)
			{
				rootTimesTwo = root * two;

				byte j = 10;
				do
				{
					xTry -= Digit.ONE;
					test = new Natural([xTry, .. rootTimesTwo.Digits]) * new Natural([xTry]);
				} while (--j > 0 && test > remainder);

				remainder -= test;
			}

			root = new Natural([xTry, .. root.Digits]);
		}

		return (new Positive(root, fCL), new Positive(remainder, fCL + splicingLength));
	}

	public static (Positive Value, Positive Remainder) Root(Positive value, Positive n, int? fractionCalculatonLength = null)
	{
		if (n.fractionLength != 0 || n.IsZero)
			throw new NotImplementedException();

		int fCL = fractionCalculatonLength ?? Positive.fractionCalculatonLength;

		if (fCL < 0)
			throw new ArgumentOutOfRangeException();

		/*Natural remainder = new();

		if (n.value < new Natural([Digit.THREE]))
		{
			return Digit.ToChar(n[0]) switch
			{
				'0' => throw new NotImplementedException(),
				'1' => (value, new Positive(remainder, 0)),
				_ => SquareRoot(value)
			};
		}

		if (value.Length == 1 && (value[0] == Digit.ZERO || value[0] == Digit.ONE))
			return (value, new Positive(remainder, 0));

		ushort nInt = Convert.ToUInt16(n.ToString());
		int splicingLength = ((fCL * nInt - value.fractionLength) * nInt + (nInt - 1)) / nInt;
		int fractionLength = (value.fractionLength + splicingLength) / nInt;

		Digit[] digits = [.. value.Digits, .. Digit.CreateArray((nInt - ((value.Digits.Length + splicingLength) % nInt)) % nInt)];

		Digit xTry;
		Natural test, kNatural, nMinusKN, binomial;
		Natural nFactorial = Natural.Factorial(n.value);
		Natural root = new();

		for (int i = digits.Length - nInt; i >= 0; i -= nInt)
		{
			remainder = new Natural([.. digits[i..(i + nInt)], .. remainder.Digits]);

			xTry = Digit.ZERO;

			if (!remainder.IsZero)
			{
				byte j = 0;
				do
				{
					xTry -= Digit.ONE;
					test = new();

					for (int k = nInt - 1; k >= 0; --k)
					{

						kNatural = k.ToString();
						nMinusKN = n - kNatural;
						binomial = nFactorial / (Natural.Factorial(kNatural) * Natural.Factorial(nMinusKN));

						test += new Natural([.. Digit.CreateArray(k), .. (binomial * (root ^ kNatural) * ((new Natural([xTry])) ^ nMinusKN)).digits]);
					}
				} while (++j < 10 && test > remainder);

				remainder -= test;
			}

			root = new Natural([xTry, .. root.digits]);
		}

		for (int i = digits.Length - nInt; i >= 0; i -= nInt)
		{
			remainder = new Natural([.. Digit.CreateArray(nInt), .. remainder.Digits]);

			xTry = Digit.ZERO;

			if (!remainder.IsZero)
			{
				byte j = 0;
				do
				{
					xTry -= Digit.ONE;
					test = new();

					for (int k = nInt - 1; k >= 0; --k)
					{

						kNatural = k.ToString();
						nMinusKN = n - kNatural;
						binomial = nFactorial / (Natural.Factorial(kNatural) * Natural.Factorial(nMinusKN));

						test += new Natural([.. Digit.CreateArray(k), .. (binomial * (root ^ kNatural) * ((new Natural([xTry])) ^ nMinusKN)).digits]);
					}
				} while (++j < 10 && test > remainder);

				remainder -= test;
			}

			root = new Natural([xTry, .. root.digits]);
		}

		return (new Positive(root, , remainder);*/


		int nInt = Convert.ToUInt16(n.ToString());
		Digit[] splicing = Digit.CreateArray(((fCL * nInt - value.fractionLength) * nInt + (nInt - 1)) / nInt);
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
	public static Positive operator %(Positive f1, Positive f2) => Divide(f1, f2, 0).Remainder;
	public static Positive operator ^(Positive f1, Positive f2) => Power(f1, f2);
	public static Positive operator ~(Positive f) => SquareRoot(f).Value;
	public static Positive operator |(Positive f1, Positive f2) => Root(f2, f1).Value;

	#endregion
}