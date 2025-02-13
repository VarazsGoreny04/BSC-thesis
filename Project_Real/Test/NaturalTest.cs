using Project_Real;

namespace Test;

[TestClass]
public class NaturalTest
{
	[TestMethod]
	public void EmptyConstructor()
	{
		Natural zeroDigit = new([Digit.ZERO]);

		Natural empty = new();
		Natural zeroString = new("0");
		Natural zeroZeroString = new("00");
		Natural zeroZeroDigit = new([Digit.ZERO, Digit.ZERO]);

		Assert.AreEqual(zeroDigit, empty);
		Assert.AreEqual(zeroDigit, zeroString);
		Assert.AreEqual(zeroDigit, zeroZeroString);
		Assert.AreEqual(zeroDigit, zeroZeroDigit);
	}

	[TestMethod]
	public void BadCharConstructor()
	{
		string nullString = null!;

		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural(nullString); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Natural(""); });
	}

	[TestMethod]
	public void StringConstructor()
	{
		string characters;
		Natural number;

		for (int i = 0; i < 1000; ++i)
		{
			characters = i.ToString();
			number = new(characters);

			for (int j = 0; j < characters.Length; ++j)
				Assert.AreEqual(characters[j], number.Digits[^(j + 1)]);
		}
	}

	[TestMethod]
	public void DigitConstructor()
	{
		char[] characters;
		Digit[] digits;
		Natural number;

		for (int i = 0; i < 1000; ++i)
		{
			characters = [.. i.ToString()];

			digits = new Digit[characters.Length];
			for (int j = 0; j < characters.Length; ++j)
				digits[j] = new Digit(characters[j]);

			number = new(i.ToString());

			for (int j = 0; j < characters.Length; ++j)
				Assert.AreEqual(digits[j], number.Digits[^(j + 1)]);
		}
	}

	[TestMethod]
	public void ToStringMethod()
	{
		string integer;
		Natural number;

		for (int i = 0; i < 1000; ++i)
		{
			integer = i.ToString();
			number = new(integer);

			Assert.AreEqual(integer, number.ToString());
		}
	}

	[TestMethod]
	public void TrimMethod()
	{
		Random rnd = new();

		Assert.AreEqual(Natural.TrimEnd(new Natural(new string('0', rnd.Next(2, 100)))), new Natural("0"));

		char[] characters = new char[100];

		for (int i = 0; i < 10; ++i)
		{
			Array.Fill(characters, '0');

			for (int j = 10; j < 20; ++j)
				characters[j] = rnd.Next(2) == 1 ? '0' : rnd.Next(1, 10).ToString()[0];

			for (int j = 20; j < 100; ++j)
				characters[j] = rnd.Next(1, 10).ToString()[0];

			Natural number = new(new string(characters));
			number = Natural.TrimEnd(number);

			Assert.IsTrue(90 >= number.Length);
			Assert.IsTrue(80 <= number.Length);
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
		Natural numberDigits;
		Natural numberCharacters;

		for (int i = 0; i < 10; ++i)
		{
			characters = rnd.Next(int.MaxValue).ToString() + rnd.Next(int.MaxValue).ToString();

			digits = new Digit[characters.Length];
			for (int j = 0; j < characters.Length; ++j)
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
}