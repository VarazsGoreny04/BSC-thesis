using System;
using System.Linq;
using ProjectReal.Number;

namespace ProjectRealTest;

[TestClass]
public class DigitTest
{
	private readonly byte[] binary = [.. Enumerable.Range(0, 16).Select(x => (byte)x)];

	private static char ToChar(int num) => Convert.ToChar('0' + num);

	[TestMethod]
	public void CharConstructor()
	{
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => { _ = Digit.Create(ToChar(-1)); });
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => { _ = Digit.Create(ToChar(10)); });

		for (int i = 0; i < 10; ++i)
		{
			byte digit = Digit.Create(ToChar(i));

			Assert.AreEqual(binary[i], digit);
		}
	}

	[TestMethod]
	public void ArrayConstructor()
	{
		int i = 0;

		for (; i < 10; ++i)
			Assert.AreEqual(binary[i], Digit.Create(binary[i]));

		for (; i < binary.Length; ++i)
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => { _ = Digit.Create(binary[i]); });

	}

	[TestMethod]
	public void ToStringMethod()
	{
		for (int i = 0; i < 10; ++i)
		{
			byte digit = Digit.Create(binary[i]);

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
			byte digit = Digit.Create(ToChar(i));

			foreach (byte element in Digit.CreateArray(10, digit))
				Assert.AreEqual(digit, element);
		}
	}

	[TestMethod]
	public void TrimEndMethod()
	{
		byte[] array = new byte[25];
		int valuableLength = 20;

		for (int i = valuableLength - 1; i >= 0; --i)
			array[i] = Digit.Create(ToChar(i % 9 + 1));

		for (int i = valuableLength; i < 25; ++i)
			array[i] = 0;

		array = Digit.TrimEnd(array);

		Assert.AreEqual(valuableLength, array.Length);

		foreach (byte digit in array)
			Assert.AreNotEqual(0, digit);


		array = Digit.CreateArray(10);
		array = Digit.TrimEnd(array);

		Assert.AreEqual(1, array.Length);
		Assert.AreEqual(0, array[0]);


		array = Digit.CreateArray(1);
		array[0] = 1;
		array = Digit.TrimEnd(array);

		Assert.AreEqual(1, array.Length);
		Assert.AreEqual(1, array[0]);


		array = Digit.TrimEnd([]);

		Assert.AreEqual(0, array.Length);
	}

	[TestMethod]
	public void EqualsMethod()
	{
		for (int i = 0; i < 10; ++i)
		{
			byte charDigit = Digit.Create(ToChar(i));
			byte charDigitPlusOne = Digit.Create(i > 8 ? '0' : ToChar(i + 1));
			byte arrayDigit = Digit.Create(binary[i]);
			byte arrayDigitPlusOne = Digit.Create(binary[(i + 1) % 10]);

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
			byte charDigit = Digit.Create(ToChar(i));
			byte charDigitPlusOne = Digit.Create(i > 8 ? '0' : ToChar(i + 1));
			byte arrayDigit = Digit.Create(binary[i]);
			byte arrayDigitPlusOne = Digit.Create(binary[(i + 1) % 10]);

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
		byte a, b, c;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = Digit.Create(ToChar(i));
				b = Digit.Create(ToChar(j));
				c = Digit.Create(ToChar((i + j) % 10));

				(bool overflow, byte digit) = Digit.Add(a, b);

				Assert.AreEqual((i + j) / 10 > 0, overflow);
				Assert.AreEqual(c, digit);
			}
		}
	}

	[TestMethod]
	public void AddOneMethod()
	{
		byte a, b;

		for (int i = 0; i < 10; ++i)
		{
			a = Digit.Create(ToChar(i));
			b = Digit.Create(ToChar((i + 1) % 10));

			(bool overflow, byte digit) = Digit.AddOne(a);

			Assert.AreEqual((i + 1) / 10 > 0, overflow);
			Assert.AreEqual(b, digit);
		}
	}

	[TestMethod]
	public void SubtractMethod()
	{
		byte a, b, c;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = Digit.Create(ToChar(i));
				b = Digit.Create(ToChar(j));
				c = Digit.Create(ToChar((i - j + 10) % 10));

				(bool borrow, byte digit) = Digit.Subtract(a, b);

				Assert.AreEqual((i - j + 10) / 10 == 0, borrow);
				Assert.AreEqual(c, digit);
			}
		}
	}

	[TestMethod]
	public void SubtractOneMethod()
	{
		byte a, b;

		for (int i = 0; i < 10; ++i)
		{
			a = Digit.Create(ToChar(i));
			b = Digit.Create(ToChar((i - 1 + 10) % 10));

			(bool borrow, byte digit) = Digit.SubtractOne(a);

			Assert.AreEqual((i - 1 + 10) / 10 == 0, borrow);
			Assert.AreEqual(b, digit);
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		byte a, b, c;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = Digit.Create(ToChar(i));
				b = Digit.Create(ToChar(j));
				c = Digit.Create(ToChar((i * j) % 10));

				(byte overflow, byte digit) = Digit.Multiply(a, b);

				Assert.AreEqual(Digit.Create(ToChar((i * j) / 10)), overflow);
				Assert.AreEqual(c, digit);
			}
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		byte a, b, c1, c2;

		for (int i = 0; i < 10; ++i)
		{
			for (int j = 0; j < 10; ++j)
			{
				a = Digit.Create(ToChar(i));
				b = Digit.Create(ToChar(j));

				if (j == 0)
					Assert.ThrowsException<DivideByZeroException>(() => { _ = Digit.Divide(a, b); });
				else
				{
					c1 = Digit.Create(ToChar(i / j));
					c2 = Digit.Create(ToChar(i % j));

					(byte whole, byte remainder) = Digit.Divide(a, b);

					Assert.AreEqual(c1, whole);
					Assert.AreEqual(c2, remainder);
				}
			}
		}
	}
}