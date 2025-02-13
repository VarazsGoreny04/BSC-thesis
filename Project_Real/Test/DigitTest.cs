using Project_Real;

namespace Test;

[TestClass]
public class DigitTest
{
	private readonly bool[][] binary =
	[
		[false, false, false, false],	// 0
		[true, false, false, false],	// 1
		[false, true, false, false],	// 2
		[true, true, false, false], 	// 3
		[false, false, true, false],	// 4
		[true, false, true, false], 	// 5
		[false, true, true, false], 	// 6
		[true, true, true, false],  	// 7
		[false, false, false, true],	// 8
		[true, false, false, true], 	// 9
		//[false, true, false, true]	// 10
	];

	private static char ToChar(int num)
	{
		return Convert.ToChar('0' + num);
	}

	[TestMethod]
	public void EmptyConstructor()
	{
		Digit digit = new();

		for (int j = 0; j < Digit.LENGTH; ++j)
			Assert.AreEqual(binary[0][j], digit[j]);

		Assert.AreEqual(Digit.ZERO, digit);
	}

	[TestMethod]
	public void BadCharConstructor()
	{
		Assert.ThrowsException<Digit.ValueOutOfRangeException>(() => { _ = new Digit(ToChar(-1)); });
		Assert.ThrowsException<Digit.ValueOutOfRangeException>(() => { _ = new Digit(Convert.ToChar('9' + 1)); });
	}

	[TestMethod]
	public void CharConstructor()
	{
		for (int i = 0; i < 10; ++i)
		{
			Digit digit = new(ToChar(i));

			for (int j = 0; j < Digit.LENGTH; ++j)
				Assert.AreEqual(binary[i][j], digit[j]);
		}
	}

	[TestMethod]
	public void ToStringMethod()
	{
		for (int i = 0; i < 10; ++i)
		{
			Digit digit = new(binary[i]);

			Assert.AreEqual(ToChar(i).ToString(), digit.ToString());
		}
	}

	[TestMethod]
	public void EqualsMethod()
	{
		for (int i = 0; i < 10; ++i)
		{
			Digit charDigit = new(ToChar(i));
			Digit charDigitPlusOne = new(i > 8 ? '0' : ToChar(i + 1));
			Digit arrayDigit = new(binary[i]);
			Digit arrayDigitPlusOne = new(binary[(i + 1) % 10]);

			Assert.IsTrue(Digit.Equals(charDigit, arrayDigit));
			Assert.IsTrue(Digit.Equals(arrayDigit, charDigit));
			Assert.IsTrue(Digit.Equals(charDigitPlusOne, arrayDigitPlusOne));
			Assert.IsTrue(Digit.Equals(arrayDigitPlusOne, charDigitPlusOne));
			Assert.IsFalse(Digit.Equals(arrayDigitPlusOne, charDigit));
			Assert.IsFalse(Digit.Equals(charDigit, arrayDigitPlusOne));
			Assert.IsFalse(Digit.Equals(charDigitPlusOne, arrayDigit));
			Assert.IsFalse(Digit.Equals(arrayDigit, charDigitPlusOne));
		}
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
		for (int i = 0; i < 9; ++i)
		{
			Digit charDigit = new(ToChar(i));
			Digit charDigitPlusOne = new(i > 8 ? '0' : ToChar(i + 1));
			Digit arrayDigit = new(binary[i]);
			Digit arrayDigitPlusOne = new(binary[(i + 1) % 10]);

			Assert.IsFalse(Digit.GreaterThan(charDigit, arrayDigit));
			Assert.IsFalse(Digit.GreaterThan(arrayDigit, charDigit));
			Assert.IsFalse(Digit.GreaterThan(charDigitPlusOne, arrayDigitPlusOne));
			Assert.IsFalse(Digit.GreaterThan(arrayDigitPlusOne, charDigitPlusOne));
			Assert.IsFalse(Digit.GreaterThan(charDigit, arrayDigitPlusOne));
			Assert.IsFalse(Digit.GreaterThan(arrayDigit, charDigitPlusOne));
			Assert.IsTrue(Digit.GreaterThan(arrayDigitPlusOne, arrayDigit));
			Assert.IsTrue(Digit.GreaterThan(arrayDigitPlusOne, charDigit));
			Assert.IsTrue(Digit.GreaterThan(charDigitPlusOne, arrayDigit));
			Assert.IsTrue(Digit.GreaterThan(charDigitPlusOne, charDigit));
		}
	}

	[TestMethod]
	public void AddMethod()
	{
		Digit a, b, c;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = new Digit(ToChar(i));
				b = new Digit(ToChar(j));
				c = new Digit(ToChar((i + j) % 10));

				Assert.AreEqual(c, Digit.Add(a, b).Digit);
			}
		}
	}

	[TestMethod]
	public void SubstractMethod()
	{
		Digit a, b, c;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = new Digit(ToChar(i));
				b = new Digit(ToChar(j));
				c = new Digit(ToChar((i - j + 10) % 10));

				Assert.AreEqual(c, Digit.Substract(a, b).Digit);
			}
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Digit a, b, c;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = new Digit(ToChar(i));
				b = new Digit(ToChar(j));
				c = new Digit(ToChar((i * j) % 10));

				Assert.AreEqual(c, Digit.Multiply(a, b).Digit);
			}
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		Digit a, b, c1, c2;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = new Digit(ToChar(i));
				b = new Digit(ToChar(j));

				if (j == 0)
					Assert.ThrowsException< DivideByZeroException> (() => { _ = Digit.Divide(a, b); });
				else
				{
					c1 = new Digit(ToChar(i / j));
					c2 = new Digit(ToChar(i % j));

					Assert.AreEqual(c1, Digit.Divide(a, b).Whole);
					Assert.AreEqual(c2, Digit.Divide(a, b).Remainder);
				}
			}
		}
	}
}