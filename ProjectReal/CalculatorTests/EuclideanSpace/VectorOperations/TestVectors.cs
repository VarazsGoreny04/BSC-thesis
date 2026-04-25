using ProjectReal.Number;
using System.Collections.Immutable;

namespace CalculatorTests.EuclideanSpace.VectorOperations;

public readonly struct TestVector(
	Rational[] vector1, Rational[] vector2,
	/*bool greater,*/
	Rational[] add, Rational[] sub)
{
	public readonly Rational[] Vector1 = vector1;
	public readonly Rational[] Vector2 = vector2;
	/*public readonly bool Greater = greater;*/
	public readonly Rational[] Add = add;
	public readonly Rational[] Sub = sub;
}

public static class TestVectors
{
	public static readonly ImmutableArray<TestVector> List =
	[
		/*Zero*/
		new(
			vector1: ["0"],
			vector2: ["0"],
			add: ["0"],
			sub: ["0"]
		),
		new(
			vector1: ["0", "0"],
			vector2: ["0", "0"],
			add: ["0", "0"],
			sub: ["0", "0"]
		),
		new(
			vector1: ["1", "1"],
			vector2: ["1", "1"],
			add: ["2", "2"],
			sub: ["0", "0"]
		),
		new(
			vector1: ["1", "2"],
			vector2: ["3", "4"],
			add: ["4", "6"],
			sub: ["-2", "-2"]
		),
		new(
			vector1: ["-5", "2"],
			vector2: ["10", "3"],
			add: ["5", "5"],
			sub: ["-15", "-1"]
		),
		new(
			vector1: ["-5", "7", "8"],
			vector2: ["1", "2", "3"],
			add: ["-4", "9", "11"],
			sub: ["-6", "5", "5"]
		),
		new(
			vector1: ["1", "19", "-7"],
			vector2: ["30", "10", "-7"],
			add: ["31", "29", "-14"],
			sub: ["-29", "9", "0"]
		),
		new(
			vector1: ["-2", "-4", "-7", "-1"],
			vector2: ["6", "3", "2", "1"],
			add: ["4", "-1", "-5", "0"],
			sub: ["-8", "-7", "-9", "-2"]
		),
		new(
			vector1: ["-12", "-2", "-1", "-1"],
			vector2: ["2", "4", "7", "1"],
			add: ["-10", "2", "6", "0"],
			sub: ["-14", "-6", "-8", "-2"]
		),
		new(
			vector1: ["-2", "0", "7", "0"],
			vector2: ["0", "0", "-7", "-1"],
			add: ["-2", "0", "0", "-1"],
			sub: ["-2", "0", "14", "1"]
		),
		new(
			vector1: ["-2", "0", "7", "0"],
			vector2: ["2", "0", "-7", "0"],
			add: ["0", "0", "0", "0"],
			sub: ["-4", "0", "14", "0"]
		)
	];
}
