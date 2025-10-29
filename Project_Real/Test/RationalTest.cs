using Project_Real;
using System;

namespace Test;

[TestClass]
public class RationalTest
{
	public const int fractionCalculationLength = 10;

	private static bool Sign(string sign)
	{
		return sign[0] switch
		{
			'-' => (new Positive(sign.Replace("+", "").Replace("-", ""))).IsZero,
			_ => true
		};
	}

	[TestMethod]
	public void ZeroConstructor()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		Rational empty = new();

		Rational[] zeros =
		[
			new("0"),
			new("00"),
			new("0."),
			new("00."),
			new("0.0"),
			new("0.00"),
			new("00.0"),
			new("00.00"),
			new("+0"),
			new("+00"),
			new("+0."),
			new("+00."),
			new("+0.0"),
			new("+0.00"),
			new("+00.0"),
			new("+00.00"),
			new("0/1"),
			new("00/1"),
			new("0./1"),
			new("00./1"),
			new("0.0/1"),
			new("0.00/1"),
			new("00.0/1"),
			new("00.00/1"),
			new("0/1.0"),
			new("00/1.0"),
			new("0./1."),
			new("00./01."),
			new("0.0/01.0"),
			new("0.00/1.0"),
			new("00.0/1.00"),
			new("00.00/1.00"),
			new("+0/+1"),
			new("+00/+1"),
			new("+0./+1"),
			new("+00./+1"),
			new("+0.0/+1"),
			new("+0.00/+1"),
			new("+00.0/+1"),
			new("+00.00/+1"),
			new("+0/+1.0"),
			new("+00/+1.0"),
			new("+0./+1."),
			new("+00./+01."),
			new("+0.0/+01.0"),
			new("+0.00/+1.0"),
			new("+00.0/+1.00"),
			new("+00.00/+1.00"),
			new(true, new("0")),
			new(true, new("00")),
			new(true, new("0.0")),
			new(true, new("0.00")),
			new(true, new("00.0")),
			new(true, new("00.00")),
			new(true, new("0"), new("1")),
			new(true, new("00"), new("1")),
			new(true, new("0.0"), new("1")),
			new(true, new("0.00"), new("1")),
			new(true, new("00.0"), new("1")),
			new(true, new("00.00"), new("1")),
			new(true, new("0"), new("1.0")),
			new(true, new("00"), new("1.0")),
			new(true, new("0.0"), new("1.0")),
			new(true, new("0.00"), new("1.0")),
			new(true, new("00.0"), new("1.0")),
			new(true, new("00.00"), new("1.0")),
			new(new("+0")),
			new(new("+00")),
			new(new("+0.0")),
			new(new("+0.00")),
			new(new("+00.0")),
			new(new("+00.00")),
			new(new Writable("+0"), new Positive("1")),
			new(new Writable("+00"), new Positive("1")),
			new(new Writable("+0.0"), new Positive("1")),
			new(new Writable("+0.00"), new Positive("1")),
			new(new Writable("+00.0"), new Positive("1")),
			new(new Writable("+00.00"), new Positive("1")),
			new(new Writable("+0"), new Positive("1.0")),
			new(new Writable("+00"), new Positive("1.0")),
			new(new Writable("+0.0"), new Positive("1.0")),
			new(new Writable("+0.00"), new Positive("1.0")),
			new(new Writable("+00.0"), new Positive("1.0")),
			new(new Writable("+00.00"), new Positive("1.0")),
			new(new("+0"), new Writable("+1")),
			new(new("+00"), new Writable("+1")),
			new(new("+0.0"), new Writable("+1")),
			new(new("+0.00"), new Writable("+1")),
			new(new("+00.0"), new Writable("+1")),
			new(new("+00.00"), new Writable("+1")),
			new(new("+0"), new Writable("+1.0")),
			new(new("+00"), new Writable("+1.0")),
			new(new("+0.0"), new Writable("+1.0")),
			new(new("+0.00"), new Writable("+1.0")),
			new(new("+00.0"), new Writable("+1.0")),
			new(new("+00.00"), new Writable("+1.0")),
			new("-0"),
			new("-00"),
			new("-0."),
			new("-00."),
			new("-0.0"),
			new("-0.00"),
			new("-00.0"),
			new("-00.00"),
			new("-0/+1"),
			new("-00/+1"),
			new("-0./+1"),
			new("-00./+1"),
			new("-0.0/+1"),
			new("-0.00/+1"),
			new("-00.0/+1"),
			new("-00.00/+1"),
			new("-0/+1.0"),
			new("-00/+1.0"),
			new("-0./+1."),
			new("-00./+01."),
			new("-0.0/+01.0"),
			new("-0.00/+1.0"),
			new("-00.0/+1.00"),
			new("-00.00/+1.00"),
			new(false, new("0")),
			new(false, new("00")),
			new(false, new("0.0")),
			new(false, new("0.00")),
			new(false, new("00.0")),
			new(false, new("00.00")),
			new(false, new("0"), new("1")),
			new(false, new("00"), new("1")),
			new(false, new("0.0"), new("1")),
			new(false, new("0.00"), new("1")),
			new(false, new("00.0"), new("1")),
			new(false, new("00.00"), new("1")),
			new(false, new("0"), new("1.0")),
			new(false, new("00"), new("1.0")),
			new(false, new("0.0"), new("1.0")),
			new(false, new("0.00"), new("1.0")),
			new(false, new("00.0"), new("1.0")),
			new(false, new("00.00"), new("1.0")),
			new(new("-0")),
			new(new("-00")),
			new(new("-0.0")),
			new(new("-0.00")),
			new(new("-00.0")),
			new(new("-00.00")),
			new(new Writable("-0"), new Positive("1")),
			new(new Writable("-00"), new Positive("1")),
			new(new Writable("-0.0"), new Positive("1")),
			new(new Writable("-0.00"), new Positive("1")),
			new(new Writable("-00.0"), new Positive("1")),
			new(new Writable("-00.00"), new Positive("1")),
			new(new Writable("-0"), new Positive("1.0")),
			new(new Writable("-00"), new Positive("1.0")),
			new(new Writable("-0.0"), new Positive("1.0")),
			new(new Writable("-0.00"), new Positive("1.0")),
			new(new Writable("-00.0"), new Positive("1.0")),
			new(new Writable("-00.00"), new Positive("1.0")),
			new(new("-0"), new Writable("+1")),
			new(new("-00"), new Writable("+1")),
			new(new("-0.0"), new Writable("+1")),
			new(new("-0.00"), new Writable("+1")),
			new(new("-00.0"), new Writable("+1")),
			new(new("-00.00"), new Writable("+1")),
			new(new("-0"), new Writable("+1.0")),
			new(new("-00"), new Writable("+1.0")),
			new(new("-0.0"), new Writable("+1.0")),
			new(new("-0.00"), new Writable("+1.0")),
			new(new("-00.0"), new Writable("+1.0")),
			new(new("-00.00"), new Writable("+1.0")),
			new("0"),
			new("00"),
			new("0."),
			new("00."),
			new("0.0"),
			new("0.00"),
			new("00.0"),
			new("00.00"),
			new("+0"),
			new("+00"),
			new("+0."),
			new("+00."),
			new("+0.0"),
			new("+0.00"),
			new("+00.0"),
			new("+00.00"),
			new("0/1"),
			new("00/1"),
			new("0./1"),
			new("00./1"),
			new("0.0/1"),
			new("0.00/1"),
			new("00.0/1"),
			new("00.00/1"),
			new("0/0.1"),
			new("00/0.1"),
			new("0./0.1"),
			new("00./00.1"),
			new("0.0/00.1"),
			new("0.00/0.1"),
			new("00.0/0.10"),
			new("00.00/0.10"),
			new("+0/+1"),
			new("+00/+1"),
			new("+0./+1"),
			new("+00./+1"),
			new("+0.0/+1"),
			new("+0.00/+1"),
			new("+00.0/+1"),
			new("+00.00/+1"),
			new("+0/+0.1"),
			new("+00/+0.1"),
			new("+0./+0.1"),
			new("+00./+00.1"),
			new("+0.0/+00.1"),
			new("+0.00/+0.1"),
			new("+00.0/+0.10"),
			new("+00.00/+0.10"),
			new(true, new("0")),
			new(true, new("00")),
			new(true, new("0.0")),
			new(true, new("0.00")),
			new(true, new("00.0")),
			new(true, new("00.00")),
			new(true, new("0"), new("0.1")),
			new(true, new("00"), new("0.1")),
			new(true, new("0.0"), new("0.1")),
			new(true, new("0.00"), new("0.1")),
			new(true, new("00.0"), new("0.1")),
			new(true, new("00.00"), new("0.1")),
			new(true, new("0"), new("0.1")),
			new(true, new("00"), new("0.1")),
			new(true, new("0.0"), new("0.1")),
			new(true, new("0.00"), new("0.1")),
			new(true, new("00.0"), new("0.1")),
			new(true, new("00.00"), new("0.1")),
			new(new("+0")),
			new(new("+00")),
			new(new("+0.0")),
			new(new("+0.00")),
			new(new("+00.0")),
			new(new("+00.00")),
			new(new Writable("+0"), new Positive("0.1")),
			new(new Writable("+00"), new Positive("0.1")),
			new(new Writable("+0.0"), new Positive("0.1")),
			new(new Writable("+0.00"), new Positive("0.1")),
			new(new Writable("+00.0"), new Positive("0.1")),
			new(new Writable("+00.00"), new Positive("0.1")),
			new(new Writable("+0"), new Positive("0.1")),
			new(new Writable("+00"), new Positive("0.1")),
			new(new Writable("+0.0"), new Positive("0.1")),
			new(new Writable("+0.00"), new Positive("0.1")),
			new(new Writable("+00.0"), new Positive("0.1")),
			new(new Writable("+00.00"), new Positive("0.1")),
			new(new("+0"), new Writable("+1")),
			new(new("+00"), new Writable("+1")),
			new(new("+0.0"), new Writable("+1")),
			new(new("+0.00"), new Writable("+1")),
			new(new("+00.0"), new Writable("+1")),
			new(new("+00.00"), new Writable("+1")),
			new(new("+0"), new Writable("+0.1")),
			new(new("+00"), new Writable("+0.1")),
			new(new("+0.0"), new Writable("+0.1")),
			new(new("+0.00"), new Writable("+0.1")),
			new(new("+00.0"), new Writable("+0.1")),
			new(new("+00.00"), new Writable("+0.1")),
			new("-0"),
			new("-00"),
			new("-0."),
			new("-00."),
			new("-0.0"),
			new("-0.00"),
			new("-00.0"),
			new("-00.00"),
			new("-0/+1"),
			new("-00/+1"),
			new("-0./+1"),
			new("-00./+1"),
			new("-0.0/+1"),
			new("-0.00/+1"),
			new("-00.0/+1"),
			new("-00.00/+1"),
			new("-0/+0.1"),
			new("-00/+0.1"),
			new("-0./+0.1"),
			new("-00./+00.1"),
			new("-0.0/+00.1"),
			new("-0.00/+0.1"),
			new("-00.0/+0.10"),
			new("-00.00/+0.10"),
			new(false, new("0")),
			new(false, new("00")),
			new(false, new("0.0")),
			new(false, new("0.00")),
			new(false, new("00.0")),
			new(false, new("00.00")),
			new(false, new("0"), new("0.1")),
			new(false, new("00"), new("0.1")),
			new(false, new("0.0"), new("0.1")),
			new(false, new("0.00"), new("0.1")),
			new(false, new("00.0"), new("0.1")),
			new(false, new("00.00"), new("0.1")),
			new(false, new("0"), new("0.1")),
			new(false, new("00"), new("0.1")),
			new(false, new("0.0"), new("0.1")),
			new(false, new("0.00"), new("0.1")),
			new(false, new("00.0"), new("0.1")),
			new(false, new("00.00"), new("0.1")),
			new(new("-0")),
			new(new("-00")),
			new(new("-0.0")),
			new(new("-0.00")),
			new(new("-00.0")),
			new(new("-00.00")),
			new(new Writable("-0"), new Positive("0.1")),
			new(new Writable("-00"), new Positive("0.1")),
			new(new Writable("-0.0"), new Positive("0.1")),
			new(new Writable("-0.00"), new Positive("0.1")),
			new(new Writable("-00.0"), new Positive("0.1")),
			new(new Writable("-00.00"), new Positive("0.1")),
			new(new Writable("-0"), new Positive("0.1")),
			new(new Writable("-00"), new Positive("0.1")),
			new(new Writable("-0.0"), new Positive("0.1")),
			new(new Writable("-0.00"), new Positive("0.1")),
			new(new Writable("-00.0"), new Positive("0.1")),
			new(new Writable("-00.00"), new Positive("0.1")),
			new(new("-0"), new Writable("+1")),
			new(new("-00"), new Writable("+1")),
			new(new("-0.0"), new Writable("+1")),
			new(new("-0.00"), new Writable("+1")),
			new(new("-00.0"), new Writable("+1")),
			new(new("-00.00"), new Writable("+1")),
			new(new("-0"), new Writable("+0.1")),
			new(new("-00"), new Writable("+0.1")),
			new(new("-0.0"), new Writable("+0.1")),
			new(new("-0.00"), new Writable("+0.1")),
			new(new("-00.0"), new Writable("+0.1")),
			new(new("-00.00"), new Writable("+0.1"))
		];

		foreach (Rational zero in zeros)
			Assert.AreEqual(empty, zero);

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	//[TestMethod]
	public void StringConstructor()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		string[] tests =
		[
			/*null!, "", "+", "-", ".", "+.", "-.",
			".123", "+.123", "-.123",
			"/", "+/", "-/", "./", "+/.", "-/.",
			"a123", "123a", "12a3",
			"+a123", "+123a", "+12a3",
			"-a123", "-123a", "-12a3",
			".12/3", "+/.123", "-.123/",
			"0/0", "+0/0", "-0/0", "0/+0", "0/-0", "+0/+0", "+0/-0", "-0/+0", "-0/-0"*/
		];

		for (int j = 0; j < tests.Length; ++j)
			Assert.ThrowsException<ArgumentException>(() => new Rational(tests[j]));

		string[] tokens1, tokens2;
		bool sign1, sign2;
		Positive positive11, positive21;
		Positive? positive12, positive22;
		Writable writable1, writable2;
		Rational number1, number2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			tokens1 = item.Number1.Split('/');
			tokens2 = item.Number2.Split('/');

			positive11 = new Positive(tokens1[0].Replace("+", "").Replace("-", ""));
			positive12 = tokens1.Length == 2 ? new Positive(tokens1[1].Replace("+", "").Replace("-", "")) : "1";

			positive21 = new Positive(tokens2[0].Replace("+", "").Replace("-", ""));
			positive22 = tokens2.Length == 2 ? new Positive(tokens2[1].Replace("+", "").Replace("-", "")) : "1";

			sign1 = positive11.IsZero || Sign(tokens1[0]) == (tokens1.Length != 2 || Sign(tokens1[1]));
			sign2 = positive21.IsZero || Sign(tokens2[0]) == (tokens2.Length != 2 || Sign(tokens2[1]));

			writable1 = new Writable(sign1, positive11);
			writable2 = new Writable(sign2, positive21);

			number1 = new Rational(item.Number1);
			number2 = new Rational(item.Number2);

			Assert.AreEqual(sign1, number1.Sign);
			Assert.AreEqual(writable1.Value.ToString(), number1.Numerator.ToString());
			Assert.AreEqual(positive12.ToString(), number1.Denominator is null ? "1" : number1.Denominator.ToString());

			Assert.AreEqual(sign2, number2.Sign);
			Assert.AreEqual(writable2.Value.ToString(), number2.Numerator.ToString());
			Assert.AreEqual(positive22.ToString(), number2.Denominator is null ? "1" : number2.Denominator.ToString());
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	//[TestMethod]
	public void PositiveConstructor()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		string[] tokens1, tokens2;
		bool sign1, sign2;
		Positive positive11, positive21;
		Positive? positive12, positive22;
		Rational number1, number2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			tokens1 = item.Number1.Split('/');
			tokens2 = item.Number2.Split('/');

			positive11 = new Positive(tokens1[0].Replace("+", "").Replace("-", ""));
			positive12 = tokens1.Length == 2 ? new Positive(tokens1[1].Replace("+", "").Replace("-", "")) : "1";

			positive21 = new Positive(tokens2[0].Replace("+", "").Replace("-", ""));
			positive22 = tokens2.Length == 2 ? new Positive(tokens2[1].Replace("+", "").Replace("-", "")) : "1";

			sign1 = positive11.IsZero || Sign(tokens1[0]) == (tokens1.Length != 2 || Sign(tokens1[1]));
			sign2 = positive21.IsZero || Sign(tokens2[0]) == (tokens2.Length != 2 || Sign(tokens2[1]));

			number1 = new Rational(sign1, positive11, positive12);
			number2 = new Rational(sign2, positive21, positive22);

			Assert.AreEqual(sign1, number1.Sign);
			Assert.AreEqual(positive11.ToString(), number1.Numerator.ToString());
			Assert.AreEqual(positive12.ToString(), number1.Denominator is null ? "1" : number1.Denominator.ToString());

			Assert.AreEqual(sign2, number2.Sign);
			Assert.AreEqual(positive21.ToString(), number2.Numerator.ToString());
			Assert.AreEqual(positive22.ToString(), number2.Denominator is null ? "1" : number2.Denominator.ToString());
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	//[TestMethod]
	public void WritableConstructor()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		string[] tokens1, tokens2;
		Writable writable1, writable2;
		Positive? positive1, positive2;
		Rational number1, number2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			tokens1 = item.Number1.Split('/');
			tokens2 = item.Number2.Split('/');

			writable1 = new Writable(Sign(tokens1[0]) == (tokens1.Length != 2 || Sign(tokens1[1])), tokens1[0].Replace("+", "").Replace("-", ""));
			writable2 = new Writable(Sign(tokens2[0]) == (tokens2.Length != 2 || Sign(tokens2[1])), tokens2[0].Replace("+", "").Replace("-", ""));

			positive1 = tokens1.Length == 2 ? new Positive(tokens1[1].Replace("+", "").Replace("-", "")) : "1";
			positive2 = tokens2.Length == 2 ? new Positive(tokens2[1].Replace("+", "").Replace("-", "")) : "1";

			number1 = new Rational(writable1, positive1);
			number2 = new Rational(writable2, positive2);

			Assert.AreEqual(writable1.Value.ToString(), number1.Numerator.ToString());
			Assert.AreEqual(positive1.ToString(), number1.Denominator is null ? "1" : number1.Denominator.ToString());

			Assert.AreEqual(writable2.Value.ToString(), number2.Numerator.ToString());
			Assert.AreEqual(positive2.ToString(), number2.Denominator is null ? "1" : number2.Denominator.ToString());
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	//[TestMethod]
	public void ToStringMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		string[] tokens1, tokens2;
		Writable writable1, writable2;
		Positive? positive1, positive2;
		Rational number1, number2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			tokens1 = item.Number1.Split('/');
			tokens2 = item.Number2.Split('/');

			writable1 = new Writable(Sign(tokens1[0]) == (tokens1.Length != 2 || Sign(tokens1[1])), tokens1[0].Replace("+", "").Replace("-", ""));
			writable2 = new Writable(Sign(tokens2[0]) == (tokens2.Length != 2 || Sign(tokens2[1])), tokens2[0].Replace("+", "").Replace("-", ""));

			positive1 = tokens1.Length == 2 ? new Positive(tokens1[1].Replace("+", "").Replace("-", "")) : null;
			positive2 = tokens2.Length == 2 ? new Positive(tokens2[1].Replace("+", "").Replace("-", "")) : null;

			number1 = new Rational(item.Number1);
			number2 = new Rational(item.Number2);

			Assert.AreEqual($"{writable1}{(positive1 is Positive p1 && p1 != "1" ? $"/{p1}" : "")}", number1.ToString());
			Assert.AreEqual($"{writable2}{(positive2 is Positive p2 && p2 != "1" ? $"/{p2}" : "")}", number2.ToString());
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	[TestMethod]
	public void EqualsMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		string[] tokens1, tokens2;
		Rational numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			tokens1 = item.Number1.Split('/');
			tokens2 = item.Number2.Split('/');

			numberCharacters1 = new(item.Number1);
			numberDigits1 = new Rational(new Writable(tokens1[0]), (tokens1.Length < 2 ? null : new Writable(tokens1[1])));

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new Rational(new Writable(tokens2[0]), (tokens2.Length < 2 ? null : new Writable(tokens2[1])));

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Equal, numberDigits1 == numberDigits2);
			Assert.AreEqual(numberDigits1 == numberDigits2, numberDigits2 == numberDigits1);
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		string[] tokens1, tokens2;
		Rational numberDigits1, numberDigits2, numberCharacters1, numberCharacters2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			tokens1 = item.Number1.Split('/');
			tokens2 = item.Number2.Split('/');

			numberCharacters1 = new(item.Number1);
			numberDigits1 = new Rational(new Writable(tokens1[0]), (tokens1.Length < 2 ? null : new Writable(tokens1[1])));

			numberCharacters2 = new(item.Number2);
			numberDigits2 = new Rational(new Writable(tokens2[0]), (tokens2.Length < 2 ? null : new Writable(tokens2[1])));

			Assert.AreEqual(numberCharacters1, numberDigits1);
			Assert.AreEqual(numberCharacters2, numberDigits2);

			Assert.AreEqual(item.Greater, Rational.GreaterThan(numberCharacters1, numberCharacters2));
			Assert.AreEqual(item.Greater, Rational.GreaterThan(numberDigits1, numberDigits2));
			Assert.AreEqual(item.Greater, Rational.GreaterThan(numberCharacters1, numberDigits2));
			Assert.AreEqual(item.Greater, Rational.GreaterThan(numberDigits1, numberCharacters2));
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}


	[TestMethod]
	public void AddMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		Rational rational1, rational2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			Assert.AreEqual(item.Add, Rational.Add(rational1, rational2));
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	[TestMethod]
	public void SubtractMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		Rational rational1, rational2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			Assert.AreEqual(item.Sub, Rational.Subtract(rational1, rational2));
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		Rational rational1, rational2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			Assert.AreEqual(item.Mul, Rational.Multiply(rational1, rational2));
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	[TestMethod]
	public void DivideMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = false;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		int length;
		Rational rational1, rational2;
		string result, expected;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			if (item.Div == "ERROR")
				Assert.ThrowsException<DivideByZeroException>(() => Rational.Divide(rational1, rational2));
			else if (item.Div != "BIG")
			{
				result = Rational.Divide(rational1, rational2).ToString();
				expected = (new Rational(item.Div)).ToString();

				length = Math.Min(expected.Length, result.Length);

				Assert.AreEqual(expected[..length], result[..length]);
			}
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	[TestMethod]
	public void PowerMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		Rational rational1, rational2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			if (item.Pow == "ERROR")
				try
				{
					Rational.Power(rational1, rational2);
				}
				catch (Exception e)
				{
					if (!(e is NotImplementedException || e is NotSupportedException))
						Assert.Fail();
				}
			else if (item.Pow != "BIG")
				Assert.AreEqual(new Rational(item.Pow), Rational.Power(rational1, rational2));
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}

	[TestMethod]
	public void RootMethod()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = false;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;
		char separator = Rational.Separator;
		Rational.Separator = '.';

		int length;
		Rational rational1, rational2, result, remainder;
		string expected;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			if (item.Root == "ERROR")
				try
				{
					Rational.Root(rational1, rational2);
				}
				catch (Exception e)
				{
					if (!(e is NotImplementedException || e is NotSupportedException))
						Assert.Fail();
				}
			else if (item.Root != "BIG")
			{
				(result, remainder) = Rational.Root(rational1, rational2, fractionCalculationLength);
				expected = (new Rational(item.Root)).ToString();

				length = Math.Min(expected.Length, result.ToString().Length);

				Assert.AreEqual(expected[..length], result.ToString()[..length]);
				Assert.AreEqual(rational1.ToString(), ((result ^ item.Number2) + remainder).ToString());
			}
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
		Rational.Separator = separator;
	}
}