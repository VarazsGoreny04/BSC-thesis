using ProjectReal.Number;
using System;

namespace Test;

[TestClass]
public class RationalTest
{
	private const int FRACTION_CALCULATION_LENGTH = 10;
	private const int VALIDATE_UNTIL = 500;

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

	private static bool Compare(string shorter, string longer)
	{
		int sLength = shorter.Length;

		for (int i = 0; i < sLength; ++i)
		{
			if (shorter[i] != longer[i])
				return false;
		}

		return true;
	}

	private static int? Validate(Func<int, Rational> func, int? maxFractionCalculationLength = null)
	{
		int fCL = maxFractionCalculationLength ?? FRACTION_CALCULATION_LENGTH;

		Rational.FractionCalculationLength = 0;
		Rational.FractionalFormat = false;

		string last = func.Invoke(0).ToString();
		string now;

		for (int i = 1; i < fCL; ++i)
		{
			Rational.FractionCalculationLength = i;

			now = func.Invoke(i).ToString();

			if (last.Length != 2 && last[1] != '0' && !Compare(last, now))
				return i;

			last = now;
		}

		Rational.FractionCalculationLength = FRACTION_CALCULATION_LENGTH;

		return null;
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
			{
				try
				{
					Rational.Power(rational1, rational2);
				}
				catch (Exception e)
				{
					if (!(e is NotImplementedException or NotSupportedException))
						Assert.Fail(e.Message);
				}
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
			{
				try
				{
					Rational.Root(rational1, rational2);
				}
				catch (Exception e)
				{
					if (!(e is ArgumentException or ArgumentOutOfRangeException or DivideByZeroException or NotSupportedException or NotImplementedException)) // TODO
						Assert.Fail(e.Message);
				}
			}
			else if (item.Root != "BIG")
			{
				(result, numeratorRemainder, denominatorRemainder) = Rational.Root(rational1, rational2, FRACTION_CALCULATION_LENGTH);
				expected = (new Rational(item.Root)).ToString();

				length = Math.Min(expected.Length, result.ToString().Length);

				Assert.AreEqual(expected[..length], result.ToString()[..length]);
				Assert.AreEqual(rational1, ((new Rational(result.Sign, result.Numerator) ^ rational2) + numeratorRemainder) /
					((result?.Denominator ?? "1") ^ rational2) + denominatorRemainder);
			}
		}
	}

	[TestMethod]
	public void PiMethod()
	{

		Rational.FractionalFormat = false;
		Rational.FractionCalculationLength = VALIDATE_UNTIL;

		string pi500 = "+3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914127372458700660631558817488152092096282925409171536436789259036001133053054882046652138414695194151160943305727036575959195309218611738193261179310511854807446237996274956735188575272489122793818301194912";

		Assert.AreEqual(pi500, Rational.Pi().ToString());

		int? result = Validate((i) => Rational.Pi(i), VALIDATE_UNTIL);

		Assert.IsNull(result, $"The mismatching fraction calculation length: {result}");
	}

	[TestMethod]
	public void EMethod()
	{
		Rational.FractionalFormat = false;
		Rational.FractionCalculationLength = VALIDATE_UNTIL;

		string e500 = "+2.71828182845904523536028747135266249775724709369995957496696762772407663035354759457138217852516642742746639193200305992181741359662904357290033429526059563073813232862794349076323382988075319525101901157383418793070215408914993488416750924476146066808226480016847741185374234544243710753907774499206955170276183860626133138458300075204493382656029760673711320070932870912744374704723069697720931014169283681902551510865746377211125238978442505695369677078544996996794686445490598793163688923009879312";

		Assert.AreEqual(e500, Rational.E().ToString());

		int? result = Validate((index) => Rational.E(index), VALIDATE_UNTIL);

		Assert.IsNull(result, $"The mismatching fraction calculation length at index {result}");
	}

	[TestMethod]
	public void ExpMethod()
	{
		/*Rational.FractionalFormat = false;
		Rational.FractionCalculationLength = VALIDATE_UNTIL; // TODO

		string e500 = "+2.71828182845904523536028747135266249775724709369995957496696762772407663035354759457138217852516642742746639193200305992181741359662904357290033429526059563073813232862794349076323382988075319525101901157383418793070215408914993488416750924476146066808226480016847741185374234544243710753907774499206955170276183860626133138458300075204493382656029760673711320070932870912744374704723069697720931014169283681902551510865746377211125238978442505695369677078544996996794686445490598793163688923009879312";

		Assert.AreEqual(e500, Rational.Exp().ToString());

		int? result = Validate((index) => Rational.Exp(index), VALIDATE_UNTIL);

		Assert.IsNull(result, $"The mismatching fraction calculation length at index {result}");*/
	}

	[TestMethod]
	public void SinMethod()
	{
		Rational.FractionalFormat = false;
		Rational.FractionCalculationLength = VALIDATE_UNTIL; // TODO

		(string Value, string Result)[] sin500 = [
			("0.7", "+0.64421768723769105367261435139872018306581384457368964474396308809382997544967566471462669216875770535830322938026758837931012921299009896152536841962607931318942607902135379854764417926576982970794170722618316661902610581990034084559568578220692451989237913208969760409593884604198504823784026801979207409119660028365588825686142113215818673628698674887425223588290679791221027895908940209312150912560824900180736633229225085458514622865564772955499837889892205942927456894623141084293121749550952568"),
			("1.5", "+0.99749498660405443094172337114148732270665142592211582194997482405934520970787064838945099773041098011758362107434377781983525546591264444329546279689323805522160638220984074127796544460850134624817768566436445817635301689308257024588280203501076619043315868613565949107333256196602810234007282890903482704365723171372349444442143228926821254741313930950955046030825225905724517201642900998834005334399412727399317195646860182530468957660873233517789117664909151699216377807789314516457368918435413545"),
			("3.1", "+0.0415806624332905791946982715966731005546134229638067506480090007658845511597234572946939472109701753234146820689647554061961178531850555155363675021848588063757273930034415001917096097245368443124561892928747946192223166201207265076276310153915113498725161318322464587005418325792881586916599054657483105710943454527306743650927259450082752433630444898500951578614990163537044313632093820962685647218358467710031326204439263461600287834297610927289689443771147430877018689316415435910390283262674607"),
			("3.14", "+0.00159265291648695254054143632444326144324052781902687418488050836712834196972681655366511928190163665945901073651890807742330828497286734711916055033342226936635372057861675042546212429020214049541556941676161958526723326440169002833679968580146111244433080452248969293081686143670824488614957815833872117155292519673920362827487851859267763880494200336235166475387136120431729701911579536140058073244047691803398621377159696319471736233063383861669575729890915481642847507004301597443661956661443519"),
			("3.1", "+0.0415806624332905791946982715966731005546134229638067506480090007658845511597234572946939472109701753234146820689647554061961178531850555155363675021848588063757273930034415001917096097245368443124561892928747946192223166201207265076276310153915113498725161318322464587005418325792881586916599054657483105710943454527306743650927259450082752433630444898500951578614990163537044313632093820962685647218358467710031326204439263461600287834297610927289689443771147430877018689316415435910390283262674607")
		];

		for (int i = 0; i < sin500.Length; ++i)
			Assert.AreEqual(sin500[i].Result, Rational.Sin(sin500[i].Value).ToString());


		for (int i = 0; i < sin500.Length; ++i)
		{
			int? result = Validate((index) => Rational.Sin(sin500[i].Value, index), VALIDATE_UNTIL);

			Assert.IsNull(result, $"The mismatching fraction calculation length for {sin500[i].Value} at index {result}");
		}
	}

	[TestMethod]
	public void CosMethod()
	{
		Rational.FractionalFormat = false;
		Rational.FractionCalculationLength = VALIDATE_UNTIL; // TODO

		(string Value, string Result)[] cos500 = [
			("+0.7", "+0.76484218728448842625585999019186490926821055037370335607293245825206587504371016303120190005266833274117721095897217354736453765475206891611893165525090317008240280866605450203793757586681896127993094652503134260055567013833042767571932675763536304748768570260451399737423505782145140644344929477532172947663713606103327167055524597591220105084217912989044212417526173957813973590220386517917607454354425293563085405963120392422624077565731417870204632859369494483782533504246673199106789827362272322"),
			("+1.5", "+0.07073720166770291008818985143426870908509102756334686942264541719092293457350070064693529895540169675279129912604263117529024429485823099685141203692193081335068371073046400711109012152922241729287533576643487075739722221670932321032915611013359836387928529940355434432640590285616354501265193323054352262217583913254472178634033547667507328179530782918846281893323009199066584541412496270775559683872851056671933707436789722986594514121111815881957310190319929759794827343420679291897326257397140265"),
			("+3.1", "-0.99913515027327946449237605454146626283664166994794274354471598254947926765028113804955321122820034130403416906253712184070125161528164856170756128666216664022143938735088611149765573050710062882752539602196765953937141629236042162092471209179199284484356531965522489012334252189986294021391848126152139718064678418793855698529367155576552788183632981098005166143223461069416182687094935317202889897158132902019436897351146232036259650323197635080108103260232056311127285978914749701892294307666313966"),
			("+3.14", "-0.99999873172753954528511430634504998385450843939212093203930006585565064720104405353134091681055956857076148390338784972502693587186235997917113459527226351929205976595865197642911074124813659065051719569109691787604859155355370875852208193409877060078003191998351845737726613453739846549507388204655492248766421329363470079828030595298241233555039572070911742058368259996770119132273739211456288835387338809941901407564210185384186914489704995310452204763343353381497356473647707576271697793956205887"),
			("+6.28", "+0.99999492691337521120835293804732270895196916047194905749344068724856502867448504153004751757463200997669845856149925997658283861351165731492494496050155382146409638109491827389633659445766418781866347981307162610767039360710539764174353778180705444577118582760247114038289809666665255894994711270968944181482596986855558684961757279256269847819648732350523110031208442196910434299634489099194473841964656787503472494209800307693115194589526723693573239648270665334589510906346673669641979997915142437")
		];

		for (int i = 0; i < cos500.Length; ++i)
			Assert.AreEqual(cos500[i].Result, Rational.Cos(cos500[i].Value).ToString());


		for (int i = 0; i < cos500.Length; ++i)
		{
			int? result = Validate((index) => Rational.Cos(cos500[i].Value, index), VALIDATE_UNTIL);

			Assert.IsNull(result, $"The mismatching fraction calculation length for {cos500[i].Value} at index {result}");
		}
	}
}