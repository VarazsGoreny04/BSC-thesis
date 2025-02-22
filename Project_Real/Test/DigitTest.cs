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
		[false, true, false, true], 	// 10
		[true, true, false, true],  	// 11
		[false, false, true, true], 	// 12
		[true, false, true, true],  	// 13
		[false, true, true, true],  	// 14
		[true, true, true, true],   	// 15
	];

	private static char ToChar(int num)
	{
		return Convert.ToChar('0' + num);
	}

	[TestMethod]
	public void ZeroConstructor()
	{
		Digit digit = new();

		for (int j = Digit.LENGTH - 1; j >= 0; --j)
			Assert.AreEqual(binary[0][j], digit[j]);

		Assert.AreEqual(Digit.ZERO, digit);
	}

	[TestMethod]
	public void CharConstructor()
	{
		Assert.ThrowsException<Digit.ValueOutOfRangeException>(() => { _ = new Digit(ToChar(-1)); });
		Assert.ThrowsException<Digit.ValueOutOfRangeException>(() => { _ = new Digit(ToChar(10)); });

		for (int i = 0; i < 10; ++i)
		{
			Digit digit = new(ToChar(i));

			for (int j = Digit.LENGTH - 1; j >= 0; --j)
				Assert.AreEqual(binary[i][j], digit[j]);
		}
	}

	[TestMethod]
	public void ArrayConstructor()
	{
		int i = 0;

		for (; i < 10; ++i)
			Assert.AreEqual(binary[i], (new Digit(binary[i])).Bits);

		for (; i < binary.Length; ++i)
			Assert.ThrowsException<Digit.ValueOutOfRangeException>(() => { _ = new Digit(binary[i]); });

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
	public void CreateArrayMethod()
	{
		for (int i = -9; i < 10; i++)
			Assert.AreEqual((i < 1 ? 0 : i), Digit.CreateArray(i).Length);

		for (int i = 0; i < 10; ++i)
		{
			Digit digit = new(ToChar(i));

			foreach (Digit element in Digit.CreateArray(10, digit))
				Assert.AreEqual(digit, element);
		}
	}

	[TestMethod]
	public void TrimEndMethod()
	{
		Random rnd = new();
		Digit[] array = new Digit[25];
		int valuableLength = rnd.Next(1, 21);

		for (int i = valuableLength - 1; i >= 0; --i)
			array[i] = new Digit(ToChar(rnd.Next(1, 10)));

		for (int i = valuableLength; i < 25; ++i)
			array[i] = Digit.ZERO;

		array = Digit.TrimEnd(array);

		Assert.AreEqual(valuableLength, array.Length);
		
		foreach (Digit digit in array)
			Assert.AreNotEqual(digit, Digit.ZERO);


		array = Digit.CreateArray(rnd.Next(1, 10), Digit.ZERO);
		array = Digit.TrimEnd(array);

		Assert.AreEqual(1, array.Length);
		Assert.AreEqual(Digit.ZERO, array[0]);


		array = Digit.CreateArray(rnd.Next(1, 10), Digit.ZERO);
		array[0] = new Digit('1');
		array = Digit.TrimEnd(array);

		Assert.AreEqual(1, array.Length);
		Assert.AreEqual(new Digit('1'), array[0]);


		array = Digit.TrimEnd([]);

		Assert.AreEqual(0, array.Length);
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

			Assert.IsTrue(charDigit.Equals(arrayDigit));
			Assert.IsTrue(arrayDigit.Equals(charDigit));
			Assert.IsTrue(charDigitPlusOne.Equals(arrayDigitPlusOne));
			Assert.IsTrue(arrayDigitPlusOne.Equals(charDigitPlusOne));
			Assert.IsFalse(arrayDigitPlusOne.Equals(charDigit));
			Assert.IsFalse(charDigit.Equals(arrayDigitPlusOne));
			Assert.IsFalse(charDigitPlusOne.Equals(arrayDigit));
			Assert.IsFalse(arrayDigit.Equals(charDigitPlusOne));

			Assert.IsFalse(charDigit.Equals(new object()));
			Assert.IsFalse(arrayDigit.Equals(new object()));
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
					Assert.ThrowsException<DivideByZeroException>(() => { _ = Digit.Divide(a, b); });
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