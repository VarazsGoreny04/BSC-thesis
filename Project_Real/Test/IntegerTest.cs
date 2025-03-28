using Project_Real;
using System;

namespace Test;

[TestClass]
public class IntegerTest
{
	private static bool Sign(string sign)
	{
		return sign[0] switch
		{
			'+' => true,
			'-' => sign.Length == 2 && sign[1] == '0',
			_ => throw new FormatException()
		};
	}
	private static string Format(bool sign, string number) => (Integer.WriteSign && sign ? '+' + number.ToString() : number.ToString());

	[TestMethod]
	public void ZeroConstructor()
	{
		Integer empty = new();

		Integer[] zeros =
		[
			new("0"),
			new("00"),
			new("+0"),
			new("+00"),
			new("-0"),
			new("-00"),
			new(true, new([Digit.ZERO])),
			new(true, new([Digit.ZERO, Digit.ZERO])),
			new(false, new([Digit.ZERO])),
			new(false, new([Digit.ZERO, Digit.ZERO]))
		];

		foreach (Integer zero in zeros)
			Assert.AreEqual(empty, zero);
	}

	[TestMethod]
	public void StringConstructor()
	{
		string nullString = null!;
		Assert.ThrowsException<ArgumentException>(() => new Integer(nullString));
		Assert.ThrowsException<ArgumentException>(() => new Integer(""));
		Assert.ThrowsException<ArgumentException>(() => new Integer("+"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("-"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("a123"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("123a"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("12a3"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("+a123"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("+123a"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("+12a3"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("-a123"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("-123a"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("-12a3"));

		Integer number1, number2;

		foreach (var item in IntegerTestCases.List)
		{
			number1 = new(item.Number1);
			number2 = new(item.Number2);

			Assert.AreEqual(Sign(item.Number1), number1.Sign);
			for (int j = 1; j < item.Number1.Length; ++j)
				Assert.AreEqual(item.Number1[j].ToString(), number1.Digits[^j].ToString());

			Assert.AreEqual(Sign(item.Number2), number2.Sign);
			for (int j = 1; j < item.Number2.Length; ++j)
				Assert.AreEqual(item.Number2[j].ToString(), number2.Digits[^j].ToString());
		}
	}

	[TestMethod]
	public void NaturalConstructor()
	{
		Natural natural1, natural2;
		Integer number1, number2;

		foreach (var item in IntegerTestCases.List)
		{
			natural1 = new(item.Number1[1..]);
			natural2 = new(item.Number2[1..]);

			number1 = new(Sign(item.Number1), natural1);
			number2 = new(Sign(item.Number2), natural2);

			Assert.AreEqual(Sign(item.Number1), number1.Sign);
			for (int j = 1; j < item.Number1.Length; ++j)
				Assert.AreEqual(item.Number1[j].ToString(), number1.Digits[^j].ToString());

			Assert.AreEqual(Sign(item.Number2), number2.Sign);
			for (int j = 1; j < item.Number2.Length; ++j)
				Assert.AreEqual(item.Number2[j].ToString(), number2.Digits[^j].ToString());
		}
	}

	[TestMethod]
	public void ToStringMethod()
	{
		Integer number1, number2;

		foreach (var item in IntegerTestCases.List)
		{
			number1 = new(item.Number1);
			number2 = new(item.Number2);

			Assert.AreEqual(item.Number1, number1.ToString());
			Assert.AreEqual(item.Number2, number2.ToString());
		}
	}

	[TestMethod]
	public void EqualsMethod()
	{
		Random rnd = new();
		string characters;
		Digit[] digits;
		Natural numberDigits, numberCharacters;

		for (int i = 0; i < 100; ++i)
		{
			characters = rnd.Next(int.MaxValue).ToString() + rnd.Next(int.MaxValue).ToString();

			digits = new Digit[characters.Length];
			for (int j = characters.Length - 1; j >= 0; --j)
				digits[j] = new Digit(characters[^(j + 1)]);

			numberDigits = new(digits);
			numberCharacters = new(characters);

			Assert.AreEqual(new Integer(true, numberCharacters), new Integer(true, numberDigits));
			Assert.AreEqual(new Integer(false, numberCharacters), new Integer(false, numberDigits));
			Assert.AreNotEqual(new Integer(true, numberCharacters), new Integer(false, numberDigits));
			Assert.AreNotEqual(new Integer(false, numberCharacters), new Integer(true, numberDigits));

			numberDigits = new([.. digits, Digit.ZERO, Digit.ZERO]);
			numberCharacters = new(new string('0', rnd.Next(5)) + characters);

			Assert.AreEqual(new Integer(true, numberCharacters), new Integer(true, numberDigits));
			Assert.AreEqual(new Integer(false, numberCharacters), new Integer(false, numberDigits));
			Assert.AreNotEqual(new Integer(true, numberCharacters), new Integer(false, numberDigits));
			Assert.AreNotEqual(new Integer(false, numberCharacters), new Integer(true, numberDigits));

			int index = rnd.Next(digits.Length);
			digits[index] = Digit.Add(digits[index], '1').Digit;

			numberDigits = new([.. digits, Digit.ZERO, Digit.ZERO]);

			Assert.AreNotEqual(new Integer(true, numberCharacters), new Integer(true, numberDigits));
			Assert.AreNotEqual(new Integer(false, numberCharacters), new Integer(false, numberDigits));
			Assert.AreNotEqual(new Integer(true, numberCharacters), new Integer(false, numberDigits));
			Assert.AreNotEqual(new Integer(false, numberCharacters), new Integer(true, numberDigits));
		}
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
		Random rnd = new();
		bool expected;
		int int1, int2;
		string characters1, characters2;
		Digit[] digits1, digits2;
		Natural numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(1, int.MaxValue - 1);
			int2 = rnd.Next(1, int.MaxValue - 1);

			characters1 = int1.ToString();
			characters2 = int2.ToString();

			digits1 = new Digit[characters1.Length];
			for (int j = characters1.Length - 1; j >= 0; --j)
				digits1[j] = new Digit(characters1[^(j + 1)]);

			digits2 = new Digit[characters2.Length];
			for (int j = characters2.Length - 1; j >= 0; --j)
				digits2[j] = new Digit(characters2[^(j + 1)]);

			numberCharacters1 = new(characters1);
			numberCharacters2 = new(characters2);
			numberDigits1 = new(digits1);
			numberDigits2 = new(digits2);

			expected = int1 > int2;

			Assert.AreEqual(expected, Integer.GreaterThan(new(true, numberCharacters1), new(true, numberCharacters2)));
			Assert.AreEqual(expected, Integer.GreaterThan(new(true, numberCharacters1), new(true, numberDigits2)));
			Assert.AreEqual(expected, Integer.GreaterThan(new(true, numberDigits1), new(true, numberDigits2)));
			Assert.AreEqual(expected, Integer.GreaterThan(new(true, numberDigits1), new(true, numberCharacters2)));

			expected = -int1 > -int2;

			Assert.AreEqual(expected, Integer.GreaterThan(new(false, numberCharacters1), new(false, numberCharacters2)));
			Assert.AreEqual(expected, Integer.GreaterThan(new(false, numberCharacters1), new(false, numberDigits2)));
			Assert.AreEqual(expected, Integer.GreaterThan(new(false, numberDigits1), new(false, numberDigits2)));
			Assert.AreEqual(expected, Integer.GreaterThan(new(false, numberDigits1), new(false, numberCharacters2)));

			Assert.AreEqual(true, Integer.GreaterThan(new(true, numberCharacters1), new(false, numberCharacters2)));
			Assert.AreEqual(true, Integer.GreaterThan(new(true, numberCharacters1), new(false, numberDigits2)));
			Assert.AreEqual(true, Integer.GreaterThan(new(true, numberDigits1), new(false, numberDigits2)));
			Assert.AreEqual(true, Integer.GreaterThan(new(true, numberDigits1), new(false, numberCharacters2)));

			Assert.AreEqual(false, Integer.GreaterThan(new(false, numberCharacters1), new(true, numberCharacters2)));
			Assert.AreEqual(false, Integer.GreaterThan(new(false, numberCharacters1), new(true, numberDigits2)));
			Assert.AreEqual(false, Integer.GreaterThan(new(false, numberDigits1), new(true, numberDigits2)));
			Assert.AreEqual(false, Integer.GreaterThan(new(false, numberDigits1), new(true, numberCharacters2)));
		}
	}

	[TestMethod]
	public void AddMethod()
	{
		Random rnd = new();
		int int1, int2, done;
		int halfOfMax = (int.MaxValue / 2);
		Integer integer1, integer2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue) - halfOfMax;
			int2 = rnd.Next(int.MaxValue) - halfOfMax;

			integer1 = new(int1.ToString());
			integer2 = new(int2.ToString());

			done = int1 + int2;

			Assert.AreEqual(Format(done >= 0, done.ToString()), Integer.Add(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void SubstractMethod()
	{
		Random rnd = new();
		int int1, int2, done;
		int halfOfMax = (int.MaxValue / 2);
		Integer integer1, integer2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue) - halfOfMax;
			int2 = rnd.Next(int.MaxValue) - halfOfMax;

			integer1 = new(int1.ToString());
			integer2 = new(int2.ToString());

			done = int1 - int2;

			Assert.AreEqual(Format(done >= 0, done.ToString()), Integer.Substract(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Random rnd = new();
		int max = (int)Math.Sqrt(int.MaxValue);
		int int1, int2, done;
		Integer integer1, integer2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(max * 2) - max;
			int2 = rnd.Next(max * 2) - max;

			integer1 = new(int1.ToString());
			integer2 = new(int2.ToString());

			done = int1 * int2;

			Assert.AreEqual(Format(done >= 0, done.ToString()), Integer.Multiply(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		Random rnd = new();
		int int1, int2, done1, done2;
		Integer integer1, integer2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue);
			int2 = rnd.Next(1, int.MaxValue);

			integer1 = new(int1.ToString());
			integer2 = new(int2.ToString());

			done1 = int1 / int2;
			done2 = int1 % int2;

			(Integer whole, Integer remainder) = Integer.Divide(integer1, integer2);

			Assert.AreEqual(Format(done1 >= 0, done1.ToString()), whole.ToString());
			Assert.AreEqual(Format(done2 >= 0, done2.ToString()), remainder.ToString());
		}

		Assert.AreEqual("0", Integer.Divide("0", "1").Whole);
		Assert.AreEqual("0", Integer.Divide("0", "-1").Whole);
		Assert.ThrowsException<DivideByZeroException>(() => Integer.Divide("1", "0"));
		Assert.ThrowsException<DivideByZeroException>(() => Integer.Divide("-1", "0"));
	}

	[TestMethod]
	public void SecondPowerMethod()
	{
		Random rnd = new();
		int max = (int)Math.Sqrt(int.MaxValue);
		int int1;
		Integer integer1;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(max) - max;

			integer1 = new(int1.ToString());

			Assert.AreEqual(Format(true, Math.Pow(int1, 2).ToString()), Integer.SecondPower(integer1).ToString());
		}
	}

	[TestMethod]
	public void PowerMethod()
	{
		Random rnd = new();
		int int1, int2;
		long done;
		Integer integer1, integer2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(1, 15);
			int2 = rnd.Next(1, 10);

			integer1 = new(int1.ToString());
			integer2 = new(int2.ToString());

			done = (long)Math.Pow(int1, int2);

			Assert.AreEqual(Format(done >= 0, done.ToString()), Integer.Power(integer1, integer2).ToString());
		}

		Assert.AreEqual(Format(true, "1"), Integer.Power("0", "0").ToString());
		Assert.AreEqual(Format(true, "1"), Integer.Power("-0", "-0").ToString());
		Assert.AreEqual(Format(true, "1"), Integer.Power("0", "-0").ToString());
		Assert.AreEqual(Format(true, "1"), Integer.Power("-0", "0").ToString());
	}
}