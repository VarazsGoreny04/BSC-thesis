using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public partial class EuclideanSpaceCalculator : Calculator
{
	[GeneratedRegex(@"^\[.*\]$")]
	protected static partial Regex BracketedRegex();

	protected static readonly FunctionToken[] functionTokens =
	[
		new("diag", () => new Diagonalize()),
		new("inv", () => new Inverse()),
	];

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

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<Matrix>(input, this));
}