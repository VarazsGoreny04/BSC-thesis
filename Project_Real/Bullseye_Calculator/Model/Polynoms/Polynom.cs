using Bullseye_Calculator.Model.EuclideanSpace;
using Project_Real;

namespace Bullseye_Calculator.Model.Polynoms;

public static class Polynom
{
	public static Point2D Evaluate(Rational[] polynom, Rational basePoint)
	{
		Rational result = new();

		if (polynom.Length < 1)
			return new(result, basePoint);

		result += polynom[0];

		if (polynom.Length < 2)
			return new(result, basePoint);

		Rational x = basePoint;
		result += polynom[1] * x;

		for (int i = 2; i < polynom.Length; ++i)
		{
			x *= basePoint;
			result += polynom[i] * x;
		}

		return new(basePoint, result);
	}

	public static Point2D[] EvaluateRange(Rational[] polynom, Rational[] basePoints)
	{
		Point2D[] result = new Point2D[basePoints.Length];

		for (int i = basePoints.Length - 1; i >= 0; --i)
			result[i] = Evaluate(polynom, basePoints[i]);

		return result;
	}

	public static string ToString(Rational[] polynom)
	{
		string result = string.Empty;

		for (int i = polynom.Length - 1; i > 0; --i)
			result += $"({polynom[i]})x^{i}+";

		return $"{result}({polynom[0]})";
	}
}