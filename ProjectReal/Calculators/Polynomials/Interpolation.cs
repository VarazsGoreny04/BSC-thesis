using Calculators.EuclideanSpace;
using ProjectReal.NumberSet;
using System;
using System.Numerics;

namespace Calculators.Polynomials;

/// <summary>
/// Contains methods for interpolation.
/// </summary>
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
	/// Calculates the Lagrange-polynomial that crosses each point in the <paramref name="points"/> array.
	/// </summary>
	/// <param name="points"></param>
	/// <returns>The coefficients of the polynomial in an array.</returns>
	/// <exception cref="ArgumentException">
	/// <paramref name="points"/> is empty
	/// -or-
	/// some of the <paramref name="points"/> have matching X coordinate.
	/// </exception>
	public static T[] Lagrange(Point2D<T>[] points)
	{
		if (points.Length < 1)
			throw new ArgumentException();

		for (int i = 0; i < points.Length; ++i)
		{
			for (int j = i + 1; j < points.Length; ++j)
			{
				if (points[i].X == points[j].X)
					throw new ArgumentException();
			}
		}

		T[] result = Matrix<T>.Zeros(points.Length);

		for (int i = 0; i < points.Length; ++i)
		{
			T multiplier = points[i].Y;
			T[] polynomial = [T.MultiplicativeIdentity];

			for (int j = 0; j < points.Length; ++j)
			{
				if (i == j)
					continue;

				multiplier /= points[i].X - points[j].X;

				T[,] temp = Matrix<T>.OuterProduct(polynomial, [-points[j].X, T.MultiplicativeIdentity]);
				polynomial = Matrix<T>.Zeros(temp.GetLength(0) + 1);

				for (int k = temp.GetLength(0) - 1; k >= 0; --k)
				{
					for (int l = temp.GetLength(1) - 1; l >= 0; --l)
						polynomial[k + l] += temp[k, l];
				}
			}

			result = Matrix<T>.Add(result, Matrix<T>.Scale(polynomial, multiplier));
		}

		return result;
	}

	#endregion
}