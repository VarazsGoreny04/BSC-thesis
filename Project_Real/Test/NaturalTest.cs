using Project_Real;

namespace Test;

[TestClass]
public class NaturalTest
{
	[TestMethod]
	public void ZeroConstructor()
	{
		Natural empty = new();

		Natural[] zeros =
		[
			new("0"),
			new("00"),
			new([Digit.ZERO]),
			new([Digit.ZERO, Digit.ZERO])
		];

		foreach (Natural zero in zeros)
			Assert.AreEqual(empty, zero);
	}

	[TestMethod]
	public void StringConstructor()
	{
		string nullString = null!;
		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural(nullString); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural(""); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural("a123"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural("123a"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural("12a3"); });

		string characters;
		Natural number;

		for (int i = 0; i < 1_000; ++i)
		{
			characters = i.ToString();
			number = new(characters);

			for (int j = characters.Length - 1; j >= 0; --j)
				Assert.AreEqual(characters[j], number.Digits[^(j + 1)]);
		}
	}

	[TestMethod]
	public void DigitConstructor()
	{
		Digit[] nullArray = null!;
		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural(nullArray); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural([]); });

		char[] characters;
		Digit[] digits;
		Natural number;

		for (int i = 0; i < 1_000; ++i)
		{
			characters = [.. i.ToString()];

			digits = new Digit[characters.Length];
			for (int j = characters.Length - 1; j >= 0; --j)
				digits[^(j + 1)] = new Digit(characters[j]);

			number = new(digits);

			for (int j = characters.Length - 1; j >= 0; --j)
				Assert.AreEqual(characters[j].ToString(), number.Digits[^(j + 1)].ToString());
		}
	}

	[TestMethod]
	public void ToStringMethod()
	{
		string characters;
		Natural number;

		for (int i = 0; i < 1_000; ++i)
		{
			characters = i.ToString();
			number = new(characters);

			Assert.AreEqual(characters, number.ToString());
		}
	}

	[TestMethod]
	public void TrimMethod()
	{
		Random rnd = new();

		Assert.AreEqual(Natural.TrimEnd(new Natural(new string('0', rnd.Next(2, 100)))), new Natural("0"));

		char[] characters = new char[100];

		for (int i = 0; i < 100; ++i)
		{
			Array.Fill(characters, '0');

			for (int j = 10; j < 20; ++j)
				characters[j] = rnd.Next(2) == 1 ? '0' : rnd.Next(1, 10).ToString()[0];

			for (int j = 20; j < 100; ++j)
				characters[j] = rnd.Next(1, 10).ToString()[0];

			Natural number = new(new string(characters));
			number = Natural.TrimEnd(number);

			Assert.AreNotEqual(Digit.ZERO, number[0]);
			Assert.AreEqual(characters.Length - characters.TakeWhile(x => x == '0').Count(), number.Length);
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

			Assert.AreEqual(numberCharacters, numberDigits);

			numberDigits = new([.. digits, Digit.ZERO, Digit.ZERO]);
			numberCharacters = new(new string('0', rnd.Next(5)) + characters);

			Assert.AreEqual(numberCharacters, numberDigits);

			int index = rnd.Next(digits.Length);
			digits[index] = Digit.Add(digits[index], '1').Digit;

			numberDigits = new([.. digits, Digit.ZERO, Digit.ZERO]);

			Assert.AreNotEqual(numberCharacters, numberDigits);
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
			int1 = rnd.Next(int.MaxValue - 1);
			int2 = rnd.Next(int.MaxValue - 1);

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

			Assert.AreEqual(expected, Natural.GreaterThan(numberCharacters1, numberCharacters2));
			Assert.AreEqual(expected, Natural.GreaterThan(numberCharacters1, numberDigits2));
			Assert.AreEqual(expected, Natural.GreaterThan(numberDigits1, numberDigits2));
			Assert.AreEqual(expected, Natural.GreaterThan(numberDigits1, numberCharacters2));
		}
	}

	[TestMethod]
	public void AddMethod()
	{
		Random rnd = new();
		int int1, int2;
		Natural natural1, natural2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue / 2);
			int2 = rnd.Next(int.MaxValue / 2);

			natural1 = new(int1.ToString());
			natural2 = new(int2.ToString());

			Assert.AreEqual((int1 + int2).ToString(), Natural.Add(natural1, natural2).ToString());
		}
	}

	[TestMethod]
	public void SubstractMethod()
	{
		Random rnd = new();
		int int1, int2;
		Natural natural1, natural2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue / 2);
			int2 = rnd.Next(int.MaxValue / 2);

			natural1 = new(int1.ToString());
			natural2 = new(int2.ToString());

			Assert.AreEqual(Math.Abs(int1 - int2).ToString(), Natural.Substract(natural1, natural2).Value.ToString());
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Random rnd = new();
		int max = (int)Math.Sqrt(int.MaxValue);
		int int1, int2;
		Natural natural1, natural2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(max);
			int2 = rnd.Next(max);

			natural1 = new(int1.ToString());
			natural2 = new(int2.ToString());

			Assert.AreEqual((int1 * int2).ToString(), Natural.Multiply(natural1, natural2).ToString());
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		Random rnd = new();
		int int1, int2;
		Natural natural1, natural2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue);
			int2 = rnd.Next(1, int.MaxValue);

			natural1 = new(int1.ToString());
			natural2 = new(int2.ToString());

			(Natural whole, Natural remainder) = Natural.Divide(natural1, natural2);

			Assert.AreEqual((int1 / int2).ToString(), whole.ToString());
			Assert.AreEqual((int1 % int2).ToString(), remainder.ToString());
		}

		Assert.AreEqual("0", Natural.Divide("0", "1").Whole);
		Assert.ThrowsException<DivideByZeroException>(() => Natural.Divide("1", "0"));
	}

	[TestMethod]
	public void SecondPowerMethod()
	{
		Random rnd = new();
		int max = (int)Math.Sqrt(int.MaxValue);
		int int1;
		Natural natural1;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(1, max);

			natural1 = new(int1.ToString());

			Assert.AreEqual(Math.Pow(int1, 2).ToString(), Natural.SecondPower(natural1).ToString());
		}

		Assert.ThrowsException<NotImplementedException>(() => Natural.SecondPower("0"));
	}

	[TestMethod]
	public void PowerMethod()
	{
		Random rnd = new();
		int int1, int2;
		Natural natural1, natural2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(1, 15);
			int2 = rnd.Next(1, 10);

			natural1 = new(int1.ToString());
			natural2 = new(int2.ToString());

			Assert.AreEqual(Math.Pow(int1, int2).ToString(), Natural.Power(natural1, natural2).ToString());
		}

		Assert.ThrowsException<NotImplementedException>(() => Natural.Power("0", "0"));
	}
}