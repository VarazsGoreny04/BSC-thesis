using ProjectReal.NumberSet;
using ProjectReal.Number;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Calculators.Derivatives;

public partial class DerivativeCalculator<T> : Calculator
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
	[GeneratedRegex(@"^'$")]
	internal static partial Regex DerivativeRegex();

	protected static readonly FunctionToken<T>[] functionTokens =
	[

	];

	public DerivativeCalculator() : base(
	[
		// Rational number
		new(NumberRegex(), value => new Number<T>(T.Parse(value, null))),
		// Function name
		new(FunctionNameRegex(), name => GetFunctionByName(functionTokens, name)),
		// Derivative
		new(DerivativeRegex(), _ => new Derivative<T>()),
		// Operators
		new(AddRegex(), _ => new Add<T>()),
		new(SubtractRegex(), _ => new Subtract<T>()),
		new(MultiplyRegex(), _ => new Multiply<T>()),
		new(DivideRegex(), _ => new Divide<T>()),
		new(PowerRegex(), _ => new Power<T>()),
		new(RootRegex(), _ => new Root<T>()),
		// Separators
		new(OpeningParenthesisRegex(), _ => new OpeningParenthesis()),
		new(ClosingParenthesisRegex(), _ => new ClosingParenthesis<Rational>()),
		new(ComaRegex(), _ => new Coma<Rational>())
	])
	{ }

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<T>(input, this));
}