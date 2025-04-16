using Project_Real;
using System;

namespace Test;

[TestClass]
public class PositiveTest
{
	[TestMethod]
	public void ZeroConstructor()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		Positive empty = new();

		Positive[] zeros =
		[
			new("0"),
			new("00"),
			new("0."),
			new("00."),
			new("0.0"),
			new("0.00"),
			new("00.0"),
			new("00.00"),
			new(new([Digit.ZERO]), 0),
			new(new([Digit.ZERO, Digit.ZERO]), 0),
			new(new([Digit.ZERO, Digit.ZERO]), 1),
			new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2),
			new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 1),
			new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2),
		];

		foreach (Positive zero in zeros)
			Assert.AreEqual(empty, zero);

		Positive.Separator = separator;
	}

	[TestMethod]
	public void StringConstructor()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		string nullString = null!;
		Assert.ThrowsException<ArgumentException>(() => new Positive(nullString));
		Assert.ThrowsException<ArgumentException>(() => new Positive(""));
		Assert.ThrowsException<ArgumentException>(() => new Positive("."));

		string[] tests = ["a123", "123a", "12a3"];

		for (int j = 0; j < tests.Length; ++j)
			Assert.ThrowsException<ArgumentException>(() => new Positive(tests[j]));

		for (int i = tests[0].Length; i > 0; --i)
		{
			for (int j = 0; j < tests.Length; ++j)
				Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(tests[j].Insert(i, ".")); });
		}

		string characters1, characters2;
		string[] tokens;
		Positive number1, number2;

		foreach (var item in PositiveTestCases.List)
		{
			characters1 = (item.Number1.Contains('.') ? item.Number1.TrimEnd('0') : item.Number1).Replace(".", "");
			characters2 = (item.Number2.Contains('.') ? item.Number2.TrimEnd('0') : item.Number2).Replace(".", "");

			number1 = new(item.Number1);
			number2 = new(item.Number2);

			for (int j = 0; j < number1.Digits.Length; ++j)
				Assert.AreEqual(characters1[^(j + 1)].ToString(), number1.Digits[j].ToString());
			tokens = item.Number1.TrimEnd('0').Split('.', StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual((tokens.Length == 2 ? tokens[1].Length : 0), number1.FractionLength);

			for (int j = 0; j < number2.Digits.Length; ++j)
				Assert.AreEqual(characters2[^(j + 1)].ToString(), number2.Digits[j].ToString());
			tokens = item.Number2.TrimEnd('0').Split('.', StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual((tokens.Length == 2 ? tokens[1].Length : 0), number2.FractionLength);

			number1 = new($"{item.Number1}{(item.Number1.Contains('.') ? "" : ".")}000");
			number2 = new($"00{item.Number2}{(item.Number2.Contains('.') ? "" : ".")}00");

			for (int j = 0; j < number1.Digits.Length; ++j)
				Assert.AreEqual(characters1[^(j + 1)].ToString(), number1.Digits[j].ToString());
			tokens = item.Number1.TrimEnd('0').Split('.', StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual((tokens.Length == 2 ? tokens[1].Length : 0), number1.FractionLength);

			for (int j = 0; j < number2.Digits.Length; ++j)
				Assert.AreEqual(characters2[^(j + 1)].ToString(), number2.Digits[j].ToString());
			tokens = item.Number2.TrimEnd('0').Split('.', StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual((tokens.Length == 2 ? tokens[1].Length : 0), number2.FractionLength);
		}

		Positive.Separator = separator;
	}

	[TestMethod]
	public void NaturalConstructor()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		Assert.ThrowsException<ArgumentException>(() => new Positive(new Natural(), -1));
		Assert.ThrowsException<ArgumentException>(() => new Positive(new Natural(), -2));

		int temp, fractionLength1, fractionLength2;
		string characters1, characters2;
		Natural natural1, natural2;
		Positive number1, number2;

		foreach (var item in PositiveTestCases.List)
		{
			characters1 = item.Number1.Contains('.') ? item.Number1.TrimEnd('0') : item.Number1;
			characters2 = item.Number2.Contains('.') ? item.Number2.TrimEnd('0') : item.Number2;

			temp = characters1.IndexOf('.');
			fractionLength1 = temp < 0 ? 0 : characters1.Length - (temp + 1);
			temp = characters2.IndexOf('.');
			fractionLength2 = temp < 0 ? 0 : characters2.Length - (temp + 1);

			characters1 = characters1.Replace(".", "");
			characters2 = characters2.Replace(".", "");

			natural1 = new(characters1);
			natural2 = new(characters2);

			number1 = new(natural1, fractionLength1);
			number2 = new(natural2, fractionLength2);

			for (int j = 0; j < number1.Digits.Length; ++j)
				Assert.AreEqual(characters1[^(j + 1)].ToString(), number1.Digits[j].ToString());
			Assert.AreEqual(fractionLength1, number1.FractionLength);

			for (int j = 0; j < number2.Digits.Length; ++j)
				Assert.AreEqual(characters2[^(j + 1)].ToString(), number2.Digits[j].ToString());
			Assert.AreEqual(fractionLength2, number2.FractionLength);

			natural1 = new($"{characters1}000");
			natural2 = new($"00{characters2}00");

			number1 = new(natural1, fractionLength1 + 3);
			number2 = new(natural2, fractionLength2 + 2);

			for (int j = 0; j < number1.Digits.Length; ++j)
				Assert.AreEqual(characters1[^(j + 1)].ToString(), number1.Digits[j].ToString());
			Assert.AreEqual(fractionLength1, number1.FractionLength);

			for (int j = 0; j < number2.Digits.Length; ++j)
				Assert.AreEqual(characters2[^(j + 1)].ToString(), number2.Digits[j].ToString());
			Assert.AreEqual(fractionLength2, number2.FractionLength);
		}

		Positive.Separator = separator;
	}

	[TestMethod]
	public void ToStringMethod()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		Positive number1, number2;

		foreach (var item in NaturalTestCases.List)
		{
			number1 = new(item.Number1);
			number2 = new(item.Number2);

			Assert.AreEqual(item.Number1, number1.ToString());
			Assert.AreEqual(item.Number2, number2.ToString());
		}

		Positive.Separator = separator;
	}

	[TestMethod]
	public void EqualsMethod()
	{
		int temp, fractionLength1, fractionLength2;
		string characters1, characters2;
		Digit[] digits1, digits2;
		Positive numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (var item in PositiveTestCases.List)
		{
			characters1 = item.Number1.Contains('.') ? item.Number1.TrimEnd('0') : item.Number1;
			characters2 = item.Number2.Contains('.') ? item.Number2.TrimEnd('0') : item.Number2;

			temp = characters1.IndexOf('.');
			fractionLength1 = temp < 0 ? 0 : characters1.Length - (temp + 1);
			temp = characters2.IndexOf('.');
			fractionLength2 = temp < 0 ? 0 : characters2.Length - (temp + 1);

			characters1 = characters1.Replace(".", "");
			characters2 = characters2.Replace(".", "");

			digits1 = new Digit[characters1.Length];
			for (int j = characters1.Length - 1; j >= 0; --j)
				digits1[j] = new Digit(characters1[^(j + 1)]);

			numberCharacters1 = new(item.Number1);
			numberDigits1 = new(new Natural(digits1), fractionLength1);

			digits2 = new Digit[characters2.Length];
			for (int j = characters2.Length - 1; j >= 0; --j)
				digits2[j] = new Digit(characters2[^(j + 1)]);

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new(new Natural(digits2), fractionLength2);

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Equal, numberDigits1 == numberDigits2);
			Assert.AreEqual(numberDigits1 == numberDigits2, numberDigits2 == numberDigits1);
		}
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
		int temp, fractionLength1, fractionLength2;
		string characters1, characters2;
		Digit[] digits1, digits2;
		Positive numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (var item in PositiveTestCases.List)
		{
			characters1 = item.Number1.Contains('.') ? item.Number1.TrimEnd('0') : item.Number1;
			characters2 = item.Number2.Contains('.') ? item.Number2.TrimEnd('0') : item.Number2;

			temp = characters1.IndexOf('.');
			fractionLength1 = temp < 0 ? 0 : characters1.Length - (temp + 1);
			temp = characters2.IndexOf('.');
			fractionLength2 = temp < 0 ? 0 : characters2.Length - (temp + 1);

			characters1 = characters1.Replace(".", "");
			characters2 = characters2.Replace(".", "");

			digits1 = new Digit[characters1.Length];
			for (int j = characters1.Length - 1; j >= 0; --j)
				digits1[j] = new Digit(characters1[^(j + 1)]);

			numberCharacters1 = new(item.Number1);
			numberDigits1 = new(new Natural(digits1), fractionLength1);

			digits2 = new Digit[characters2.Length];
			for (int j = characters2.Length - 1; j >= 0; --j)
				digits2[j] = new Digit(characters2[^(j + 1)]);

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new(new Natural(digits2), fractionLength2);

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Greater, Positive.GreaterThan(numberCharacters1, numberCharacters2));
			Assert.AreEqual(item.Greater, Positive.GreaterThan(numberDigits1, numberDigits2));
			Assert.AreEqual(item.Greater, Positive.GreaterThan(numberCharacters1, numberDigits2));
			Assert.AreEqual(item.Greater, Positive.GreaterThan(numberDigits1, numberCharacters2));
		}
	}

	[TestMethod]
	public void AddMethod()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		Positive positive1, positive2;

		foreach (var item in PositiveTestCases.List)
		{
			positive1 = new(item.Number1);
			positive2 = new(item.Number2);

			Assert.AreEqual(item.Add, Positive.Add(positive1, positive2).ToString());
		}

		Positive.Separator = separator;
	}

	[TestMethod]
	public void SubstractMethod()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		bool swap;
		Positive positive1, positive2, subNum;

		foreach (var item in PositiveTestCases.List)
		{
			positive1 = new(item.Number1);
			positive2 = new(item.Number2);

			(swap, subNum) = Positive.Substract(positive1, positive2);

			Assert.AreEqual(item.SubSwap, swap);
			Assert.AreEqual(item.SubNum, subNum.ToString());
		}

		Positive.Separator = separator;
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		Positive positive1, positive2;

		foreach (var item in PositiveTestCases.List)
		{
			positive1 = new(item.Number1);
			positive2 = new(item.Number2);

			Assert.AreEqual(item.Mul, Positive.Multiply(positive1, positive2).ToString());
		}

		Positive.Separator = separator;
	}

	[TestMethod]
	public void DivideMethod()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		string[] tokens;
		Positive positive1, positive2, whole, remainder;

		foreach (var item in PositiveTestCases.List)
		{
			positive1 = new(item.Number1);
			positive2 = new(item.Number2);

			if (item.Div == "ERROR")
				Assert.ThrowsException<DivideByZeroException>(() => Positive.Divide(positive1, positive2));
			else if (item.Div != "BIG")
			{
				tokens = item.Div.Split('.', StringSplitOptions.RemoveEmptyEntries);
				(whole, remainder) = Positive.Divide(positive1, positive2);

				Assert.AreEqual(new Positive(tokens[0] +
					(tokens.Length == 2 ? $".{ [.. tokens[1][..Math.Min(tokens[1].Length, whole.FractionLength)]]}" : "")).ToString(),
					whole.ToString());
				Assert.AreEqual((new Positive(item.Number1)).ToString(), ((whole * item.Number2) + remainder).ToString());
			}
		}

		Positive.Separator = separator;
	}

	[TestMethod]
	public void PowerMethod()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';

		Positive positive1, positive2;

		foreach (var item in PositiveTestCases.List)
		{
			positive1 = new(item.Number1);
			positive2 = new(item.Number2);

			if (item.Pow == "ERROR")
				Assert.ThrowsException<NotImplementedException>(() => Positive.Power(positive1, positive2));
			else if (item.Pow != "BIG")
				Assert.AreEqual(item.Pow, Positive.Power(positive1, positive2).ToString());
		}

		Positive.Separator = separator;
	}

	[TestMethod]
	public void RootMethod()
	{
		char separator = Positive.Separator;
		Positive.Separator = '.';
		int fractionCalculatonLength = Positive.FractionCalculatonLength;
		Positive.FractionCalculatonLength = 10;

		string[] tokens;
		Positive positive1, positive2, whole, remainder;

		foreach (var item in PositiveTestCases.List)
		{
			positive1 = new(item.Number1);
			positive2 = new(item.Number2);

			if (item.Root == "ERROR")
				Assert.ThrowsException<NotImplementedException>(() => Positive.Root(positive1, positive2));
			else if (item.Root != "BIG")
			{
				tokens = item.Root.Split('.', StringSplitOptions.RemoveEmptyEntries);
				(whole, remainder) = Positive.Root(positive1, positive2);

				Assert.AreEqual(new Positive(tokens[0] +
					(tokens.Length == 2 ? $".{ [.. tokens[1][..Math.Min(tokens[1].Length, whole.FractionLength)]]}" : "")).ToString(),
					whole.ToString());
				Assert.AreEqual((new Positive(item.Number1)).ToString(), ((whole ^ item.Number2) + remainder).ToString());
			}
		}

		Positive.Separator = separator;
		Positive.FractionCalculatonLength = fractionCalculatonLength;
	}
}