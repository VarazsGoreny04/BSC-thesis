using ProjectReal.Number;
using System.Collections.Immutable;

namespace CalculatorsTest.Interpolation.Point;

public readonly struct Point2DTestCase(
	(Rational X, Rational Y) point1, (Rational X, Rational Y) point2)
{
	public readonly (Rational X, Rational Y) Point1 = point1;
	public readonly (Rational X, Rational Y) Point2 = point2;
}

public class Point2DTestCases
{
	public static readonly ImmutableArray<Point2DTestCase> List =
	[
		new(
			("0", "0"),
			("0", "0")
		),
		new(
			("1", "0"),
			("0", "0")
		),
		new(
			("0", "1"),
			("0", "0")
		),
		new(
			("1", "0"),
			("0", "1")
		),
		new(
			("2", "0"),
			("0", "0")
		),
		new(
			("1", "0"),
			("1", "0")
		),
		new(
			("1", "0"),
			("-1", "0")
		),
		new(
			("2", "0"),
			("-1", "4")
		),
		new(
			("3", "-8"),
			("-6", "4")
		)
	];
}
