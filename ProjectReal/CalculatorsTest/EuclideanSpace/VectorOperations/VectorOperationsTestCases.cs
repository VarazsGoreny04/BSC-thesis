using ProjectReal.Number;
using System.Collections.Immutable;

namespace CalculatorsTest.EuclideanSpace.VectorOperations;

public class VectorOperationsTestCase(
	Rational[] vector1, Rational[] vector2,
	Rational[] add, Rational[] sub,
	Rational[,] outerProduct1, Rational[,] outerProduct2)
{
	public readonly Rational[] Vector1 = vector1;
	public readonly Rational[] Vector2 = vector2;
	public readonly Rational[] Add = add;
	public readonly Rational[] Sub = sub;
	public readonly Rational[,] OuterProduct1 = outerProduct1;
	public readonly Rational[,] OuterProduct2 = outerProduct2;
}

public static class VectorOperationsTestCases
{
	public static readonly ImmutableArray<VectorOperationsTestCase> List =
	[
		/*Zero*/
		new(
			vector1: ["0"],
			vector2: ["0"],
			add: ["0"],
			sub: ["0"],
			outerProduct1: new Rational[,] {{"0"}},
			outerProduct2: new Rational[,] {{"0"}}
		),
		new(
			vector1: ["0", "0"],
			vector2: ["0", "0"],
			add: ["0", "0"],
			sub: ["0", "0"],
			outerProduct1: new Rational[,] {{"0", "0"}, {"0", "0"}},
			outerProduct2: new Rational[,] {{"0", "0"}, {"0", "0"}}
		),
		new(
			vector1: ["1", "1"],
			vector2: ["1", "1"],
			add: ["2", "2"],
			sub: ["0", "0"],
			outerProduct1: new Rational[,] {{"1", "1"}, {"1", "1"}},
			outerProduct2: new Rational[,] {{"1", "1"}, {"1", "1"}}
		),
		new(
			vector1: ["1", "2"],
			vector2: ["3", "4"],
			add: ["4", "6"],
			sub: ["-2", "-2"],
			outerProduct1: new Rational[,] {{"3", "4"}, {"6", "8"}},
			outerProduct2: new Rational[,] {{"3", "4"}, {"6", "8"}}
		),
		new(
			vector1: ["-5", "2"],
			vector2: ["10", "3"],
			add: ["5", "5"],
			sub: ["-15", "-1"],
			outerProduct1: new Rational[,] {{"-50", "-15"}, {"20", "6"}},
			outerProduct2: new Rational[,] {{"-50", "20"}, {"-15", "6"}}
		),
		new(
			vector1: ["-5", "7", "8"],
			vector2: ["1", "2", "3"],
			add: ["-4", "9", "11"],
			sub: ["-6", "5", "5"],
			outerProduct1: new Rational[,] {{"-5", "-10", "-15"}, {"7", "14", "21"}, {"8", "16", "24"}},
			outerProduct2: new Rational[,] {{"-5", "7", "8"}, {"-10", "14", "24"}, {"-15", "21", "24"}}
		),
		new(
			vector1: ["1", "19", "-7"],
			vector2: ["30", "10", "-7"],
			add: ["31", "29", "-14"],
			sub: ["-29", "9", "0"],
			outerProduct1: new Rational[,] {{"30", "10", "-7"}, {"570", "190", "-133"}, {"-210", "-70", "49"}},
			outerProduct2: new Rational[,] {{"30", "570", "-210"}, {"10", "190", "-70"}, {"-7", "-133", "49"}}
		),
		new(
			vector1: ["-2", "-4", "-7", "-1"],
			vector2: ["6", "3", "2", "1"],
			add: ["4", "-1", "-5", "0"],
			sub: ["-8", "-7", "-9", "-2"],
			outerProduct1: new Rational[,] {{"-12", "-6", "-4", "-2"}, {"-24", "-12", "-8", "-4"}, {"-42", "-21", "-14", "-7"}, {"-6", "-3", "-2", "-1"}},
			outerProduct2: new Rational[,] {{"-12", "-24", "-42", "-6"}, {"-6", "-12", "-21", "-4"}, {"-2", "-8", "-14", "-3"}, {"-2", "-4", "-7", "-1"}}
		),
		new(
			vector1: ["-12", "-2", "-1", "-1"],
			vector2: ["2", "4", "7", "1"],
			add: ["-10", "2", "6", "0"],
			sub: ["-14", "-6", "-8", "-2"],
			outerProduct1: new Rational[,] {{"-24", "-48", "-84", "-12"}, {"-4", "-8", "-14", "-2"}, {"-2", "-4", "-7", "-1"}, {"-2", "-4", "-7", "-1"}},
			outerProduct2: new Rational[,] {{"-24", "-4", "-2", "-2"}, {"-48", "-8", "-4", "-4"}, {"-84", "-14", "-7", "-7"}, {"-12", "-2", "-1", "-1"}}
		),
		new(
			vector1: ["-2", "0", "7", "0"],
			vector2: ["0", "0", "-7", "-1"],
			add: ["-2", "0", "0", "-1"],
			sub: ["-2", "0", "14", "1"],
			outerProduct1: new Rational[,] {{"0", "0", "14", "2"}, {"0", "0", "0", "0"}, {"0", "0", "-49", "-7"}, {"0", "0", "0", "0"}},
			outerProduct2: new Rational[,] {{"0", "0", "0", "0"}, {"0", "0", "0", "0"}, {"14", "0", "-49", "0"}, {"2", "0", "-7", "0"}}
		),
		new(
			vector1: ["-2", "0", "7", "0"],
			vector2: ["2", "0", "-7", "0"],
			add: ["0", "0", "0", "0"],
			sub: ["-4", "0", "14", "0"],
			outerProduct1: new Rational[,] {{"-4", "0", "14", "0"}, {"0", "0", "0", "0"}, {"14", "0", "-49", "0"}, {"0", "0", "0", "0"}},
			outerProduct2: new Rational[,] {{"-4", "0", "14", "0"}, {"0", "0", "0", "0"}, {"14", "0", "-49", "0"}, {"0", "0", "0", "0"}}
		)
	];
}
