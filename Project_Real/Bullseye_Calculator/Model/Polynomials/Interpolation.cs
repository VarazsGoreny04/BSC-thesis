using Bullseye_Calculator.Model.EuclideanSpace;
using Project_Real;
using System;

namespace Bullseye_Calculator.Model.Polynomials;

/// <summary>
/// Contains methods for interpolation.
/// </summary>
public static class Interpolation
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
	public static Rational[] Lagrange(Point2D[] points)
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

		Rational[] result = Matrix.Zeros(points.Length);

		for (int i = 0; i < points.Length; ++i)
		{
			Rational multiplier = points[i].Y;
			Rational[] polynomial = ["1"];

			for (int j = 0; j < points.Length; ++j)
			{
				if (i == j)
					continue;

				multiplier /= points[i].X - points[j].X;

				Rational[,] temp = Matrix.OuterProduct(polynomial, [-points[j].X, "1"]);
				polynomial = Matrix.Zeros(temp.GetLength(0) + 1);

				for (int k = temp.GetLength(0) - 1; k >= 0; --k)
				{
					for (int l = temp.GetLength(1) - 1; l >= 0; --l)
						polynomial[k + l] += temp[k, l];
				}
			}

			result = Matrix.Add(result, Matrix.Scale(polynomial, multiplier));
		}

		return result;
	}

	#endregion
}