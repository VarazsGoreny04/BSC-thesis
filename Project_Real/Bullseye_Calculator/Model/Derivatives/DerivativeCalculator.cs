using Bullseye_Calculator.Model.Standard;
using Project_Real;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model.Derivatives;

public partial class DerivativeCalculator : Calculator
{
	[GeneratedRegex(@"^'$")]
	protected static partial Regex DerivativeRegex();

	protected static readonly FunctionToken[] functionTokens =
	[

	];

	public DerivativeCalculator() : base(
	[
		// Rational number
		new(new(new($"^\\p{{Nd}}+[{Rational.Separator}]?\\p{{Nd}}*$")), value => new Number(value)),
		// Function name
		new(FunctionNameRegex(), name => GetFunctionByName(functionTokens, name)),
		// Derivative
		new(DerivativeRegex(), _ => new Derivative()),
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

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<Rational>(input, this));
}