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
	}

	[TestMethod]
	public void StringConstructor()
	{
		string nullString = null!;
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(nullString); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(""); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("."); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("a123"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("123a"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("12a3"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(".123"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(".a123"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(".123a"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive(".12a3"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("a.123"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("1.23a"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("1.2a3"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("a12.3"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("123.a"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("12a.3"); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("a123."); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("123a."); });
		Assert.ThrowsException<ArgumentException>(() => { _ = new Positive("12a3."); });

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

			Assert.AreEqual(characters.Split('.')[1].Length, number.FractionLength);

			characters = characters.Remove(fractionMarkerIndex, 1);

			for (int j = characters.Length - 1; j >= 0; --j)
				Assert.AreEqual(characters[j].ToString(), number.Digits[^(j + 1)].ToString());
		}
	}
}