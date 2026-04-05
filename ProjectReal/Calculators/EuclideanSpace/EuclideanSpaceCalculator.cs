using Calculators.Standard;
using ProjectReal.NumberSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Calculators.EuclideanSpace;

/// <summary>
/// A calculator that understands matrices and can perform operations with them.
/// </summary>
public partial class EuclideanSpaceCalculator<T> : Calculator
where T :
	IComparisonOperators<T, T, bool>,
	IEqualityOperators<T, T, bool>,
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

	protected readonly FunctionToken<Matrix<T>>[] functionTokens = [];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a calculator that understands matrices and can perform operations with them.
	/// </summary>
	public EuclideanSpaceCalculator(FunctionToken<Matrix<T>>[] functionTokens, StandardCalculator<T> standardCalculator) : base(
		[
			// Matrix
			new(BracketedRegex(), value => new MatrixHolder<T>(new Matrix<T>(value[1..^1], standardCalculator))),
			// Function name
			new(FunctionNameRegex(), name => GetFunctionByName(functionTokens, name)),
			// Operators
			new(AddRegex(), _ => new Add<Matrix<T>>()),
			new(SubtractRegex(), _ => new Subtract<Matrix<T>>()),
			new(MultiplyRegex(), _ => new Multiply<Matrix<T>>()),
			new(DivideRegex(), _ => new Divide<Matrix<T>>()),
			// Separators
			new(OpeningParenthesisRegex(), _ => new OpeningParenthesis()),
			new(ClosingParenthesisRegex(), _ => new ClosingParenthesis<Matrix<T>>()),
			new(ComaRegex(), _ => new Coma<Matrix<T>>())
		]
	) => this.functionTokens = functionTokens;

	#endregion

	#region Protected methods

	protected override string[] GetFunctions() => [.. functionTokens.Select(t => t.Name)];

	#endregion

	#region Public methods

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<Matrix<T>>(input, this));

	#endregion
}