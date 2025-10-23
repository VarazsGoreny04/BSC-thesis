using Project_Real;

namespace Bullseye_Calculator.Model.EuclideanSpace;

/// <summary>
/// Contains methods for basic operations with the Cartesian 2D coordinate system.
/// </summary>
public class CoordinateSystem
{
	/// <summary>
	/// Divides the space between the <paramref name="start"/> and <paramref name="end"/> points to the given number of points.
	/// </summary>
	/// <param name="start">The start point.</param>
	/// <param name="end">The end point.</param>
	/// <param name="points">The number of points in the result array.</param>
	/// <returns>The array of the divided points.</returns>
	public static Rational[] LinSpace(Rational start, Rational end, int points = 100)
	{
		Rational[] result = new Rational[points];
		Rational step = (end - start) / (new Rational(points.ToString()) - "1");

		result[0] = start;
		for (int i = 1; i < points; ++i)
			result[i] = result[i - 1] + step;

		return result;
	}
}