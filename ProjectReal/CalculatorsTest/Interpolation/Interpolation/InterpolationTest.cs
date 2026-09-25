using Calculators.Interpolation;
using ProjectReal.Number;
using System;

namespace CalculatorsTest.Interpolation.Interpolation;

[TestClass]
public class InterpolationTest
{
	[TestMethod]
	public void CheckBasesMethod()
	{
		foreach (Point2D<Rational>[] points in InterpolationTestCases.MatchingXCoordinatesList)
			Assert.ThrowsException<ArgumentException>(() => { Interpolation<Rational>.CheckBases(points); });

		foreach (InterpolationTestCase item in InterpolationTestCases.List)
			Interpolation<Rational>.CheckBases(item.Points);
	}

	[TestMethod]
	public void LagrangeBasisMethod()
	{
		foreach (Point2D<Rational>[] points in InterpolationTestCases.MatchingXCoordinatesList)
		{
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => { Interpolation<Rational>.LagrangeBasis(points, -1); });
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => { Interpolation<Rational>.LagrangeBasis(points, points.Length); });
		}

		Rational[] result;

		foreach (InterpolationTestCase item in InterpolationTestCases.List)
		{
			for (int i = item.Points.Length - 1; i >= 0; --i)
			{
				result = Interpolation<Rational>.LagrangeBasis(item.Points, i);

				Assert.AreEqual(item.LBasisPolynomials[i].Length, result.Length);

				for (int j = item.LBasisPolynomials[i].Length - 1; j >= 0; --j)
					Assert.AreEqual(item.LBasisPolynomials[i][j], result[j]);
			}
		}
	}

	[TestMethod]
	public void LagrangeMethod()
	{
		foreach (Point2D<Rational>[] points in InterpolationTestCases.MatchingXCoordinatesList)
			Assert.ThrowsException<ArgumentException>(() => { Interpolation<Rational>.Lagrange(points); });

		Rational[] result;

		foreach (InterpolationTestCase item in InterpolationTestCases.List)
		{
			result = Interpolation<Rational>.Lagrange(item.Points);

			Assert.AreEqual(item.LPolynomial.Length, result.Length);

			for (int i = item.LPolynomial.Length - 1; i >= 0; --i)
				Assert.AreEqual(item.LPolynomial[i], result[i]);
		}
	}

	[TestMethod]
	public void LagrangeWithBasisPolynomialsMethod()
	{
		foreach (Point2D<Rational>[] points in InterpolationTestCases.MatchingXCoordinatesList)
			Assert.ThrowsException<ArgumentException>(() => { Interpolation<Rational>.Lagrange(points, []); });

		Rational[] result;

		foreach (InterpolationTestCase item in InterpolationTestCases.List)
		{
			result = Interpolation<Rational>.Lagrange(item.Points, item.LBasisPolynomials);

			Assert.AreEqual(item.LPolynomial.Length, result.Length);

			for (int i = item.LPolynomial.Length - 1; i >= 0; --i)
				Assert.AreEqual(item.LPolynomial[i], result[i]);
		}
	}
}