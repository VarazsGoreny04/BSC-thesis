using Project_Real;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Standard;

/// <summary>
/// A calculator that understands the basic numbers, operations and trigonometric functions.
/// </summary>
public partial class StandardCalculator : Calculator
{
	#region Fields

	protected static readonly FunctionToken[] functionTokens =
	[
		new("ceiling", () => new Ceiling()),
		new("round", () => new Round()),
		new("floor", () => new Floor()),
		new("fact", () => new Fact()),
		new("exp", () => new Exp()),
		new("cos", () => new Cos()),
		new("sin", () => new Sin()),
		new("max", () => new Max()),
		new("min", () => new Min()),
		new("abs", () => new Abs()),
		new("ln", () => new Ln()),
		new("pi", () => new PI()),
		new("e", () => new E()),
	];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a calculator that understands the basic numbers, operations and trigonometric functions.
	/// </summary>
	public StandardCalculator() : base(
	[
		// Rational number
		new(new(new($"^\\p{{Nd}}+\\{Rational.Separator}?\\p{{Nd}}*$")), value => new Number(value)),
		// Function name
		new(FunctionNameRegex(), name => GetFunctionByName(functionTokens, name)),
		// Operators
		new(AddRegex(), _ => new Add()),
		new(SubtractRegex(), _ => new Subtract()),
		new(MultiplyRegex(), _ => new Multiply()),
		new(DivideRegex(), _ => new Divide()),
		new(PowerRegex(), _ => new Power()),
		new(RootRegex(), _ => new Root()),
		// Separators
		new(OpeningParenthesisRegex(), _ => new OpeningParenthesis()),
		new(ClosingParenthesisRegex(), _ => new ClosingParenthesis<Rational>()),
		new(ComaRegex(), _ => new Coma<Rational>())
	])
	{ }

	#endregion

	#region Public methods

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<Rational>(input, this));

	#endregion
}