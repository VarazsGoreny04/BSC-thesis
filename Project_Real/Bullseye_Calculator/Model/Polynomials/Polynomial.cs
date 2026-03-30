using Bullseye_Calculator.Model.EuclideanSpace;
using Project_Real.NumberSet;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Bullseye_Calculator.Model.Polynomials;

/// <summary>
/// Contains methods for basic operations with polynomials.
/// </summary>
public static class Polynomial<T>
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
	/// Calculates the value of a <paramref name="polynomial"/> at the given <paramref name="basePoint"/>.
	/// </summary>
	/// <param name="polynomial">The polynomial.</param>
	/// <param name="basePoint">The base point of the calculation.</param>
	/// <returns>The value of a <paramref name="polynomial"/> at the given <paramref name="basePoint"/>.</returns>
	public static Point2D<T> Evaluate(T[] polynomial, T basePoint)
	{
		T result = T.AdditiveIdentity;

		if (polynomial.Length < 1)
			return new Point2D<T>(result, basePoint);

		result += polynomial[0];

		if (polynomial.Length < 2)
			return new Point2D<T>(result, basePoint);

		T x = basePoint;
		result += polynomial[1] * x;

		for (int i = 2; i < polynomial.Length; ++i)
		{
			x *= basePoint;
			result += polynomial[i] * x;
		}

		return new Point2D<T>(basePoint, result);
	}

	/// <summary>
	/// Calculates the values of a <paramref name="polynomial"/> at the given <paramref name="basePoints"/>.
	/// </summary>
	/// <param name="polynomial">The polynomial.</param>
	/// <param name="basePoints">The base points of the calculation.</param>
	/// <returns>The values of a <paramref name="polynomial"/> at the given <paramref name="basePoints"/>.</returns>
	public static Point2D<T>[] EvaluateRange(T[] polynomial, T[] basePoints)
	{
		Point2D<T>[] result = new Point2D<T>[basePoints.Length];

		for (int i = basePoints.Length - 1; i >= 0; --i)
			result[i] = Evaluate(polynomial, basePoints[i]);

		return result;
	}

	/// <summary>
	/// Returns a <see cref="string"/> that represents the given <paramref name="polynomial"/>.
	/// </summary>
	/// <returns>The <paramref name="polynomial"/> as a <see langword="string"/>.</returns>
	public static string ToString(T[] polynomial)
	{
		List<string> parts = [];

		for (int i = polynomial.Length - 1; i > 0; --i)
		{
			if (polynomial[i] != T.AdditiveIdentity)
				parts.Add($"({polynomial[i]})x^{i}");
		}

		if (polynomial[0] != T.AdditiveIdentity)
			parts.Add($"({polynomial[0]})");

		return string.Join('+', parts);
	}

	#endregion
}