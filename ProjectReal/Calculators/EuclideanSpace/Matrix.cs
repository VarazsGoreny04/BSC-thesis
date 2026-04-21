using Calculators.Standard;
using ProjectReal.NumberSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Calculators.EuclideanSpace;

/// <summary>
/// Represents a matrix.
/// </summary>
/// <typeparam name="T">The type of values within the matrix.</typeparam>
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
	IAdditiveIdentity<Matrix<T>, Matrix<T>>,
	IMultiplicativeIdentity<Matrix<T>, Matrix<T>>
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
	#region Constants

	private const char rowSeparator = '&';
	private const char columnSeparator = ';';

	#endregion

	#region Fields

	private readonly ValueHolder<T>[,] value;

	#endregion

	#region Properties

	/// <returns>The row separator used in the <see cref="string"/> representation of the <see cref="Matrix{T}"/>.</returns>
	public static char RowSeparator => rowSeparator;

	/// <returns>The column separator used in the <see cref="string"/> representation of the <see cref="Matrix{T}"/>.</returns>
	public static char ColumnSeparator => columnSeparator;

	/// <returns>The 1-by-1 zero <see cref="Matrix{T}"/>. [0]</returns>
	public static Matrix<T> AdditiveIdentity => new(new T[1, 1] { { T.AdditiveIdentity } });

	/// <returns>The 1-by-1 one <see cref="Matrix{T}"/>. [1]</returns>
	public static Matrix<T> MultiplicativeIdentity => new(new T[1, 1] { { T.MultiplicativeIdentity } });

	/// <returns>The number of rows in <see langword="this"/> <see cref="Matrix{T}"/>.</returns>
	public int Rows => value.GetLength(0);

	/// <returns>The number of columns in <see langword="this"/> <see cref="Matrix{T}"/>.</returns>
	public int Columns => value.GetLength(1);

	/// <returns>The value inside <see langword="this"/> <see cref="Matrix{T}"/>.</returns>
	public ValueHolder<T>[,] Value => value;

	/// <returns>
	/// The <see cref="ValueHolder{T}"/> inside <see langword="this"/> <see cref="Matrix{T}"/>
	/// indexed by the <paramref name="row"/> and <paramref name="column"/>.
	/// </returns>
	public ValueHolder<T> this[int row, int column] => value[row, column];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Matrix{T}"/> by the given <see cref="string"/>.
	/// </summary>
	/// <param name="content">The matrix in <see cref="string"/> format.</param>
	/// <param name="standardCalculator">The calculator for the elements of the matrix.</param>
	/// <exception cref="FormatException">The rows of the matrix must have the the same number of elements.</exception>
	public Matrix(string content, StandardCalculator<T> standardCalculator)
	{
		string[] rows = content.Split(rowSeparator, StringSplitOptions.TrimEntries);
		string[][] tokenized = [.. rows.Select(row => row.Split(columnSeparator, StringSplitOptions.TrimEntries))];

		for (int i = 1; i < tokenized.Length; ++i)
		{
			if (tokenized[0].Length != tokenized[i].Length)
				throw new FormatException();
		}

		value = new ValueHolder<T>[tokenized.Length, tokenized[0].Length];

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

	/// <summary>
	/// Constructs a <see cref="Matrix{T}"/> by the given <see cref="string"/> using the <see cref="StandardCalculator{T}"/> without any functions.
	/// </summary>
	/// <param name="content">The matrix in <see cref="string"/> format.</param>
	/// <exception cref="FormatException">The rows of the matrix must have the the same number of elements.</exception>
	public Matrix(string content) : this(content, new StandardCalculator<T>([])) { }

	/// <summary>
	/// Creates a <see cref="Matrix{T}"/> by the given <see cref="ValueHolder{T}"/> matrix.
	/// </summary>
	/// <param name="value">The given <see cref="ValueHolder{T}"/> matrix.</param>	
	/// /// <exception cref="ArgumentException">The given matrix must have at least one element.</exception>
	public Matrix(ValueHolder<T>[,] value)
	{
		int rows = value.GetLength(0);
		int cols = value.GetLength(1);

		if (rows < 1 || cols < 1)
			throw new ArgumentException();

		this.value = value;
	}

	/// <summary>
	/// Creates a <see cref="Matrix{T}"/> by the given matrix.
	/// </summary>
	/// <param name="value">The given matrix.</param>
	/// <exception cref="ArgumentException">The given matrix must have at least one element.</exception>
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

	#endregion

	#region Public methods

	/// <summary>
	/// Creates a matrix by the given <see cref="Matrix{T}"/>.
	/// </summary>
	/// <param name="matrix">The given <see cref="Matrix{T}"/>.</param>
	/// <returns>The created matrix.</returns>
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

	/// <summary>
	/// Returns a <see cref="string"/> that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Matrix{T}"/> as a <see langword="string"/>.</returns>
	public override string ToString()
	{
		List<string> rows = [];

		for (int i = 0; i < Rows; ++i)
		{
			List<string> row = [];

			for (int j = 0; j < Columns; ++j)
				row.Add(value[i, j].ToString());

			rows.Add(string.Join(ColumnSeparator, row));
		}

		return $"[{string.Join(RowSeparator, rows)}]";
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

	#endregion

	#region Operators

	public static implicit operator Matrix<T>(ValueHolder<T>[,] value) => new(value);
	public static implicit operator Matrix<T>(T[,] value) => new(value);
	public static bool operator ==(Matrix<T>? left, Matrix<T>? right) => left is Matrix<T> l && right is Matrix<T> r && Equals(ToMatrix(l), ToMatrix(r));
	public static bool operator !=(Matrix<T>? left, Matrix<T>? right) => !(left == right);
	public static bool operator >(Matrix<T> left, Matrix<T> right) => MatrixOperations<T>.GreaterThan(ToMatrix(left), ToMatrix(right));
	public static bool operator <(Matrix<T> left, Matrix<T> right) => MatrixOperations<T>.GreaterThan(ToMatrix(right), ToMatrix(left));
	public static bool operator >=(Matrix<T> left, Matrix<T> right) => !(left < right);
	public static bool operator <=(Matrix<T> left, Matrix<T> right) => !(left > right);
	public static bool operator ==(Matrix<T>? left, T? right)
	{
		return left is Matrix<T> l && right is T r && Equals(ToMatrix(l), MatrixOperations<T>.Diagonal(l.Rows, l.Columns, r));
	}
	public static bool operator !=(Matrix<T>? left, T? right) => !(left == right);
	public static bool operator >(Matrix<T> left, T right)
	{
		return MatrixOperations<T>.GreaterThan(ToMatrix(left), MatrixOperations<T>.Diagonal(left.Rows, left.Columns, right));
	}
	public static bool operator <(Matrix<T> left, T right)
	{
		return MatrixOperations<T>.GreaterThan(MatrixOperations<T>.Diagonal(left.Rows, left.Columns, right), ToMatrix(left));
	}
	public static bool operator >=(Matrix<T> left, T right) => !(left < right);
	public static bool operator <=(Matrix<T> left, T right) => !(left > right);
	public static bool operator ==(T? left, Matrix<T>? right) => right == left;
	public static bool operator !=(T? left, Matrix<T>? right) => right != left;
	public static bool operator >(T left, Matrix<T> right) => right < left;
	public static bool operator <(T left, Matrix<T> right) => right > left;
	public static bool operator >=(T left, Matrix<T> right) => !(right > left);
	public static bool operator <=(T left, Matrix<T> right) => !(right < left);
	public static Matrix<T> operator +(Matrix<T> value) => value;
	public static Matrix<T> operator -(Matrix<T> value) => MatrixOperations<T>.Scale(ToMatrix(value), -T.MultiplicativeIdentity);
	public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)  // TODO: Computing with identity matrix is not correct
	{
		return (left, right) switch
		{
			(Matrix<T>, Matrix<T>) when left.Rows == 1 && left.Columns == 1 => left[0, 0].GetValue() + right,
			(Matrix<T>, Matrix<T>) when right.Rows == 1 && right.Columns == 1 => left + right[0, 0].GetValue(),
			_ => MatrixOperations<T>.Add(ToMatrix(left), ToMatrix(right))
		};
	}
	public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
	{
		return (left, right) switch
		{
			(Matrix<T>, Matrix<T>) when left.Rows == 1 && left.Columns == 1 => left[0, 0].GetValue() - right,
			(Matrix<T>, Matrix<T>) when right.Rows == 1 && right.Columns == 1 => left - right[0, 0].GetValue(),
			_ => MatrixOperations<T>.Subtract(ToMatrix(left), ToMatrix(right))
		};
	}
	public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
	{
		return (left, right) switch
		{
			(Matrix<T>, Matrix<T>) when left.Rows == 1 && left.Columns == 1 => left[0, 0].GetValue() * right,
			(Matrix<T>, Matrix<T>) when right.Rows == 1 && right.Columns == 1 => left * right[0, 0].GetValue(),
			_ => MatrixOperations<T>.Product(ToMatrix(left), ToMatrix(right))
		};
	}
	public static Matrix<T> operator /(Matrix<T> left, Matrix<T> right)
	{
		return (left, right) switch
		{
			(Matrix<T>, Matrix<T>) when left.Rows == 1 && left.Columns == 1 => left[0, 0].GetValue() / right,
			(Matrix<T>, Matrix<T>) when right.Rows == 1 && right.Columns == 1 => left / right[0, 0].GetValue(),
			_ => MatrixOperations<T>.InverseProduct(ToMatrix(left), ToMatrix(right))
		};
	}
	public static Matrix<T> operator +(Matrix<T> left, T right)
	{
		return MatrixOperations<T>.Add(ToMatrix(left), MatrixOperations<T>.Full(left.Rows, left.Columns, right));
	}
	public static Matrix<T> operator -(Matrix<T> left, T right)
	{
		return MatrixOperations<T>.Subtract(ToMatrix(left), MatrixOperations<T>.Full(left.Rows, left.Columns, right));
	}
	public static Matrix<T> operator *(Matrix<T> left, T right) => MatrixOperations<T>.Scale(ToMatrix(left), right);
	public static Matrix<T> operator /(Matrix<T> left, T right) => MatrixOperations<T>.Scale(ToMatrix(left), T.MultiplicativeIdentity / right);
	public static Matrix<T> operator +(T left, Matrix<T> right) => right + left;
	public static Matrix<T> operator -(T left, Matrix<T> right) => (-right) + left;
	public static Matrix<T> operator *(T left, Matrix<T> right) => right * left;
	public static Matrix<T> operator /(T left, Matrix<T> right) => right / left;

	#endregion
}