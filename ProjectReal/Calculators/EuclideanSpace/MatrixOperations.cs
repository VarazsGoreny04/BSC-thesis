using ProjectReal.NumberSet;
using System;
using System.Collections.Generic;
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
	/// Gets the <paramref name="n"/>-th row of the given matrix.
	/// </summary>
	/// <param name="A">The matrix.</param>
	/// <param name="n">The index of the row.</param>
	/// <returns>The row vector.</returns>
	public static T[] GetRow(T[,] A, int n)
	{
		int m = A.GetLength(1);
		T[] result = new T[m];

		for (int i = 0; i < m; ++i)
			result[i] = A[n, i];

		return result;
	}

	/// <summary>
	/// Gets the <paramref name="m"/>-th column of the given matrix.
	/// </summary>
	/// <param name="A">The matrix.</param>
	/// <param name="m">The index of the column.</param>
	/// <returns>The column vector.</returns>
	public static T[] GetColumn(T[,] A, int m)
	{
		int n = A.GetLength(0);
		T[] result = new T[n];

		for (int i = 0; i < n; ++i)
			result[i] = A[i, m];

		return result;
	}

	/// <summary>
	/// Duplicates the given matrix.
	/// </summary>
	/// <param name="A">The matrix to duplicate.</param>
	/// <returns>The duplicated matrix.</returns>
	public static T[,] Duplicate(T[,] A)
	{
		int n = A.GetLength(0);
		int m = A.GetLength(1);

		T[,] result = new T[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
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
		int n = A.GetLength(0);
		int m = A.GetLength(1);

		T[,] result = new T[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
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
		int n = A.GetLength(0);
		int m = A.GetLength(1);

		T[,] result = new T[m, n];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
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
		int an = A.GetLength(0);
		int am = A.GetLength(1);
		int bn = B.GetLength(0);
		int bm = B.GetLength(1);

		T[,] result = Zeros(Math.Max(an, bn), am + bm);

		for (int i = 0; i < an; ++i)
		{
			for (int j = 0; j < am; ++j)
				result[i, j] = A[i, j];
		}

		for (int i = 0; i < bn; ++i)
		{
			for (int j = 0; j < bm; ++j)
				result[i, an + j] = B[i, j];
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
		int an = A.GetLength(0);
		int am = A.GetLength(1);
		int bn = B.GetLength(0);
		int bm = B.GetLength(1);

		T[,] result = Zeros(an + bn, Math.Max(am, bm));

		for (int i = 0; i < an; ++i)
		{
			for (int j = 0; j < am; ++j)
				result[i, j] = A[i, j];
		}

		for (int i = 0; i < bn; ++i)
		{
			for (int j = 0; j < bm; ++j)
				result[an + i, j] = B[i, j];
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
	/// <exception cref="ArgumentException">
	/// The <paramref name="A"/> matrix must have the same number of rows and columns as the <paramref name="B"/> matrix.
	/// </exception>
	public static bool Equals(T[,] A, T[,] B)
	{
		int n = A.GetLength(0);
		int m = A.GetLength(1);

		if (n != B.GetLength(0) || m != B.GetLength(1))
			throw new ArgumentException("The two matrices must have the same number of rows and columns!");

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
			{
				if (A[i, j] != B[i, j])
					return false;
			}
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
		int n = A.GetLength(0);
		int m = A.GetLength(1);

		if (n != B.GetLength(0) || m != B.GetLength(1))
			throw new ArgumentException("The extents of the two matrices are not equal!");

		T[,] result = new T[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
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
		int n = A.GetLength(0);
		int m = A.GetLength(1);

		if (n != B.GetLength(0) || m != B.GetLength(1))
			throw new ArgumentException("The extents of the two matrices are not equal!");

		T[,] result = new T[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
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
	/// <exception cref="ArgumentException">The column count of the matrix in not equal to the length of the vector.</exception>
	public static T[] Product(T[,] A, T[] b)
	{
		int n = A.GetLength(0);
		int m = A.GetLength(1);

		if (m != b.Length)
			throw new ArgumentException("The column count of the matrix in not equal to the length of the vector!");

		T[] result = VectorOperations<T>.Zeros(n);

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
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
		int an = A.GetLength(0);
		int am = A.GetLength(1);
		int bm = B.GetLength(1);

		if (am != B.GetLength(0))
			throw new ArgumentException("The extents of the two matrices are not equal!");

		T[,] result = Zeros(an, bm);

		for (int i = 0; i < an; ++i)
		{
			for (int j = 0; j < bm; ++j)
			{
				for (int k = 0; k < am; ++k)
					result[i, j] += A[i, k] * B[k, j];
			}
		}

		return result;
	}

	/// <summary>
	/// Runs the Gaussian elimination on the given matrix.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/Gaussian_elimination"/></remarks>
	/// <param name="A">The given matrix.</param>
	/// <returns>
	/// The LU decomposed form of the matrix and the sign of the determinant.
	/// The lower triangular matrix of the result contains the L matrix and the main diagonal and the upper triangular matrix contains the U matrix.
	/// </returns>
	/// <exception cref="DivideByZeroException">The matrix cannot be Gaussian eliminated because of a zero value in the main diagonal.</exception>
	public static (T[,] EliminatedMatrix, bool DeterminantSign) GaussianElimination(T[,] A)
	{
		int n = A.GetLength(0);
		int m = A.GetLength(1);
		T[,] LU = Duplicate(A);

		bool determinantSign = true;

		for (int k = 0; k < n - 1; ++k)
		{
			if (LU[k, k] == T.AdditiveIdentity)
			{
				int rowToSwap = k + 1;
				while (rowToSwap < n && LU[rowToSwap, k] == T.AdditiveIdentity)
					++rowToSwap;

				if (rowToSwap >= n)
					throw new DivideByZeroException("The matrix cannot be Gaussian eliminated!");

				for (int i = 0; i < m; ++i)
					(LU[k, i], LU[rowToSwap, i]) = (LU[rowToSwap, i], LU[k, i]);

				determinantSign = !determinantSign;
			}

			for (int i = k + 1; i < n; ++i)
			{
				T temp = LU[i, k] / LU[k, k];

				for (int j = k; j < m; ++j)
					LU[i, j] -= j != k ? LU[k, j] * temp : T.AdditiveIdentity;

				LU[i, k] = temp;
			}
		}

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
			throw new ArgumentException("The given matrix must be a square matrix!", nameof(A));

		T[,] B = GaussianElimination(HorizontalConcat(A, Identity(n, n))).EliminatedMatrix;

		for (int i = 0; i < n; ++i)
		{
			T temp = B[i, i];

			for (int j = 2 * n - 1; j >= 0; --j)
				B[i, j] = B[i, j] / temp;
		}

		T[,] C = new T[n, n];
		for (int i = 0; i < n; ++i)
		{
			for (int j = 2 * n - 1; j >= n; --j)
				C[i, j - n] = B[i, j];
		}

		return C;
	}

	/// <summary>
	/// Runs the Gaussian elimination to find the determinant of the given square matrix.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/Gaussian_elimination#Computing_determinants"/></remarks>
	/// <param name="A">The matrix for which the determinant should be found.</param>
	/// <returns>The determinant of the matrix.</returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	/// <exception cref="DivideByZeroException">The matrix cannot be Gaussian eliminated because of a zero value in the main diagonal.</exception>
	public static T Determinant(T[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException("The given matrix must be a square matrix!", nameof(A));

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
	/// <exception cref="DivideByZeroException">The matrix cannot be Gaussian eliminated because of a zero value in the main diagonal.</exception>
	/// <exception cref="ArgumentException">The extents of the two matrices are not equal.</exception>
	public static T[,] InverseProduct(T[,] A, T[,] B) => Product(A, Inverse(B));

	/// <summary>
	/// Calculates the LU decomposition of the given matrix using the Gaussian method.
	/// </summary>
	/// <remarks><see href="https://en.wikipedia.org/wiki/LU_decomposition"/></remarks>
	/// <param name="A">The matrix to invert.</param>
	/// <returns>The two parts of the decomposition: The lower (L) and upper (U) triangular matrix.</returns>
	/// <exception cref="ArgumentException"><paramref name="A"/> is not a square matrix.</exception>
	/// <exception cref="DivideByZeroException">
	/// The matrix cannot be Gaussian eliminated because of a zero value in the main diagonal.
	/// </exception>
	public static (T[,] L, T[,] U) LUDecomposition(T[,] A)
	{
		int n = A.GetLength(0);

		if (n != A.GetLength(1))
			throw new ArgumentException("The given matrix must be a square matrix!", nameof(A));

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
			throw new ArgumentException("The given matrix must be a square matrix!", nameof(A));

		T[,] Q = Duplicate(A);

		for (int j = 1; j < n; ++j)
		{
			T[] v = GetColumn(Q, j);
			T[] u = VectorOperations<T>.Duplicate(v);

			for (int k = j - 1; k >= 0; --k)
				u = VectorOperations<T>.Subtract(u, VectorOperations<T>.Project(GetColumn(Q, k), v));

			for (int i = 0; i < n; ++i)
				Q[i, j] = u[i];
		}

		for (int j = 0; j < n; ++j)
		{
			T[] u = GetColumn(Q, j);
			T magnitude = VectorOperations<T>.Magnitude(u);

			for (int i = 0; i < n; ++i)
				Q[i, j] = u[i] / magnitude;
		}

		T[,] R = Product(Transpose(Q), A);

		return (Q, R);
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

		T[,] B = Duplicate(A);
		T[,] C = Identity(n, n);

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
		int n = A.GetLength(0);
		int m = A.GetLength(1);

		List<string> rows = [];

		for (int i = 0; i < n; ++i)
		{
			List<string> row = [];

			for (int j = 0; j < m; ++j)
				row.Add($"{A[i, j]}");

			rows.Add(string.Join(Matrix<T>.ColumnSeparator, row));
		}

		return $"[{string.Join(Matrix<T>.RowSeparator, rows)}]";
	}

	#endregion
}