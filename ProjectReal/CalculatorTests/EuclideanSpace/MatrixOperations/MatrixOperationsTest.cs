using Calculators.EuclideanSpace;
using ProjectReal.Number;
using System.Diagnostics;

namespace CalculatorTests.EuclideanSpace.MatrixOperations;

[TestClass]
public class MatrixOperationsTest
{
	private readonly bool fractionalFormat;
	private readonly bool writeSign;

	private readonly (int N, int M)[] testDimensions;
	private readonly Rational[] testNumbers;

	public MatrixOperationsTest()
	{
		fractionalFormat = Rational.FractionalFormat;
		writeSign = Rational.WriteSign;

		Rational.FractionalFormat = true;
		Rational.WriteSign = true;

		testDimensions =
		[
			(1, 1),
			(2, 1),
			(1, 2),
			(2, 2),
			(3, 2),
			(2, 3),
			(3, 3),
			(4, 3),
			(3, 4),
			(4, 4)
		];

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
		foreach ((int n, int m) in testDimensions)
		{
			foreach (Rational testNumber in testNumbers)
			{
				Rational[,] matrix = MatrixOperations<Rational>.Full(n, m, testNumber);

				for (int i = 0; i < n; ++i)
				{
					for (int j = 0; j < m; ++j)
						Assert.AreEqual(testNumber, matrix[i, j]);
				}
			}
		}
	}

	[TestMethod]
	public void ZerosTest()
	{
		foreach ((int n, int m) in testDimensions)
		{
			Rational[,] matrix = MatrixOperations<Rational>.Zeros(n, m);

			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < m; ++j)
					Assert.AreEqual(Digit.ZERO, matrix[i, j]);
			}
		}
	}

	[TestMethod]
	public void OnesTest()
	{
		foreach ((int n, int m) in testDimensions)
		{
			Rational[,] matrix = MatrixOperations<Rational>.Ones(n, m);

			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < m; ++j)
					Assert.AreEqual(Digit.ONE, matrix[i, j]);
			}
		}
	}

	[TestMethod]
	public void DiagonalTest()
	{
		foreach ((int n, int m) in testDimensions)
		{
			foreach (Rational testNumber in testNumbers)
			{
				Rational[,] matrix = MatrixOperations<Rational>.Diagonal(n, m, testNumber);

				for (int i = 0; i < n; ++i)
				{
					for (int j = 0; j < m; ++j)
						Assert.AreEqual(i == j ? testNumber : Digit.ZERO, matrix[i, j]);
				}
			}
		}
	}

	[TestMethod]
	public void IdentityTest()
	{
		foreach ((int n, int m) in testDimensions)
		{
			Rational[,] matrix = MatrixOperations<Rational>.Identity(n, m);

			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < m; ++j)
					Assert.AreEqual(i == j ? Digit.ONE : Digit.ZERO, matrix[i, j]);
			}
		}
	}

	[TestMethod]
	public void GetRowTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
			{
				Rational[] vector = MatrixOperations<Rational>.GetRow(item.Matrix1, n);

				for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix1[n, m], vector[m]);
			}

			for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
			{
				Rational[] vector = MatrixOperations<Rational>.GetRow(item.Matrix2, n);

				for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix2[n, m], vector[m]);
			}
		}
	}

	[TestMethod]
	public void GetColumnTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
			{
				Rational[] vector = MatrixOperations<Rational>.GetColumn(item.Matrix1, m);

				for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
					Assert.AreEqual(item.Matrix1[n, m], vector[n]);
			}

			for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
			{
				Rational[] vector = MatrixOperations<Rational>.GetColumn(item.Matrix2, m);

				for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
					Assert.AreEqual(item.Matrix2[n, m], vector[n]);
			}
		}
	}

	[TestMethod]
	public void DuplicateTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			Rational[,] result1 = MatrixOperations<Rational>.Duplicate(item.Matrix1);

			for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix1[n, m], result1[n, m]);
			}

			Rational[,] result2 = MatrixOperations<Rational>.Duplicate(item.Matrix2);

			for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix2[n, m], result2[n, m]);
			}
		}
	}

	[TestMethod]
	public void ScaleTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			foreach (Rational testNumber in testNumbers)
			{
				Rational[,] result1 = MatrixOperations<Rational>.Scale(item.Matrix1, testNumber);

				for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
				{
					for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
						Assert.AreEqual(item.Matrix1[n, m] * testNumber, result1[n, m]);
				}

				Rational[,] result2 = MatrixOperations<Rational>.Scale(item.Matrix2, testNumber);

				for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
				{
					for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
						Assert.AreEqual(item.Matrix2[n, m] * testNumber, result2[n, m]);
				}
			}
		}
	}

	[TestMethod]
	public void TransposeTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			Rational[,] result1 = MatrixOperations<Rational>.Transpose(item.Matrix1);

			for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix1[n, m], result1[m, n]);
			}

			Rational[,] result2 = MatrixOperations<Rational>.Transpose(item.Matrix2);

			for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix2[n, m], result2[m, n]);
			}
		}
	}

	[TestMethod]
	public void HorizontalConcatTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			Rational[,] result1 = MatrixOperations<Rational>.HorizontalConcat(item.Matrix1, item.Matrix2);

			for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix1[n, m], result1[n, m]);
			}
			for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix2[n, m], result1[n, m + item.Matrix1.GetLength(0)]);
			}

			Rational[,] result2 = MatrixOperations<Rational>.HorizontalConcat(item.Matrix2, item.Matrix1);

			for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix2[n, m], result2[n, m]);
			}
			for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix1[n, m], result2[n, m + item.Matrix2.GetLength(0)]);
			}
		}
	}

	[TestMethod]
	public void VerticalConcatTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			Rational[,] result1 = MatrixOperations<Rational>.VerticalConcat(item.Matrix1, item.Matrix2);

			for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix1[n, m], result1[n, m]);
			}
			for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix2[n, m], result1[n + item.Matrix1.GetLength(0), m]);
			}

			Rational[,] result2 = MatrixOperations<Rational>.VerticalConcat(item.Matrix2, item.Matrix1);

			for (int n = item.Matrix2.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix2.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix2[n, m], result2[n, m]);
			}
			for (int n = item.Matrix1.GetLength(0) - 1; n >= 0; --n)
			{
				for (int m = item.Matrix1.GetLength(1) - 1; m >= 0; --m)
					Assert.AreEqual(item.Matrix1[n, m], result2[n + item.Matrix2.GetLength(0), m]);
			}
		}
	}

	[TestMethod]
	public void EqualTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			bool result1 = MatrixOperations<Rational>.Equals(item.Matrix1, item.Matrix2);
			bool result2 = MatrixOperations<Rational>.Equals(item.Matrix2, item.Matrix1);

			bool equal = true;
			for (int i = item.Matrix1.GetLength(0) - 1; i >= 0; --i)
			{
				for (int j = item.Matrix1.GetLength(1) - 1; j >= 0; --j)
				{
					if (item.Matrix1[i, j] != item.Matrix2[i, j])
						equal = false;
				}
			}

			Assert.AreEqual(equal, result1);
			Assert.AreEqual(equal, result2);
			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.Matrix1, item.Matrix1));
			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.Matrix2, item.Matrix2));
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
		foreach (TestMatrix item in TestMatrices.List)
		{
			Rational[,] result = MatrixOperations<Rational>.Add(item.Matrix1, item.Matrix2);

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.Add, result),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Add)}\n\nActual: {MatrixOperations<Rational>.ToString(result)}");
		}
	}

	[TestMethod]
	public void SubtractTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			Rational[,] result = MatrixOperations<Rational>.Subtract(item.Matrix1, item.Matrix2);

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.Sub, result),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Sub)}\n\nActual: {MatrixOperations<Rational>.ToString(result)}");
		}
	}

	[TestMethod]
	public void ProductTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			Rational[,] result = MatrixOperations<Rational>.Product(item.Matrix1, item.Matrix2);

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.Mul, result),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Mul)}\n\nActual: {MatrixOperations<Rational>.ToString(result)}");
		}
	}

	[TestMethod]
	public void InverseTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			Rational[,] result1 = MatrixOperations<Rational>.Inverse(item.Matrix1);
			Rational[,] result2 = MatrixOperations<Rational>.Inverse(item.Matrix2);

			Rational[,] id1 = MatrixOperations<Rational>.Identity(result1.GetLength(0), result1.GetLength(1));
			Rational[,] id2 = MatrixOperations<Rational>.Identity(result2.GetLength(0), result2.GetLength(1));

			Rational[,] product1 = MatrixOperations<Rational>.Identity(result1.GetLength(0), result1.GetLength(1));
			Rational[,] product2 = MatrixOperations<Rational>.Identity(result2.GetLength(0), result2.GetLength(1));

			Assert.IsTrue(MatrixOperations<Rational>.Equals(id1, product1),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(id1)}\n\nActual: {MatrixOperations<Rational>.ToString(result1)}");

			Assert.IsTrue(MatrixOperations<Rational>.Equals(id2, product2),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Inv2)}\n\nActual: {MatrixOperations<Rational>.ToString(result2)}");
		}
	}

	[TestMethod]
	public void LUDecompositionTest()
	{
		foreach (TestMatrix item in TestMatrices.List)
		{
			(Rational[,] L1, Rational[,] U1) = MatrixOperations<Rational>.LUDecomposition(item.Matrix1);
			(Rational[,] L2, Rational[,] U2) = MatrixOperations<Rational>.LUDecomposition(item.Matrix2);

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.LU1.L, L1),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.LU1.L)}\n\nActual: {MatrixOperations<Rational>.ToString(L1)}");
			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.LU1.U, U1),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.LU1.U)}\n\nActual: {MatrixOperations<Rational>.ToString(U1)}");

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.LU2.L, L2),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.LU2.L)}\n\nActual: {MatrixOperations<Rational>.ToString(L2)}");
			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.LU2.U, U2),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.LU2.U)}\n\nActual: {MatrixOperations<Rational>.ToString(U2)}");
		}
	}
}