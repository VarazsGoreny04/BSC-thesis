namespace Bullseye_Calculator.Model.Standard;

public partial class StandardCalculator : Calculator
{
	protected static readonly FunctionToken[] functionTokens = 
	[
		new("ceiling", () => new Ceiling()),
		new("round", () => new Round()),
		new("floor", () => new Floor()),
		new("fact", () => new Fact()),
		new("abs", () => new Abs()),
		new("pi", () => new PI()),
		new("e", () => new E()),
	];

	public StandardCalculator() : base(
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
		new(ClosingParenthesisRegex(), _ => new ClosingParenthesis())
	]) { }
}