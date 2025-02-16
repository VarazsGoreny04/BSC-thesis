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

		for (int i = 0; i < 1_000; ++i)
		{
			characters = i.ToString();
			number = new(characters);

			Assert.AreEqual('+' + characters, number.ToString());

			number = new('+' + characters);

			Assert.AreEqual('+' + characters, number.ToString());

			number = new('-' + characters);

			Assert.AreEqual('-' + characters, number.ToString());
		}
	}
}