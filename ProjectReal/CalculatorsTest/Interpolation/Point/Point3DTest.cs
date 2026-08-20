using Calculators.Interpolation;
using ProjectReal.Number;

namespace CalculatorsTest.Interpolation.Point;

[TestClass]
public class Point3DTest
{
	[TestMethod]
	public void TwoVariableConstructor()
	{
		Point3D<Rational> point1, point2;

		foreach (Point3DTestCase item in Point3DTestCases.List)
		{
			point1 = new(item.Point1.X, item.Point1.Y, item.Point1.Z);
			point2 = new(item.Point2.X, item.Point2.Y, item.Point2.Z);

			Assert.AreEqual(item.Point1.X, point1.X);
			Assert.AreEqual(item.Point1.Y, point1.Y);
			Assert.AreEqual(item.Point1.Z, point1.Z);

			Assert.AreEqual(item.Point2.X, point2.X);
			Assert.AreEqual(item.Point2.Y, point2.Y);
			Assert.AreEqual(item.Point2.Z, point2.Z);
		}
	}

	[TestMethod]
	public void TupleConstructor()
	{
		Point3D<Rational> point1, point2;

		foreach (Point3DTestCase item in Point3DTestCases.List)
		{
			point1 = new Point3D<Rational>(item.Point1);
			point2 = new Point3D<Rational>(item.Point2);

			Assert.AreEqual(item.Point1.X, point1.X);
			Assert.AreEqual(item.Point1.Y, point1.Y);
			Assert.AreEqual(item.Point1.Z, point1.Z);

			Assert.AreEqual(item.Point2.X, point2.X);
			Assert.AreEqual(item.Point2.Y, point2.Y);
			Assert.AreEqual(item.Point2.Z, point2.Z);
		}
	}

	[TestMethod]
	public void EqualsMethod()
	{
		Point3D<Rational> point1, point2;
		bool expected, result;

		foreach (Point3DTestCase item in Point3DTestCases.List)
		{

			point1 = new Point3D<Rational>(item.Point1);
			point2 = new Point3D<Rational>(item.Point2);

			result = Point3D<Rational>.Equals(point1, point2);

			expected = point1.X == point2.X && point1.Y == point2.Y && point1.Z == point2.Z;

			Assert.IsTrue(Point3D<Rational>.Equals(point1, point1));
			Assert.IsTrue(Point3D<Rational>.Equals(point2, point2));

			Assert.IsTrue(Point3D<Rational>.Equals(point1, new Point3D<Rational>(point1.X, point1.Y, point1.Z)));
			Assert.IsTrue(Point3D<Rational>.Equals(point2, new Point3D<Rational>(point2.X, point2.Y, point2.Z)));

			Assert.IsFalse(Point3D<Rational>.Equals(point1, new Point3D<Rational>(point1.X - 1, point1.Y, point1.Z)));
			Assert.IsFalse(Point3D<Rational>.Equals(point1, new Point3D<Rational>(point1.X, point1.Y - 1, point1.Z)));
			Assert.IsFalse(Point3D<Rational>.Equals(point1, new Point3D<Rational>(point1.X, point1.Y, point1.Z - 1)));

			Assert.IsFalse(Point3D<Rational>.Equals(point2, new Point3D<Rational>(point2.X + 1, point2.Y, point2.Z)));
			Assert.IsFalse(Point3D<Rational>.Equals(point2, new Point3D<Rational>(point2.X, point2.Y + 1, point2.Z)));
			Assert.IsFalse(Point3D<Rational>.Equals(point2, new Point3D<Rational>(point2.X, point2.Y, point2.Z + 1)));

			Assert.AreEqual(expected, result);
		}
	}

	[TestMethod]
	public void AddMethod()
	{
		Point3D<Rational> point1, point2, expected, result;

		foreach (Point3DTestCase item in Point3DTestCases.List)
		{
			point1 = new Point3D<Rational>(item.Point1);
			point2 = new Point3D<Rational>(item.Point2);

			result = Point3D<Rational>.Add(point1, point2);

			expected = new Point3D<Rational>(item.Point1.X + item.Point2.X, item.Point1.Y + item.Point2.Y, item.Point1.Z + item.Point2.Z);

			Assert.AreEqual(expected, result);
		}
	}

	[TestMethod]
	public void SubtractMethod()
	{
		Point3D<Rational> point1, point2, expected, result;

		foreach (Point3DTestCase item in Point3DTestCases.List)
		{
			point1 = new Point3D<Rational>(item.Point1);
			point2 = new Point3D<Rational>(item.Point2);

			result = Point3D<Rational>.Subtract(point1, point2);

			expected = new Point3D<Rational>(item.Point1.X - item.Point2.X, item.Point1.Y - item.Point2.Y, item.Point1.Z - item.Point2.Z);

			Assert.AreEqual(expected, result);
		}
	}

	[TestMethod]
	public void DistanceMethod()
	{
		Point3D<Rational> point1, point2;
		Rational expected, result;

		foreach (Point3DTestCase item in Point3DTestCases.List)
		{
			point1 = new Point3D<Rational>(item.Point1);
			point2 = new Point3D<Rational>(item.Point2);

			result = Point3D<Rational>.Distance(point1, point2);

			expected = ~(((item.Point1.X - item.Point2.X) ^ 2) + ((item.Point1.Y - item.Point2.Y) ^ 2) + ((item.Point1.Z - item.Point2.Z) ^ 2));

			Assert.AreEqual(expected, result);
		}
	}
}