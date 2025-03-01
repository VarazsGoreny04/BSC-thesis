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
}