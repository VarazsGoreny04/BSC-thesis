using System;
using System.Collections.Immutable;
using System.Linq;

namespace Project_Real;

public readonly struct Natural
{
	#region Fields

	private readonly int length;
	private readonly bool isZero;
	private readonly ImmutableArray<Digit> digits;

	#endregion

	#region Properties

	public int Length => length;
	public bool IsZero => isZero;
	public ImmutableArray<Digit> Digits => digits;
	public readonly Digit this[Index i] => digits[i];

	#endregion

	#region Constructors

	public Natural()
	{
		length = 1;
		digits = [Digit.ZERO];
		isZero = true;
	}

	public Natural(string number)
	{
		if (number is null || number.Length < 1)
			throw new ArgumentException();

		number = number.TrimStart('0');

		isZero = number.Length < 1;

		if (isZero)
			number = "0";

		length = number.Length;
		Digit[] digits = new Digit[length];

		try
		{
			for (int i = 0; i < length; ++i)
				digits[i] = new Digit(number[^(i + 1)]);
		}
		catch (Digit.ValueOutOfRangeException)
		{
			throw new ArgumentException();
		}

		this.digits = ImmutableArray.Create(digits);
	}

	public Natural(Digit[] digits)
	{
		if (digits is null || digits.Length < 1)
			throw new ArgumentException();

		this.digits = ImmutableArray.Create(Digit.TrimEnd(digits));
		length = this.digits.Length;
		isZero = length == 1 && this.digits[0] == Digit.ZERO;
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		string number = string.Empty;

		for (int i = digits.Length - 1; i >= 0; --i)
			number += digits[i];

		return number;
	}

	public static bool Equals(Natural n1, Natural n2)
	{
		if (n1.Length != n2.Length)
			return false;

		int i = n1.Length;

		while (--i > 0 && n1[i] == n2[i]) { }

		return i == 0 && n1[0] == n2[0];
	}

	public static bool GreaterThan(Natural n1, Natural n2)
	{
		if (n1.Length != n2.Length)
			return n1.Length > n2.Length;

		int i = n1.Length;

		while (--i > 0 && n1[i] == n2[i]) { }

		return n1[i] > n2[i];
	}

	public static Natural Add(Natural n1, Natural n2, bool carry = false)
	{
		if (n1.Length < n2.Length)
			(n1, n2) = (n2, n1);

		Digit[] result = new Digit[n1.Length];

		for (int i = 0; i < n1.Length; ++i)
			(carry, result[i]) = Digit.Add(n1[i], (i < n2.Length ? n2[i] : Digit.ZERO), carry);

		return carry ? new Natural([.. result, '1']) : new Natural(result);
	}

	public static (bool Swap, Natural Value) Substract(Natural n1, Natural n2, bool carry = false)
	{
		bool swap = n2 > n1;

		if (swap)
			(n1, n2) = (n2, n1);

		Digit[] result = new Digit[n1.Length];

		for (int i = 0; i < n1.Length; ++i)
			(carry, result[i]) = Digit.Substract(n1[i], (i < n2.Length ? n2[i] : Digit.ZERO), carry);

		return (swap, new Natural(result));
	}

	public static Natural Multiply(Natural n1, Natural n2)
	{
		if (n2 > n1)
			(n1, n2) = (n2, n1);

		if (n2.isZero)
			return new Natural();
		else if (n2 == "1")
			return n1;

		Natural result = new();
		Digit[] temp;
		Digit overflowD, digit;
		bool overflowB;
		int addedIndex;

		for (int n2i = 0; n2i < n2.Length; ++n2i)
		{
			if (n2[n2i] == Digit.ZERO)
				continue;

			temp = new Digit[n1.Length + n2i + 1];
			Array.Fill(temp, Digit.ZERO, 0, n2i + 1);

			addedIndex = n2i;

			for (int n1i = 0; n1i < n1.Length; ++n1i)
			{
				(overflowD, digit) = Digit.Multiply(n1[n1i], n2[n2i]);
				(overflowB, temp[addedIndex]) = Digit.Add(temp[addedIndex], digit);
				++addedIndex;
				temp[addedIndex] = Digit.Add(overflowD, Digit.ZERO, overflowB).Digit;
			}

			result += new Natural(temp);
		}

		return result;
	}

	public static (Natural Whole, Natural Remainder) Divide(Natural n1, Natural n2)
	{
		if (n2.isZero)
			throw new DivideByZeroException();
		else if (n1.Length < n2.Length)
			return (new Natural(), n1);
		else if (n2.Length == 1 && n2[0] == Digit.ONE)
			return (n1, new Natural());

		int tempLength;
		Natural temp = new();
		Digit[] remainder = [.. n1.digits];
		Digit[] result = Digit.CreateArray(n1.Length - n2.Length + 1);

		int i = n1.Length - n2.Length;
		while (i >= 0)
		{
			temp = new Natural(remainder.Skip(i).ToArray());
			tempLength = temp.Length;

			while (n2 <= temp)
			{
				temp -= n2;
				result[i] += Digit.ONE;
			}

			Array.Copy(temp.digits.ToArray(), 0, remainder, i, temp.Length);
			Array.Fill(remainder, Digit.ZERO, i + temp.Length, tempLength - temp.Length);

			if (temp.isZero)
				while (--i >= 0 && remainder[i] == Digit.ZERO) { }
			else
				--i;
		}

		return (new Natural(result), temp);
	}

	public static Natural SecondPower(Natural n)
	{
		return n * n;
	}

	public static Natural Power(Natural n1, Natural n2)
	{
		Natural result = new([new Digit('1')]);
		Natural two = new([new Digit('2')]);

		if (n2.isZero)
			return /*n1.IsZero ? throw new NotImplementedException() :*/ result;
		else if (n2 == result)
			return n1;
		else if (n2 == two)
			return n1 * n1;

		Natural lastPowerCalculated = n1;

		(Natural whole, Natural remainder) = Divide(n2, two);

		if (!remainder.isZero)
			result = lastPowerCalculated;

		while (!whole.isZero)
		{
			lastPowerCalculated *= lastPowerCalculated;
			(whole, remainder) = Divide(whole, two);

			if (!remainder.isZero)
				result *= lastPowerCalculated;
		}

		return result;
	}

	public static (Natural Whole, Natural Remainder) SquareRoot(Natural n)
	{
		if (n.isZero || n == new Natural([Digit.ONE]))
			return (n, new Natural());

		Natural two = new([Digit.TWO]);
		Natural rootTimesTwo, test;
		Natural remainder = new();
		Natural root = new();
		Digit xTry;

		for (int i = ((n.length + 1) / 2 - 1) * 2; i >= 0; i -= 2)
		{
			remainder = new([n[i], (i + 1 < n.digits.Length ? n[i + 1] : Digit.ZERO), .. remainder.digits]);

			xTry = Digit.ZERO;

			if (!remainder.isZero)
			{
				rootTimesTwo = root * two;

				byte j = 10;
				do
				{
					xTry -= Digit.ONE;
					test = new Natural([xTry, .. rootTimesTwo.digits]) * new Natural([xTry]);
				} while (--j > 0 && test > remainder);

				remainder -= test;
			}

			root = new Natural([xTry, .. root.digits]);
		}

		return (root, remainder);
	}

	public static (Natural Whole, Natural Remainder) Root(Natural value, Natural n)
	{
		Natural remainder = new();

		if (n < new Natural([Digit.THREE]))
		{
			return Digit.ToChar(n[0]) switch
			{
				'0' => throw new NotImplementedException(),
				'1' => (value, remainder),
				_ => SquareRoot(value)
			};
		}

		if (value.Length == 1 && (value[0] == Digit.ZERO || value[0] == Digit.ONE))
			return (value, remainder);

		ushort nInt = Convert.ToUInt16(n.ToString());
		Digit[] digits = [.. value.digits, .. Digit.CreateArray((nInt - (value.digits.Length % nInt)) % nInt)];

		Digit xTry;
		Natural test, kNatural, nMinusKN, binomial;
		Natural nFactorial = Factorial(n);
		Natural root = new();

		for (int i = digits.Length - nInt; i >= 0; i -= nInt)
		{
			remainder = new Natural([.. digits[i..(i + nInt)], .. remainder.digits]);

			xTry = Digit.ZERO;

			if (!remainder.isZero)
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
						binomial = nFactorial / (Factorial(kNatural) * Factorial(nMinusKN));

						test += new Natural([.. Digit.CreateArray(k), .. (binomial * (root ^ kNatural) * ((new Natural([xTry])) ^ nMinusKN)).digits]);
					}
				} while (++j < 10 && test > remainder);

				remainder -= test;
			}

			root = new Natural([xTry, .. root.digits]);
		}

		return (root, remainder);
	}

	public static Natural Factorial(Natural n)
	{
		Natural one = new([Digit.ONE]);

		if (n.isZero || n == one)
			return one;

		Natural result = n;

		while (n != one)
		{
			n -= one;
			result *= n;
		}

		return result;
	}

	public static Natural Log(Natural n1, Natural n2)
	{
		if (n1.isZero || n2.isZero)
			throw new NotImplementedException();

		Natural one = new([Digit.ONE]);
		Natural result = new();

		while (n2 <= n1)
		{
			n1 /= n2;
			result += one;
		}

		return result;
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Natural natural && this == natural;
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Natural(string num) => new(num);
	public static bool operator ==(Natural f1, Natural f2) => Equals(f1, f2);
	public static bool operator !=(Natural f1, Natural f2) => !Equals(f1, f2);
	public static bool operator >(Natural f1, Natural f2) => GreaterThan(f1, f2);
	public static bool operator <(Natural f1, Natural f2) => GreaterThan(f2, f1);
	public static bool operator >=(Natural f1, Natural f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Natural f1, Natural f2) => !GreaterThan(f1, f2);
	public static Natural operator +(Natural f1, Natural f2) => Add(f1, f2);
	public static Natural operator -(Natural f1, Natural f2) => Substract(f1, f2).Value;
	public static Natural operator *(Natural f1, Natural f2) => Multiply(f1, f2);
	public static Natural operator /(Natural f1, Natural f2) => Divide(f1, f2).Whole;
	public static Natural operator %(Natural f1, Natural f2) => Divide(f1, f2).Remainder;
	public static Natural operator ^(Natural f1, Natural f2) => Power(f1, f2);
	public static Natural operator ~(Natural f) => SquareRoot(f).Whole;
	public static Natural operator |(Natural f1, Natural f2) => Root(f2, f1).Whole;

	#endregion
}