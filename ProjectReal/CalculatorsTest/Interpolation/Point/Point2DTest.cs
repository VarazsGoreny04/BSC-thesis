using Calculators.Interpolation;
using ProjectReal.Number;

namespace CalculatorsTest.Interpolation.Point;

[TestClass]
public class Point2DTest
{
	[TestMethod]
	public void TwoVariableConstructor()
	{
		Point2D<Rational> point1, point2;

		foreach (Point2DTestCase item in Point2DTestCases.List)
		{
			point1 = new(item.Point1.X, item.Point1.Y);
			point2 = new(item.Point2.X, item.Point2.Y);

			Assert.AreEqual(item.Point1.X, point1.X);
			Assert.AreEqual(item.Point1.Y, point1.Y);

			Assert.AreEqual(item.Point2.X, point2.X);
			Assert.AreEqual(item.Point2.Y, point2.Y);
		}
	}

	[TestMethod]
	public void TupleConstructor()
	{
		Point2D<Rational> point1, point2;

		foreach (Point2DTestCase item in Point2DTestCases.List)
		{
			point1 = new Point2D<Rational>(item.Point1);
			point2 = new Point2D<Rational>(item.Point2);

			Assert.AreEqual(item.Point1.X, point1.X);
			Assert.AreEqual(item.Point1.Y, point1.Y);

			Assert.AreEqual(item.Point2.X, point2.X);
			Assert.AreEqual(item.Point2.Y, point2.Y);
		}
	}

	[TestMethod]
	public void EqualsMethod()
	{
		Point2D<Rational> point1, point2;
		bool expected, result;

		foreach (Point2DTestCase item in Point2DTestCases.List)
		{

			point1 = new Point2D<Rational>(item.Point1);
			point2 = new Point2D<Rational>(item.Point2);

			result = Point2D<Rational>.Equals(point1, point2);

			expected = point1.X == point2.X && point1.Y == point2.Y;

			Assert.IsTrue(Point2D<Rational>.Equals(point1, point1));
			Assert.IsTrue(Point2D<Rational>.Equals(point2, point2));

			Assert.IsTrue(Point2D<Rational>.Equals(point1, new Point2D<Rational>(point1.X, point1.Y)));
			Assert.IsTrue(Point2D<Rational>.Equals(point2, new Point2D<Rational>(point2.X, point2.Y)));

			Assert.IsFalse(Point2D<Rational>.Equals(point1, new Point2D<Rational>(point1.X - 1, point1.Y)));
			Assert.IsFalse(Point2D<Rational>.Equals(point1, new Point2D<Rational>(point1.X, point1.Y - 1)));

			Assert.IsFalse(Point2D<Rational>.Equals(point2, new Point2D<Rational>(point2.X + 1, point2.Y)));
			Assert.IsFalse(Point2D<Rational>.Equals(point2, new Point2D<Rational>(point2.X, point2.Y + 1)));

			Assert.AreEqual(expected, result);
		}
	}

	[TestMethod]
	public void AddMethod()
	{
		Point2D<Rational> point1, point2, expected, result;

		foreach (Point2DTestCase item in Point2DTestCases.List)
		{
			point1 = new Point2D<Rational>(item.Point1);
			point2 = new Point2D<Rational>(item.Point2);

			result = Point2D<Rational>.Add(point1, point2);

			expected = new Point2D<Rational>(item.Point1.X + item.Point2.X, item.Point1.Y + item.Point2.Y);

			Assert.AreEqual(expected, result);
		}
	}

	[TestMethod]
	public void SubtractMethod()
	{
		Point2D<Rational> point1, point2, expected, result;

		foreach (Point2DTestCase item in Point2DTestCases.List)
		{
			point1 = new Point2D<Rational>(item.Point1);
			point2 = new Point2D<Rational>(item.Point2);

			result = Point2D<Rational>.Subtract(point1, point2);

			expected = new Point2D<Rational>(item.Point1.X - item.Point2.X, item.Point1.Y - item.Point2.Y);

			Assert.AreEqual(expected, result);
		}
	}

	[TestMethod]
	public void DistanceMethod()
	{
		Point2D<Rational> point1, point2;
		Rational expected, result;

		foreach (Point2DTestCase item in Point2DTestCases.List)
		{
			point1 = new Point2D<Rational>(item.Point1);
			point2 = new Point2D<Rational>(item.Point2);

			result = Point2D<Rational>.Distance(point1, point2);

			expected = ~(((item.Point1.X - item.Point2.X) ^ 2) + ((item.Point1.Y - item.Point2.Y) ^ 2));

			Assert.AreEqual(expected, result);
		}
	}
}