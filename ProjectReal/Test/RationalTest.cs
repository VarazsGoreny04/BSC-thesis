using ProjectReal.Number;
using System;

namespace Test;

[TestClass]
public class RationalTest
{
	private const int FRACTION_CALCULATION_LENGTH = 10;

	private readonly bool fractionalFormat;
	private readonly bool writeSign;

	public RationalTest()
	{
		fractionalFormat = Rational.FractionalFormat;
		writeSign = Rational.WriteSign;

		Rational.FractionalFormat = true;
		Rational.WriteSign = true;
	}

	[TestCleanup()]
	public void Cleanup()
	{
		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
	}

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
			new(new Writable("+0")),
			new(new Writable("+00")),
			new(new Writable("+0.0")),
			new(new Writable("+0.00")),
			new(new Writable("+00.0")),
			new(new Writable("+00.00")),
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
			new(new Writable("-0")),
			new(new Writable("-00")),
			new(new Writable("-0.0")),
			new(new Writable("-0.00")),
			new(new Writable("-00.0")),
			new(new Writable("-00.00")),
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
			new(new Writable("+0")),
			new(new Writable("+00")),
			new(new Writable("+0.0")),
			new(new Writable("+0.00")),
			new(new Writable("+00.0")),
			new(new Writable("+00.00")),
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
			new(new Writable("-0")),
			new(new Writable("-00")),
			new(new Writable("-0.0")),
			new(new Writable("-0.00")),
			new(new Writable("-00.0")),
			new(new Writable("-00.00")),
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
	}

	[TestMethod]
	public void StringConstructor()
	{
		bool fractionalFormat = Rational.FractionalFormat;
		Rational.FractionalFormat = true;
		bool writeSign = Rational.WriteSign;
		Rational.WriteSign = true;

		string[] zeroTests = ["0/0", "+0/0", "-0/0", "0/+0", "0/-0", "+0/+0", "+0/-0", "-0/+0", "-0/-0"];

		string[] tests =
		[
			"", "+", "-", ".", "+.", "-.",
			".123", "+.123", "-.123",
			"/", "+/", "-/", "./", "+/.", "-/.",
			"a123", "123a", "12a3",
			"+a123", "+123a", "+12a3",
			"-a123", "-123a", "-12a3",
			".12/3", "+/.123", "-.123/"
		];

		Assert.ThrowsException<NullReferenceException>(() => new Rational(null!));

		for (int i = 0; i < zeroTests.Length; ++i)
			Assert.ThrowsException<DivideByZeroException>(() => new Rational(zeroTests[i]));

		for (int i = 0; i < tests.Length; ++i)
			Assert.ThrowsException<ArgumentException>(() => new Rational(tests[i]));

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
			Assert.AreEqual((new Writable(sign1, positive11 / positive12)).ToString(),
				(new Writable(sign1, number1.Numerator / (number1.Denominator ?? "1")).ToString()));

			Assert.AreEqual(sign2, number2.Sign);
			Assert.AreEqual((new Writable(sign2, positive21 / positive22)).ToString(),
				(new Writable(sign2, number2.Numerator / (number2.Denominator ?? "1")).ToString()));
		}

		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
	}

	[TestMethod]
	public void PositiveConstructor()
	{
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
			Assert.AreEqual(sign2, number2.Sign);
			Assert.AreEqual((new Rational(item.Number1)).ToString(), number1.ToString());
			Assert.AreEqual((new Rational(item.Number2)).ToString(), number2.ToString());
		}
	}

	[TestMethod]
	public void WritableConstructor()
	{
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

			Assert.AreEqual(writable1.Sign, number1.Sign);
			Assert.AreEqual(writable2.Sign, number2.Sign);
			Assert.AreEqual((new Rational(item.Number1)).ToString(), number1.ToString());
			Assert.AreEqual((new Rational(item.Number2)).ToString(), number2.ToString());
		}
	}

	[TestMethod]
	public void ToStringMethod()
	{
		Rational.FractionalFormat = false;

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

			Assert.AreEqual((positive1 is not null ? writable1 / positive1 : writable1).ToString(), number1.ToString());
			Assert.AreEqual((positive2 is not null ? writable2 / positive2 : writable2).ToString(), number2.ToString());
		}
	}

	[TestMethod]
	public void EqualsMethod()
	{
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
	}

	[TestMethod]
	public void GreaterThanMethod()
	{
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
	}


	[TestMethod]
	public void AddMethod()
	{
		Rational rational1, rational2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			Assert.AreEqual(item.Add, Rational.Add(rational1, rational2));
		}
	}

	[TestMethod]
	public void SubtractMethod()
	{
		Rational rational1, rational2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			Assert.AreEqual(item.Sub, Rational.Subtract(rational1, rational2));
		}
	}

	[TestMethod]
	public void MultiplyMethod()
	{
		Rational rational1, rational2;

		foreach (RationalTestCase item in RationalTestCases.List)
		{
			rational1 = new(item.Number1);
			rational2 = new(item.Number2);

			Assert.AreEqual(item.Mul, Rational.Multiply(rational1, rational2));
		}
	}

	[TestMethod]
	public void DivideMethod()
	{
		Rational.FractionalFormat = false;

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
	}

	[TestMethod]
	public void PowerMethod()
	{
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
					if (!(e is NotImplementedException or NotSupportedException))
						Assert.Fail(e.Message);
				}
			else if (item.Pow != "BIG")
				Assert.AreEqual(new Rational(item.Pow), Rational.Power(rational1, rational2));
		}
	}

	[TestMethod]
	public void RootMethod()
	{
		int length;
		Rational rational1, rational2, result;
		Writable numeratorRemainder;
		Positive? denominatorRemainder;
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
					if (!(e is ArgumentException or ArgumentOutOfRangeException or DivideByZeroException or NotSupportedException or NotImplementedException)) // TODO
						Assert.Fail(e.Message);
				}
			else if (item.Root != "BIG")
			{
				(result, numeratorRemainder, denominatorRemainder) = Rational.Root(rational1, rational2, FRACTION_CALCULATION_LENGTH);
				expected = (new Rational(item.Root)).ToString();

				length = Math.Min(expected.Length, result.ToString().Length);

				Assert.AreEqual(expected[..length], result.ToString()[..length]);
				Assert.AreEqual(rational1, ((new Rational(result.Sign, result.Numerator) ^ rational2) + numeratorRemainder) /
					((result?.Denominator ?? Digit.ONE) ^ rational2) + denominatorRemainder);
			}
		}
	}
}