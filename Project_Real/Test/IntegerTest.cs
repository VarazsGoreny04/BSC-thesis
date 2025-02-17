using Project_Real;

namespace Test;

[TestClass]
public class IntegerTest
{
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
		Assert.ThrowsException<ArgumentException>(() => { _ = new Integer(nullString); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Integer(""); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Integer("+"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Integer("-"); });

		string characters;
		Integer number;

		for (int i = 0; i < 1_000; ++i)
		{
			characters = i.ToString();
			number = new(characters);

			Assert.IsTrue(number.Sign);
			for (int j = 0; j < characters.Length; ++j)
				Assert.AreEqual(characters[j].ToString(), number.Digits[^(j + 1)].ToString());

			characters = '+' + i.ToString();
			number = new(characters);

			Assert.IsTrue(number.Sign);
			for (int j = 1; j < characters.Length; ++j)
				Assert.AreEqual(characters[j].ToString(), number.Digits[^j].ToString());
		}

		for (int i = 1; i < 1_000; ++i)
		{
			characters = '-' + i.ToString();
			number = new(characters);

			Assert.IsFalse(number.Sign);
			for (int j = 1; j < characters.Length; ++j)
				Assert.AreEqual(characters[j].ToString(), number.Digits[^j].ToString());
		}
	}

	[TestMethod]
	public void NaturalConstructor()
	{
		Random rnd = new();

		Natural natural = new("0");

		Integer numberPositive = new(true, natural);
		Integer numberNegative = new(false, natural);

		Assert.AreEqual(numberPositive, numberNegative);

		natural = new(rnd.Next(1, int.MaxValue).ToString());

		numberPositive = new(true, natural);
		numberNegative = new(false, natural);

		Assert.AreEqual(natural, numberPositive.Value);
		Assert.AreEqual(natural, numberNegative.Value);
	}

	[TestMethod]
	public void ToStringMethod()
	{
		string characters;
		Integer number;

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

		Assert.AreEqual("+0", new Integer("0"));
		Assert.AreEqual("+0", new Integer("+0"));
		Assert.AreEqual("+0", new Integer("-0"));
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
			for (int j = 0; j < characters1.Length; ++j)
				digits1[j] = new Digit(characters1[^(j + 1)]);

			digits2 = new Digit[characters2.Length];
			for (int j = 0; j < characters2.Length; ++j)
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
		int int1, int2, sum;
		int halfOfMax = (int.MaxValue / 2);
		Integer integer1, integer2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue) - halfOfMax;
			int2 = rnd.Next(int.MaxValue) - halfOfMax;

			integer1 = new(int1.ToString());
			integer2 = new(int2.ToString());

			sum = int1 + int2;

			Assert.AreEqual((sum < 0 ? sum.ToString() : '+' + sum.ToString()), Integer.Add(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void SubstractMethod()
	{
		Random rnd = new();
		int int1, int2, sum;
		int halfOfMax = (int.MaxValue / 2);
		Integer integer1, integer2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue) - halfOfMax;
			int2 = rnd.Next(int.MaxValue) - halfOfMax;

			integer1 = new(int1.ToString());
			integer2 = new(int2.ToString());

			sum = int1 - int2;

			Assert.AreEqual((sum < 0 ? sum.ToString() : '+' + sum.ToString()), Integer.Substract(integer1, integer2).ToString());
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Random rnd = new();
		int max = (int)Math.Sqrt(int.MaxValue);
		int int1, int2, sum;
		Integer integer1, integer2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(max * 2) - max;
			int2 = rnd.Next(max * 2) - max;

			integer1 = new(int1.ToString());
			integer2 = new(int2.ToString());

			sum = int1 * int2;

			Assert.AreEqual((sum < 0 ? sum.ToString() : '+' + sum.ToString()), Integer.Multiply(integer1, integer2).ToString());
		}
	}
}