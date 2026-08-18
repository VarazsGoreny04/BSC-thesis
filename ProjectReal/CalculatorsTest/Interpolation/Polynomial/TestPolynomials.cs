using ProjectReal.Number;
using System.Collections.Immutable;

namespace CalculatorsTest.Interpolation.Polynomial;

public readonly struct TestPolynomial(Rational[] polynomial, Rational[] basePoints, Rational[] evaluations)
{
	public readonly Rational[] Polynomial = polynomial;
	public readonly Rational[] BasePoints = basePoints;
	public readonly Rational[] Evaluations = evaluations;
}

public static class TestPolynomials
{
	public static readonly ImmutableArray<TestPolynomial> List =
	[
		new(
			["0"],
			["0", "1", "2", "3.5"],
			["0", "0", "0", "0"]
		),
		new(
			["1"],
			["0", "1", "2", "3.5"],
			["1", "1", "1", "1"]
		),
		new(
			["0", "1"],
			["0", "1", "2", "3.5"],
			["0", "1", "2", "3.5"]
		),
		new(
			["0", "2"],
			["0", "1", "2", "3.5"],
			["0", "2", "4", "7"]
		),
		new(
			["0", "0", "1"],
			["0", "1", "2", "3.5"],
			["0", "1", "4", "12.25"]
		),
		new(
			["0", "0", "2"],
			["0", "1", "2", "3.5"],
			["0", "2", "8", "24.5"]
		),
		new(
			["2", "0", "2"],
			["0", "1", "2", "3.5"],
			["2", "4", "10", "26.5"]
		),
		new(
			["0", "-3", "2"],
			["0", "1", "2", "3.5"],
			["0", "-1", "2", "14"]
		),
		new(
			["4", "-3", "2"],
			["0", "1", "2", "3.5"],
			["4", "3", "6", "18"]
		)
	];
}