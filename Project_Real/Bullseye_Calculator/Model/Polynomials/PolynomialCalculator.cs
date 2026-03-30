using Project_Real.NumberSet;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model.EuclideanSpace;

/// <summary>
/// A calculator that understands matrices and can perform operations with them.
/// </summary>
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

	#region Constructors

	/// <summary>
	/// Constructs a calculator that understands matrices and can perform operations with them.
	/// </summary>
	public PolynomialCalculator() : base(
	[
		// Matrix
		new(BracketedRegex(), match => new MatrixHolder<T>(new Matrix<T>(match[1..^1]))),
		// Separators
		new(OpeningParenthesisRegex(), _ => new OpeningParenthesis()),
		new(ClosingParenthesisRegex(), _ => new ClosingParenthesis<Matrix<T>>()),
		new(ComaRegex(), _ => new Coma<Matrix<T>>())
	])
	{ }

	#endregion

	#region Public methods

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<Matrix<T>>(input, this));

	#endregion
}