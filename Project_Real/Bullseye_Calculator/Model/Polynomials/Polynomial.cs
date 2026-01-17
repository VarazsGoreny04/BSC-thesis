using Bullseye_Calculator.Model.EuclideanSpace;
using Project_Real;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Polynomials;

/// <summary>
/// Contains methods for basic operations with polynomials.
/// </summary>
public static class Polynomial
{
	#region Public methods

	/// <summary>
	/// Calculates the value of a <paramref name="polynomial"/> at the given <paramref name="basePoint"/>.
	/// </summary>
	/// <param name="polynomial">The polynomial.</param>
	/// <param name="basePoint">The base point of the calculation.</param>
	/// <returns>The value of a <paramref name="polynomial"/> at the given <paramref name="basePoint"/>.</returns>
	public static Point2D Evaluate(Rational[] polynomial, Rational basePoint)
	{
		Rational result = new();

		if (polynomial.Length < 1)
			return new Point2D(result, basePoint);

		result += polynomial[0];

		if (polynomial.Length < 2)
			return new Point2D(result, basePoint);

		Rational x = basePoint;
		result += polynomial[1] * x;

		for (int i = 2; i < polynomial.Length; ++i)
		{
			x *= basePoint;
			result += polynomial[i] * x;
		}

		return new Point2D(basePoint, result);
	}

	/// <summary>
	/// Calculates the values of a <paramref name="polynomial"/> at the given <paramref name="basePoints"/>.
	/// </summary>
	/// <param name="polynomial">The polynomial.</param>
	/// <param name="basePoints">The base points of the calculation.</param>
	/// <returns>The values of a <paramref name="polynomial"/> at the given <paramref name="basePoints"/>.</returns>
	public static Point2D[] EvaluateRange(Rational[] polynomial, Rational[] basePoints)
	{
		Point2D[] result = new Point2D[basePoints.Length];

		for (int i = basePoints.Length - 1; i >= 0; --i)
			result[i] = Evaluate(polynomial, basePoints[i]);

		return result;
	}

	/// <summary>
	/// Returns a <see cref="string"/> that represents the given <paramref name="polynomial"/>.
	/// </summary>
	/// <returns>The <paramref name="polynomial"/> as a <see langword="string"/>.</returns>
	public static string ToString(Rational[] polynomial)
	{
		List<string> parts = [];

		for (int i = polynomial.Length - 1; i > 0; --i)
		{
			if (polynomial[i] != "0")
				parts.Add($"({polynomial[i]})x^{i}");
		}

		if (polynomial[0] != "0")
			parts.Add($"({polynomial[0]})");

		return string.Join('+', parts);
	}

	#endregion
}