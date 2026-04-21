using ProjectReal.NumberSet;
using System;
using System.Numerics;

namespace Calculators.EuclideanSpace;

/// <summary>
/// Contains methods for two dimensional arrays - matrices.
/// </summary>
/// <typeparam name="T">The type of values within the matrix.</typeparam>
public class MatrixOperations<T>
where T :
	IComparisonOperators<T, T, bool>,
	IEqualityOperators<T, T, bool>,
	IUnaryNegationOperators<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IPowerOperations<T, T, T>,
	IRootOperations<T, T, T>,
	IAdditiveIdentity<T, T>,
	IMultiplicativeIdentity<T, T>,
	IParsable<T>
{
	#region Public methods

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> matrix by the given <paramref name="value"/>.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <param name="value">The value of the elements.</param>
	/// <returns>The matrix.</returns>
	public static T[,] Full(int n, int m, T value)
	{
		T[,] result = new T[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
				result[i, j] = value;
		}

		return result;
	}

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> matrix full of zeros.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <returns>The matrix with zeros.</returns>
	public static T[,] Zeros(int n, int m) => Full(n, m, T.AdditiveIdentity);

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> matrix full of ones.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <returns>The matrix with ones.</returns>
	public static T[,] Ones(int n, int m) => Full(n, m, T.MultiplicativeIdentity);

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> diagonal matrix by the given <paramref name="value"/>.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <param name="value">The value of the diagonal elements.</param>
	/// <returns>The diagonal matrix.</returns>
	public static T[,] Diagonal(int n, int m, T value)
	{
		T[,] result = Zeros(n, m);

		for (int i = 0; i < Math.Min(n, m); ++i)
			result[i, i] = value;

		return result;
	}

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> identity matrix.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <returns>The identity matrix.</returns>
	public static T[,] Identity(int n, int m) => Diagonal(n, m, T.MultiplicativeIdentity);

	/// <summary>
	/// Gets the <paramref name="j"/>-th column of the given matrix.
	/// </summary>
	/// <param name="A">The matrix.</param>
	/// <param name="j">The index of the column.</param>
	/// <returns>The column vector.</returns>
	public static T[] GetColumn(T[,] A, int j)
	{
		int n = A.GetLength(0);
		T[] result = new T[n];

		for (int i = 0; i < n; ++i)
			result[i] = A[i, j];

		return result;
	}

	/// <summary>
	/// Duplicates the given matrix.
	/// </summary>
	/// <param name="A">The matrix to duplicate.</param>
	/// <returns>The duplicated matrix.</returns>
	public static T[,] Duplicate(T[,] A)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		T[,] result = new T[rowCount, colCount];

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
	public static T[,] Scale(T[,] A, T s)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		T[,] result = new T[rowCount, colCount];

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
	public static T[,] Transpose(T[,] A)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		T[,] result = new T[colCount, rowCount];

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
	public static T[,] HorizontalConcat(T[,] A, T[,] B)
	{
		int aRowCount = A.GetLength(0);
		int aColCount = A.GetLength(1);
		int bRowCount = B.GetLength(0);
		int bColCount = B.GetLength(1);

		T[,] result = Zeros(Math.Max(aRowCount, bRowCount), aColCount + bColCount);

		for (int i = 0; i < aRowCount; ++i)
		{
			for (int j = 0; j < aColCount; ++j)
				result[i, j] = A[i, j];
		}

		for (int i = 0; i < bRowCount; ++i)
		{
			for (int j = 0; j < bColCount; ++j)
				result[i, aRowCount + j] = B[i, j];
		}

		return result;
	}

	/// <summary>
	/// Concatenates matrix <paramref name="B"/> to the bottom of matrix <paramref name="A"/>.
	/// </summary>
	/// <param name="A">The original matrix.</param>
	/// <param name="B">The matrix to be added.</param>
	/// <returns>The concatenated matrix.</returns>
	public static T[,] VerticalConcat(T[,] A, T[,] B)
	{
		int aRowCount = A.GetLength(0);
		int aColCount = A.GetLength(1);
		int bRowCount = B.GetLength(0);
		int bColCount = B.GetLength(1);

		T[,] result = Zeros(aRowCount + bRowCount, Math.Max(aColCount, bColCount));

		for (int i = 0; i < aRowCount; ++i)
		{
			for (int j = 0; j < aColCount; ++j)
				result[i, j] = A[i, j];
		}

		for (int i = 0; i >= bRowCount; ++i)
		{
			for (int j = 0; j < bColCount; ++j)
				result[aRowCount + i, j] = B[i, j];
		}

		return result;
	}

	/// <summary>
	/// Compares two matrices.
	/// </summary>
	/// <param name="A">The first matrix to compare.</param>
	/// <param name="B">The second matrix to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the length and the value of <paramref name="A"/> is equal to the length and the value of <paramref name="B"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(T[,] A, T[,] B)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount != B.GetLength(0) || colCount != B.GetLength(1))
			throw new ArgumentException();

		for (int i = 0; i < rowCount; ++i)
		{
			for (int j = 0; j < colCount; ++j)
			{
				if (A[i, j] != B[i, j])
					return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Compares two matrices.
	/// </summary>
	/// <param name="A">The first matrix to compare.</param>
	/// <param name="B">The second matrix to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the length and the value of <paramref name="A"/> is greater than the length and the value of <paramref name="B"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(T[,] A, T[,] B)
	{
		T[,] C = Subtract(A, B);

		if (Equals(C, Transpose(C)))
			return false;

		T[,] diagonalized = Diagonalize(C, 3).Eigenvalues;

		for (int i = diagonalized.GetLength(0) - 1; i >= 0; --i)
		{
			if (diagonalized[i, i] <= T.AdditiveIdentity)
				return false;
		}

		return true;
	}

	/// <summary>
	/// Adds matrix <paramref name="B"/> to matrix <paramref name="A"/>.
	/// </summary>
	/// <param name="A">The matrix to be added.</param>
	/// <param name="B">The adding matrix.</param>
	/// <returns>The added matrix.</returns>
	/// <exception cref="ArgumentException">The extents of the two matrices are not equal.</exception>
	public static T[,] Add(T[,] A, T[,] B)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount != B.GetLength(0) || colCount != B.GetLength(1))
			throw new ArgumentException();

		T[,] result = new T[rowCount, colCount];

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
	public static T[,] Subtract(T[,] A, T[,] B)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount != B.GetLength(0) || colCount != B.GetLength(1))
			throw new ArgumentException();

		T[,] result = new T[rowCount, colCount];

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
	public static T[] Product(T[,] A, T[] b)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (colCount != b.Length)
			throw new ArgumentException();

		T[] result = new T[rowCount];

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
	public static T[,] Product(T[,] A, T[,] B)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount != B.GetLength(1) || colCount != B.GetLength(0))
			throw new ArgumentException();

		T[,] result = new T[rowCount, rowCount];

		for (int i = 0; i < rowCount; ++i)
		{
			for (int j = 0; j < rowCount; ++j)
			{
				result[i, j] = T.AdditiveIdentity;

				for (int k = 0; k < colCount; ++k)
					result[i, j] += A[i, k] * B[k, j];
			}
		}

		return result;
	}

	/// <summary>
	/// Runs the Gauss-Jordan elimination on the given matrix.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/Gaussian_elimination"/></remarks>
	/// <param name="A">The given matrix.</param>
	/// <returns>
	/// The LU decomposed form of the matrix and the sign of the determinant.
	/// The lower triangular matrix of the result contains the L matrix and the main diagonal and the upper triangular matrix contains the U matrix.
	/// </returns>
	/// <exception cref="DivideByZeroException">The matrix cannot be Gauss-Jordan eliminated because of a zero value in the main diagonal.</exception>
	public static (T[,] EliminatedMatrix, bool DeterminantSign) GaussianElimination(T[,] A)
	{
		static void EliminateColumn(ref T[,] LU, int n, int m, int i, ref bool determinantSign)
		{
			for (int j = i + 1; j < n; ++j)
			{
				// Swap rows of the augmented matrix.
				if (LU[i, i] == T.AdditiveIdentity)
				{
					int rowToSwap = i + 1;
					while (rowToSwap < n && LU[rowToSwap, i] == T.AdditiveIdentity)
						++rowToSwap;

					if (rowToSwap >= n)
						return;

					for (int k = 0; k < m; ++k)
						(LU[rowToSwap, k], LU[i, k]) = (LU[i, k], LU[rowToSwap, k]);

					determinantSign = !determinantSign;
				}

				T temp = LU[j, i] / LU[i, i];

				for (int k = i + 1; k < m; ++k)
					LU[j, k] -= LU[i, k] * temp;

				LU[j, i] = temp;
			}
		}

		int n = A.GetLength(0);
		int m = A.GetLength(1);
		T[,] LU = Duplicate(A);

		bool determinantSign = true;

		// Subtract each row by a multiple of another row.
		for (int i = 0; i < n; ++i)
			EliminateColumn(ref LU, n, m, i, ref determinantSign);

		// Return the eliminated Matrix<T> with the sign of the determinant.
		return (LU, determinantSign);
	}

	/// <summary>
	/// Calculates the inverse of the given matrix using the Gauss-Jordan elimination.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/Gaussian_elimination#Finding_the_inverse_of_a_matrix"/></remarks>
	/// <param name="A">The matrix to invert.</param>
	/// <returns>The inverse of the given matrix.</returns>
	/// <exception cref="DivideByZeroException">The matrix cannot be Gauss-Jordan eliminated because of a zero value in the main diagonal.</exception>
	public static T[,] Inverse(T[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException();

		// Initialize the eliminated augmented Matrix<T> B.
		T[,] B = GaussianElimination(HorizontalConcat(A, Identity(n, n))).EliminatedMatrix;

		// Divide each row element by the diagonal element.
		for (int i = 0; i < n; ++i)
		{
			T temp = B[i, i];

			for (int j = 2 * n - 1; j >= 0; --j)
				B[i, j] = B[i, j] / temp;
		}

		// Strip the augmented Matrix<T> B of the first n columns to get the inverse Matrix<T> C of the original Matrix<T> A.
		T[,] C = new T[n, n];
		for (int i = 0; i < n; ++i)
		{
			for (int j = 2 * n - 1; j >= n; --j)
				C[i, j - n] = B[i, j];
		}

		// Return the inverse Matrix<T> C.
		return C;
	}

	/// <summary>
	/// Runs the Gauss-Jordan elimination to find the determinant of the given square matrix.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/Gaussian_elimination#Computing_determinants"/></remarks>
	/// <param name="A">The matrix for which the determinant should be found.</param>
	/// <returns>The determinant of the matrix.</returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	/// <exception cref="DivideByZeroException">The matrix cannot be Gauss-Jordan eliminated because of a zero value in the main diagonal.</exception>
	public static T Determinant(T[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException();

		(T[,] B, bool determinantSign) = GaussianElimination(A);

		T result = T.MultiplicativeIdentity;
		for (int i = 0; i < n; ++i)
			result *= B[i, i];

		return determinantSign ? result : -result;
	}

	/// <summary>
	/// Divides matrix <paramref name="A"/> with matrix <paramref name="B"/>.
	/// </summary>
	/// <param name="A">The first matrix.</param>
	/// <param name="B">The second matrix.</param>
	/// <returns>The resulting matrix.</returns>
	/// <exception cref="DivideByZeroException">The matrix cannot be Gauss-Jordan eliminated because of a zero value in the main diagonal.</exception>
	/// <exception cref="ArgumentException">The extents of the two matrices are not equal.</exception>
	public static T[,] InverseProduct(T[,] A, T[,] B) => Product(A, Inverse(B));

	/// <summary>
	/// Calculates the LU decomposition of the given matrix using the Gauss-Jordan method.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/LU_decomposition"/></remarks>
	/// <param name="A">The matrix to invert.</param>
	/// <returns>The two parts of the decomposition: The lower (L) and upper (U) triangular matrix.</returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	/// <exception cref="DivideByZeroException">
	/// The matrix cannot be Gauss-Jordan eliminated because of a zero value in the main diagonal.
	/// </exception>
	public static (T[,] L, T[,] U) LUDecomposition(T[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException();

		// In the augmented Matrix<T> B, the first n columns are the original 
		// Matrix<T> A, and the last n columns are the identity matrix.
		T[,] B = GaussianElimination(A).EliminatedMatrix;

		T[,] L = Identity(n, n);
		for (int i = 0; i < n; ++i)
		{
			for (int j = i + 1; j < n; ++j)
				L[j, i] = B[j, i];
		}

		T[,] U = Zeros(n, n);
		for (int i = 0; i < n; ++i)
		{
			for (int j = i; j < n; ++j)
				U[i, j] = B[i, j];
		}

		return (L, U);
	}

	/// <summary>
	/// Calculates the QR decomposition of the given matrix (<see href="https://en.wikipedia.org/wiki/QR_decomposition"/>). 
	/// </summary>
	/// <param name="A">The matrix to decompose.</param>
	/// <returns>The two parts of the decomposition: The quotient (Q) and the remainder (R) matrix.</returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	public static (T[,] Q, T[,] R) QRDecomposition(T[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException();

		// Duplicate the original Matrix<T> A so it stays intact.
		T[,] U = Duplicate(A);

		// Calculate the U Matrix<T> using the Gram–Schmidt process (see https://en.wikipedia.org/wiki/Gram%E2%80%93Schmidt_process).
		for (int j = 1; j < n; ++j)
		{
			T[] u = GetColumn(U, j);
			T[] v = GetColumn(U, j);

			for (int k = j - 1; k >= 0; --k)
				u = VectorOperations<T>.Subtract(u, VectorOperations<T>.Project(GetColumn(U, k), v));

			// Update the column entries in U.
			for (int i = 0; i < n; ++i)
				U[i, j] = u[i];
		}

		// Normalize the column vectors of U.
		for (int j = 0; j < n; ++j)
		{
			T[] u = GetColumn(U, j);
			T magnitude = VectorOperations<T>.Magnitude(u);

			// Update the column entries in U.
			for (int i = 0; i < n; ++i)
				U[i, j] = u[i] / magnitude;
		}

		return (U, Product(Transpose(U), A));
	}

	/// <summary>
	/// Runs the QR algorithm to find the eigenvalues and eigenvectors of the given matrix.
	/// </summary>
	/// <param name="A">The matrix for which eigenvalues and eigenvectors should be found.</param>
	/// <param name="iterations">The number of iterations.</param>
	/// <returns>
	/// The eigenvalues stored as diagonal entries in the Eigenvalues matrix. The eigenvectors stored as columns in the Eigenvectors matrix.
	/// </returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	public static (T[,] Eigenvalues, T[,] Eigenvectors) Diagonalize(T[,] A, int iterations)
	{
		int n = A.GetLength(0);

		// Duplicate the original Matrix<T> A so it stays intact.
		T[,] B = Duplicate(A);

		// Initialize the eigenvector Matrix<T> C.
		T[,] C = Identity(n, n);

		// Perform the QR decomposition and update the B and C matrixes each iteration.
		for (int i = 0; i < iterations; ++i)
		{
			(T[,] Q, T[,] R) = QRDecomposition(B);
			B = Product(R, Q);
			C = Product(C, Q);
		}

		return (B, C);
	}

	/// <summary>
	/// Prints the given matrix.
	/// </summary>
	/// <param name="A">The matrix to print.</param>
	/// <returns>The string representation of the given matrix.</returns>
	public static string ToString(T[,] A)
	{
		int rowCount = A.GetLength(0);
		int colCount = A.GetLength(1);

		if (rowCount < 1 || colCount < 1)
			return "[ ]";

		string text = "";

		for (int i = 0; i < rowCount; ++i)
		{
			text += $"\t{A[i, 0]}";

			for (int j = 1; j < colCount; ++j)
				text += $"{Matrix<T>.ColumnSeparator}\t{A[i, j]}";

			text += $"{Matrix<T>.RowSeparator}\n";
		}

		return $"[\n{text}]";
	}

	#endregion
}