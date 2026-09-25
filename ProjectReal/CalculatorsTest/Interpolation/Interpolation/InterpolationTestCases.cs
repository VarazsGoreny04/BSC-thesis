using Calculators.Interpolation;
using ProjectReal.Number;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CalculatorsTest.Interpolation.Interpolation;

public class InterpolationTestCase(
	Point2D<Rational>[] points, Rational[][] lBasePolynomials, Rational[] lPolynomial)
{
	public readonly Point2D<Rational>[] Points = points;
	public readonly Rational[][] LBasisPolynomials = lBasePolynomials;
	public readonly Rational[] LPolynomial = lPolynomial;
}

public class InterpolationTestCases
{
	public static readonly List<Point2D<Rational>[]> MatchingXCoordinatesList =
	[
		[
			("0", "1"),
			("0", "2")
		],
		[
			("0", "1"),
			("0", "2"),
			("1", "3")
		],
		[
			("0", "1"),
			("1", "2"),
			("0", "3")
		]
	];

	public static readonly ImmutableArray<InterpolationTestCase> List =
	[
		new(
			[
				("0", "1"),
			],
			[
				["1"]  
			],
			["1"]
		),
		new(
			[
				("0", "1"),
				("1", "3")
			],
			[
				["1", "-1"],
				["0", "1"]  
			],
			["1", "2"]
		),
		new(
			[
				("0", "2"),
				("2", "5"),
				("5", "-1")
			],
			[
				["1", "-7/10", "1/10"],
				["0", "5/6", "-1/6"],
				["0", "-2/15", "1/15"]
			],
			["2", "29/10", "-7/10"]
		),
		new(
			[
				("-2", "3"),
				("0", "-1"),
				("3", "4")
			],
			[
				["0", "-3/10", "1/10"],
				["1", "1/6", "-1/6"],
				["0", "2/15", "1/15"]
			],
			["-1", "-8/15", "11/15"]
		),
		new(
			[
				("-1", "2"),
				("1", "2"),
				("2", "-1")
			],
			[
				["1/3", "-1/2", "1/6"],
				["1", "1/2", "-1/2"],
				["-1/3", "0", "1/3"]
			],
			["3", "0", "-1"]
		),
		new(
			[
				("0", "0"),
				("1", "1"),
				("2", "8"),
				("3", "27")
			],
			[
				["1", "-11/6", "1", "-1/6"],
				["0", "3", "-5/2", "1/2"],
				["0", "-3/2", "2", "-1/2"],
				["0", "1/3", "-1/2", "1/6"]
			],
			["0", "0", "0", "1"]
		),
		new(
			[
				("-3", "5"),
				("-1", "-2"),
				("2", "7"),
				("4", "1")
			],
			[
				["-4/35", "-1/35", "1/14", "-1/70"],
				["4/5", "-1/3", "-1/10", "1/30"],
				["2/5", "13/30", "0", "-1/30"],
				["-3/35", "-1/14", "1/35", "1/70"]
			],
			["19/35", "122/35", "41/70", "-5/14"]
		)
	];
}