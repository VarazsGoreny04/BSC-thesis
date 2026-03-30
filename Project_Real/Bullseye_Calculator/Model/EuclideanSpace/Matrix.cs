using Project_Real.NumberSet;
using System;
using System.Linq;
using System.Numerics;

namespace Bullseye_Calculator.Model.EuclideanSpace;

/// <summary>
/// Contains methods for operations with matrices.
/// </summary>
public class Matrix<T> :
	IComparisonOperators<Matrix<T>, Matrix<T>, bool>,
	IEqualityOperators<Matrix<T>, Matrix<T>, bool>,
	IComparisonOperators<Matrix<T>, T, bool>,
	IEqualityOperators<Matrix<T>, T, bool>,
	IUnaryPlusOperators<Matrix<T>, Matrix<T>>,
	IUnaryNegationOperators<Matrix<T>, Matrix<T>>,
	IAdditionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
	ISubtractionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
	IMultiplyOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
	IDivisionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
	IAdditionOperators<Matrix<T>, T, Matrix<T>>,
	ISubtractionOperators<Matrix<T>, T, Matrix<T>>,
	IMultiplyOperators<Matrix<T>, T, Matrix<T>>,
	IDivisionOperators<Matrix<T>, T, Matrix<T>>,
	IAdditiveIdentity<Matrix<T>, T>,
	IMultiplicativeIdentity<Matrix<T>, T>
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
	private const char rowSeparator = '&';
	private const char columnSeparator = ';';

	private readonly ValueHolder<T>[,] value;

	public static char RowSeparator => rowSeparator;
	public static char ColumnSeparator => columnSeparator;
	public static T AdditiveIdentity => T.AdditiveIdentity;
	public static T MultiplicativeIdentity => T.MultiplicativeIdentity;

	public int Rows => value.GetLength(0);
	public int Columns => value.GetLength(1);
	public ValueHolder<T>[,] Value => value;
	public ValueHolder<T> this[int row, int column] => value[row, column];

	public Matrix(string content)
	{
		string[] rows = content.Split(rowSeparator, StringSplitOptions.TrimEntries);
		string[][] tokenized = [.. rows.Select(row => row.Split(columnSeparator, StringSplitOptions.TrimEntries))];

		for (int i = 1; i < tokenized.Length; ++i)
		{
			if (tokenized[0].Length != tokenized[i].Length)
				throw new FormatException();
		}

		value = new ValueHolder<T>[tokenized.Length, tokenized[0].Length];
		Standard.StandardCalculator<T> standardCalculator = new();

		try
		{
			for (int row = tokenized.Length - 1; row >= 0; --row)
			{
				for (int col = tokenized[row].Length - 1; col >= 0; --col)
					value[row, col] = Calculator.Evaluate<T>(tokenized[row][col], standardCalculator);
			}
		}
		catch (IndexOutOfRangeException)
		{
			throw new FormatException();
		}
	}

	public Matrix(ValueHolder<T>[,] value)
	{
		int rows = value.GetLength(0);
		int cols = value.GetLength(1);

		if (rows < 1 || cols < 1)
			throw new ArgumentException();

		this.value = value;
	}

	public Matrix(T[,] value)
	{
		int rows = value.GetLength(0);
		int cols = value.GetLength(1);

		if (rows < 1 || cols < 1)
			throw new ArgumentException();

		this.value = new ValueHolder<T>[rows, cols];

		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
				this.value[i, j] = new Number<T>(value[i, j]);
		}
	}

	public static T[,] ToMatrix(Matrix<T> matrix)
	{
		int rows = matrix.value.GetLength(0);
		int cols = matrix.value.GetLength(1);

		T[,] result = new T[rows, cols];

		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
				result[i, j] = matrix[i, j].GetValue();
		}

		return result;
	}

	public static T[,] ToMatrix(int n, int m, T value)
	{
		T[,] id = Identity(n, m);

		for (int i = 0; i < Math.Min(n, m); i++)
			id[i, i] = value;

		return id;
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Matrix{T}"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Matrix<T> matrix && this == matrix;

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode() => throw new NotImplementedException();

	public static implicit operator Matrix<T>(ValueHolder<T>[,] value) => new(value);
	public static implicit operator Matrix<T>(T[,] value) => new(value);
	public static bool operator ==(Matrix<T>? left, Matrix<T>? right) => left is Matrix<T> l && right is Matrix<T> r && Equals(ToMatrix(l), ToMatrix(r));
	public static bool operator !=(Matrix<T>? left, Matrix<T>? right) => !(left == right);
	public static bool operator >(Matrix<T> left, Matrix<T> right) => GreaterThan(ToMatrix(left), ToMatrix(right));
	public static bool operator <(Matrix<T> left, Matrix<T> right) => GreaterThan(ToMatrix(right), ToMatrix(left));
	public static bool operator >=(Matrix<T> left, Matrix<T> right) => !(left < right);
	public static bool operator <=(Matrix<T> left, Matrix<T> right) => !(left > right);
	public static bool operator ==(Matrix<T>? left, T? right) => left is Matrix<T> l && right is T r && Equals(ToMatrix(l), ToMatrix(l.Rows, l.Columns, r));
	public static bool operator !=(Matrix<T>? left, T? right) => !(left == right);
	public static bool operator >(Matrix<T> left, T right) => GreaterThan(ToMatrix(left), ToMatrix(left.Rows, left.Columns, right));
	public static bool operator <(Matrix<T> left, T right) => GreaterThan(ToMatrix(left.Rows, left.Columns, right), ToMatrix(left));
	public static bool operator >=(Matrix<T> left, T right) => !(left < right);
	public static bool operator <=(Matrix<T> left, T right) => !(left > right);
	public static bool operator ==(T? left, Matrix<T>? right) => right == left;
	public static bool operator !=(T? left, Matrix<T>? right) => right != left;
	public static bool operator >(T left, Matrix<T> right) => right < left;
	public static bool operator <(T left, Matrix<T> right) => right > left;
	public static bool operator >=(T left, Matrix<T> right) => !(right > left);
	public static bool operator <=(T left, Matrix<T> right) => !(right < left);
	public static Matrix<T> operator +(Matrix<T> value) => value;
	public static Matrix<T> operator -(Matrix<T> value) => Scale(ToMatrix(value), -T.MultiplicativeIdentity);
	public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right) => Add(ToMatrix(left), ToMatrix(right));
	public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right) => Subtract(ToMatrix(left), ToMatrix(right));
	public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right) => Product(ToMatrix(left), ToMatrix(right));
	public static Matrix<T> operator /(Matrix<T> left, Matrix<T> right) => InverseProduct(ToMatrix(left), ToMatrix(right));
	public static Matrix<T> operator +(Matrix<T> left, T right) => Add(ToMatrix(left), ToMatrix(left.Rows, left.Columns, right));
	public static Matrix<T> operator -(Matrix<T> left, T right) => Subtract(ToMatrix(left), ToMatrix(left.Rows, left.Columns, right));
	public static Matrix<T> operator *(Matrix<T> left, T right) => Scale(ToMatrix(left), right);
	public static Matrix<T> operator /(Matrix<T> left, T right) => Scale(ToMatrix(left), T.MultiplicativeIdentity / right);
	public static Matrix<T> operator +(T left, Matrix<T> right) => Add(ToMatrix(right.Rows, right.Columns, left), ToMatrix(right));
	public static Matrix<T> operator -(T left, Matrix<T> right) => Subtract(ToMatrix(right.Rows, right.Columns, left), ToMatrix(right));
	public static Matrix<T> operator *(T left, Matrix<T> right) => Scale(ToMatrix(right), left);
	public static Matrix<T> operator /(T left, Matrix<T> right) => InverseProduct(ToMatrix(right.Rows, right.Columns, left), ToMatrix(right));





	/// <summary>
	/// Constructs a vector of <paramref name="n"/> length with full of zeros.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <returns>The matrix with zeros.</returns>
	public static T[] Zeros(int n)
	{
		T[] result = new T[n];
		Array.Fill(result, T.AdditiveIdentity);

		return result;
	}

	/// <summary>
	/// Constructs a vector of n length with full of ones.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <returns>The matrix with ones.</returns>
	public static T[] Ones(int n)
	{
		T[] result = new T[n];
		Array.Fill(result, T.MultiplicativeIdentity);

		return result;
	}

	/// <summary>
	/// Scales the given vector by a scalar.
	/// </summary>
	/// <param name="a">The vector to scale.</param>
	/// <param name="s">The scalar.</param>
	/// <returns>The scaled vector.</returns>
	public static T[] Scale(T[] a, T s)
	{
		int n = a.Length;
		T[] result = new T[n];

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
	public static T Magnitude(T[] a)
	{
		T result = T.AdditiveIdentity;

		for (int i = a.Length - 1; i >= 0; --i)
			result += a[i] * a[i];

		return ~result;
	}

	/// <summary>
	/// Calculates the inner product between two vectors.
	/// </summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The inner product value.</returns>
	/// <exception cref="ArgumentException">The length of the two vectors are not equal.</exception>
	public static T InnerProduct(T[] a, T[] b)
	{
		if (a.Length != b.Length)
			throw new ArgumentException();

		T result = T.AdditiveIdentity;

		for (int i = a.Length - 1; i >= 0; --i)
			result += a[i] * b[i];

		return result;
	}

	/// <summary>
	/// Calculates the outer product between two vectors.
	/// </summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The outer product matrix.</returns>
	public static T[,] OuterProduct(T[] a, T[] b)
	{
		int aN = a.Length;
		int bN = b.Length;
		T[,] result = new T[aN, bN];

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
	public static T[] Project(T[] a, T[] b) => Scale(a, InnerProduct(a, b) / InnerProduct(a, a));

	/// <summary>
	/// Compares two vectors.
	/// </summary>
	/// <param name="a">The first vector to compare.</param>
	/// <param name="b">The second vector to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the length and the value of <paramref name="a"/> is equal to the length and the value of <paramref name="b"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(T[] a, T[] b)
	{
		if (a.Length != b.Length)
			return false;

		int i = a.Length;

		while (--i > 0 && a[i] == b[i]) { }

		return i == 0 && a[0] == b[0];
	}

	/// <summary>
	/// Compares two vectors.
	/// </summary>
	/// <param name="a">The first vector to compare.</param>
	/// <param name="b">The second vector to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the length and the value of <paramref name="a"/> is greater than the length and the value of <paramref name="b"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool GreaterThan(T[] a, T[] b)
	{
		if (a.Length != b.Length)
			return a.Length > b.Length;

		int i = a.Length;

		while (--i > 0 && a[i] == b[i]) { }

		return a[i] > b[i];
	}

	/// <summary>
	/// Adds vector <paramref name="b"/> from vector <paramref name="a"/>.
	/// </summary>
	/// <param name="a">The vector to be added.</param>
	/// <param name="b">The adding vector.</param>
	/// <returns>The added vector.</returns>
	/// <exception cref="ArgumentException">The length of the two vectors are not equal.</exception>
	public static T[] Add(T[] a, T[] b)
	{
		int n = a.Length;

		if (n != b.Length)
			throw new ArgumentException();

		T[] result = new T[n];

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
	public static T[] Subtract(T[] a, T[] b)
	{
		int n = a.Length;

		if (n != b.Length)
			throw new ArgumentException();

		T[] result = new T[n];

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
	public static T[,] Zeros(int n, int m)
	{
		T[,] result = new T[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
				result[i, j] = T.AdditiveIdentity;
		}

		return result;
	}

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> matrix full of ones.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <returns>The matrix with ones.</returns>
	public static T[,] Ones(int n, int m)
	{
		T[,] result = new T[n, m];

		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < m; ++j)
				result[i, j] = T.MultiplicativeIdentity;
		}

		return result;
	}

	/// <summary>
	/// Constructs an <paramref name="n"/>-by-<paramref name="m"/> identity matrix.
	/// </summary>
	/// <param name="n">The number of rows in the matrix.</param>
	/// <param name="m">The number of columns in the matrix.</param>
	/// <returns>The identity matrix.</returns>
	public static T[,] Identity(int n, int m)
	{
		T[,] result = Zeros(n, m);

		for (int i = 0; i < Math.Min(n, m); ++i)
			result[i, i] = T.MultiplicativeIdentity;

		return result;
	}

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

		T[,] result = new T[Math.Max(aRowCount, bRowCount), aColCount + bColCount];

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

		T[,] result = new T[aRowCount + bRowCount, Math.Max(aColCount, bColCount)];

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
				u = Subtract(u, Project(GetColumn(U, k), v));

			// Update the column entries in U.
			for (int i = 0; i < n; ++i)
				U[i, j] = u[i];
		}

		// Normalize the column vectors of U.
		for (int j = 0; j < n; ++j)
		{
			T[] u = GetColumn(U, j);
			T magnitude = Magnitude(u);

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
	/// Prints the given vector.
	/// </summary>
	/// <param name="a">The vector to print.</param>
	/// <returns>The string representation of the given vector.</returns>
	/// <exception cref="ArgumentException">The vector</exception>
	public static string ToString(T[] a) => a.Length < 1 ? "[ ]" : $"[ {string.Join(",\t", a)} ]";

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