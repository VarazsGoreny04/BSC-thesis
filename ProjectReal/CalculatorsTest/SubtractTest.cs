using Calculators;
using ProjectReal.Number;
using ProjectRealTest;

namespace CalculatorsTest;

[TestClass]
public class SubtractTest
{
	[TestMethod]
	public void EmptyConstructor()
	{
		Subtract<Rational> sub = new();

		Assert.AreEqual(Rational.AdditiveIdentity, sub.Left.GetValue());
		Assert.IsNull(sub.Right);

		Assert.ThrowsException<NullReferenceException>(sub.GetValue);
	}

	[TestMethod]
	public void RightConstructor()
	{
		Rational rational1, rational2;
		Number<Rational> number1, number2;
		Subtract<Rational> sub1, sub2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			sub1 = new(number1);
			sub2 = new(number2);

			Assert.AreEqual(Rational.AdditiveIdentity, sub1.Left.GetValue());
			Assert.AreEqual(rational1, sub1.Right.GetValue());

			Assert.AreEqual(Rational.AdditiveIdentity, sub2.Left.GetValue());
			Assert.AreEqual(rational2, sub2.Right.GetValue());
		}
	}

	[TestMethod]
	public void LeftAndRightConstructor()
	{
		Rational rational1, rational2;
		Number<Rational> number1, number2;
		Subtract<Rational> sub;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			sub = new(number1, number2);

			Assert.AreEqual(rational1, sub.Left.GetValue());
			Assert.AreEqual(rational2, sub.Right.GetValue());
		}
	}

	[TestMethod]
	public void GetValueMethod()
	{
		Rational first = "0";
		Rational second = "5";

		Rational rational1, rational2, expected;
		Number<Rational> number1, number2;
		Subtract<Rational> sub, sub1, sub2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			sub = new(number1, number2);

			expected = item.Sub;

			Assert.AreEqual(expected, sub.GetValue());
		}

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			sub1 = new(number1);
			sub2 = new(number2);

			Assert.AreEqual(-rational1, sub1.GetValue());
			Assert.AreEqual(-rational2, sub2.GetValue());

			sub1.Left = new Number<Rational>(first);
			sub2.Left = new Number<Rational>(second);

			Assert.AreEqual(first - rational1, sub1.GetValue());
			Assert.AreEqual(second - rational2, sub2.GetValue());
		}
	}
}