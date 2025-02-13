namespace Project_Real;

public readonly struct Natural
{
	#region Fields

	public readonly bool IsZero;
	public readonly Digit[] Digits;

	#endregion

	#region Properties

	public readonly int Length => Digits.Length;
	public readonly Digit this[Index i] => Digits[i];

	#endregion

	#region Constructors

	public Natural()
	{
		IsZero = true;
		Digits = [Digit.ZERO];
	}

	public Natural(string number)
	{
		if (number is null || number.Length < 1)
			throw new ArgumentException();

		number = number.TrimStart('0');

		IsZero = number.Length < 1;

		if (IsZero)
			number = "0";

		Digits = new Digit[number.Length];

		for (int i = 0; i < Length; ++i)
			Digits[i] = new Digit(number[^(i + 1)]);
	}

	public Natural(Digit[] digits)
	{
		if (digits.Length < 1)
			throw new ArgumentException();

		int i = 0;

		while (i < digits.Length && Digit.Equals(digits[^(i + 1)], Digit.ZERO)) { ++i; }

		IsZero = i == digits.Length;
		Digits = IsZero ? [Digit.ZERO] : digits[..(digits.Length - i)];
	}

	#endregion

	#region Public methods

	public override string ToString()
	{
		string number = string.Empty;

		for (int i = 1; i <= Digits.Length; ++i)
			number += Digits[^i];

		return number;
	}

	public static Natural TrimEnd(Natural n)
	{
		return new Natural(Digit.TrimEnd(n.Digits));
	}

	public static bool Equals(Natural n1, Natural n2)
	{
		if (n1.Length != n2.Length)
			return false;

		int i = 0;

		while (++i < n1.Length && Digit.Equals(n1[^i], n2[^i])) { }

		return i == n1.Length && Digit.Equals(n1[^i], n2[^i]);
	}

	public static bool GreaterThan(Natural n1, Natural n2)
	{
		if (n1.Length != n2.Length)
			return n1.Length > n2.Length;

		int i = 0;

		while (++i < n1.Length && Digit.Equals(n1[^i], n2[^i])) { }

		return Digit.GreaterThan(n1[^i], n2[^i]);
	}

	public static Natural Add(Natural n1, Natural n2, bool carry = false)
	{
		if (n1.Length < n2.Length)
			(n2, n1) = (n1, n2);

		Digit[] result = new Digit[n1.Length];

		for (int i = 0; i < n1.Length; ++i)
			(carry, result[i]) = Digit.Add(n1[i], (i < n2.Length ? n2[i] : Digit.ZERO), carry);

		return carry ? new Natural([.. result, '1']) : new Natural(result);
	}

	public static (bool Swap, Natural Value) Substract(Natural n1, Natural n2, bool carry = false)
	{
		bool swap = GreaterThan(n2, n1);

		if (swap)
			(n2, n1) = (n1, n2);

		Digit[] result = new Digit[n1.Length];

		for (int i = 0; i < Math.Max(n1.Length, n2.Length); ++i)
			(carry, result[i]) = Digit.Substract((i < n1.Length ? n1[i] : Digit.ZERO), (i < n2.Length ? n2[i] : Digit.ZERO), carry);

		return (swap, new Natural(result));
	}

	public static Natural Multiply(Natural n1, Natural n2)
	{
		Natural result = new();
		Digit[] temp;
		Digit overflowD, digit;
		bool overflowB;
		int addedIndex;

		for (int n2i = 0; n2i < n2.Length; ++n2i)
		{
			if (Digit.Equals(n2[n2i], Digit.ZERO))
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

			result = Add(result, new Natural(temp));
		}

		return result;
	}

	public static (Natural Whole, Natural Remainder) Divide(Natural n1, Natural n2)
	{
		if (n2.IsZero)
			throw new DivideByZeroException();
		else if (n1.Length < n2.Length)
			return (new Natural(), n1);

		Natural temp = new();
		Digit one = new('1');
		Digit[] remainder = n1.Digits;
		Digit[] result = Digit.CreateArray(n1.Length - n2.Length + 1);

		int i = n1.Length - n2.Length;
		while (i >= 0)
		{
			temp = new Natural(remainder.Skip(i).ToArray());

			while (!GreaterThan(n2, temp))
			{
				temp = Substract(temp, n2).Value;
				result[i] = Digit.Add(result[i], one).Digit;
			}

			Array.Copy(temp.Digits, 0, remainder, i, temp.Length);

			if (temp.IsZero)
				while (--i >= 0 && Digit.Equals(remainder[i], Digit.ZERO)) { }
			else
				--i;
		}

		return (new Natural(result), temp);
	}

	public static Natural SecondPower(Natural n)
	{
		return n.IsZero ? throw new NotImplementedException() : Multiply(n, n);
	}

	public static Natural Power(Natural n1, Natural n2)
	{
		Natural result = new([new Digit('1')]);

		if (n2.IsZero)
			return n1.IsZero ? throw new NotImplementedException() : result;

		Natural two = new([new Digit('2')]);
		Natural lastPowerCalculated = n1;

		(Natural whole, Natural remainder) = Divide(n2, two);

		if (!remainder.IsZero)
			result = Multiply(result, lastPowerCalculated);

		while (!whole.IsZero)
		{
			lastPowerCalculated = Multiply(lastPowerCalculated, lastPowerCalculated);
			(whole, remainder) = Divide(whole, two);

			if (!remainder.IsZero)
				result = Multiply(result, lastPowerCalculated);
		}

		return result;
	}

	public override readonly bool Equals(object? obj)
	{
		return obj is Natural natural && Equals(this, natural);
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

	#endregion
}