using Calculators.EuclideanSpace;
using Calculators.Standard;
using ProjectReal.NumberSet;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Calculators.Polynomials;

/// <summary>
/// A calculator that understands 2×N matrices as (X, Y) coordinate pairs and can perform interpolations with them.
/// </summary>
/// <typeparam name="T">The type to calculate with.</typeparam>
public partial class PolynomialCalculator<T> : Calculator
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
	#region GeneratedRegex

	[GeneratedRegex(@"^\[.*\]$")]
	protected static partial Regex BracketedRegex();

	#endregion

	#region Fields

	protected EuclideanSpaceCalculator<T> euclideanSpaceCalculator;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a calculator that is capable of doing interpolations.
	/// </summary>
	public PolynomialCalculator(StandardCalculator<T> standardCalculator) : base(
		[
			// Matrix
			new(BracketedRegex(), match => new MatrixHolder<T>(new Matrix<T>(match[1..^1], standardCalculator)))
		]
	) => euclideanSpaceCalculator = new EuclideanSpaceCalculator<T>([], standardCalculator);

	#endregion

	#region Protected methods

	protected override string[] GetFunctions() => [];

	/// <summary>
	/// Turns the given expression tree to points.
	/// </summary>
	/// <param name="expressionTree">The expression tree.</param>
	/// <returns>The created <see cref="Point2D{T}"/> array.</returns>
	/// <exception cref="ArgumentException">The evaluated matrix must have exactly 2 columns.</exception>
	protected Point2D<T>[] GetPoints(ValueHolder<Matrix<T>> expressionTree)
	{
		Matrix<T> matrix = expressionTree.GetValue();

		if (matrix.Columns != 2)
			throw new ArgumentException("The evaluated matrix must have exactly 2 columns!");
		
		Point2D<T>[] points = new Point2D<T>[matrix.Rows];

		for (int i = matrix.Rows - 1; i >= 0; --i)
			points[i] = new Point2D<T>(matrix[i, 0].GetValue(), matrix[i, 1].GetValue());

		return points;
	}

	#endregion

	#region Public methods

	public override List<(string Calculation, string State)> FullEvaluation(string input)
	{
		ValueHolder<Matrix<T>> expressionTree = Evaluate<Matrix<T>>(input, euclideanSpaceCalculator);

		List<(string, string)> result = FullEvaluation(expressionTree);
		Point2D<T>[] points = GetPoints(expressionTree);

		Interpolation<T>.CheckBases(points);

		List<T[]> lagrangeBasisPolynomials = [];

		for (int i = 0; i < points.Length; ++i)
		{
			T[] lagrangeBasisPolynomial = Interpolation<T>.LagrangeBasis(points, i);

			lagrangeBasisPolynomials.Add(lagrangeBasisPolynomial);
			result.Add(($"l({i})", Polynomial<T>.ToString(lagrangeBasisPolynomial)));
		}

		result.Add(($"L=∑{{k=0..{points.Length}}}y(k)l(k)", Polynomial<T>.ToString(Interpolation<T>.Lagrange(points, [.. lagrangeBasisPolynomials]))));

		return result;
	}

	#endregion
}