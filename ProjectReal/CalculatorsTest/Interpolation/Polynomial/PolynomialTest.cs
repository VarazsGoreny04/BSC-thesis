using Calculators.Interpolation;
using ProjectReal.Number;

namespace CalculatorsTest.Interpolation.Polynomial;

[TestClass]
public class PolynomialTest
{
	[TestMethod]
	public void EvaluateTest()
	{
		Point2D<Rational> result, expected;

		foreach (TestPolynomial item in TestPolynomials.List)
		{
			for (int i = item.BasePoints.Length - 1; i >= 0; --i)
			{
				result = Polynomial<Rational>.Evaluate(item.Polynomial, item.BasePoints[i]);

				expected = new(item.BasePoints[i], item.Evaluations[i]);

				Assert.AreEqual(expected, result);
			}
		}
	}

	[TestMethod]
	public void EvaluateRangeTest()
	{
		Point2D<Rational>[] result, expected;

		foreach (TestPolynomial item in TestPolynomials.List)
		{
			result = Polynomial<Rational>.EvaluateRange(item.Polynomial, item.BasePoints);

			expected = new Point2D<Rational>[item.Evaluations.Length];
			for (int i = item.BasePoints.Length - 1; i >= 0; --i)
				expected[i] = new Point2D<Rational>(item.BasePoints[i], item.Evaluations[i]);

			for (int i = expected.Length - 1; i >= 0; --i)
				Assert.AreEqual(expected[i], result[i]);
		}
	}
}