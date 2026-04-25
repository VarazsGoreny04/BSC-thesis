using Calculators.EuclideanSpace;
using ProjectReal.Number;

namespace CalculatorTests.EuclideanSpace.VectorOperations;

[TestClass]
public class VectorOperationsTest
{
	private readonly bool fractionalFormat;
	private readonly bool writeSign;

	private readonly int[] testDimensions;
	private readonly Rational[] testNumbers;

	public VectorOperationsTest()
	{
		fractionalFormat = Rational.FractionalFormat;
		writeSign = Rational.WriteSign;

		Rational.FractionalFormat = true;
		Rational.WriteSign = true;

		testDimensions = [1, 2, 3, 4, 5, 6, 7];

		testNumbers = ["0", "1", "-1", "2", "15", "1/2", "-3/2", "-3/-2"];
	}

	[TestCleanup()]
	public void CleanUp()
	{
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
	public void EqualTest()
	{
		foreach (TestVector item in TestVectors.List)
		{
			bool result1 = VectorOperations<Rational>.Equals(item.Vector1, item.Vector2);
			bool result2 = VectorOperations<Rational>.Equals(item.Vector2, item.Vector1);

			bool equal = true;
			for (int i = item.Vector1.Length - 1; i >= 0; --i)
			{
				if (item.Vector1[i] != item.Vector2[i])
					equal = false;
			}

			Assert.AreEqual(equal, result1);
			Assert.AreEqual(equal, result2);
			Assert.IsTrue(VectorOperations<Rational>.Equals(item.Vector1, item.Vector1));
			Assert.IsTrue(VectorOperations<Rational>.Equals(item.Vector2, item.Vector2));
		}
	}

	[TestMethod]
	public void GreaterThanTest()
	{
		throw new NotImplementedException();
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