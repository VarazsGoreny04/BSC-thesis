using System.Collections.Immutable;

namespace Test;

public static class PositiveTestCases
{
	public static readonly
	ImmutableArray<(
	string Number1,
	string Number2,
	bool Equal,
	bool Greater,
	string Add,
	bool SubSwap,
	string SubNum,
	string Mul,
	string Div,
	string Pow,
	string Root
	)> List =
	[
		/*Zero*/
		(
			Number1: "0",
			Number2: "0",
			Equal: true,
			Greater: false,
			Add: "0",
			SubSwap: false,
			SubNum: "0",
			Mul: "0",
			Div: "ERROR",
			Pow: "1",
			Root: "ERROR"
		),
		(
			Number1: "0.0",
			Number2: "0",
			Equal: true,
			Greater: false,
			Add: "0",
			SubSwap: false,
			SubNum: "0",
			Mul: "0",
			Div: "ERROR",
			Pow: "1",
			Root: "ERROR"
		),
		(
			Number1: "0",
			Number2: "0.0",
			Equal: true,
			Greater: false,
			Add: "0",
			SubSwap: false,
			SubNum: "0",
			Mul: "0",
			Div: "ERROR",
			Pow: "1",
			Root: "ERROR"
		),
		(
			Number1: "0.0",
			Number2: "0.0",
			Equal: true,
			Greater: false,
			Add: "0",
			SubSwap: false,
			SubNum: "0",
			Mul: "0",
			Div: "ERROR",
			Pow: "1",
			Root: "ERROR"
		),

		/*Whole or not*/
		(
			Number1: "1.0",
			Number2: "0.1",
			Equal: false,
			Greater: true,
			Add: "1.1",
			SubSwap: false,
			SubNum: "0.9",
			Mul: "0.1",
			Div: "10",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "0.1",
			Number2: "1.0",
			Equal: false,
			Greater: false,
			Add: "1.1",
			SubSwap: true,
			SubNum: "0.9",
			Mul: "0.1",
			Div: "0.1",
			Pow: "0.1",
			Root: "0.1"
		),
		(
			Number1: "0.1",
			Number2: "0.01",
			Equal: false,
			Greater: true,
			Add: "0.11",
			SubSwap: false,
			SubNum: "0.09",
			Mul: "0.001",
			Div: "10",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "0.01",
			Number2: "0.1",
			Equal: false,
			Greater: false,
			Add: "0.11",
			SubSwap: true,
			SubNum: "0.09",
			Mul: "0.001",
			Div: "0.1",
			Pow: "ERROR",
			Root: "ERROR"
		),

		/*Small numbers*/
		(
			Number1: "0.8",
			Number2: "0.07",
			Equal: false,
			Greater: true,
			Add: "0.87",
			SubSwap: false,
			SubNum: "0.73",
			Mul: "0.056",
			Div: "11.4285714285714285714285714285714285714285714285714285714285714285714285714",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "0.07",
			Number2: "0.8",
			Equal: false,
			Greater: false,
			Add: "0.87",
			SubSwap: true,
			SubNum: "0.73",
			Mul: "0.056",
			Div: "0.0875",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "0.06",
			Number2: "7",
			Equal: false,
			Greater: false,
			Add: "7.06",
			SubSwap: true,
			SubNum: "6.94",
			Mul: "0.42",
			Div: "0.00857142857142857",
			Pow: "0.00000000279936",
			Root: "0.6690370650810749321815782183670074822411054592576955805354899731204"
		),
		(
			Number1: "0.007",
			Number2: "6",
			Equal: false,
			Greater: false,
			Add: "6.007",
			SubSwap: true,
			SubNum: "5.993",
			Mul: "0.042",
			Div: "0.0011666666666666665",
			Pow: "0.000000000000117649",
			Root: "0.43737068749201621607283403411560400315281112121905670094156241834452444"
		),
		(
			Number1: "152.2756",
			Number2: "2",
			Equal: false,
			Greater: true,
			Add: "154.2756",
			SubSwap: false,
			SubNum: "150.2756",
			Mul: "304.5512",
			Div: "76.1378",
			Pow: "23187.85835536",
			Root: "12.34"
		),
		(
			Number1: "1.2",
			Number2: "2.1",
			Equal: false,
			Greater: false,
			Add: "3.3",
			SubSwap: true,
			SubNum: "0.9",
			Mul: "2.52",
			Div: "0.5714285714285714",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "2.1",
			Number2: "1.2",
			Equal: false,
			Greater: true,
			Add: "3.3",
			SubSwap: false,
			SubNum: "0.9",
			Mul: "2.52",
			Div: "1.75",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "12.45",
			Number2: "12.45",
			Equal: true,
			Greater: false,
			Add: "24.9",
			SubSwap: false,
			SubNum: "0",
			Mul: "155.0025",
			Div: "1",
			Pow: "ERROR",
			Root: "ERROR"
		),

		/*Big numbers*/
		(
			Number1: "654.321",
			Number2: "123.456",
			Equal: false,
			Greater: true,
			Add: "777.777",
			SubSwap: false,
			SubNum: "530.865",
			Mul: "80779.853376",
			Div: "5.30003402021773",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "9999999999999999.9999999999999999",
			Number2: "1",
			Equal: false,
			Greater: true,
			Add: "10000000000000000.9999999999999999",
			SubSwap: false,
			SubNum: "9999999999999998.9999999999999999",
			Mul: "9999999999999999.9999999999999999",
			Div: "9999999999999999.9999999999999999",
			Pow: "9999999999999999.9999999999999999",
			Root: "9999999999999999.9999999999999999"
		),
		(
			Number1: "1",
			Number2: "9999999999999999.9999999999999999",
			Equal: false,
			Greater: false,
			Add: "10000000000000000.9999999999999999",
			SubSwap: true,
			SubNum: "9999999999999998.9999999999999999",
			Mul: "9999999999999999.9999999999999999",
			Div: "0.00000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001000000000000000000000000000000010000000000000000000000000000000100000000000000000000000000000001",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "0.0000000000000001",
			Number2: "9999999999999999",
			Equal: false,
			Greater: false,
			Add: "9999999999999999.0000000000000001",
			SubSwap: true,
			SubNum: "9999999999999998.9999999999999999",
			Mul: "0.9999999999999999",
			Div: "0",
			Pow: "BIG",
			Root: "BIG"
		),
		(
			Number1: "9999999999999999",
			Number2: "0.0000000000000001",
			Equal: false,
			Greater: true,
			Add: "9999999999999999.0000000000000001",
			SubSwap: false,
			SubNum: "9999999999999998.9999999999999999",
			Mul: "0.9999999999999999",
			Div: "BIG",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "123456789123.456789123456789",
			Number2: "123.4567",
			Equal: false,
			Greater: true,
			Add: "123456789246.913489123456789",
			SubSwap: false,
			SubNum: "123456789000.000089123456789",
			Mul: "15241567777777.8677777778677625363",
			Div: "1000000721.900526979284694868727254170895544753747670235799272133468657",
			Pow: "ERROR",
			Root: "ERROR"
		),
		(
			Number1: "123.4567",
			Number2: "123456789123.456789123456789",
			Equal: false,
			Greater: false,
			Add: "123456789246.913489123456789",
			SubSwap: true,
			SubNum: "123456789000.000089123456789",
			Mul: "15241567777777.8677777778677625363",
			Div: "0.00000000099999927809999416070994686346051572649069384106531395469435625671864",
			Pow: "ERROR",
			Root: "ERROR"
		)
	];
}