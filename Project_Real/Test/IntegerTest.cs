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
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Integer number1, number2;

		foreach (var item in IntegerTestCases.List)
		{
			number1 = new(item.Number1);
			number2 = new(item.Number2);

			Assert.AreEqual(item.Number1 == "-0" ? "+0" : item.Number1, number1.ToString());
			Assert.AreEqual(item.Number2 == "-0" ? "+0" : item.Number2, number2.ToString());
		}

		Integer.WriteSign = writeSign;
	}

	[TestMethod]
	public void EqualsMethod()
	{
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Digit[] digits1, digits2;
		Integer numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (var item in IntegerTestCases.List)
		{
			digits1 = new Digit[item.Number1.Length - 1];
			for (int j = item.Number1.Length - 2; j >= 0; --j)
				digits1[j] = new Digit(item.Number1[^(j + 1)]);

			numberCharacters1 = new(item.Number1);
			numberDigits1 = new(Sign(item.Number1), new Natural(digits1));

			digits2 = new Digit[item.Number2.Length- 1];
			for (int j = item.Number2.Length - 2; j >= 0; --j)
				digits2[j] = new Digit(item.Number2[^(j + 1)]);

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new(Sign(item.Number2), new Natural(digits2));

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Equal, numberDigits1 == numberDigits2);
			Assert.AreEqual(numberDigits1 == numberDigits2, numberDigits2 == numberDigits1);
		}

		Integer.WriteSign = writeSign;
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Digit[] digits1, digits2;
		Integer numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (var item in IntegerTestCases.List)
		{
			digits1 = new Digit[item.Number1.Length - 1];
			for (int j = item.Number1.Length - 2; j >= 0; --j)
				digits1[j] = new Digit(item.Number1[^(j + 1)]);

			numberCharacters1 = new(item.Number1);
			numberDigits1 = new(Sign(item.Number1), new Natural(digits1));

			digits2 = new Digit[item.Number2.Length - 1];
			for (int j = item.Number2.Length - 2; j >= 0; --j)
				digits2[j] = new Digit(item.Number2[^(j + 1)]);

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new(Sign(item.Number2), new Natural(digits2));

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Greater, Integer.GreaterThan(numberCharacters1, numberCharacters2));
			Assert.AreEqual(item.Greater, Integer.GreaterThan(numberDigits1, numberDigits2));
			Assert.AreEqual(item.Greater, Integer.GreaterThan(numberCharacters1, numberDigits2));
			Assert.AreEqual(item.Greater, Integer.GreaterThan(numberDigits1, numberCharacters2));
		}

		Integer.WriteSign = writeSign;
	}

	[TestMethod]
	public void AddMethod()
	{
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Integer integer1, integer2;

		foreach (var item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			Assert.AreEqual(item.Add, Integer.Add(integer1, integer2).ToString());
		}

		Integer.WriteSign = writeSign;
	}

	[TestMethod]
	public void SubstractMethod()
	{
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Integer integer1, integer2;

		foreach (var item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			Assert.AreEqual(item.Sub, Integer.Substract(integer1, integer2).ToString());
		}

		Integer.WriteSign = writeSign;
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Integer integer1, integer2;

		foreach (var item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			Assert.AreEqual(item.Mul, Integer.Multiply(integer1, integer2).ToString());
		}

		Integer.WriteSign = writeSign;
	}

	[TestMethod]
	public void DivideMethod()
	{
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Integer integer1, integer2, whole, remainder;

		foreach (var item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			if (item.DivWhole == "ERROR" || item.DivRemain == "ERROR")
				Assert.ThrowsException<DivideByZeroException>(() => Integer.Divide(integer1, integer2));
			else if (!(item.DivWhole == "BIG" || item.DivRemain == "BIG"))
			{
				(whole, remainder) = Integer.Divide(integer1, integer2);

				Assert.AreEqual(item.DivWhole, whole.ToString());
				Assert.AreEqual(item.DivRemain, remainder.ToString());
			}
		}

		Integer.WriteSign = writeSign;
	}

	[TestMethod]
	public void PowerMethod()
	{
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Integer integer1, integer2;

		foreach (var item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			if (item.Pow == "ERROR")
				Assert.ThrowsException<NotImplementedException>(() => Integer.Power(integer1, integer2));
			else if (item.Pow != "BIG")
				Assert.AreEqual(item.Pow, Integer.Power(integer1, integer2).ToString());
		}

		Integer.WriteSign = writeSign;
	}

	[TestMethod]
	public void RootMethod()
	{
		bool writeSign = Integer.WriteSign;
		Integer.WriteSign = true;

		Integer integer1, integer2, whole, remainder;

		foreach (var item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			if (item.RootWhole == "ERROR" || item.RootRemain == "ERROR")
				Assert.ThrowsException<NotImplementedException>(() => Integer.Root(integer1, integer2));
			else if (!(item.RootWhole == "BIG" || item.RootRemain == "BIG"))
			{
				(whole, remainder) = Integer.Root(integer1, integer2);

				Assert.AreEqual(item.RootWhole, whole.ToString());
				Assert.AreEqual(item.RootRemain, remainder.ToString());
			}
		}

		Integer.WriteSign = writeSign;
	}
}