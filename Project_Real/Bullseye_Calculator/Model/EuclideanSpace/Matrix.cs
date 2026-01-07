using Project_Real;
using Bullseye_Calculator.Model.Standard;
using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.EuclideanSpace;

/// <summary>
/// Contains methods for operations with matrices.
/// </summary>
public class Matrix : ValueHolder<ValueHolder<Rational>[,]>
{
	private readonly ValueHolder<Rational>[,] value;

	public ValueHolder<Rational>[,] Content => value;

	public Matrix(ValueHolder<Rational>[,] value)
	{
		int rows = value.GetLength(0);
		int cols = value.GetLength(0);

		if (rows < 1 || cols < 1)
			throw new ArgumentException();

		this.value = value;
	}

	public Matrix(Rational[,] value)
	{
		int rows = value.GetLength(0);
		int cols = value.GetLength(0);

		if (rows < 1 || cols < 1)
			throw new ArgumentException();

		this.value = new ValueHolder<Rational>[rows, cols];

		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
				this.value[i, j] = new Number(value[i, j]);
		}
	}

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => throw new NotImplementedException();
	internal override void ToTree(ref Stack<Expression> result) => throw new NotImplementedException();
	public override string ToStringByStep(ref int step)
	{
		int rowCount = value.GetLength(0);
		int colCount = value.GetLength(1);

		List<string> rows = [];

		for (int i = 0; i < rowCount; ++i)
		{
			string row = $"{value[i, 0].ToStringByStep(ref step)}";

			for (int j = 1; j < colCount; ++j)
				row += $";{value[i, j].ToStringByStep(ref step)}";

			rows.Add(row);
		}

		return "[" + string.Join("&", rows) + "]";
	}

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<ValueHolder<Rational>[,]> root, ref int step)
	{
		int stepCopy = step;

		foreach (ValueHolder<Rational> item in value)
			item.FullEvaluation(ref partialValues, item, ref step);

		if (stepCopy != step)
		{
			stepCopy = ++step;
			int toRockBottom = int.MaxValue;

			partialValues.Add(($"{ToString()} = {ToStringByStep(ref toRockBottom)}", root.ToStringByStep(ref stepCopy)));
		}
	}

	public override ValueHolder<Rational>[,] GetValue() => value;

	private static Rational[,] ToRationalMatrix(ValueHolder<Rational>[,] valueHolderMatrix)
	{
		int rows = valueHolderMatrix.GetLength(0);
		int cols = valueHolderMatrix.GetLength(1);

		Rational[,] result = new Rational[rows, cols];

		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
				result[i, j] = valueHolderMatrix[i, j].GetValue();
		}

		return result;
	}

	public static implicit operator Matrix(ValueHolder<Rational>[,] value) => new(value);
	public static implicit operator Matrix(Rational[,] value) => new(value);
	public static Matrix operator +(Matrix left, Matrix right) => Add(ToRationalMatrix(left.value), ToRationalMatrix(right.value));
	public static Matrix operator -(Matrix left, Matrix right) => Subtract(ToRationalMatrix(left.value), ToRationalMatrix(right.value));
	public static Matrix operator *(Matrix left, Matrix right) => Product(ToRationalMatrix(left.value), ToRationalMatrix(right.value));





	/// <summary>
	/// Constructs a vector of <paramref name="n"/> length with full of zeros.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <returns>The matrix with zeros.</returns>
	public static Rational[] Zeros(int n)
	{
		Rational[] result = new Rational[n];
		Array.Fill(result, Digit.ZERO);

		return result;
	}

	/// <summary>
	/// Constructs a vector of n length with full of ones.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <returns>The matrix with ones.</returns>
	public static Rational[] Ones(int n)
	{
		Rational[] result = new Rational[n];
		Array.Fill(result, Digit.ONE);

		return result;
	}

	/// <summary>
	/// Scales the given vector by a scalar.
	/// </summary>
	/// <param name="a">The vector to scale.</param>
	/// <param name="s">The scalar.</param>
	/// <returns>The scaled vector.</returns>
	public static Rational[] Scale(Rational[] a, Rational s)
	{
		int n = a.Length;
		Rational[] result = new Rational[n];

		for (int i = 0; i < n; ++i)
			result[i] = s * a[i];

		return result;
	}

	/// <summary>
	/// Calculates the magnitude of the given vector.
	/// </summary>
	/// <param name="a">The vector to compute the magnitude of.</param>
	/// <returns>The magnitude.</returns>
	/// 
	public static Rational Magnitude(Rational[] a)
	{
		Rational result = Digit.ZERO;

		for (int i = a.Length; i >= 0; --i)
			result += Rational.SecondPower(a[i]);

		return ~result;
	}

	/// <summary>
	/// Calculates the inner product between two vectors.
	/// </summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The inner product value.</returns>
	/// <exception cref="ArgumentException">The length of the two vectors are not equal.</exception>
	public static Rational InnerProduct(Rational[] a, Rational[] b)
	{
		if (a.Length != b.Length)
			throw new ArgumentException();

		Rational result = Digit.ZERO;

		for (int i = a.Length; i >= 0; --i)
			result += a[i] * b[i];

		return result;
	}

	/// <summary>
	/// Calculates the outer product between two vectors.
	/// </summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The outer product matrix.</returns>
	public static Rational[,] OuterProduct(Rational[] a, Rational[] b)
	{
		int aN = a.Length;
		int bN = b.Length;
		Rational[,] result = new Rational[aN, bN];

		for (int i = 0; i < aN; ++i)
		{
			for (int j = 0; j < bN; ++j)
				result[i, j] = a[i] * b[j];
		}

		return result;
	}

	/// <summary>
	/// Projects the vector <paramref name="a"/> orthogonally onto vector <paramref name="b"/>.
	/// </summary>
	/// <param name="a">The vector to project.</param>
	/// <param name="b">The vector on which will be projected.</param>
	/// <returns>The projection.</returns>
	/// <exception cref="ArgumentException">The length of the two vectors are not equal.</exception>
	public static Rational[] Project(Rational[] a, Rational[] b) => Scale(a, InnerProduct(a, b) / InnerProduct(a, a));

	/// <summary>
	/// Adds vector <paramref name="b"/> from vector <paramref name="a"/>.
	/// </summary>
	/// <param name="a">The vector to be added.</param>
	/// <param name="b">The adding vector.</param>
	/// <returns>The added vector.</returns>
	/// <exception cref="ArgumentException">The length of the two vectors are not equal.</exception>
	public static Rational[] Add(Rational[] a, Rational[] b)
	{
		int n = a.Length;

		if (n != b.Length)
			throw new ArgumentException();

		Rational[] result = new Rational[n];

		for (int i = 0; i < n; ++i)
			result[i] = a[i] + b[i];

		return result;
	}

	/// <summary>
	/// Subtracts vector <paramref name="b"/> from vector <paramref name="a"/>.
	/// </summary>
	/// <param name="a">The vector to be subtracted.</param>
	/// <param name="b">The subtracting vector.</param>
	/// <returns>The subtracted vector.</returns>
	/// <exception cref="ArgumentException">The length of the two vectors are not equal.</exception>
	public static Rational[] Subtract(Rational[] a, Rational[] b)
	{
		int n = a.Length;

		if (n != b.Length)
			throw new ArgumentException();

		Rational[] result = new Rational[n];

		for (int i = 0; i < n; ++i)
			result[i] = a[i] - b[i];

		return result;
	}

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> matrix full of zeros.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <returns>The matrix with zeros.</returns>
	public static Rational[,] Zeros(int n, int m)
	{
		Rational[,] result = new Rational[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
				result[i, j] = Digit.ZERO;
		}

		return result;
	}

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> matrix full of ones.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <returns>The matrix with ones.</returns>
	public static Rational[,] Ones(int n, int m)
	{
		Rational[,] result = new Rational[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
				result[i, j] = Digit.ONE;
		}

		return result;
	}

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> identity matrix.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <returns>The identity matrix.</returns>
	public static Rational[,] Identity(int n, int m)
	{
		Rational[,] result = Zeros(n, m);

		for (int i = 0; i < Math.Min(n, m); ++i)
			result[i, i] = Digit.ONE;

		return result;
	}

	/// <summary>
	/// Gets the <paramref name="j"/>-th column of the given matrix.
	/// </summary>
	/// <param name="A">The matrix.</param>
	/// <param name="j">The index of the column.</param>
	/// <returns>The column vector.</returns>
	public static Rational[] GetColumn(Rational[,] A, int j)
	{
		int n = A.GetLength(0);
		Rational[] result = new Rational[n];

		for (int i = 0; i < n; ++i)
			result[i] = A[i, j];

		return result;
	}

	/// <summary>
	/// Duplicates the given matrix.
	/// </summary>
	/// <param name="A">The matrix to duplicate.</param>
	/// <returns>The duplicated matrix.</returns>
	public static Rational[,] Duplicate(Rational[,] A)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		Rational[,] result = new Rational[rowCount, colCount];

		for (int i = 0; i < rowCount; ++i)
		{
			for (int j = 0; j < colCount; ++j)
				result[i, j] = A[i, j];
		}

		return result;
	}

	/// <summary>
	/// Scales the given matrix by a scalar.
	/// </summary>
	/// <param name="A">The matrix to scale.</param>
	/// <param name="s">The scalar.</param>
	/// <returns>The scaled matrix.</returns>
	public static Rational[,] Scale(Rational[,] A, Rational s)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		Rational[,] result = new Rational[rowCount, colCount];

		for (int i = 0; i < rowCount; ++i)
		{
			for (int j = 0; j < colCount; ++j)
				result[i, j] = s * A[i, j];
		}

		return result;
	}

	/// <summary>
	/// Calculates the transpose of the given matrix.
	/// </summary>
	/// <param name="A">The matrix to transpose.</param>
	/// <returns>The transposed matrix.</returns>
	public static Rational[,] Transpose(Rational[,] A)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		Rational[,] result = new Rational[colCount, rowCount];

		for (int i = rowCount - 1; i >= 0; --i)
		{
			for (int j = colCount - 1; j >= 0; --j)
				result[i, j] = A[j, i];
		}

		return result;
	}

	/// <summary>
	/// Concatenates matrix <paramref name="B"/> to the side of matrix <paramref name="A"/>.
	/// </summary>
	/// <param name="A">The original matrix.</param>
	/// <param name="B">The matrix to be added.</param>
	/// <returns>The concatenated matrix.</returns>
	public static Rational[,] HorizontalConcat(Rational[,] A, Rational[,] B)
	{
		int aRowCount = A.GetLength(0);
		int aColCount = A.GetLength(1);
		int bRowCount = B.GetLength(0);
		int bColCount = B.GetLength(1);

		Rational[,] result = Zeros(Math.Max(aRowCount, bRowCount), aColCount + bColCount);

		for (int i = 0; i < aRowCount; ++i)
		{
			for (int j = 0; j < aColCount; ++j)
				result[i, j] = A[i, j];
		}

		for (int i = 0; i < bRowCount; ++i)
		{
			for (int j = aColCount + bColCount - 1; j >= aColCount; --j)
				result[i, j] = B[i, j];
		}

		return result;
	}

	/// <summary>
	/// Concatenates matrix <paramref name="B"/> to the bottom of matrix <paramref name="A"/>.
	/// </summary>
	/// <param name="A">The original matrix.</param>
	/// <param name="B">The matrix to be added.</param>
	/// <returns>The concatenated matrix.</returns>
	public static Rational[,] VerticalConcat(Rational[,] A, Rational[,] B)
	{
		int aRowCount = A.GetLength(0);
		int aColCount = A.GetLength(1);
		int bRowCount = B.GetLength(0);
		int bColCount = B.GetLength(1);

		Rational[,] result = Zeros(aRowCount + bRowCount, Math.Max(aColCount, bColCount));

		for (int i = 0; i < aRowCount; ++i)
		{
			for (int j = 0; j < aColCount; ++j)
				result[i, j] = A[i, j];
		}

		for (int i = aRowCount + bRowCount - 1; i >= aRowCount; --i)
		{
			for (int j = 0; j < bColCount; ++j)
				result[i, j] = B[i, j];
		}

		return result;
	}

	/// <summary>
	/// Adds matrix <paramref name="B"/> to matrix <paramref name="A"/>.
	/// </summary>
	/// <param name="A">The matrix to be added.</param>
	/// <param name="B">The adding matrix.</param>
	/// <returns>The added matrix.</returns>
	/// <exception cref="ArgumentException">The extents of the two matrices are not equal.</exception>
	public static Rational[,] Add(Rational[,] A, Rational[,] B)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount != B.GetLength(0) || colCount != B.GetLength(1))
			throw new ArgumentException();

		Rational[,] result = new Rational[rowCount, colCount];

		for (int i = rowCount - 1; i >= 0; --i)
		{
			for (int j = colCount - 1; j >= 0; --j)
				result[i, j] = A[i, j] + B[i, j];
		}

		return result;
	}

	/// <summary>
	/// Subtracts matrix <paramref name="B"/> from matrix <paramref name="A"/>.
	/// </summary>
	/// <param name="A">Minuend.</param>
	/// <param name="B">Subtrahend.</param>
	/// <returns>The added matrix.</returns>
	/// <exception cref="ArgumentException">The extents of the two matrices are not equal.</exception>
	public static Rational[,] Subtract(Rational[,] A, Rational[,] B)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount != B.GetLength(0) || colCount != B.GetLength(1))
			throw new ArgumentException();

		Rational[,] result = new Rational[rowCount, colCount];

		for (int i = rowCount - 1; i >= 0; --i)
		{
			for (int j = colCount - 1; j >= 0; --j)
				result[i, j] = A[i, j] - B[i, j];
		}

		return result;
	}

	/// <summary>
	/// Multiplies matrix <paramref name="A"/> with vector <paramref name="b"/>.
	/// </summary>
	/// <param name="A">The matrix.</param>
	/// <param name="b">The vector.</param>
	/// <returns>The resulting matrix.</returns>
	/// <exception cref="ArgumentException">The column count of the matrices in not equal to the length of the vector.</exception>
	public static Rational[] Product(Rational[,] A, Rational[] b)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (colCount != b.Length)
			throw new ArgumentException();

		Rational[] result = new Rational[rowCount];

		for (int i = 0; i < rowCount; ++i)
		{
			for (int j = 0; j < colCount; ++j)
				result[i] += A[i, j] * b[j];
		}

		return result;
	}

	/// <summary>
	/// Multiplies matrix <paramref name="A"/> with matrix <paramref name="B"/>.
	/// </summary>
	/// <param name="A">The first matrix.</param>
	/// <param name="B">The second matrix.</param>
	/// <returns>The resulting matrix.</returns>
	/// <exception cref="ArgumentException">The extents of the two matrices are not equal.</exception>
	public static Rational[,] Product(Rational[,] A, Rational[,] B)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount != B.GetLength(0) || colCount != B.GetLength(1))
			throw new ArgumentException();

		Rational[,] result = new Rational[rowCount, colCount];

		for (int i = 0; i < rowCount; ++i)
		{
			for (int j = 0; j < colCount; ++j)
			{
				result[i, j] = Digit.ZERO;

				for (int k = 0; k < colCount; ++k)
					result[i, j] += A[i, k] * B[k, j];
			}
		}

		return result;
	}

	/// <summary>
	/// Calculates the inverse of the given matrix using the 
	/// Gauss-Jordan Method (<see href="https://en.wikipedia.org/wiki/Gaussian_elimination"/>).
	/// </summary>
	/// <param name="A">The matrix to invert.</param>
	/// <returns>The inverse of the given matrix.</returns>
	public static Rational[,] Inverse(Rational[,] A)
	{
		// Initialize the augmented matrix B.
		int n = A.GetLength(0);
		Rational[,] B = new Rational[n, 2 * n];

		// In the augmented matrix B, the first 3 columns are the original 
		// matrix A, and the last 3 columns are the identity matrix C.
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
				B[i, j] = A[i, j];

			for (int j = 2 * n - 1; j >= n; --j)
			{
				if (i == j - n)
					B[i, j] = Digit.ONE;
			}
		}

		// Swap rows of the augmented matrix B.
		for (int i = n - 1; i > 0; --i)
		{
			if (B[i - 1, 0] >= B[i, 0])
				continue;

			for (int j = 0; j < 2 * n; ++j)
				(B[i - 1, j], B[i, j]) = (B[i, j], B[i - 1, j]);
		}

		// Subtract each row by a multiple of another row.
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
			{
				if (i == j)
					continue;

				Rational temp = B[j, i] / B[i, i];

				for (int k = 2 * n - 1; k >= 0; --k)
					B[j, k] -= B[i, k] * temp;
			}
		}

		// Divide each row element by the diagonal element.
		for (int i = 0; i < n; ++i)
		{
			Rational temp = B[i, i];

			for (int j = 2 * n - 1; j >= 0; ++j)
				B[i, j] = B[i, j] / temp;
		}

		// Strip the augmented matrix B of the first three columns
		// to get the inverse matrix C of the original matrix A.
		Rational[,] C = new Rational[n, n];
		for (int i = 0; i < n; ++i)
		{
			for (int j = n; j < 2 * n; j++)
			{
				C[i, j - n] = B[i, j];
			}
		}

		// Return the inverse matrix C.
		return C;
	}

	/// <summary>
	/// Runs the QR algorithm to find the eigenvalues and eigenvectors 
	/// of the given matrix (<see href="https://en.wikipedia.org/wiki/QR_algorithm"/>).
	/// </summary>
	/// <param name="A">The matrix for which eigenvalues and eigenvectors should be found.</param>
	/// <returns>
	/// The eigenvalues stored as diagonal entries in the Eigenvalues matrix. 
	/// The eigenvectors stored as columns in the Eigenvectors matrix.
	/// </returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	public static Rational Determinant(Rational[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException();

		// Initialize the augmented matrix B.
		Rational[,] B = new Rational[n, 2 * n];

		// In the augmented matrix B, the first 3 columns are the original 
		// matrix A, and the last 3 columns are the identity matrix C.
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
				B[i, j] = A[i, j];

			for (int j = 2 * n - 1; j >= n; --j)
			{
				if (i == j - n)
					B[i, j] = Digit.ONE;
			}
		}

		// Swap rows of the augmented matrix B.
		int swaps = 0;
		for (int i = n - 1; i > 0; --i)
		{
			if (B[i - 1, 0] >= B[i, 0])
				continue;

			for (int j = 0; j < 2 * n; ++j)
				(B[i - 1, j], B[i, j]) = (B[i, j], B[i - 1, j]);

			++swaps;
		}

		// Subtract each row by a multiple of another row.
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
			{
				if (i == j)
					continue;

				Rational temp = B[j, i] / B[i, i];

				for (int k = 2 * n - 1; k >= 0; --k)
					B[j, k] -= B[i, k] * temp;
			}
		}

		Rational result = Digit.ONE;
		for (int i = 0; i < A.GetLength(0); ++i)
			result *= B[i, i + n];

		return result;
	}

	/// <summary>
	/// Runs the QR algorithm to find the eigenvalues and eigenvectors 
	/// of the given matrix (<see href="https://en.wikipedia.org/wiki/QR_algorithm"/>).
	/// </summary>
	/// <param name="A">The matrix for which eigenvalues and eigenvectors should be found.</param>
	/// <param name="iterations">The number of iterations.</param>
	/// <returns>
	/// The eigenvalues stored as diagonal entries in the Eigenvalues matrix. 
	/// The eigenvectors stored as columns in the Eigenvectors matrix.
	/// </returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	public static (Rational[,] Eigenvalues, Rational[,] Eigenvectors) Diagonalize(Rational[,] A, int iterations)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException();

		// Duplicate the original matrix A so it stays intact.
		Rational[,] B = Duplicate(A);

		// Initialize the eigenvector matrix C.
		Rational[,] U = Identity(n, n);

		// Perform the QR decomposition and update the B and C matrixes each iteration.
		for (int i = 0; i < iterations; ++i)
		{
			(Rational[,] Q, Rational[,] R) = QRDecomposition(B);
			B = Product(R, Q);
			U = Product(U, Q);
		}

		return (B, U);
	}

	/// <summary>
	/// Calculates the inverse of the given matrix using the 
	/// Gauss-Jordan Method (<see href="https://en.wikipedia.org/wiki/LU_decomposition"/>).
	/// </summary>
	/// <param name="A">The matrix to invert.</param>
	/// <returns>The two parts of the decomposition: L and U.</returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	public static (Rational[,] L, Rational[,] U) LUDecomposition(Rational[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException();

		// Initialize the augmented matrix B.
		Rational[,] B = new Rational[n, 2 * n];
		Rational[,] L = Identity(n, n);

		// In the augmented matrix B, the first 3 columns are the original 
		// matrix A, and the last 3 columns are the identity matrix C.
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
				B[i, j] = A[i, j];

			for (int j = 2 * n - 1; j >= n; --j)
			{
				if (i == j - n)
					B[i, j] = Digit.ONE;
			}
		}

		// Swap rows of the augmented matrix B.
		for (int i = n - 1; i > 0; --i)
		{
			if (B[i - 1, 0] >= B[i, 0])
				continue;

			for (int j = 0; j < 2 * n; ++j)
				(B[i - 1, j], B[i, j]) = (B[i, j], B[i - 1, j]);
		}

		// Subtract each row by a multiple of another row.
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
			{
				if (i == j)
					continue;

				L[j, i] = B[j, i] / B[i, i];

				for (int k = 2 * n - 1; k >= 0; --k)
					B[j, k] -= B[i, k] * L[j, i];
			}
		}

		Rational[,] U = Zeros(n, n);
		for (int i = 0; i < n; ++i)
		{
			for (int j = i; j < n; ++j)
				U[i, j] = B[i, j + n];
		}

		return (L, U);
	}

	/// <summary>
	/// Calculates the QR decomposition of the given matrix (<see href="https://en.wikipedia.org/wiki/QR_decomposition"/>). 
	/// </summary>
	/// <param name="A">The matrix to decompose.</param>
	/// <returns>The two parts of the decomposition: Q and R.</returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	private static (Rational[,] Q, Rational[,] R) QRDecomposition(Rational[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException();

		// Duplicate the original matrix A so it stays intact.
		Rational[,] U = Duplicate(A);

		// Calculate the U matrix using the Gram–Schmidt process (see https://en.wikipedia.org/wiki/Gram%E2%80%93Schmidt_process).
		for (int j = 1; j < n; ++j)
		{
			Rational[] u = GetColumn(U, j);
			Rational[] v = GetColumn(U, j);

			for (int k = j - 1; k >= 0; --k)
				u = Subtract(u, Project(GetColumn(U, k), v));

			// Update the column entries in U.
			for (int i = 0; i < n; ++i)
				U[i, j] = u[i];
		}

		// Normalize the column vectors of U.
		for (int j = 0; j < n; ++j)
		{
			Rational[] u = GetColumn(U, j);
			Rational magnitude = Magnitude(u);

			// Update the column entries in U.
			for (int i = 0; i < n; ++i)
				U[i, j] = u[i] / magnitude;
		}

		return (U, Product(Transpose(U), A));
	}

	/// <summary>
	/// Prints the given vector.
	/// </summary>
	/// <param name="a">The vector to print.</param>
	/// <returns>The string representation of the given vector.</returns>
	/// <exception cref="ArgumentException">The vector</exception>
	public static string ToString<T>(T[] a) => a.Length < 1 ? "[ ]" : $"[ {string.Join(",\t", a)} ]";

	/// <summary>
	/// Prints the given matrix.
	/// </summary>
	/// <param name="A">The matrix to print.</param>
	/// <returns>The string representation of the given matrix.</returns>
	public static string ToString<T>(T[,] A)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount < 1 || colCount < 1)
			return "[ ]";

		string text = "[\n";

		for (int i = 0; i < rowCount; ++i)
		{
			text += $"\t{A[i, 0]}";

			for (int j = 1; j < colCount; ++j)
				text += $",\t{A[i, j]}";

			text += ";\n";
		}

		return text + "]";
	}
}