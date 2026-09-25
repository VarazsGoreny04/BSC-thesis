using Calculators;
using ProjectReal.Number;
using ProjectRealTest;
using System;

namespace CalculatorsTest;

[TestClass]
public class MultiplyTest
{
	[TestMethod]
	public void EmptyConstructor()
	{
		{
			Multiply<Rational> mul = new();

			Assert.IsNull(mul.Left);
			Assert.IsNull(mul.Right);

			Assert.ThrowsException<NullReferenceException>(mul.GetValue);
		}

		{
			Multiply<Rational> mul1 = new()
			{
				Left = new Number<Rational>("1")
			};

			Assert.ThrowsException<NullReferenceException>(mul1.GetValue);
		}

		{
			Multiply<Rational> mul2 = new()
			{
				Right = new Number<Rational>("1")
			};

			Assert.ThrowsException<NullReferenceException>(mul2.GetValue);
		}

		{
			Multiply<Rational> mul3 = new()
			{
				Left = new Number<Rational>("1"),
				Right = new Number<Rational>("1")
			};

			mul3.GetValue();
		}
	}

	[TestMethod]
	public void LeftAndRightConstructor()
	{
		Rational rational1, rational2;
		Number<Rational> number1, number2;
		Multiply<Rational> mul;

		foreach (NumberTestCase item in NumberTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			mul = new(number1, number2);

			Assert.AreEqual(rational1, mul.Left.GetValue());
			Assert.AreEqual(rational2, mul.Right.GetValue());
		}
	}

	[TestMethod]
	public void GetValueMethod()
	{
		Rational rational1, rational2, expected;
		Number<Rational> number1, number2;
		Multiply<Rational> mul1, mul2;

		foreach (NumberTestCase item in NumberTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			mul1 = new(number1, number2);
			mul2 = new(number2, number1);

			expected = item.Mul;

			Assert.AreEqual(expected, mul1.GetValue());
			Assert.IsTrue(mul2.GetValue() == mul1.GetValue());
		}
	}
}