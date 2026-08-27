using Calculators;
using ProjectReal.Number;
using ProjectRealTest;

namespace CalculatorsTest;

[TestClass]
public class AddTest
{
	[TestMethod]
	public void EmptyConstructor()
	{
		Add<Rational> add = new();

		Assert.AreEqual(Rational.AdditiveIdentity, add.Left.GetValue());
		Assert.IsNull(add.Right);

		Assert.ThrowsException<NullReferenceException>(add.GetValue);
	}

	[TestMethod]
	public void RightConstructor()
	{
		Rational rational1, rational2;
		Number<Rational> number1, number2;
		Add<Rational> add1, add2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			add1 = new(number1);
			add2 = new(number2);

			Assert.AreEqual(Rational.AdditiveIdentity, add1.Left.GetValue());
			Assert.AreEqual(rational1, add1.Right.GetValue());

			Assert.AreEqual(Rational.AdditiveIdentity, add2.Left.GetValue());
			Assert.AreEqual(rational2, add2.Right.GetValue());
		}
	}

	[TestMethod]
	public void LeftAndRightConstructor()
	{
		Rational rational1, rational2;
		Number<Rational> number1, number2;
		Add<Rational> add;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			add = new(number1, number2);

			Assert.AreEqual(rational1, add.Left.GetValue());
			Assert.AreEqual(rational2, add.Right.GetValue());
		}
	}

	[TestMethod]
	public void GetValueMethod()
	{
		Rational first = "0";
		Rational second = "5";

		Rational rational1, rational2, expected;
		Number<Rational> number1, number2;
		Add<Rational> add1, add2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			add1 = new(number1, number2);
			add2 = new(number2, number1);

			expected = item.Add;

			Assert.AreEqual(expected, add1.GetValue());
			Assert.IsTrue(add2.GetValue() == add1.GetValue());
		}

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = item.Number1;
			rational2 = item.Number2;

			number1 = new(rational1);
			number2 = new(rational2);

			add1 = new(number1);
			add2 = new(number2);

			Assert.AreEqual(rational1, add1.GetValue());
			Assert.AreEqual(rational2, add2.GetValue());

			add1.Left = new Number<Rational>(first);
			add2.Left = new Number<Rational>(second);

			Assert.AreEqual(first + rational1, add1.GetValue());
			Assert.AreEqual(second + rational2, add2.GetValue());
		}
	}
}