using Project_Real;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class CoordinateSystem
{
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