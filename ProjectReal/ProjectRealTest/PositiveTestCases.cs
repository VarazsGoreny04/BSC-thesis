using System.Collections.Immutable;

namespace ProjectRealTest;

public readonly struct PositiveTestCase(
	string number1, string number2,
	bool equal, bool greater,
	string add, bool subSwap, string subNum,
	string mul, string div,
	string pow, string root)
{
	public readonly string Number1 = number1;
	public readonly string Number2 = number2;
	public readonly bool Equal = equal;
	public readonly bool Greater = greater;
	public readonly string Add = add;
	public readonly bool SubSwap = subSwap;
	public readonly string SubNum = subNum;
	public readonly string Mul = mul;
	public readonly string Div = div;
	public readonly string Pow = pow;
	public readonly string Root = root;
}

public static class PositiveTestCases
{
	public static readonly ImmutableArray<PositiveTestCase> List =
	[
		/*Zero*/
		new(
			number1: "0",
			number2: "0",
			equal: true,
			greater: false,
			add: "0",
			subSwap: false,
			subNum: "0",
			mul: "0",
			div: "ERROR",
			pow: "1",
			root: "ERROR"
		),
		new(
			number1: "0.0",
			number2: "0",
			equal: true,
			greater: false,
			add: "0",
			subSwap: false,
			subNum: "0",
			mul: "0",
			div: "ERROR",
			pow: "1",
			root: "ERROR"
		),
		new(
			number1: "0",
			number2: "0.0",
			equal: true,
			greater: false,
			add: "0",
			subSwap: false,
			subNum: "0",
			mul: "0",
			div: "ERROR",
			pow: "1",
			root: "ERROR"
		),
		new(
			number1: "0.0",
			number2: "0.0",
			equal: true,
			greater: false,
			add: "0",
			subSwap: false,
			subNum: "0",
			mul: "0",
			div: "ERROR",
			pow: "1",
			root: "ERROR"
		),

		/*Whole or not*/
		new(
			number1: "1.0",
			number2: "0.1",
			equal: false,
			greater: true,
			add: "1.1",
			subSwap: false,
			subNum: "0.9",
			mul: "0.1",
			div: "10",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "0.1",
			number2: "1.0",
			equal: false,
			greater: false,
			add: "1.1",
			subSwap: true,
			subNum: "0.9",
			mul: "0.1",
			div: "0.1",
			pow: "0.1",
			root: "0.1"
		),
		new(
			number1: "0.1",
			number2: "0.01",
			equal: false,
			greater: true,
			add: "0.11",
			subSwap: false,
			subNum: "0.09",
			mul: "0.001",
			div: "10",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "0.01",
			number2: "0.1",
			equal: false,
			greater: false,
			add: "0.11",
			subSwap: true,
			subNum: "0.09",
			mul: "0.001",
			div: "0.1",
			pow: "ERROR",
			root: "ERROR"
		),

		/*Small numbers*/
		new(
			number1: "0.8",
			number2: "0.07",
			equal: false,
			greater: true,
			add: "0.87",
			subSwap: false,
			subNum: "0.73",
			mul: "0.056",
			div: "11.4285714285714285714285714285714285714285714285714285714285714285714285714",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "0.07",
			number2: "0.8",
			equal: false,
			greater: false,
			add: "0.87",
			subSwap: true,
			subNum: "0.73",
			mul: "0.056",
			div: "0.0875",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "0.06",
			number2: "7",
			equal: false,
			greater: false,
			add: "7.06",
			subSwap: true,
			subNum: "6.94",
			mul: "0.42",
			div: "0.00857142857142857",
			pow: "0.00000000279936",
			root: "0.6690370650810749321815782183670074822411054592576955805354899731204"
		),
		new(
			number1: "0.007",
			number2: "6",
			equal: false,
			greater: false,
			add: "6.007",
			subSwap: true,
			subNum: "5.993",
			mul: "0.042",
			div: "0.0011666666666666665",
			pow: "0.000000000000117649",
			root: "0.43737068749201621607283403411560400315281112121905670094156241834452444"
		),
		new(
			number1: "152.2756",
			number2: "2",
			equal: false,
			greater: true,
			add: "154.2756",
			subSwap: false,
			subNum: "150.2756",
			mul: "304.5512",
			div: "76.1378",
			pow: "23187.85835536",
			root: "12.34"
		),
		new(
			number1: "1.2",
			number2: "2.1",
			equal: false,
			greater: false,
			add: "3.3",
			subSwap: true,
			subNum: "0.9",
			mul: "2.52",
			div: "0.5714285714285714",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "2.1",
			number2: "1.2",
			equal: false,
			greater: true,
			add: "3.3",
			subSwap: false,
			subNum: "0.9",
			mul: "2.52",
			div: "1.75",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "12.45",
			number2: "12.45",
			equal: true,
			greater: false,
			add: "24.9",
			subSwap: false,
			subNum: "0",
			mul: "155.0025",
			div: "1",
			pow: "ERROR",
			root: "ERROR"
		),

		/*Big numbers*/
		new(
			number1: "654.321",
			number2: "123.456",
			equal: false,
			greater: true,
			add: "777.777",
			subSwap: false,
			subNum: "530.865",
			mul: "80779.853376",
			div: "5.30003402021773",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "9999999999999999.9999999999999999",
			number2: "1",
			equal: false,
			greater: true,
			add: "10000000000000000.9999999999999999",
			subSwap: false,
			subNum: "9999999999999998.9999999999999999",
			mul: "9999999999999999.9999999999999999",
			div: "9999999999999999.9999999999999999",
			pow: "9999999999999999.9999999999999999",
			root: "9999999999999999.9999999999999999"
		),
		new(
			number1: "1",
			number2: "9999999999999999.9999999999999999",
			equal: false,
			greater: false,
			add: "10000000000000000.9999999999999999",
			subSwap: true,
			subNum: "9999999999999998.9999999999999999",
			mul: "9999999999999999.9999999999999999",
			div: "0.00000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "0.0000000000000001",
			number2: "9999999999999999",
			equal: false,
			greater: false,
			add: "9999999999999999.0000000000000001",
			subSwap: true,
			subNum: "9999999999999998.9999999999999999",
			mul: "0.9999999999999999",
			div: "0",
			pow: "BIG",
			root: "BIG"
		),
		new(
			number1: "9999999999999999",
			number2: "0.0000000000000001",
			equal: false,
			greater: true,
			add: "9999999999999999.0000000000000001",
			subSwap: false,
			subNum: "9999999999999998.9999999999999999",
			mul: "0.9999999999999999",
			div: "BIG",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "123456789123.456789123456789",
			number2: "123.4567",
			equal: false,
			greater: true,
			add: "123456789246.913489123456789",
			subSwap: false,
			subNum: "123456789000.000089123456789",
			mul: "15241567777777.8677777778677625363",
			div: "1000000721.900526979284694868727254170895544753747670235799272133468657",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "123.4567",
			number2: "123456789123.456789123456789",
			equal: false,
			greater: false,
			add: "123456789246.913489123456789",
			subSwap: true,
			subNum: "123456789000.000089123456789",
			mul: "15241567777777.8677777778677625363",
			div: "0.00000000099999927809999416070994686346051572649069384106531395469435625671864",
			pow: "ERROR",
			root: "ERROR"
		)
	];
}