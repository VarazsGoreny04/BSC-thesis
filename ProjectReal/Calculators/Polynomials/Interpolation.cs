using Calculators.EuclideanSpace;
using ProjectReal.NumberSet;
using System;
using System.Numerics;

namespace Calculators.Polynomials;

/// <summary>
/// Contains methods for interpolation.
/// </summary>
/// <typeparam name="T">The type to calculate with.</typeparam>
public static class Interpolation<T>
where T :
	IComparisonOperators<T, T, bool>,
	IEqualityOperators<T, T, bool>,
	IUnaryPlusOperators<T, T>,
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
	/// Checks if there are matching bases amongst the coordinates.
	/// </summary>
	/// <param name="points">The points to check.</param>
	/// <exception cref="ArgumentException">
	/// <paramref name="points"/> is empty
	/// -or-
	/// some of the <paramref name="points"/> have matching X coordinate.
	/// </exception>
	public static void CheckBases(Point2D<T>[] points)
	{
		if (points.Length < 1)
			throw new ArgumentException("The vector must be at least one dimensional!");

		for (int i = 0; i < points.Length; ++i)
		{
			for (int j = i + 1; j < points.Length; ++j)
			{
				if (points[i].X == points[j].X)
					throw new ArgumentException("Some of the points have matching X coordinate");
			}
		}
	}

	/// <summary>
	/// Calculates the Lagrange basis polynomial for one point of the <paramref name="points"/> array.
	/// </summary>
	/// <param name="points">The fix points of the polynomial.</param>
	/// <param name="index">The index of the point in the <paramref name="points"/> array.</param>
	/// <returns>The created Lagrange basis polynomial.</returns>
	/// <exception cref="ArgumentException">The <paramref name="points"/> array cannot be empty.</exception>
	/// <exception cref="IndexOutOfRangeException">The <paramref name="index"/> must be within the bounds of the array.</exception>
	public static T[] LagrangeBasis(Point2D<T>[] points, int index)
	{
		if (points.Length < 1)
			throw new ArgumentException("The array cannot be empty!", nameof(points));

		T denominator = T.MultiplicativeIdentity;
		T[] numerator = [T.MultiplicativeIdentity];

		for (int i = 0; i < points.Length; ++i)
		{
			if (index == i)
				continue;

			denominator /= points[index].X - points[i].X;

			T[,] temp = VectorOperations<T>.OuterProduct(numerator, [-points[i].X, T.MultiplicativeIdentity]);
			int tempLength = temp.GetLength(0);
			numerator = VectorOperations<T>.Zeros(tempLength + 1);

			for (int j = tempLength - 1; j >= 0; --j)
			{
				for (int k = 0; k < 2; ++k)
					numerator[j + k] += temp[j, k];
			}
		}

		return VectorOperations<T>.Scale(numerator, denominator);
	}

	/// <summary>
	/// Calculates the Lagrange interpolating polynomial that crosses each point in the <paramref name="points"/> array.
	/// </summary>
	/// <param name="points">The points the polynomial needs to cross.</param>
	/// <returns>The coefficients of the polynomial in an array.</returns>
	/// <exception cref="ArgumentException">The <paramref name="points"/> array cannot be empty.</exception>
	public static T[] Lagrange(Point2D<T>[] points)
	{
		T[] result = VectorOperations<T>.Zeros(points.Length);

		for (int i = 0; i < points.Length; ++i)
			result = VectorOperations<T>.Add(result, VectorOperations<T>.Scale(LagrangeBasis(points, i), points[i].Y));

		return result;
	}

	/// <summary>
	/// Calculates the Lagrange interpolating polynomial using the precalculated Lagrange basis polynomials.
	/// </summary>
	/// <param name="points">The points the polynomial needs to cross.</param>
	/// <param name="lagrangeBasisPolynomials">An array of corresponding Lagrange basis polynomials.</param>
	/// <returns>The coefficients of the polynomial in an array.</returns>
	/// <exception cref="ArgumentException">Make sure to give the corresponding points and basis polynomials to this method.</exception>
	public static T[] Lagrange(Point2D<T>[] points, T[][] lagrangeBasisPolynomials)
	{
		T[] result = VectorOperations<T>.Zeros(points.Length);

		for (int i = points.Length - 1; i >= 0; --i)
			result = VectorOperations<T>.Add(result, VectorOperations<T>.Scale(lagrangeBasisPolynomials[i], points[i].Y));

		return result;
	}

	#endregion
}