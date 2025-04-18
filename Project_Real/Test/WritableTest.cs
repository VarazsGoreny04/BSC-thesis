using Project_Real;
using System;

namespace Test;

[TestClass]
public class WritableTest
{
	private static bool Sign(string sign)
	{
		return sign[0] switch
		{
			'+' => true,
			'-' => sign.Length == 2 && sign[1] == '0',
			_ => throw new FormatException()
		};
	}

	[TestMethod]
	public void ZeroConstructor()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Writable empty = new();

		Writable[] zeros =
		[
			new("0"),
			new("00"),
			new($"0."),
			new($"00."),
			new($"0.0"),
			new($"0.00"),
			new($"00.0"),
			new($"00.00"),
			new("+0"),
			new("+00"),
			new($"+0."),
			new($"+00."),
			new($"+0.0"),
			new($"+0.00"),
			new($"+00.0"),
			new($"+00.00"),
			new(true, new(new([Digit.ZERO]), 0)),
			new(true, new(new([Digit.ZERO, Digit.ZERO]), 0)),
			new(true, new(new([Digit.ZERO, Digit.ZERO]), 1)),
			new(true, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2)),
			new(true, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 1)),
			new(true, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2)),
			new("-0"),
			new("-00"),
			new($"-0."),
			new($"-00."),
			new($"-0.0"),
			new($"-0.00"),
			new($"-00.0"),
			new($"-00.00"),
			new(false, new(new([Digit.ZERO]), 0)),
			new(false, new(new([Digit.ZERO, Digit.ZERO]), 0)),
			new(false, new(new([Digit.ZERO, Digit.ZERO]), 1)),
			new(false, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2)),
			new(false, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO]), 1)),
			new(false, new(new([Digit.ZERO, Digit.ZERO, Digit.ZERO, Digit.ZERO]), 2)),
		];

		foreach (Writable zero in zeros)
			Assert.AreEqual(empty, zero);

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void StringConstructor()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		string nullString = null!;
		Assert.ThrowsException<ArgumentException>(() => new Writable(nullString));
		Assert.ThrowsException<ArgumentException>(() => new Writable(""));
		Assert.ThrowsException<ArgumentException>(() => new Writable("+"));
		Assert.ThrowsException<ArgumentException>(() => new Writable("-"));
		Assert.ThrowsException<ArgumentException>(() => new Writable("."));
		Assert.ThrowsException<ArgumentException>(() => new Writable("+."));
		Assert.ThrowsException<ArgumentException>(() => new Writable("-."));
		Assert.ThrowsException<ArgumentException>(() => new Writable($".123"));
		Assert.ThrowsException<ArgumentException>(() => new Writable($"+.123"));
		Assert.ThrowsException<ArgumentException>(() => new Writable($"-.123"));

		string[] tests = ["a123", "123a", "12a3"];

		for (int j = 0; j < tests.Length; ++j)
			Assert.ThrowsException<ArgumentException>(() => new Writable(tests[j]));

		for (int i = tests[0].Length; i > 0; --i)
		{
			for (int j = 0; j < tests.Length; ++j)
				Assert.ThrowsException<ArgumentException>(() => new Writable(tests[j].Insert(i, ".")));
		}

		Writable number1, number2;

		foreach (var item in WritableTestCases.List)
		{
			number1 = new(item.Number1);
			number2 = new(item.Number2);

			Assert.AreEqual(Sign(item.Number1), number1.Sign);
			Assert.AreEqual((new Positive(item.Number1.Replace("+", "").Replace("-", ""))).ToString(), number1.Value.ToString());

			Assert.AreEqual(Sign(item.Number2), number2.Sign);
			Assert.AreEqual((new Positive(item.Number2.Replace("+", "").Replace("-", ""))).ToString(), number2.Value.ToString());
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void PositiveConstructor()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Positive positive1, positive2;
		Writable number1, number2;

		foreach (var item in WritableTestCases.List)
		{
			positive1 = new(item.Number1.Replace("+", "").Replace("-", ""));
			positive2 = new(item.Number2.Replace("+", "").Replace("-", ""));

			number1 = new(Sign(item.Number1), positive1);
			number2 = new(Sign(item.Number2), positive2);

			Assert.AreEqual(Sign(item.Number1), number1.Sign);
			Assert.AreEqual((new Positive(item.Number1.Replace("+", "").Replace("-", ""))).ToString(), number1.Value.ToString());

			Assert.AreEqual(Sign(item.Number2), number2.Sign);
			Assert.AreEqual((new Positive(item.Number2.Replace("+", "").Replace("-", ""))).ToString(), number2.Value.ToString());
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void ToStringMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Positive positive1, positive2;
		Writable number1, number2;

		foreach (var item in WritableTestCases.List)
		{
			positive1 = new(item.Number1.Replace("+", "").Replace("-", ""));
			positive2 = new(item.Number2.Replace("+", "").Replace("-", ""));

			number1 = new(item.Number1);
			number2 = new(item.Number2);

			Assert.AreEqual(positive1.IsZero ? "+0" : $"{(Sign(item.Number1) ? '+' : '-')}{positive1}", number1.ToString());
			Assert.AreEqual(positive2.IsZero ? "+0" : $"{(Sign(item.Number2) ? '+' : '-')}{positive2}", number2.ToString());
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void EqualsMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Writable numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (var item in IntegerTestCases.List)
		{
			numberCharacters1 = new(item.Number1);
			numberDigits1 = new(Sign(item.Number1), new Positive(item.Number1.Replace("+", "").Replace("-", "")));

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new(Sign(item.Number2), new Positive(item.Number2.Replace("+", "").Replace("-", "")));

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Equal, numberDigits1 == numberDigits2);
			Assert.AreEqual(numberDigits1 == numberDigits2, numberDigits2 == numberDigits1);
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Writable numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (var item in IntegerTestCases.List)
		{
			numberCharacters1 = new(item.Number1);
			numberDigits1 = new(Sign(item.Number1), new Positive(item.Number1.Replace("+", "").Replace("-", "")));

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new(Sign(item.Number2), new Positive(item.Number2.Replace("+", "").Replace("-", "")));

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Greater, Writable.GreaterThan(numberCharacters1, numberCharacters2));
			Assert.AreEqual(item.Greater, Writable.GreaterThan(numberDigits1, numberDigits2));
			Assert.AreEqual(item.Greater, Writable.GreaterThan(numberCharacters1, numberDigits2));
			Assert.AreEqual(item.Greater, Writable.GreaterThan(numberDigits1, numberCharacters2));
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}


	[TestMethod]
	public void AddMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Writable writable1, writable2;

		foreach (var item in WritableTestCases.List)
		{
			writable1 = new(item.Number1);
			writable2 = new(item.Number2);

			Assert.AreEqual(item.Add, Writable.Add(writable1, writable2).ToString());
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void SubstractMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Writable writable1, writable2;

		foreach (var item in WritableTestCases.List)
		{
			writable1 = new(item.Number1);
			writable2 = new(item.Number2);

			Assert.AreEqual(item.Sub, Writable.Substract(writable1, writable2).ToString());
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Writable writable1, writable2;

		foreach (var item in WritableTestCases.List)
		{
			writable1 = new(item.Number1);
			writable2 = new(item.Number2);

			Assert.AreEqual(item.Mul, Writable.Multiply(writable1, writable2).ToString());
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void DivideMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';
		int fractionCalculatonLength = Writable.FractionCalculatonLength;
		Writable.FractionCalculatonLength = 10;

		string[] tokens;
		Writable writable1, writable2, whole, remainder;

		foreach (var item in WritableTestCases.List)
		{
			writable1 = new(item.Number1);
			writable2 = new(item.Number2);

			if (item.Div == "ERROR")
				Assert.ThrowsException<DivideByZeroException>(() => Writable.Divide(writable1, writable2));
			else if (item.Div != "BIG")
			{
				tokens = item.Div.Split('.', StringSplitOptions.RemoveEmptyEntries);
				(whole, remainder) = Writable.Divide(writable1, writable2);

				Assert.AreEqual(new Writable(tokens[0] +
					(tokens.Length == 2 ? $".{ [.. tokens[1][..Math.Min(tokens[1].Length, whole.FractionLength)]]}" : "")).ToString(),
					whole.ToString());
				Assert.AreEqual((new Writable(item.Number1)).ToString(), ((whole * item.Number2) + remainder).ToString());
			}
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
		Writable.FractionCalculatonLength = fractionCalculatonLength;
	}

	[TestMethod]
	public void PowerMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';

		Writable writable1, writable2;

		foreach (var item in WritableTestCases.List)
		{
			writable1 = new(item.Number1);
			writable2 = new(item.Number2);

			if (item.Pow == "ERROR")
				Assert.ThrowsException<NotImplementedException>(() => Writable.Power(writable1, writable2));
			else if (item.Pow != "BIG")
				Assert.AreEqual(item.Pow, Writable.Power(writable1, writable2).ToString());
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
	}

	[TestMethod]
	public void RootMethod()
	{
		bool writeSign = Writable.WriteSign;
		Writable.WriteSign = true;
		char separator = Writable.Separator;
		Writable.Separator = '.';
		int fractionCalculatonLength = Writable.FractionCalculatonLength;
		Writable.FractionCalculatonLength = 10;

		string[] tokens;
		Writable writable1, writable2, whole, remainder;

		foreach (var item in WritableTestCases.List)
		{
			writable1 = new(item.Number1);
			writable2 = new(item.Number2);

			if (item.Root == "ERROR")
				Assert.ThrowsException<NotImplementedException>(() => Writable.Root(writable1, writable2));
			else if (item.Root != "BIG")
			{
				tokens = item.Root.Split('.', StringSplitOptions.RemoveEmptyEntries);
				(whole, remainder) = Writable.Root(writable1, writable2);

				Assert.AreEqual(new Writable(tokens[0] +
					(tokens.Length == 2 ? $".{ [.. tokens[1][..Math.Min(tokens[1].Length, whole.FractionLength)]]}" : "")).ToString(), 
					whole.ToString());
				Assert.AreEqual((new Writable(item.Number1)).ToString(), ((whole ^ item.Number2) + remainder).ToString());
			}
		}

		Writable.WriteSign = writeSign;
		Writable.Separator = separator;
		Writable.FractionCalculatonLength = fractionCalculatonLength;
	}
}