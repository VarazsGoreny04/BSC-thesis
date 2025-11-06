using Bullseye_Calculator.Model.Standard;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public partial class EuclideanSpaceCalculator : Calculator
{
	[GeneratedRegex(@"^\[$")]
	protected static partial Regex OpeningBracketRegex();

	[GeneratedRegex(@"^]$")]
	protected static partial Regex ClosingBracketRegex();

	[GeneratedRegex(@"^;$")]
	protected static partial Regex ColumnEndRegex();

	[GeneratedRegex(@"^&$")]
	protected static partial Regex RowEndRegex();

	protected static readonly FunctionToken[] functionTokens =
	[
		//new("abs", () => new Abs()),
		new("pi", () => new PI()),
		new("e", () => new E()),
	];

	public EuclideanSpaceCalculator() : base(
	[
		// Rational number
		new(null!, value => new Number(value)),
		// Function name
		new(FunctionNameRegex(), name => GetFunctionByName(functionTokens, name)),
		// Operators
		new(AddRegex(), _ => new Add()),
		new(SubtractRegex(), _ => new Subtract()),
		new(MultiplyRegex(), _ => new Multiply()),
		new(DivideRegex(), _ => new Divide()),
		new(PowerRegex(), _ => new Power()),
		new(RootRegex(), _ => new Root()),
		// Parentheses
		new(OpeningParenthesisRegex(), _ => new OpeningParenthesis()),
		new(ClosingParenthesisRegex(), _ => new ClosingParenthesis()),
		// Matrix components
		new(OpeningBracketRegex(), _ => new ClosingParenthesis()),
		new(ClosingBracketRegex(), _ => new ClosingParenthesis()),
		new(ColumnEndRegex(), _ => new ClosingParenthesis()),
		new(RowEndRegex(), _ => new ClosingParenthesis())
	]) { }
}