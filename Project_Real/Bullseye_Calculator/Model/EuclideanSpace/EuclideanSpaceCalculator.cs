using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model.EuclideanSpace;

/// <summary>
/// A calculator that understands matrices and can perform operations with them.
/// </summary>
public partial class EuclideanSpaceCalculator : Calculator
{
	#region GeneratedRegex

	[GeneratedRegex(@"^\[.*\]$")]
	protected static partial Regex BracketedRegex();

	#endregion

	#region Fields

	protected static readonly FunctionToken[] functionTokens =
	[
		new("diag", () => new Diagonalize()),
		new("inv", () => new Inverse()),
	];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a calculator that understands matrices and can perform operations with them.
	/// </summary>
	public EuclideanSpaceCalculator() : base(
	[
		// Matrix
		new(BracketedRegex(), match => new MatrixHolder(new Matrix(match[1..^1]))),
		// Function name
		new(FunctionNameRegex(), name => GetFunctionByName(functionTokens, name)),
		// Operators
		new(AddRegex(), _ => new Add()),
		new(SubtractRegex(), _ => new Subtract()),
		new(MultiplyRegex(), _ => new Product()),
		// Separators
		new(OpeningParenthesisRegex(), _ => new OpeningParenthesis()),
		new(ClosingParenthesisRegex(), _ => new ClosingParenthesis<Matrix>()),
		new(ComaRegex(), _ => new Coma<Matrix>())
	])
	{ }

	#endregion

	#region Public methods

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<Matrix>(input, this));

	#endregion
}