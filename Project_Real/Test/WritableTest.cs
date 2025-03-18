/*using Project_Real;

namespace Test;

[TestClass]
public class WritableTest
{
	private static string Format(bool sign, string number) => (Integer.WriteSign && sign ? '+' + number.ToString() : number.ToString());

	[TestMethod]
	public void ZeroConstructor()
	{
		Writable empty = new();

		Writable[] zeros =
		[
			new("0"),
			new("00"),
			new($"0{Positive.Separator}"),
			new($"00{Positive.Separator}"),
			new($"0{Positive.Separator}0"),
			new($"0{Positive.Separator}00"),
			new($"00{Positive.Separator}0"),
			new($"00{Positive.Separator}00"),
			new("+0"),
			new("+00"),
			new($"+0{Positive.Separator}"),
			new($"+00{Positive.Separator}"),
			new($"+0{Positive.Separator}0"),
			new($"+0{Positive.Separator}00"),
			new($"+00{Positive.Separator}0"),
			new($"+00{Positive.Separator}00"),
			new(true, new(new([Digit.ZERO]), 0)),
			new(true, new(new([Digit.ZERO, Digit.ZERO]), 0)),
			new(true, new(new([Digit.ZERO, Digit.ZERO]), 1)),
			new(true, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2)),
			new(true, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 1)),
			new(true, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2)),
			new("-0"),
			new("-00"),
			new($"-0{Positive.Separator}"),
			new($"-00{Positive.Separator}"),
			new($"-0{Positive.Separator}0"),
			new($"-0{Positive.Separator}00"),
			new($"-00{Positive.Separator}0"),
			new($"-00{Positive.Separator}00"),
			new(false, new(new([Digit.ZERO]), 0)),
			new(false, new(new([Digit.ZERO, Digit.ZERO]), 0)),
			new(false, new(new([Digit.ZERO, Digit.ZERO]), 1)),
			new(false, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2)),
			new(false, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 1)),
			new(false, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2)),
		];

		foreach (Writable zero in zeros)
			Assert.AreEqual(empty, zero);
	}

	[TestMethod]
	public void StringConstructor()
	{
		string nullString = null!;
		Assert.ThrowsException<ArgumentException>(() => { _ = new Writable(nullString); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Writable(""); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Writable("+"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Writable("-"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(Positive.Separator.ToString()); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive($"{Positive.Separator}123"); });

		string[] tests = ["a123", "123a", "12a3"];

		for (int j = 0; j < tests.Length; ++j)
			Assert.ThrowsException<ArgumentException>(() => { _ = new Writable(tests[j]); });

		for (int i = tests[0].Length; i > 0; --i)
		{
			for (int j = 0; j < tests.Length; ++j)
				Assert.ThrowsException<ArgumentException>(() => { _ = new Writable(tests[j].Insert(i, Positive.Separator.ToString())); });
		}

		string characters;
		Writable number;

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

		Writable numberPositive = new(true, natural);
		Writable numberNegative = new(false, natural);

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
		Writable number;

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

		Assert.AreEqual("+0", new Writable("0"));
		Assert.AreEqual("+0", new Writable("+0"));
		Assert.AreEqual("+0", new Writable("-0"));
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

			Assert.AreEqual(new Writable(true, numberCharacters), new Writable(true, numberDigits));
			Assert.AreEqual(new Writable(false, numberCharacters), new Writable(false, numberDigits));
			Assert.AreNotEqual(new Writable(true, numberCharacters), new Writable(false, numberDigits));
			Assert.AreNotEqual(new Writable(false, numberCharacters), new Writable(true, numberDigits));

			numberDigits = new([.. digits, Digit.ZERO, Digit.ZERO]);
			numberCharacters = new(new string('0', rnd.Next(5)) + characters);

			Assert.AreEqual(new Writable(true, numberCharacters), new Writable(true, numberDigits));
			Assert.AreEqual(new Writable(false, numberCharacters), new Writable(false, numberDigits));
			Assert.AreNotEqual(new Writable(true, numberCharacters), new Writable(false, numberDigits));
			Assert.AreNotEqual(new Writable(false, numberCharacters), new Writable(true, numberDigits));

			int index = rnd.Next(digits.Length);
			digits[index] = Digit.Add(digits[index], '1').Digit;

			numberDigits = new([.. digits, Digit.ZERO, Digit.ZERO]);

			Assert.AreNotEqual(new Writable(true, numberCharacters), new Writable(true, numberDigits));
			Assert.AreNotEqual(new Writable(false, numberCharacters), new Writable(false, numberDigits));
			Assert.AreNotEqual(new Writable(true, numberCharacters), new Writable(false, numberDigits));
			Assert.AreNotEqual(new Writable(false, numberCharacters), new Writable(true, numberDigits));
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

			Assert.AreEqual(expected, Writable.GreaterThan(new(true, numberCharacters1), new(true, numberCharacters2)));
			Assert.AreEqual(expected, Writable.GreaterThan(new(true, numberCharacters1), new(true, numberDigits2)));
			Assert.AreEqual(expected, Writable.GreaterThan(new(true, numberDigits1), new(true, numberDigits2)));
			Assert.AreEqual(expected, Writable.GreaterThan(new(true, numberDigits1), new(true, numberCharacters2)));

			expected = -int1 > -int2;

			Assert.AreEqual(expected, Writable.GreaterThan(new(false, numberCharacters1), new(false, numberCharacters2)));
			Assert.AreEqual(expected, Writable.GreaterThan(new(false, numberCharacters1), new(false, numberDigits2)));
			Assert.AreEqual(expected, Writable.GreaterThan(new(false, numberDigits1), new(false, numberDigits2)));
			Assert.AreEqual(expected, Writable.GreaterThan(new(false, numberDigits1), new(false, numberCharacters2)));

			Assert.AreEqual(true, Writable.GreaterThan(new(true, numberCharacters1), new(false, numberCharacters2)));
			Assert.AreEqual(true, Writable.GreaterThan(new(true, numberCharacters1), new(false, numberDigits2)));
			Assert.AreEqual(true, Writable.GreaterThan(new(true, numberDigits1), new(false, numberDigits2)));
			Assert.AreEqual(true, Writable.GreaterThan(new(true, numberDigits1), new(false, numberCharacters2)));

			Assert.AreEqual(false, Writable.GreaterThan(new(false, numberCharacters1), new(true, numberCharacters2)));
			Assert.AreEqual(false, Writable.GreaterThan(new(false, numberCharacters1), new(true, numberDigits2)));
			Assert.AreEqual(false, Writable.GreaterThan(new(false, numberDigits1), new(true, numberDigits2)));
			Assert.AreEqual(false, Writable.GreaterThan(new(false, numberDigits1), new(true, numberCharacters2)));
		}
	}

	[TestMethod]
	public void AddMethod()
	{
		Random rnd = new();
		int int1, int2, done;
		int halfOfMax = (int.MaxValue / 2);
		Writable Writable1, Writable2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue) - halfOfMax;
			int2 = rnd.Next(int.MaxValue) - halfOfMax;

			Writable1 = new(int1.ToString());
			Writable2 = new(int2.ToString());

			done = int1 + int2;

			Assert.AreEqual(Format(done >= 0, done.ToString()), Writable.Add(Writable1, Writable2).ToString());
		}
	}

	[TestMethod]
	public void SubstractMethod()
	{
		Random rnd = new();
		int int1, int2, done;
		int halfOfMax = (int.MaxValue / 2);
		Writable Writable1, Writable2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue) - halfOfMax;
			int2 = rnd.Next(int.MaxValue) - halfOfMax;

			Writable1 = new(int1.ToString());
			Writable2 = new(int2.ToString());

			done = int1 - int2;

			Assert.AreEqual(Format(done >= 0, done.ToString()), Writable.Substract(Writable1, Writable2).ToString());
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Random rnd = new();
		int max = (int)Math.Sqrt(int.MaxValue);
		int int1, int2, done;
		Writable Writable1, Writable2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(max * 2) - max;
			int2 = rnd.Next(max * 2) - max;

			Writable1 = new(int1.ToString());
			Writable2 = new(int2.ToString());

			done = int1 * int2;

			Assert.AreEqual(Format(done >= 0, done.ToString()), Writable.Multiply(Writable1, Writable2).ToString());
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		Random rnd = new();
		int int1, int2, done1, done2;
		Writable Writable1, Writable2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(int.MaxValue);
			int2 = rnd.Next(1, int.MaxValue);

			Writable1 = new(int1.ToString());
			Writable2 = new(int2.ToString());

			done1 = int1 / int2;
			done2 = int1 % int2;

			(Writable whole, Writable remainder) = Writable.Divide(Writable1, Writable2);

			Assert.AreEqual(Format(done1 >= 0, done1.ToString()), whole.ToString());
			Assert.AreEqual(Format(done2 >= 0, done2.ToString()), remainder.ToString());
		}

		Assert.AreEqual("0", Writable.Divide("0", "1").Whole);
		Assert.AreEqual("0", Writable.Divide("0", "-1").Whole);
		Assert.ThrowsException<DivideByZeroException>(() => Writable.Divide("1", "0"));
		Assert.ThrowsException<DivideByZeroException>(() => Writable.Divide("-1", "0"));
	}

	[TestMethod]
	public void SecondPowerMethod()
	{
		Random rnd = new();
		int max = (int)Math.Sqrt(int.MaxValue);
		int int1;
		Writable Writable1;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(max) - max;

			Writable1 = new(int1.ToString());

			Assert.AreEqual(Format(true, Math.Pow(int1, 2).ToString()), Writable.SecondPower(Writable1).ToString());
		}

		Assert.ThrowsException<NotImplementedException>(() => Writable.SecondPower("0"));
		Assert.ThrowsException<NotImplementedException>(() => Writable.SecondPower("-0"));
	}

	[TestMethod]
	public void PowerMethod()
	{
		Random rnd = new();
		int int1, int2;
		long done;
		Writable Writable1, Writable2;

		for (int i = 0; i < 100; ++i)
		{
			int1 = rnd.Next(1, 15);
			int2 = rnd.Next(1, 10);

			Writable1 = new(int1.ToString());
			Writable2 = new(int2.ToString());

			done = (long)Math.Pow(int1, int2);

			Assert.AreEqual(Format(done >= 0, done.ToString()), Writable.Power(Writable1, Writable2).ToString());
		}

		Assert.ThrowsException<NotImplementedException>(() => Writable.Power("0", "0"));
		Assert.ThrowsException<NotImplementedException>(() => Writable.Power("-0", "-0"));
		Assert.ThrowsException<NotImplementedException>(() => Writable.Power("0", "-0"));
		Assert.ThrowsException<NotImplementedException>(() => Writable.Power("-0", "0"));
	}
}*/