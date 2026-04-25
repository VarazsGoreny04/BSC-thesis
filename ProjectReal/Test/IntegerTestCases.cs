using System.Collections.Immutable;

namespace Test;

public readonly struct IntegerTestCase(
	string number1, string number2,
	bool equal, bool greater,
	string add, string sub,
	string mul, string div,
	string pow, string root)
{
	public readonly string Number1 = number1;
	public readonly string Number2 = number2;
	public readonly bool Equal = equal;
	public readonly bool Greater = greater;
	public readonly string Add = add;
	public readonly string Sub = sub;
	public readonly string Mul = mul;
	public readonly string Div = div;
	public readonly string Pow = pow;
	public readonly string Root = root;
}

public static class IntegerTestCases
{
	public static readonly ImmutableArray<IntegerTestCase> List =
	[
		/*Zero*/
		new(
			number1: "+0",
			number2: "+0",
			equal: true,
			greater: false,
			add: "+0",
			sub: "+0",
			mul: "+0",
			div: "ERROR",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "+0",
			number2: "-0",
			equal: true,
			greater: false,
			add: "+0",
			sub: "+0",
			mul: "+0",
			div: "ERROR",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "-0",
			number2: "+0",
			equal: true,
			greater: false,
			add: "+0",
			sub: "+0",
			mul: "+0",
			div: "ERROR",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "-0",
			number2: "-0",
			equal: true,
			greater: false,
			add: "+0",
			sub: "+0",
			mul: "+0",
			div: "ERROR",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "+0",
			number2: "+1",
			equal: false,
			greater: false,
			add: "+1",
			sub: "-1",
			mul: "+0",
			div: "+0",
			pow: "+0",
			root: "+0"
		),
		new(
			number1: "+0",
			number2: "-1",
			equal: false,
			greater: true,
			add: "-1",
			sub: "+1",
			mul: "+0",
			div: "+0",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "-0",
			number2: "+1",
			equal: false,
			greater: false,
			add: "+1",
			sub: "-1",
			mul: "+0",
			div: "+0",
			pow: "+0",
			root: "+0"
		),
		new(
			number1: "-0",
			number2: "-1",
			equal: false,
			greater: true,
			add: "-1",
			sub: "+1",
			mul: "+0",
			div: "+0",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "+1",
			number2: "+0",
			equal: false,
			greater: true,
			add: "+1",
			sub: "+1",
			mul: "+0",
			div: "ERROR",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "+1",
			number2: "-0",
			equal: false,
			greater: true,
			add: "+1",
			sub: "+1",
			mul: "+0",
			div: "ERROR",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "-1",
			number2: "+0",
			equal: false,
			greater: false,
			add: "-1",
			sub: "-1",
			mul: "+0",
			div: "ERROR",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "-1",
			number2: "-0",
			equal: false,
			greater: false,
			add: "-1",
			sub: "-1",
			mul: "+0",
			div: "ERROR",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "+999999999999999999999",
			number2: "+1",
			equal: false,
			greater: true,
			add: "+1000000000000000000000",
			sub: "+999999999999999999998",
			mul: "+999999999999999999999",
			div: "+999999999999999999999",
			pow: "+999999999999999999999",
			root: "+999999999999999999999"
		),
		new(
			number1: "+999999999999999999999",
			number2: "-1",
			equal: false,
			greater: true,
			add: "+999999999999999999998",
			sub: "+1000000000000000000000",
			mul: "-999999999999999999999",
			div: "-999999999999999999999",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "-999999999999999999999",
			number2: "+1",
			equal: false,
			greater: false,
			add: "-999999999999999999998",
			sub: "-1000000000000000000000",
			mul: "-999999999999999999999",
			div: "-999999999999999999999",
			pow: "BIG",
			root: "-999999999999999999999"
		),
		new(
			number1: "-999999999999999999999",
			number2: "-1",
			equal: false,
			greater: false,
			add: "-1000000000000000000000",
			sub: "-999999999999999999998",
			mul: "+999999999999999999999",
			div: "+999999999999999999999",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "+1",
			number2: "+999999999999999999999",
			equal: false,
			greater: false,
			add: "+1000000000000000000000",
			sub: "-999999999999999999998",
			mul: "+999999999999999999999",
			div: "+0",
			pow: "+1",
			root: "+1"
		),
		new(
			number1: "+1",
			number2: "-999999999999999999999",
			equal: false,
			greater: true,
			add: "-999999999999999999998",
			sub: "+1000000000000000000000",
			mul: "-999999999999999999999",
			div: "+0",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "-1",
			number2: "+999999999999999999999",
			equal: false,
			greater: false,
			add: "+999999999999999999998",
			sub: "-1000000000000000000000",
			mul: "-999999999999999999999",
			div: "+0",
			pow: "-1",
			root: "-1"
		),
		new(
			number1: "-1",
			number2: "-999999999999999999999",
			equal: false,
			greater: true,
			add: "-1000000000000000000000",
			sub: "+999999999999999999998",
			mul: "+999999999999999999999",
			div: "+0",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "+123456789",
			number2: "+123456789",
			equal: true,
			greater: false,
			add: "+246913578",
			sub: "+0",
			mul: "+15241578750190521",
			div: "+1",
			pow: "BIG",
			root: "BIG"
		),
		new(
			number1: "+123456789",
			number2: "-123456789",
			equal: false,
			greater: true,
			add: "+0",
			sub: "+246913578",
			mul: "-15241578750190521",
			div: "-1",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "-123456789",
			number2: "+123456789",
			equal: false,
			greater: false,
			add: "+0",
			sub: "-246913578",
			mul: "-15241578750190521",
			div: "-1",
			pow: "BIG",
			root: "BIG"
		),
		new(
			number1: "-123456789",
			number2: "-123456789",
			equal: true,
			greater: false,
			add: "-246913578",
			sub: "+0",
			mul: "+15241578750190521",
			div: "+1",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "+1",
			number2: "+2",
			equal: false,
			greater: false,
			add: "+3",
			sub: "-1",
			mul: "+2",
			div: "+0",
			pow: "+1",
			root: "+1"
		),
		new(
			number1: "+1",
			number2: "-2",
			equal: false,
			greater: true,
			add: "-1",
			sub: "+3",
			mul: "-2",
			div: "+0",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "-1",
			number2: "+2",
			equal: false,
			greater: false,
			add: "+1",
			sub: "-3",
			mul: "-2",
			div: "+0",
			pow: "+1",
			root: "ERROR"
		),
		new(
			number1: "-1",
			number2: "-2",
			equal: false,
			greater: true,
			add: "-3",
			sub: "+1",
			mul: "+2",
			div: "+0",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "+2",
			number2: "+1",
			equal: false,
			greater: true,
			add: "+3",
			sub: "+1",
			mul: "+2",
			div: "+2",
			pow: "+2",
			root: "+2"
		),
		new(
			number1: "+2",
			number2: "-1",
			equal: false,
			greater: true,
			add: "+1",
			sub: "+3",
			mul: "-2",
			div: "-2",
			pow: "ERROR",
			root: "ERROR"
		),
		new(
			number1: "-2",
			number2: "+1",
			equal: false,
			greater: false,
			add: "-1",
			sub: "-3",
			mul: "-2",
			div: "-2",
			pow: "-2",
			root: "-2"
		),
		new(
			number1: "-2",
			number2: "-1",
			equal: false,
			greater: false,
			add: "-3",
			sub: "-1",
			mul: "+2",
			div: "+2",
			pow: "ERROR",
			root: "ERROR"
		),
	];
}