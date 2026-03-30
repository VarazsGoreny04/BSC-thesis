using System;
using Project_Real.Number;

namespace Test;

[TestClass]
public class NaturalTest
{
	[TestMethod]
	public void ZeroConstructor()
	{
		Natural empty = new();
		Assert.IsTrue(empty.IsZero);

		Natural[] zeros =
		[
			new("0"),
			new("00"),
			new([Digit.ZERO]),
			new([Digit.ZERO, Digit.ZERO])
		];

		foreach (Natural zero in zeros)
		{
			Assert.IsTrue(zero.IsZero);
			Assert.AreEqual(empty, zero);
		}
	}

	[TestMethod]
	public void StringConstructor() // Leading zero is not tested
	{
		string nullString = null!;
		Assert.ThrowsException<ArgumentException>(() => new Natural(nullString));
		Assert.ThrowsException<ArgumentException>(() => new Natural(""));
		Assert.ThrowsException<ArgumentException>(() => new Natural("a123"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("123a"));
		Assert.ThrowsException<ArgumentException>(() => new Natural("12a3"));

		Natural number1, number2;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			number1 = new(item.Number1);
			number2 = new(item.Number2);

			for (int j = item.Number1.Length - 1; j >= 0; --j)
				Assert.AreEqual(item.Number1[j].ToString(), number1.Digits[^(j + 1)].ToString());

			for (int j = item.Number2.Length - 1; j >= 0; --j)
				Assert.AreEqual(item.Number2[j].ToString(), number2.Digits[^(j + 1)].ToString());
		}
	}

	[TestMethod]
	public void DigitConstructor() // Leading zero is not tested
	{
		Digit[] nullArray = null!;
		Assert.ThrowsException<ArgumentException>(() => new Natural(nullArray));
		Assert.ThrowsException<ArgumentException>(() => new Natural([]));

		Digit[] digits1, digits2;
		Natural number1, number2;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			digits1 = new Digit[item.Number1.Length];
			for (int j = item.Number1.Length - 1; j >= 0; --j)
				digits1[^(j + 1)] = new Digit(item.Number1[j]);

			digits2 = new Digit[item.Number2.Length];
			for (int j = item.Number2.Length - 1; j >= 0; --j)
				digits2[^(j + 1)] = new Digit(item.Number2[j]);

			number1 = new(digits1);
			number2 = new(digits2);

			for (int j = item.Number1.Length - 1; j >= 0; --j)
				Assert.AreEqual(item.Number1[j].ToString(), number1.Digits[^(j + 1)].ToString());

			for (int j = item.Number2.Length - 1; j >= 0; --j)
				Assert.AreEqual(item.Number2[j].ToString(), number2.Digits[^(j + 1)].ToString());
		}
	}

	[TestMethod]
	public void ToStringMethod()
	{
		Natural number1, number2;

		foreach (NaturalTestCase item in NaturalTestCases.List)
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
		Digit[] digits1, digits2;
		Natural numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			digits1 = new Digit[item.Number1.Length];
			for (int j = item.Number1.Length - 1; j >= 0; --j)
				digits1[j] = new Digit(item.Number1[^(j + 1)]);

			numberCharacters1 = new(item.Number1);
			numberDigits1 = new(digits1);

			digits2 = new Digit[item.Number2.Length];
			for (int j = item.Number2.Length - 1; j >= 0; --j)
				digits2[j] = new Digit(item.Number2[^(j + 1)]);

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new(digits2);

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
		Natural numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			digits1 = new Digit[item.Number1.Length];
			for (int j = item.Number1.Length - 1; j >= 0; --j)
				digits1[j] = new Digit(item.Number1[^(j + 1)]);

			numberCharacters1 = new(item.Number1);
			numberDigits1 = new(item.Number1);

			digits2 = new Digit[item.Number2.Length];
			for (int j = item.Number2.Length - 1; j >= 0; --j)
				digits2[j] = new Digit(item.Number2[^(j + 1)]);

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new(item.Number2);

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Greater, Natural.GreaterThan(numberCharacters1, numberCharacters2));
			Assert.AreEqual(item.Greater, Natural.GreaterThan(numberDigits1, numberDigits2));
			Assert.AreEqual(item.Greater, Natural.GreaterThan(numberCharacters1, numberDigits2));
			Assert.AreEqual(item.Greater, Natural.GreaterThan(numberDigits1, numberCharacters2));
		}
	}

	[TestMethod]
	public void AddMethod()
	{
		Natural natural1, natural2;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			natural1 = new(item.Number1);
			natural2 = new(item.Number2);

			Assert.AreEqual(item.Add, Natural.Add(natural1, natural2).ToString());
		}
	}

	[TestMethod]
	public void SubtractMethod()
	{
		bool swap;
		Natural natural1, natural2, subNum;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			natural1 = new(item.Number1);
			natural2 = new(item.Number2);

			(swap, subNum) = Natural.Subtract(natural1, natural2);

			Assert.AreEqual(item.SubSwap, swap);
			Assert.AreEqual(item.SubNum, subNum.ToString());
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Natural natural1, natural2;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			natural1 = new(item.Number1);
			natural2 = new(item.Number2);

			Assert.AreEqual(item.Mul, Natural.Multiply(natural1, natural2).ToString());
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		Natural natural1, natural2, whole, remainder;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			natural1 = new(item.Number1);
			natural2 = new(item.Number2);

			if (item.Div == "ERROR")
				Assert.ThrowsException<DivideByZeroException>(() => Natural.Divide(natural1, natural2));
			else if (item.Div != "BIG")
			{
				(whole, remainder) = Natural.Divide(natural1, natural2);

				Assert.AreEqual(item.Div, whole.ToString());
				Assert.AreEqual((new Natural(item.Number1)).ToString(), ((whole * item.Number2) + remainder).ToString());
			}
		}
	}

	[TestMethod]
	public void PowerMethod()
	{
		Natural natural1, natural2;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			natural1 = new(item.Number1);
			natural2 = new(item.Number2);

			if (item.Pow == "ERROR")
				Assert.ThrowsException<NotImplementedException>(() => Natural.Power(natural1, natural2));
			else if (item.Pow != "BIG")
				Assert.AreEqual(item.Pow, Natural.Power(natural1, natural2).ToString());
		}
	}

	[TestMethod]
	public void RootMethod()
	{
		Natural natural1, natural2, whole, remainder;

		foreach (NaturalTestCase item in NaturalTestCases.List)
		{
			natural1 = new(item.Number1);
			natural2 = new(item.Number2);

			if (item.Root == "ERROR")
				Assert.ThrowsException<NotImplementedException>(() => Natural.Root(natural1, natural2));
			else if (item.Root != "BIG")
			{
				(whole, remainder) = Natural.Root(natural1, natural2);

				Assert.AreEqual(item.Root, whole.ToString());
				Assert.AreEqual((new Natural(item.Number1)).ToString(), ((whole ^ item.Number2) + remainder).ToString());
			}
		}
	}
}