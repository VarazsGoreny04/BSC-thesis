using System;
using Project_Real;

namespace Test;

[TestClass]
public class PositiveTest
{
	[TestMethod]
	public void ZeroConstructor()
	{
		Positive empty = new();

		Positive[] zeros =
		[
			new("0"),
			new("00"),
			new($"0{Positive.Separator}"),
			new($"00{Positive.Separator}"),
			new($"0{Positive.Separator}0"),
			new($"0{Positive.Separator}00"),
			new($"00{Positive.Separator}0"),
			new($"00{Positive.Separator}00"),
			new(new([Digit.ZERO]), 0),
			new(new([Digit.ZERO, Digit.ZERO]), 0),
			new(new([Digit.ZERO, Digit.ZERO]), 1),
			new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2),
			new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 1),
			new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2),
		];

		foreach (Positive zero in zeros)
			Assert.AreEqual(empty, zero);
	}

	[TestMethod]
	public void StringConstructor()
	{
		string nullString = null!;
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(nullString); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(""); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(Positive.Separator.ToString()); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive($"{Positive.Separator}123"); });

		string[] tests = ["a123", "123a", "12a3"];

		for (int j = 0; j < tests.Length; ++j)
			Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(tests[j]); });

		for (int i = tests[0].Length; i > 0; --i)
		{
			for (int j = 0; j < tests.Length; ++j)
				Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(tests[j].Insert(i, Positive.Separator.ToString())); });
		}

		Random rnd = new();
		int fractionMarkerIndex;
		string characters;
		Positive number;

		for (int i = 0; i < 1_000; ++i)
		{
			characters = i.ToString();
			number = new(characters);

			for (int j = characters.Length - 1; j >= 0; --j)
				Assert.AreEqual(characters[j].ToString(), number.Digits[^(j + 1)].ToString());

			fractionMarkerIndex = rnd.Next(1, characters.Length);
			characters = characters.Insert(fractionMarkerIndex, Positive.Separator.ToString());
			number = new(characters);
			characters = characters.TrimEnd('0');

			Assert.AreEqual(characters.Split(Positive.Separator)[1].Length, number.FractionLength);

			characters = characters.Remove(fractionMarkerIndex, 1);

			for (int j = characters.Length - 1; j >= 0; --j)
				Assert.AreEqual(characters[j].ToString(), number.Digits[^(j + 1)].ToString());
		}
	}

	/*[TestMethod]
	public void NaturalConstructor()
	{
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(new Natural(), -1); });

		char[] characters;
		Digit[] digits;
		Positive number;

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
		Positive number;

		for (int i = 1; i < 1_000; ++i)
		{
			characters = i.ToString();
			number = new(characters);

			Assert.AreEqual('+' + characters, number.ToString());

			number = new('+' + characters);

			Assert.AreEqual('+' + characters, number.ToString());

			number = new('-' + characters);

			Assert.AreEqual('-' + characters, number.ToString());
		}

		Assert.AreEqual("+0", new Positive("0"));
		Assert.AreEqual("+0", new Positive("+0"));
		Assert.AreEqual("+0", new Positive("-0"));
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
			for (int j = 0; j < characters.Length; ++j)
				digits[j] = new Digit(characters[^(j + 1)]);

			numberDigits = new(digits);
			numberCharacters = new(characters);

			Assert.AreEqual(new Positive(true, numberCharacters), new Positive(true, numberDigits));
			Assert.AreEqual(new Positive(false, numberCharacters), new Positive(false, numberDigits));
			Assert.AreNotEqual(new Positive(true, numberCharacters), new Positive(false, numberDigits));
			Assert.AreNotEqual(new Positive(false, numberCharacters), new Positive(true, numberDigits));

			numberDigits = new([.. digits, Digit.ZERO, Digit.ZERO]);
			numberCharacters = new(new string('0', rnd.Next(5)) + characters);

			Assert.AreEqual(new Positive(true, numberCharacters), new Positive(true, numberDigits));
			Assert.AreEqual(new Positive(false, numberCharacters), new Positive(false, numberDigits));
			Assert.AreNotEqual(new Positive(true, numberCharacters), new Positive(false, numberDigits));
			Assert.AreNotEqual(new Positive(false, numberCharacters), new Positive(true, numberDigits));

			int index = rnd.Next(digits.Length);
			digits[index] = Digit.Add(digits[index], '1').Digit;

			numberDigits = new([.. digits, Digit.ZERO, Digit.ZERO]);

			Assert.AreNotEqual(new Positive(true, numberCharacters), new Positive(true, numberDigits));
			Assert.AreNotEqual(new Positive(false, numberCharacters), new Positive(false, numberDigits));
			Assert.AreNotEqual(new Positive(true, numberCharacters), new Positive(false, numberDigits));
			Assert.AreNotEqual(new Positive(false, numberCharacters), new Positive(true, numberDigits));
		}
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
		Random rnd = new();
		bool expected;
		int number1, number2;
		string characters1, characters2;
		Digit[] digits1, digits2;
		Natural numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		for (int i = 0; i < 100; ++i)
		{
			number1 = rnd.Next(1, int.MaxValue - 1);
			number2 = rnd.Next(1, int.MaxValue - 1);

			characters1 = number1.ToString();
			characters2 = number2.ToString();

			digits1 = new Digit[characters1.Length];
			for (int j = 0; j < characters1.Length; ++j)
				digits1[j] = new Digit(characters1[^(j + 1)]);

			digits2 = new Digit[characters2.Length];
			for (int j = 0; j < characters2.Length; ++j)
				digits2[j] = new Digit(characters2[^(j + 1)]);

			numberCharacters1 = new(characters1);
			numberCharacters2 = new(characters2);
			numberDigits1 = new(digits1);
			numberDigits2 = new(digits2);

			expected = number1 > number2;

			Assert.AreEqual(expected, Positive.GreaterThan(new(true, numberCharacters1), new(true, numberCharacters2)));
			Assert.AreEqual(expected, Positive.GreaterThan(new(true, numberCharacters1), new(true, numberDigits2)));
			Assert.AreEqual(expected, Positive.GreaterThan(new(true, numberDigits1), new(true, numberDigits2)));
			Assert.AreEqual(expected, Positive.GreaterThan(new(true, numberDigits1), new(true, numberCharacters2)));

			expected = -number1 > -number2;

			Assert.AreEqual(expected, Positive.GreaterThan(new(false, numberCharacters1), new(false, numberCharacters2)));
			Assert.AreEqual(expected, Positive.GreaterThan(new(false, numberCharacters1), new(false, numberDigits2)));
			Assert.AreEqual(expected, Positive.GreaterThan(new(false, numberDigits1), new(false, numberDigits2)));
			Assert.AreEqual(expected, Positive.GreaterThan(new(false, numberDigits1), new(false, numberCharacters2)));

			Assert.AreEqual(true, Positive.GreaterThan(new(true, numberCharacters1), new(false, numberCharacters2)));
			Assert.AreEqual(true, Positive.GreaterThan(new(true, numberCharacters1), new(false, numberDigits2)));
			Assert.AreEqual(true, Positive.GreaterThan(new(true, numberDigits1), new(false, numberDigits2)));
			Assert.AreEqual(true, Positive.GreaterThan(new(true, numberDigits1), new(false, numberCharacters2)));

			Assert.AreEqual(false, Positive.GreaterThan(new(false, numberCharacters1), new(true, numberCharacters2)));
			Assert.AreEqual(false, Positive.GreaterThan(new(false, numberCharacters1), new(true, numberDigits2)));
			Assert.AreEqual(false, Positive.GreaterThan(new(false, numberDigits1), new(true, numberDigits2)));
			Assert.AreEqual(false, Positive.GreaterThan(new(false, numberDigits1), new(true, numberCharacters2)));
		}
	}*/

	/*[TestMethod]
	public void AddMethod()
	{
		Random rnd = new();
		int maxRandom = int.MaxValue / 10;
		int maxPower = maxRandom.ToString().Length + 2;
		float number1, number2;
		Positive positive1, positive2;

		string expected, actual;
		int commonLength;

		for (int i = 0; i < 100; ++i)
		{
			number1 = rnd.Next(maxRandom) / (float)Math.Pow(10, rnd.Next(maxPower));
			number2 = rnd.Next(maxRandom) / (float)Math.Pow(10, rnd.Next(maxPower));

			positive1 = new(number1.ToString());
			positive2 = new(number2.ToString());

			expected = (number1 + number2).ToString();
			actual = Positive.Add(positive1, positive2).ToString();
			commonLength = Math.Min(expected.Length, actual.Length) - 3;

			Assert.AreEqual(expected[..commonLength], actual[..commonLength]);
		}
	}

	[TestMethod]
	public void SubstractMethod()
	{
		Random rnd = new();
		int maxPower = int.MaxValue.ToString().Length + 2;
		float number1, number2;
		Positive positive1, positive2;

		string expected, actual;
		int commonLength;

		for (int i = 0; i < 100; ++i)
		{
			number1 = rnd.Next(int.MaxValue) / (float)Math.Pow(10, rnd.Next(maxPower));
			number2 = rnd.Next(int.MaxValue) / (float)Math.Pow(10, rnd.Next(maxPower));

			positive1 = new(number1.ToString());
			positive2 = new(number2.ToString());

			expected = Math.Abs(number1 - number2).ToString();
			actual = Positive.Substract(positive1, positive2).Value.ToString();
			commonLength = Math.Min(expected.Length, actual.Length) - 3;

			Assert.AreEqual(expected[..commonLength], actual[..commonLength]);
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Random rnd = new();
		int maxRandom = (int)Math.Sqrt(int.MaxValue);
		int maxPower = maxRandom.ToString().Length + 2;
		float number1, number2;
		Positive positive1, positive2;

		string expected, actual;
		int commonLength;

		for (int i = 0; i < 100; ++i)
		{
			number1 = rnd.Next(maxRandom) / (float)Math.Pow(10, rnd.Next(maxPower));
			number2 = rnd.Next(maxRandom) / (float)Math.Pow(10, rnd.Next(maxPower));

			positive1 = new(number1.ToString());
			positive2 = new(number2.ToString());

			expected = (number1 * number2).ToString();
			actual = Positive.Multiply(positive1, positive2).ToString();
			commonLength = Math.Min(expected.Length, actual.Length) - 3;

			Assert.AreEqual(expected[..commonLength], actual[..commonLength]);
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		Random rnd = new();
		int maxRandom = (int)Math.Sqrt(int.MaxValue);
		int maxPower = maxRandom.ToString().Length + 2;
		float number1, number2;
		Positive positive1, positive2;

		string expected, actual;
		int temp, commonLength;

		for (int i = 0; i < 100; ++i)
		{
			temp = rnd.Next(maxPower);
			number1 = rnd.Next(maxRandom) / (float)Math.Pow(10, temp);
			number2 = rnd.Next(1, maxRandom) / (float)Math.Pow(10, temp + rnd.Next(-2, 2));

			positive1 = new(number1.ToString());
			positive2 = new(number2.ToString());

			expected = (number1 / number2).ToString();
			actual = Positive.Divide(positive1, positive2).Value.ToString();
			commonLength = Math.Min(expected.Length, actual.Length) - 3;

			Assert.AreEqual(expected[..commonLength], actual[..commonLength]);
		}

		Assert.AreEqual("0", Positive.Divide("0", "2.718").Value);
		Assert.AreEqual("0", Positive.Divide("0", "3.1415").Value);
		Assert.ThrowsException<DivideByZeroException>(() => Positive.Divide("2.718", "0"));
		Assert.ThrowsException<DivideByZeroException>(() => Positive.Divide("3.1415", "0"));
	}

	[TestMethod]
	public void SecondPowerMethod()
	{
		Random rnd = new();
		int max = (int)Math.Sqrt(int.MaxValue);
		int number1;
		Positive positive1;

		for (int i = 0; i < 100; ++i)
		{
			number1 = rnd.Next(max) - max;

			positive1 = new(number1.ToString());

			Assert.AreEqual(Math.Pow(number1, 2).ToString(), Positive.SecondPower(positive1).ToString());
		}

		Assert.ThrowsException<NotImplementedException>(() => Positive.SecondPower("0"));
		Assert.ThrowsException<NotImplementedException>(() => Positive.SecondPower("-0"));
	}

	[TestMethod]
	public void PowerMethod()
	{
		Random rnd = new();
		int number1, number2;
		long done;
		Positive positive1, positive2;

		for (int i = 0; i < 100; ++i)
		{
			number1 = rnd.Next(1, 15);
			number2 = rnd.Next(1, 10);

			positive1 = new(number1.ToString());
			positive2 = new(number2.ToString());

			done = (long)Math.Pow(number1, number2);

			Assert.AreEqual(done.ToString(), Positive.Power(positive1, positive2).ToString());
		}

		Assert.ThrowsException<NotImplementedException>(() => Positive.Power("0", "0"));
		Assert.ThrowsException<NotImplementedException>(() => Positive.Power("-0", "-0"));
		Assert.ThrowsException<NotImplementedException>(() => Positive.Power("0", "-0"));
		Assert.ThrowsException<NotImplementedException>(() => Positive.Power("-0", "0"));
	}*/
}