namespace Project_Real;
using System;
using System.Linq;

public struct Real_old
{
	#region Exceptions

	private class CalculationError : Exception { }

	#endregion

	#region Constants

	private const uint MAX = 8u;

	public static readonly uint MAXWHOLEINT = (uint)Math.Pow(2, MAX) - 1u;
	public static readonly uint FRACTIONLENGTH = (uint)ToUInt(Enumerable.Repeat(true, (int)MAX - 1).Append(false).ToArray()).ToString().Length - 1u;
	public static readonly uint MAXFRACTIONINT = Convert.ToUInt32(new string('9', (int)FRACTIONLENGTH));
	public static readonly bool[] MAXFRACTIONPLUSONE = ToBits(MAXFRACTIONINT + 1).Bits;
	public static readonly bool[] ONE = ToBits(1).Bits;

	#endregion

	#region Fields

	private readonly bool sign;
	private readonly bool[] whole;
	private readonly bool[] fraction;

	#endregion

	#region Properties

	public readonly bool Sign => sign;
	public readonly bool[] Whole => whole;
	public readonly bool[] Fraction => fraction;

	#endregion

	#region Constructors

	public Real_old(string num)
	{
		if (num == string.Empty)
			throw new CalculationError();

		sign = num[0] switch
		{
			'+' => true,
			'-' => false,
			_ => throw new CalculationError()
		};

		string[] tokens = num.Split('.');

		if (tokens.Length != 2)
			throw new CalculationError();

		whole = new bool[MAX];
		fraction = new bool[MAX];

		uint wholeInt, fractionInt;

		try
		{
			wholeInt = uint.Parse(tokens[0][1..]);
			fractionInt = uint.Parse(tokens[1]) * (uint)Math.Pow(10u, FRACTIONLENGTH - tokens[1].Length);
		}
		catch
		{
			throw new CalculationError();
		}

		if (wholeInt > MAXWHOLEINT || fractionInt > MAXFRACTIONINT || FRACTIONLENGTH < tokens[1].Length)
			throw new CalculationError();

		(bool[], int) temp = ToBits(wholeInt);

		for (int i = 0; i < temp.Item2; ++i)
			whole[i] = temp.Item1[i];

		temp = ToBits(fractionInt);

		for (int i = 0; i < temp.Item2; ++i)
			fraction[i] = temp.Item1[i];
	}

	private Real_old(bool sign, bool[] whole, bool[] fraction)
	{
		if (whole.Length != MAX || fraction.Length != MAX)
			throw new CalculationError();

		this.sign = sign;
		this.whole = whole;
		this.fraction = fraction;
	}

	#endregion

	#region Private methods 

	private static (bool[] Bits, int Power) ToBits(uint num)
	{
		int pow = 0;
		bool[] bits = new bool[MAX];

		do
		{
			bits[pow++] = num % 2 == 1;
			num /= 2;
		} while (num > 0);

		return (bits, pow);
	}

	private static uint ToUInt(bool[] b)
	{
		if (1 > b.Length && b.Length > MAX)
			throw new CalculationError();

		uint num = 0;

		for (int i = 0; i < b.Length; ++i)
		{
			if (b[i])
				num += (uint)Math.Pow(2, i);
		}

		return num;
	}

	private static bool BitEquals(bool[] b1, bool[] b2)
	{
		if (1 > b1.Length && b1.Length > MAX && b1.Length != b2.Length)
			throw new CalculationError();

		for (int i = 1; i <= b1.Length; ++i)
		{
			if (b1[^i] != b2[^i])
				return false;
		}

		return true;
	}

	private static bool BitGreaterThan(bool[] b1, bool[] b2)
	{
		if (1 > b1.Length && b1.Length > MAX && b1.Length != b2.Length)
			throw new CalculationError();

		int i = 0;

		while (++i < b1.Length && b1[^i] == b2[^i]) { }

		return b1[^i] && !b2[^i];
	}

	private static (bool OverFlow, bool[] Bits) BitAdd(bool[] b1, bool[] b2, bool carry = false)
	{
		if (1 > b1.Length && b1.Length > MAX && b1.Length != b2.Length)
			throw new CalculationError();

		bool[] result = new bool[b1.Length];

		for (int i = 0; i < result.Length; ++i)
		{
			if (b1[i] && b2[i] && carry)
				result[i] = true;
			else if (b1[i] && b2[i])
				carry = true;
			else if ((b1[i] || b2[i]) && carry) { }
			else if (b1[i] || b2[i] || carry)
			{
				result[i] = true;
				carry = false;
			}
		}

		return (carry, result);
	}

	private static bool[] TwosComplement(bool[] b)
	{
		if (1 > b.Length && b.Length > MAX)
			throw new CalculationError();

		bool[] bOC = new bool[b.Length];

		for (int i = 0; i < b.Length; ++i)
			bOC[i] = !b[i];

		return BitAdd(bOC, ONE).Bits;
	}

	private static (bool Sign, bool[] Bits) BitSubstract(bool[] b1, bool[] b2, bool carry = false)
	{
		bool[] b2PC;

		if (carry)
			b2PC = BitAdd(b2, ONE).Bits;
		else
			b2PC = b2;

		if (!BitGreaterThan(b2PC, b1))
			return (true, BitAdd(b1, TwosComplement(b2PC)).Bits);
		else
			return (false, BitSubstract(b2PC, b1).Bits);
	}

	private static bool[] BitMultiply(bool[] b1, bool[] b2)
	{
		if (1 > b1.Length && b1.Length > MAX && b1.Length != b2.Length)
			throw new CalculationError();

		bool[] result = new bool[b1.Length * 2];

		for (int i = 0; i < b2.Length; ++i)
		{
			if (b2[i])
				result = BitAdd(result, [.. new bool[i], .. b1, .. new bool[b1.Length - i]]).Bits;
		}

		return result;
	}

	private static Real_old ChangeSign(Real_old f, bool sign)
	{
		return new Real_old(sign, f.whole, f.fraction);
	}

	#endregion

	#region Public methods

	public readonly string ToBinaryString()
	{
		string num = string.Empty;

		num += sign ? '+' : '-';

		for (int i = 1; i <= MAX; ++i)
			num += whole[^i] ? 1 : 0;

		num += '.';

		for (int i = 1; i <= MAX; ++i)
			num += fraction[^i] ? 1 : 0;

		return num;
	}

	public override readonly string ToString()
	{
		uint wholeInt = ToUInt(whole);

		uint fractionInt = ToUInt(fraction);

		if (wholeInt > MAXWHOLEINT || fractionInt > MAXFRACTIONINT)
			throw new CalculationError();

		char signString = sign ? '+' : '-';

		return $"{signString}{wholeInt}.{new string('0', (int)FRACTIONLENGTH - fractionInt.ToString().Length)}{fractionInt}";
	}

	public static bool Equals(Real_old f1, Real_old f2)
	{
		return f1.sign == f2.sign && BitEquals(f1.whole, f2.whole) && BitEquals(f1.fraction, f2.fraction);
	}

	public override readonly bool Equals(object? obj)
	{
		return Equals(this, obj);
	}

	public static bool GreaterThan(Real_old f1, Real_old f2)
	{
		return BitGreaterThan([.. f1.fraction, .. f1.whole, f1.sign], [.. f2.fraction, .. f2.whole, f2.sign]);
	}

	public static Real_old Add(Real_old f1, Real_old f2)
	{
		if (f1.sign)
		{
			if (f2.sign)
			{
				bool[] addedFraction = BitAdd(f1.fraction, f2.fraction).Bits;

				bool carry = !BitGreaterThan(MAXFRACTIONPLUSONE, addedFraction);

				if (carry)
					addedFraction = BitSubstract(addedFraction, MAXFRACTIONPLUSONE).Bits;

				(bool overFlow, bool[] addedWhole) = BitAdd(f1.whole, f2.whole, carry);

				if (overFlow)
					throw new CalculationError();

				return new Real_old(true, addedWhole, addedFraction);
			}
			else
				return Substract(f1, Abs(f2));
		}
		else
		{
			if (f2.sign)
				return Substract(f2, Abs(f1));
			else
			{
				Real_old temp = Add(Abs(f1), Abs(f2));
				return ChangeSign(temp, false);
			}
		}
	}

	public static Real_old Substract(Real_old f1, Real_old f2)
	{
		if (f1.sign)
		{
			if (f2.sign)
			{
				if (GreaterThan(f2, f1))
					return ChangeSign(Substract(f2, f1), false);
				else
				{
					(bool sign, bool[] addedFraction) = BitSubstract(f1.fraction, f2.fraction);

					if (!sign)
						addedFraction = BitSubstract(MAXFRACTIONPLUSONE, addedFraction).Bits;

					(sign, bool[] addedWhole) = BitSubstract(f1.whole, f2.whole, !sign);

					return new Real_old(sign, addedWhole, addedFraction);
				}
			}
			else
				return Add(f1, Abs(f2));
		}
		else
		{
			if (f2.sign)
				return Add(f2, Abs(f1));
			else
			{
				Real_old temp = Substract(Abs(f1), Abs(f2));
				return ChangeSign(temp, false);
			}
		}
	}

	/*public static Real Multiply(Real f1, Real f2)
	{
		if (f1.sign == f2.sign)
		{
			
		}
		else
			return ChangeSign(Multiply(Abs(f1), Abs(f2)), false);
	}*/

	public static Real_old Abs(Real_old f)
	{
		return ChangeSign(f, true);
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Operators

	public static implicit operator Real_old(string num) => new(num);
	public static bool operator ==(Real_old f1, Real_old f2) => Equals(f1, f2);
	public static bool operator !=(Real_old f1, Real_old f2) => !Equals(f1, f2);
	public static bool operator >(Real_old f1, Real_old f2) => GreaterThan(f1, f2);
	public static bool operator <(Real_old f1, Real_old f2) => GreaterThan(f2, f1);
	public static bool operator >=(Real_old f1, Real_old f2) => !GreaterThan(f2, f1);
	public static bool operator <=(Real_old f1, Real_old f2) => !GreaterThan(f1, f2);
	public static Real_old operator +(Real_old f) => f;
	public static Real_old operator +(Real_old f1, Real_old f2) => Add(f1, f2);
	public static Real_old operator -(Real_old f) => ChangeSign(f, !f.sign);
	public static Real_old operator -(Real_old f1, Real_old f2) => Substract(f1, f2);
	//public static Real operator *(Real f1, Real f2) => Multiply(f1, f2);

	#endregion
}