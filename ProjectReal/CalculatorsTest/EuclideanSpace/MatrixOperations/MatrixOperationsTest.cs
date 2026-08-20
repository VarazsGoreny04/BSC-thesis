using Calculators.EuclideanSpace;
using CalculatorsTest.EuclideanSpace.VectorOperations;
using ProjectReal.Number;

namespace CalculatorsTest.EuclideanSpace.MatrixOperations;

[TestClass]
public class MatrixOperationsTest
{
	private readonly int fractionCalculationLength;
	private readonly bool fractionalFormat;
	private readonly bool writeSign;

	private readonly (int N, int M)[] testDimensions;
	private readonly Rational[] testNumbers;

	public MatrixOperationsTest()
	{
		fractionCalculationLength = Rational.FractionCalculationLength;
		fractionalFormat = Rational.FractionalFormat;
		writeSign = Rational.WriteSign;

		Rational.FractionCalculationLength = 10;
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
		Rational.FractionCalculationLength = fractionCalculationLength;
		Rational.FractionalFormat = fractionalFormat;
		Rational.WriteSign = writeSign;
	}

	[TestMethod]
	public void FullMethod()
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
	public void ZerosMethod()
	{
		foreach ((int n, int m) in testDimensions)
		{
			Rational[,] matrix = MatrixOperations<Rational>.Zeros(n, m);

			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < m; ++j)
					Assert.AreEqual((byte)0, matrix[i, j]);
			}
		}
	}

	[TestMethod]
	public void OnesMethod()
	{
		foreach ((int n, int m) in testDimensions)
		{
			Rational[,] matrix = MatrixOperations<Rational>.Ones(n, m);

			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < m; ++j)
					Assert.AreEqual((byte)1, matrix[i, j]);
			}
		}
	}

	[TestMethod]
	public void DiagonalMethod()
	{
		foreach ((int n, int m) in testDimensions)
		{
			foreach (Rational testNumber in testNumbers)
			{
				Rational[,] matrix = MatrixOperations<Rational>.Diagonal(n, m, testNumber);

				for (int i = 0; i < n; ++i)
				{
					for (int j = 0; j < m; ++j)
						Assert.AreEqual(i == j ? testNumber : 0, matrix[i, j]);
				}
			}
		}
	}

	[TestMethod]
	public void IdentityMethod()
	{
		foreach ((int n, int m) in testDimensions)
		{
			Rational[,] matrix = MatrixOperations<Rational>.Identity(n, m);

			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < m; ++j)
					Assert.AreEqual((byte)(i == j ? 1 : 0), matrix[i, j]);
			}
		}
	}

	[TestMethod]
	public void GetRowMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void GetColumnMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void DuplicateMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void ScaleMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void TransposeMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void HorizontalConcatMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void VerticalConcatMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void EqualMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void AddMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
		{
			Rational[,] result = MatrixOperations<Rational>.Add(item.Matrix1, item.Matrix2);

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.Add, result),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Add)}\n\nActual: {MatrixOperations<Rational>.ToString(result)}");
		}
	}

	[TestMethod]
	public void SubtractMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
		{
			Rational[,] result = MatrixOperations<Rational>.Subtract(item.Matrix1, item.Matrix2);

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.Sub, result),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Sub)}\n\nActual: {MatrixOperations<Rational>.ToString(result)}");
		}
	}

	[TestMethod]
	public void ProductWithVectorMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
		{
			int n = item.Matrix1.GetLength(0);
			int m = item.Matrix1.GetLength(0);
			List<Rational[]> vectors = [.. VectorOperationsTestCases.List.Where(x => x.Vector1.Length == m).SelectMany(x => new List<Rational[]> { x.Vector1, x.Vector2 })];

			foreach (Rational[] vector in vectors)
			{
				Rational[] result1 = MatrixOperations<Rational>.Product(item.Matrix1, vector);
				Rational[] result2 = MatrixOperations<Rational>.Product(item.Matrix2, vector);

				Rational[,] matrixForm = new Rational[m, 1];
				for (int i = 0; i < m; ++i)
					matrixForm[i, 0] = vector[i];

				Rational[] expected1 = MatrixOperations<Rational>.GetColumn(MatrixOperations<Rational>.Product(item.Matrix1, matrixForm), 0);
				Rational[] expected2 = MatrixOperations<Rational>.GetColumn(MatrixOperations<Rational>.Product(item.Matrix2, matrixForm), 0);

				Assert.IsTrue(VectorOperations<Rational>.Equals(expected1, result1),
					$"\n\nExpected: {VectorOperations<Rational>.ToString(expected1)}\n\nActual: {VectorOperations<Rational>.ToString(result1)}");

				Assert.IsTrue(VectorOperations<Rational>.Equals(expected2, result2),
					$"\n\nExpected: {VectorOperations<Rational>.ToString(expected2)}\n\nActual: {VectorOperations<Rational>.ToString(result2)}");
			}
		}
	}

	[TestMethod]
	public void ProductMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
		{
			Rational[,] result = MatrixOperations<Rational>.Product(item.Matrix1, item.Matrix2);

			Assert.IsTrue(MatrixOperations<Rational>.Equals(item.Mul, result),
				$"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Mul)}\n\nActual: {MatrixOperations<Rational>.ToString(result)}");
		}
	}

	[TestMethod]
	public void InverseMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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
	public void LUDecompositionMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
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

	[TestMethod]
	public void DeterminantMethod()
	{
		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
		{
			(Rational[,] matrix1, bool sign1) = MatrixOperations<Rational>.GaussianElimination(item.Matrix1);
			(Rational[,] matrix2, bool sign2) = MatrixOperations<Rational>.GaussianElimination(item.Matrix2);

			Rational expected1 = 1;
			for (int i = matrix1.GetLength(0) - 1; i >= 0; --i)
				expected1 *= matrix1[i, i];

			Rational expected2 = 1;
			for (int i = matrix2.GetLength(0) - 1; i >= 0; --i)
				expected2 *= matrix2[i, i];

			Rational result1 = MatrixOperations<Rational>.Determinant(item.Matrix1);
			Rational result2 = MatrixOperations<Rational>.Determinant(item.Matrix2);

			Assert.AreEqual(sign1 ? expected1 : -expected1, result1);

			Assert.AreEqual(sign2 ? expected2 : -expected2, result2);
		}
	}

	[TestMethod]
	public void QRDecompositionMethod()
	{
		Rational epsilon = $"0.{new string('0', fractionCalculationLength / 2)}1";

		foreach (MatrixOperationsTestCase item in MatrixOperationsTestCases.List)
		{
			(Rational[,] Q1, Rational[,] R1) = MatrixOperations<Rational>.QRDecomposition(item.Matrix1);
			(Rational[,] Q2, Rational[,] R2) = MatrixOperations<Rational>.QRDecomposition(item.Matrix2);

			Rational[,] product1 = MatrixOperations<Rational>.Product(Q1, R1);
			Rational[,] product2 = MatrixOperations<Rational>.Product(Q2, R2);

			Rational[,] subtracted1 = MatrixOperations<Rational>.Subtract(item.Matrix1, product1);
			Rational[,] subtracted2 = MatrixOperations<Rational>.Subtract(item.Matrix2, product2);

			bool result1 = true;
			for (int i = subtracted1.GetLength(0) - 1; i >= 0; --i)
			{
				for (int j = subtracted1.GetLength(1) - 1; j >= 0; --j)
				{
					if (subtracted1[i, j] > epsilon)
						result1 = false;
				}
			}

			bool result2 = true;
			for (int i = subtracted2.GetLength(0) - 1; i >= 0; --i)
			{
				for (int j = subtracted2.GetLength(1) - 1; j >= 0; --j)
				{
					if (subtracted2[i, j] > epsilon)
						result2 = false;
				}
			}

			Assert.IsTrue(result1, $"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Matrix1)}\n\nActual: {MatrixOperations<Rational>.ToString(product1)}");

			Assert.IsTrue(result2, $"\n\nExpected: {MatrixOperations<Rational>.ToString(item.Matrix2)}\n\nActual: {MatrixOperations<Rational>.ToString(product2)}");
		}
	}
}