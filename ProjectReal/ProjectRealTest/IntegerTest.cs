using System;
using ProjectReal.Number;

namespace ProjectRealTest;

[TestClass]
public class IntegerTest
{
	private readonly bool writeSign;

	public IntegerTest()
	{
		writeSign = Integer.WriteSign;

		Integer.WriteSign = true;
	}

	[TestCleanup()]
	public void CleanUp() => Integer.WriteSign = writeSign;

	private static bool Sign(string sign)
	{
		return sign[0] switch
		{
			'-' => sign.Length == 2 && sign[1] == '0',
			_ => true
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
		Assert.ThrowsException<NullReferenceException>(() => new Integer(null!));

		Assert.ThrowsException<ArgumentException>(() => new Integer(""));
		Assert.ThrowsException<ArgumentException>(() => new Integer("+"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("-"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("a123"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("123a"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("12a3"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("+a123"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("+123a"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("+12a3"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("-a123"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("-123a"));
		Assert.ThrowsException<ArgumentException>(() => new Integer("-12a3"));

		Integer number1, number2;

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			number1 = new(item.Number1);
			number2 = new(item.Number2);

			Assert.AreEqual(Sign(item.Number1), number1.Sign);
			Assert.AreEqual((new Natural(item.Number1.Replace("+", "").Replace("-", ""))).ToString(), number1.Value.ToString());

			Assert.AreEqual(Sign(item.Number2), number2.Sign);
			Assert.AreEqual((new Natural(item.Number2.Replace("+", "").Replace("-", ""))).ToString(), number2.Value.ToString());
		}
	}

	[TestMethod]
	public void NaturalConstructor()
	{
		Natural natural1, natural2;
		Integer number1, number2;

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			natural1 = new(item.Number1.Replace("+", "").Replace("-", ""));
			natural2 = new(item.Number2.Replace("+", "").Replace("-", ""));

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

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			number1 = new(item.Number1);
			number2 = new(item.Number2);

			Assert.AreEqual(item.Number1 == "-0" ? "+0" : item.Number1, number1.ToString());
			Assert.AreEqual(item.Number2 == "-0" ? "+0" : item.Number2, number2.ToString());
		}
	}

	[TestMethod]
	public void EqualsMethod()
	{
		Digit[] digits1, digits2;
		Integer numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (IntegerTestCase item in IntegerTestCases.List)
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

			Assert.AreEqual(item.Equal, numberDigits1 == numberDigits2);
			Assert.AreEqual(numberDigits1 == numberDigits2, numberDigits2 == numberDigits1);
		}
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
		Digit[] digits1, digits2;
		Integer numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (IntegerTestCase item in IntegerTestCases.List)
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
	}

	[TestMethod]
	public void AddMethod()
	{
		Integer integer1, integer2;

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			Assert.AreEqual(item.Add, Integer.Add(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void SubtractMethod()
	{
		Integer integer1, integer2;

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			Assert.AreEqual(item.Sub, Integer.Subtract(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Integer integer1, integer2;

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			Assert.AreEqual(item.Mul, Integer.Multiply(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		Integer integer1, integer2, whole, remainder;

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			if (item.Div == "ERROR")
				Assert.ThrowsException<DivideByZeroException>(() => Integer.Divide(integer1, integer2));
			else if (item.Div != "BIG")
			{
				(whole, remainder) = Integer.Divide(integer1, integer2);

				Assert.AreEqual(item.Div, whole.ToString());
				Assert.AreEqual((new Integer(item.Number1)).ToString(), ((whole * item.Number2) + remainder).ToString());
			}
		}
	}

	[TestMethod]
	public void PowerMethod()
	{
		Integer integer1, integer2;

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			if (item.Pow == "ERROR")
			{
				try
				{
					Integer.Power(integer1, integer2);
					throw new Exception($"({integer1})^({integer2}) did not fail!");
				}
				catch (Exception e)
				{
					if (!(e is NotImplementedException or DivideByZeroException or NotSupportedException))
						Assert.Fail(e.Message);
				}
			}
			else if (item.Pow != "BIG")
				Assert.AreEqual(item.Pow, Integer.Power(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void RootMethod()
	{
		Integer integer1, integer2, whole, remainder;

		foreach (IntegerTestCase item in IntegerTestCases.List)
		{
			integer1 = new(item.Number1);
			integer2 = new(item.Number2);

			if (item.Root == "ERROR")
			{
				try
				{
					Integer.Root(integer1, integer2);
					throw new Exception($"({integer2})|({integer1}) did not fail!");
				}
				catch (Exception e)
				{
					if (!(e is ArgumentException or DivideByZeroException or NotSupportedException))
						Assert.Fail(e.Message);
				}
			}
			else if (item.Root != "BIG")
			{
				(whole, remainder) = Integer.Root(integer1, integer2);

				Assert.AreEqual(item.Root, whole.ToString());
				Assert.AreEqual((new Integer(item.Number1)).ToString(), ((whole ^ item.Number2) + remainder).ToString());
			}
		}
	}
}