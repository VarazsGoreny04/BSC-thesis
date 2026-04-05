using System;
using System.Linq;
using ProjectReal.Number;

namespace Test;

[TestClass]
public class DigitTest
{
	private readonly byte[] binary = [.. Enumerable.Range(0, 16).Select(x => (byte)x)];

	private static char ToChar(int num) => Convert.ToChar('0' + num);

	[TestMethod]
	public void ZeroConstructor()
	{
		Digit digit = new();

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

			Assert.AreEqual(binary[i], digit);
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
		array[0] = Digit.ONE;
		array = Digit.TrimEnd(array);

		Assert.AreEqual(1, array.Length);
		Assert.AreEqual(Digit.ONE, array[0]);


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
	public void SubtractMethod()
	{
		Digit a, b, c;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = new Digit(ToChar(i));
				b = new Digit(ToChar(j));
				c = new Digit(ToChar((i - j + 10) % 10));

				(bool borrow, Digit digit) = Digit.Subtract(a, b);

				Assert.AreEqual(((i - j + 10) / 10) == 0, borrow);
				Assert.AreEqual(c, digit);
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

				(Digit overflow, Digit digit) = Digit.Multiply(a, b);

				Assert.AreEqual(new Digit(ToChar((i * j) / 10)), overflow);
				Assert.AreEqual(c, digit);
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

					(Digit whole, Digit remainder) = Digit.Divide(a, b);

					Assert.AreEqual(c1, whole);
					Assert.AreEqual(c2, remainder);
				}
			}
		}
	}
}