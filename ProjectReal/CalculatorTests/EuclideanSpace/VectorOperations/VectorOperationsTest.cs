using Calculators.EuclideanSpace;
using ProjectReal.Number;

namespace CalculatorTests.EuclideanSpace.VectorOperations;

[TestClass]
public class VectorOperationsTest
{
	private readonly int fractionCalculationLength;
	private readonly bool fractionalFormat;
	private readonly bool writeSign;

	private readonly int[] testDimensions;
	private readonly Rational[] testNumbers;

	public VectorOperationsTest()
	{
		fractionCalculationLength = Rational.FractionCalculationLength;
		fractionalFormat = Rational.FractionalFormat;
		writeSign = Rational.WriteSign;

		Rational.FractionCalculationLength = 10;
		Rational.FractionalFormat = true;
		Rational.WriteSign = true;

		testDimensions = [1, 2, 3, 4, 5, 6, 7];

		testNumbers = ["0", "1", "-1", "2", "15", "1/2", "-3/2", "-3/-2"];
	}

	[TestCleanup()]
	public void CleanUp()
	{
		Rational.FractionCalculationLength = fractionCalculationLength;
		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
	}

	[TestMethod]
	public void FullTest()
	{
		foreach (int n in testDimensions)
		{
			foreach (Rational testNumber in testNumbers)
			{
				Rational[] vector = VectorOperations<Rational>.Full(n, testNumber);

				for (int i = 0; i < n; ++i)
					Assert.AreEqual(testNumber, vector[i]);
			}
		}
	}

	[TestMethod]
	public void ZerosTest()
	{
		foreach (int n in testDimensions)
		{
			Rational[] vector = VectorOperations<Rational>.Zeros(n);

			for (int i = 0; i < n; ++i)
				Assert.AreEqual(Digit.ZERO, vector[i]);
		}
	}

	[TestMethod]
	public void OnesTest()
	{
		foreach (int n in testDimensions)
		{
			Rational[] vector = VectorOperations<Rational>.Ones(n);

			for (int i = 0; i < n; ++i)
				Assert.AreEqual(Digit.ONE, vector[i]);
		}
	}

	[TestMethod]
	public void DuplicateTest()
	{
		foreach (TestVector item in TestVectors.List)
		{
			Rational[] result1 = VectorOperations<Rational>.Duplicate(item.Vector1);

			for (int n = item.Vector1.Length - 1; n >= 0; --n)
				Assert.AreEqual(item.Vector1[n], result1[n]);

			Rational[] result2 = VectorOperations<Rational>.Duplicate(item.Vector2);

			for (int n = item.Vector2.Length - 1; n >= 0; --n)
				Assert.AreEqual(item.Vector2[n], result2[n]);
		}
	}

	[TestMethod]
	public void ScaleTest()
	{
		foreach (TestVector item in TestVectors.List)
		{
			foreach (Rational testNumber in testNumbers)
			{
				Rational[] result1 = VectorOperations<Rational>.Scale(item.Vector1, testNumber);

				for (int n = item.Vector1.Length - 1; n >= 0; --n)
					Assert.AreEqual(item.Vector1[n] * testNumber, result1[n]);

				Rational[] result2 = VectorOperations<Rational>.Scale(item.Vector2, testNumber);

				for (int n = item.Vector2.Length - 1; n >= 0; --n)
					Assert.AreEqual(item.Vector2[n] * testNumber, result2[n]);
			}
		}
	}

	[TestMethod]
	public void MagnitudeTest()
	{
		Rational epsilon = $"0.{new string('0', fractionCalculationLength - 3)}1";

		foreach (TestVector item in TestVectors.List)
		{
			Rational result1 = VectorOperations<Rational>.Magnitude(item.Vector1);
			Rational result2 = VectorOperations<Rational>.Magnitude(item.Vector2);

			Rational expected1 = item.Vector1.Select(x => x * x).Aggregate((a, b) => a + b);
			Rational expected2 = item.Vector2.Select(x => x * x).Aggregate((a, b) => a + b);

			Rational difference1 = Rational.Abs(expected1 - (result1 * result1));
			Rational difference2 = Rational.Abs(expected2 - (result2 * result2));


			Assert.IsTrue(difference1 <= epsilon, $"\n\nExpected: {~expected1}\n\nActual: {result1}");

			Assert.IsTrue(difference2 <= epsilon, $"\n\nExpected: {~expected2}\n\nActual: {result2}");
		}
	}

	[TestMethod]
	public void InnerProductTest()
	{
		foreach (TestVector item in TestVectors.List)
		{
			Rational result1 = VectorOperations<Rational>.InnerProduct(item.Vector1, item.Vector2);
			Rational result2 = VectorOperations<Rational>.InnerProduct(item.Vector2, item.Vector1);

			Assert.AreEqual(item.Vector1.Zip(item.Vector2).Select(x => x.First * x.Second).Aggregate((a, b) => a + b), result1);

			Assert.AreEqual(item.Vector2.Zip(item.Vector1).Select(x => x.First * x.Second).Aggregate((a, b) => a + b), result2);
		}
	}

	[TestMethod]
	public void OuterProductTest()
	{
		foreach (TestVector item in TestVectors.List)
		{
			Rational[,] result = VectorOperations<Rational>.OuterProduct(item.Vector1, item.Vector2);

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.OuterProduct1, result),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.OuterProduct1)}\n\nActual: {MatrixOperations<Rational>.ToString(result)}");
		}
	}

	[TestMethod]
	public void EqualTest()
	{
		foreach (TestVector item in TestVectors.List)
		{
			bool result1 = VectorOperations<Rational>.Equals(item.Vector1, item.Vector2);
			bool result2 = VectorOperations<Rational>.Equals(item.Vector2, item.Vector1);

			bool equal = item.Vector1.Length == item.Vector2.Length && item.Vector1.ToList().Zip(item.Vector2.ToList()).All(x => x.First == x.Second);

			Assert.AreEqual(equal, result1);
			Assert.AreEqual(equal, result2);
			Assert.IsTrue(VectorOperations<Rational>.Equals(item.Vector1, item.Vector1));
			Assert.IsTrue(VectorOperations<Rational>.Equals(item.Vector2, item.Vector2));
		}
	}

	[TestMethod]
	public void AddTest()
	{
		foreach (TestVector item in TestVectors.List)
		{
			Rational[] result = VectorOperations<Rational>.Add(item.Vector1, item.Vector2);

			Assert.IsTrue(VectorOperations<Rational>.Equals(item.Add, result),
				$"\n\nExpected: {VectorOperations<Rational>.ToString(item.Add)}\n\nActual: {VectorOperations<Rational>.ToString(result)}");
		}
	}

	[TestMethod]
	public void SubtractTest()
	{
		foreach (TestVector item in TestVectors.List)
		{
			Rational[] result = VectorOperations<Rational>.Subtract(item.Vector1, item.Vector2);

			Assert.IsTrue(VectorOperations<Rational>.Equals(item.Sub, result),
				$"\n\nExpected: {VectorOperations<Rational>.ToString(item.Sub)}\n\nActual: {VectorOperations<Rational>.ToString(result)}");
		}
	}
}