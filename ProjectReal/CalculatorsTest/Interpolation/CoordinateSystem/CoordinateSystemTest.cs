using Calculators.Interpolation;
using ProjectReal.Number;

namespace CalculatorsTest.Interpolation.CoordinateSystem;

[TestClass]
public class CoordinateSystemTest
{
	[TestMethod]
	public void LinSpaceMethod()
	{
		Rational[] result;

		foreach (CoordinateSystemTestCase item in CoordinateSystemTestCases.List)
		{
			result = CoordinateSystem<Rational>.LinSpace(item.Start, item.End, item.Points);

			Assert.AreEqual(item.LinSpace.Length, result.Length);

			for (int i = item.LinSpace.Length - 1; i >= 0; --i)
				Assert.AreEqual(item.LinSpace[i], result[i]);
		}
	}
}