using ProjectReal.NumberSet;
using System;
using System.Numerics;

namespace Calculators.EuclideanSpace;

/// <summary>
/// Contains methods for one dimensional arrays - vectors.
/// </summary>
/// <typeparam name="T">The type of values within the vector.</typeparam>
public class VectorOperations<T>
where T :
	IComparisonOperators<T, T, bool>,
	IEqualityOperators<T, T, bool>,
	IUnaryNegationOperators<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IModulusOperators<T, T, T>,
	IPowerOperations<T, T, T>,
	IRootOperations<T, T, T>,
	IAdditiveIdentity<T, T>,
	IMultiplicativeIdentity<T, T>,
	IParsable<T>
{
	#region Public methods

	/// <summary>
	/// Constructs a vector of <paramref name="n"/> length by the given <paramref name="value"/>.
	/// </summary>
	/// <param name="n">The number of rows in the vector.</param>
	/// <param name="value">The value of the elements.</param>
	/// <returns>The vector.</returns>
	public static T[] Full(int n, T value)
	{
		T[] result = new T[n];
		Array.Fill(result, value);

		return result;
	}

	/// <summary>
	/// Constructs a vector of <paramref name="n"/> length with full of zeros.
	/// </summary>
	/// <param name="n">The number of rows in the vector.</param>
	/// <returns>The vector with zeros.</returns>
	public static T[] Zeros(int n)
	{
		T[] result = new T[n];
		Array.Fill(result, T.AdditiveIdentity);

		return result;
	}

	/// <summary>
	/// Constructs a vector of n length with full of ones.
	/// </summary>
	/// <param name="n">The number of rows in the vector.</param>
	/// <returns>The vector with ones.</returns>
	public static T[] Ones(int n)
	{
		T[] result = new T[n];
		Array.Fill(result, T.MultiplicativeIdentity);

		return result;
	}

	/// <summary>
	/// Duplicates the given vector.
	/// </summary>
	/// <param name="a">The vector to duplicate.</param>
	/// <returns>The duplicated vector.</returns>
	public static T[] Duplicate(T[] a)
	{
		int n = a.Length;
		T[] result = new T[n];

		for (int i = 0; i < n; ++i)
			result[i] = a[i];

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
			throw new ArgumentException("The two vectors must have the same length!");

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
	/// <returns>The outer product vector.</returns>
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

		return i < 0 || i == 0 && a[0] == b[0];
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
			throw new ArgumentException("The two vectors must have the same length!");

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
			throw new ArgumentException("The two vectors must have the same length!");

		T[] result = new T[n];

		for (int i = 0; i < n; ++i)
			result[i] = a[i] - b[i];

		return result;
	}

	/// <summary>
	/// Prints the given vector.
	/// </summary>
	/// <param name="a">The vector to print.</param>
	/// <returns>The string representation of the given vector.</returns>
	/// <exception cref="ArgumentException">The vector</exception>
	public static string ToString(T[] a) => $"[{string.Join(Matrix<T>.RowSeparator, a)}]";

	#endregion
}