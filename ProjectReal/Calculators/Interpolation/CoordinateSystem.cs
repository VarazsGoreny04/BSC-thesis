using System;
using System.Numerics;

namespace Calculators.Interpolation;

/// <summary>
/// Contains methods for basic operations with the Cartesian 2D coordinate system.
/// </summary>
/// <typeparam name="T">The type to calculate with.</typeparam>
public static class CoordinateSystem<T>
where T : 
	IMultiplicativeIdentity<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IParsable<T>
{
	#region Public methods

	/// <summary>
	/// Divides the space between the <paramref name="start"/> and <paramref name="end"/> points to the given number of points.
	/// </summary>
	/// <param name="start">The start point.</param>
	/// <param name="end">The end point.</param>
	/// <param name="points">The number of points in the result array.</param>
	/// <returns>The array of the divided points.</returns>
	/// <exception cref="ArgumentOutOfRangeException">"The number of <paramref name="points"/> must be at least 2."</exception>
	public static T[] LinSpace(T start, T end, int points = 100)
	{
		if (points < 2)
			throw new ArgumentOutOfRangeException(nameof(points), points, "The number of points must be at least 2!");

		T[] result = new T[points];
		T step = (end - start) / (T.Parse(points.ToString(), null) - T.MultiplicativeIdentity);

		result[0] = start;
		for (int i = 1; i < points - 1; ++i)
			result[i] = result[i - 1] + step;
		result[points - 1] = end;

		return result;
	}

	#endregion
}