using ProjectReal.Number;
using System.Collections.Immutable;

namespace CalculatorsTest.Interpolation.Point;

public readonly struct Point3DTestCase(
	(Rational X, Rational Y, Rational Z) point1, (Rational X, Rational Y, Rational Z) point2)
{
	public readonly (Rational X, Rational Y, Rational Z) Point1 = point1;
	public readonly (Rational X, Rational Y, Rational Z) Point2 = point2;
}

public class Point3DTestCases
{
	public static readonly ImmutableArray<Point3DTestCase> List =
	[
		new(
			("0", "0", "0"),
			("0", "0", "0")
		),
		new(
			("1", "0", "0"),
			("0", "0", "0")
		),
		new(
			("0", "1", "0"),
			("0", "0", "0")
		),
		new(
			("0", "0", "1"),
			("0", "0", "0")
		),
		new(
			("1", "0", "0"),
			("0", "1", "1")
		),
		new(
			("2", "0", "0"),
			("0", "0", "0")
		),
		new(
			("1", "0", "0"),
			("1", "0", "0")
		),
		new(
			("1", "0", "2"),
			("-1", "0", "-2")
		),
		new(
			("2", "0", "1"),
			("-1", "4", "9")
		),
		new(
			("3", "-8", "7"),
			("-6", "4", "-1")
		)
	];
}
